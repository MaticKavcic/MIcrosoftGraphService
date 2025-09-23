using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using Microsoft.Identity.Client;
using MicrosoftGraphService.Model;
using System.Net.Http.Headers;

namespace MicrosoftGraphServiceServer
{
    class Graph
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly string email;
        private readonly GraphServiceClient graphClient;

        public Graph(Credentials credentials)
        {
            try
            {
                this.email = credentials.Email;

                IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
                        .Create(credentials.ClientId)
                        .WithTenantId(credentials.TenantId)
                        .WithClientSecret(credentials.Secret)
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

        public async Task<List<Message>> GetEmails(int top)
        {
            try
            {
                MessageCollectionResponse response = await graphClient.Users[email].Messages.GetAsync(x =>
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
                if (response == null || response.Value == null)
                {
                    throw new Exception("No emails meet the required criteria.");
                }

                return response.Value;
            }
            catch (Exception ex)
            {
                logger.Error(ex);

                throw;
            }
        }

        public async Task<List<Message>> GetEmailsDetailed(string[] emails)
        {
            List<Message> messages = [];

            try
            {
                foreach (string email in emails)
                {
                    Message? message = await graphClient.Users[this.email].Messages[email].GetAsync(x => 
                    {
                        x.QueryParameters.Expand = [
                            "attachments"
                        ];
                    });
                    if (message == null)
                    {
                        continue;
                    }

                    messages.Add(message);
                }

                return messages;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task SendEmail(Message message)
        {
            try
            {
                SendMailPostRequestBody request = new SendMailPostRequestBody();
                request.Message = message;
                request.SaveToSentItems = true;

                await graphClient.Users[email].SendMail.PostAsync(request);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public async Task DeleteEmail(string email)
        {
            try
            {
                await graphClient.Users[this.email].Messages[email].DeleteAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}
