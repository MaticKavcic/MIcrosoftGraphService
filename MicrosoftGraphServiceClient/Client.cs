using System.Text;
using System.Text.Json;
using System.IO.Pipes;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceClient
{
    class Client
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string pipeName;

        public Client(string pipeName) {
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

        public async Task<Union<OkResponse, ErrorResponse>> Config(string email, string secret, string clientId, string tenantId)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new ConfigRequest(
                    email,
                    secret,
                    clientId,
                    tenantId
                )));

                Response? responseObj = JsonSerializer.Deserialize<Response>(response);
                if (responseObj == null)
                {
                    throw new Exception("Failed to parse response.");
                }

                switch (responseObj.Type)
                {
                    case ResponseType.OK:
                        OkResponse? okResponse = JsonSerializer.Deserialize<OkResponse>(response);
                        if (okResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<OkResponse, ErrorResponse>(okResponse);
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<OkResponse, ErrorResponse>(errorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task<Union<GetEmailsResponse, ErrorResponse>> GetEmails(int top)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new GetEmailsRequest(top)));

                Response? responseObj = JsonSerializer.Deserialize<Response>(response);
                if (responseObj == null)
                {
                    throw new Exception("Failed to parse response.");
                }

                switch (responseObj.Type)
                {
                case ResponseType.OK:
                        GetEmailsResponse? getEmailsResponse = JsonSerializer.Deserialize<GetEmailsResponse>(response);
                        if (getEmailsResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<GetEmailsResponse, ErrorResponse>(getEmailsResponse);
                default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<GetEmailsResponse, ErrorResponse>(errorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task<Union<GetEmailsDetailedResponse, ErrorResponse>> GetEmailsDetailed(string[] emails)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new GetEmailsDetailedRequest(emails)));

                Response? responseObj = JsonSerializer.Deserialize<Response>(response);
                if (responseObj == null)
                {
                    throw new Exception("Failed to parse response.");
                }

                switch (responseObj.Type)
                {
                    case ResponseType.OK:
                        GetEmailsDetailedResponse? getEmailsDetailedResponse = JsonSerializer.Deserialize<GetEmailsDetailedResponse>(response);
                        if (getEmailsDetailedResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<GetEmailsDetailedResponse, ErrorResponse>(getEmailsDetailedResponse);
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<GetEmailsDetailedResponse, ErrorResponse>(errorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw;
            }
        }

        public async Task<Union<OkResponse, ErrorResponse>> SendEmail(Email email)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new SendEmailRequest(email)));

                Response? responseObj = JsonSerializer.Deserialize<Response>(response);
                if (responseObj == null)
                {
                    throw new Exception("Failed to parse response.");
                }

                switch (responseObj.Type)
                {
                    case ResponseType.OK:
                        OkResponse? okResponse = JsonSerializer.Deserialize<OkResponse>(response);
                        if (okResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<OkResponse, ErrorResponse>(okResponse);
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<OkResponse, ErrorResponse>(errorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw;
            }
        }

        public async Task<Union<OkResponse, ErrorResponse>> DeleteEmail(string email)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new DeleteEmailRequest(email)));

                Response? responseObj = JsonSerializer.Deserialize<Response>(response);
                if (responseObj == null)
                {
                    throw new Exception("Failed to parse response.");
                }

                switch (responseObj.Type)
                {
                    case ResponseType.OK:
                        OkResponse? okResponse = JsonSerializer.Deserialize<OkResponse>(response);
                        if (okResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<OkResponse, ErrorResponse>(okResponse);
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        return new Union<OkResponse, ErrorResponse>(errorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw;
            }
        }
    }
}
