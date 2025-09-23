using System.Text.Json;
using MicrosoftGraphService.Shared;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceClient
{
    class Client : PipeClient
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Credentials credentials;

        public Client(string pipeName, Credentials credentials) : base(pipeName)
        {
            this.credentials = credentials;
        }

        public async Task<GetEmailsResponse> GetEmails(int top)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new GetEmailsRequest(credentials, top)));

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

                        return getEmailsResponse;
                default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        throw new Exception(errorResponse.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task<GetEmailsDetailedResponse> GetEmailsDetailed(string[] emails)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new GetEmailsDetailedRequest(credentials, emails)));

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

                        return getEmailsDetailedResponse;
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        throw new Exception(errorResponse.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task<OkResponse> SendEmail(Email email)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new SendEmailRequest(credentials, email)));

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

                        return okResponse;
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        throw new Exception(errorResponse.Error);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task<OkResponse> DeleteEmail(string email)
        {
            try
            {
                string response = await Send(JsonSerializer.Serialize(new DeleteEmailRequest(credentials, email)));

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

                        return okResponse;
                    default:
                        ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(response);
                        if (errorResponse == null)
                        {
                            throw new Exception("Failed to parse response.");
                        }

                        throw new Exception(errorResponse.Error);
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
