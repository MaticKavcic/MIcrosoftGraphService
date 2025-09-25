using System.IO.Pipes;
using System.Text;

namespace MicrosoftGraphService.Shared
{
    public class PipeClient
    {
        private readonly string pipeName;

        public PipeClient(string pipeName)
        {
            this.pipeName = pipeName;
        }

        public async Task<string> Send(string request, int timeOut = 1000)
        {
            NamedPipeClientStream pipeClient = new NamedPipeClientStream(
                ".",
                pipeName,
                PipeDirection.InOut,
                PipeOptions.Asynchronous
            );

            pipeClient.Connect();

            byte[] lengthBuffer = new byte[4];
            byte[] writeBuffer = Encoding.UTF8.GetBytes(request);
            lengthBuffer = BitConverter.GetBytes(writeBuffer.Length);

            await pipeClient.WriteAsync(lengthBuffer, 0, lengthBuffer.Length);
            await pipeClient.WriteAsync(writeBuffer, 0, writeBuffer.Length);

            int readByte = await pipeClient.ReadAsync(lengthBuffer, 0, lengthBuffer.Length);

            if (readByte != 4)
            {
                throw new Exception("Invalid message length in pipe.");
            }

            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
            byte[] messageBuffer = new byte[messageLength];
            readByte = await pipeClient.ReadAsync(messageBuffer, 0, messageBuffer.Length);

            return Encoding.UTF8.GetString(messageBuffer, 0, messageBuffer.Length);
        }
    }
}
