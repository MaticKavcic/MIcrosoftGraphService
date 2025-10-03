using Microsoft.Graph.Models;
using MicrosoftGraphService.Model;
using System.Text.Json;

namespace MicrosoftGraphServiceServer
{
    public class ServerHandlers
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static async Task<string> GetEmailsHandler(string request)
        {
            GetEmailsRequest? getEmialsRequest = JsonSerializer.Deserialize<GetEmailsRequest>(request);
            if (getEmialsRequest == null)
            {
                logger.Warn("The get emails request handler recived a request that was not in the right format.");

                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                Graph graph = new Graph(getEmialsRequest.Credentials);

                List<Message> emails = await graph.GetEmails(getEmialsRequest.Top);

                return JsonSerializer.Serialize(new GetEmailsResponse(Utils.MessagesToEmails(emails.ToArray()).ToArray()));
            }
            catch (Exception ex)
            {
                logger.Warn("The get emails request handler failed to retrive emails from the microsoft graph API.");

                return JsonSerializer.Serialize(new ErrorResponse($"Failed to retrive emails: {ex.Message}"));
            }
        }

        public static async Task<string> GetEmailsDetailedHandler(string request)
        {
            GetEmailsDetailedRequest? getEmailsDetailedRequest = JsonSerializer.Deserialize<GetEmailsDetailedRequest>(request);
            if (getEmailsDetailedRequest == null)
            {
                logger.Warn("The get emails detailed request handler recived a request that was not in the right format.");

                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                Graph graph = new Graph(getEmailsDetailedRequest.Credentials);

                List<Message> emails = await graph.GetEmailsDetailed(getEmailsDetailedRequest.Emails);

                return JsonSerializer.Serialize(new GetEmailsDetailedResponse(Utils.MessagesToEmails(emails.ToArray()).ToArray()));
            }
            catch (Exception ex)
            {
                logger.Warn("The get emails detaile request handler failed to retrive detailed emails from the microsoft graph API.");

                return JsonSerializer.Serialize(new ErrorResponse($"Failed to retrive detailed emails: {ex.Message}"));
            }

        }

        public static async Task<string> SendEmailHandler(string request)
        {
            SendEmailRequest? sendEmailRequest = JsonSerializer.Deserialize<SendEmailRequest>(request);
            if (sendEmailRequest == null)
            {
                logger.Warn("The send email request handler recived a request that was not in the right format.");

                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                Graph graph = new Graph(sendEmailRequest.Credentials);

                await graph.SendEmail(Utils.EmailToMessage(sendEmailRequest.Email));

                return JsonSerializer.Serialize(new OkResponse());
            }
            catch (Exception ex)
            {
                logger.Warn("The send email request handler failed to send an email using the microsoft graph API.");

                return JsonSerializer.Serialize(new ErrorResponse($"Failed to send an email: {ex.Message}"));
            }
        }

        public static async Task<string> DeleteEmailHandler(string request)
        {
            DeleteEmailRequest? deleteEmailRequest = JsonSerializer.Deserialize<DeleteEmailRequest>(request);
            if (deleteEmailRequest == null)
            {
                logger.Warn("The delete email request handler recived a request that was not in the right format.");

                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                Graph graph = new Graph(deleteEmailRequest.Credentials);

                await graph.DeleteEmail(deleteEmailRequest.Email);

                return JsonSerializer.Serialize(new OkResponse());
            }
            catch (Exception ex)
            {
                logger.Warn("The delete email request handler failed to delete an email using the microsoft graph API.");

                return JsonSerializer.Serialize(new ErrorResponse($"Failed to delete an email from the inbox: {ex.Message}"));
            }
        }
    }
}
