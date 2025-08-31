using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace MicrosoftGraphServiceServer
{
    class Graph
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Settings settings;
        private readonly GraphServiceClient graphClient;

        public Graph(Settings settings)
        {
            try
            {
                this.settings = settings;

                IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
                        .Create(settings.ClientId)
                        .WithTenantId(settings.TenantId)
                        .WithClientSecret(settings.Secret)
                        .Build();

                string[] scopes = ["https://graph.microsoft.com/.default"];

                AuthenticationResult authResult = confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync().Result;

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);

                graphClient = new GraphServiceClient(httpClient);
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw;
            }
        }

        public async Task<List<Message>?> GetEmails(int top)
        {
            try
            {
                MessageCollectionResponse response = await graphClient.Users[settings.Email].Messages.GetAsync(x =>
                {
                    x.QueryParameters.Top = top;
                    x.QueryParameters.Select = [
                        "id",
                        "from",
                        "toRecipients",
                        "ccRecipients",
                        "bccRecipients",
                        "receivedDateTime",
                        "subject"
                    ];
                });
                if (response == null)
                {
                    return null;
                }

                return response.Value;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return null;
            }
        }

        public async Task<List<Message>> GetEmailsDetailed(string[] emails)
        {
            List<Message> messages = [];

            try
            {
                foreach (string email in emails)
                {
                    Message? message = await graphClient.Users[settings.Email].Messages[email].GetAsync();
                    if (message == null)
                    {
                        continue;
                    }

                    messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return messages;
        }

        public async Task<bool> SendEmail(Message message)
        {
            try
            {
                SendMailPostRequestBody request = new SendMailPostRequestBody();
                request.Message = message;
                request.SaveToSentItems = true;

                await graphClient.Users[settings.Email].SendMail.PostAsync(request);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return false;
            }
        }

        public async Task<bool> DeleteEmail(string email)
        {
            try
            {
                await graphClient.Users[settings.Email].Messages[email].DeleteAsync();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                return false;
            }
        }
    }
}
