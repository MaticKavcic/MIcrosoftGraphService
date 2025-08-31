using System.Text;
using System.IO.Pipes;
using System.Text.Json;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceServer
{
    class Server : IDisposable
    {
        public delegate Task<string> RequestHandler(string request);

        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string pipeName;
        private NamedPipeServerStream? pipeServer;
        private readonly Dictionary<RequestType, RequestHandler> handlers;

        public Server(string pipeName)
        {
            this.pipeName = pipeName;
            handlers = [];
        }

        public void Dispose()
        {
            pipeServer?.Dispose();
        }

        public void SetRequestHandler(RequestType type, RequestHandler handler)
        {
            handlers[type] = handler;
        }

        public void Listen()
        {
            pipeServer = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                254,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous
            );

            pipeServer.BeginWaitForConnection(
               new AsyncCallback(ConnectionCallBack),
               pipeServer
            );
        }

        private async Task<string> MessageCallback(string message)
        {
            Request? request = JsonSerializer.Deserialize<Request>(message);
            if (request == null)
            {
                logger.Error("Failed to deserialize request.");

                return JsonSerializer.Serialize(new ErrorResponse("Failed to deserialize request."));
            }

            if (handlers.TryGetValue(request.Type, out RequestHandler? handler))
            {
                return await handler(message);
            }
            else
            {
                logger.Error("Client has made a request that has no registered handler.");

                return JsonSerializer.Serialize(new ErrorResponse("This request does not have a registered handler."));
            }
        }

        private async void ConnectionCallBack(IAsyncResult iar)
        {
            logger.Trace("Client connection recived.");

            try
            {
                Listen();

                using (NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState)
                {
                    pipeServer.EndWaitForConnection(iar);

                    byte[] lengthBuffer = new byte[4];
                    int readByte = await pipeServer.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);
                    if (readByte != 4)
                    {
                        throw new Exception("Invalid message length in pipe.");
                    }

                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                    if (messageLength < 0 || messageLength > 1024 * 1024)
                    {
                        throw new ArgumentOutOfRangeException("Message length is out of bounds.");
                    }

                    var messageBuffer = new byte[messageLength];
                    await pipeServer.ReadExactlyAsync(messageBuffer);

                    string message = Encoding.UTF8.GetString(messageBuffer, 0, messageBuffer.Length);

                    string reply = await MessageCallback(message);

                    var replyBytes = Encoding.UTF8.GetBytes(reply);
                    lengthBuffer = BitConverter.GetBytes(replyBytes.Length);

                    await pipeServer.WriteAsync(lengthBuffer);
                    await pipeServer.WriteAsync(replyBytes);
                }
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}