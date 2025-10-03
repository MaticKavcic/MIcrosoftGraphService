using System.IO.Pipes;
using System.Text;

namespace MicrosoftGraphService.Shared
{
    public class PipeServer
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string pipeName;
        private NamedPipeServerStream? pipeServer;

        public PipeServer(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public void Dispose()
        {
            pipeServer?.Dispose();
        }

        public void Listen()
        {
            logger.Trace("Server is listening for connections...");

            pipeServer = new NamedPipeServerStream(
                pipeName,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous
            );

            pipeServer.BeginWaitForConnection(
               new AsyncCallback(ConnectionCallBack),
               pipeServer
            );
        }

        protected virtual async Task<string> HandleRequest(string request)
        {
            logger.Warn("Request recived by a default handler, this method should be overriten.");

            return "";
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
                    var messageBuffer = new byte[messageLength];
                    await pipeServer.ReadExactlyAsync(messageBuffer);

                    string message = Encoding.UTF8.GetString(messageBuffer, 0, messageBuffer.Length);

                    string reply = await HandleRequest(message);

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
