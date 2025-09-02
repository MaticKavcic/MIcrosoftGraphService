using System.Text.Json;
using Microsoft.Graph.Models;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceServer
{
    static class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static Graph? graph;
        private static Server? server;

        static async Task<string> ConfigHandler(string request)
        {
            ConfigRequest? configRequest = JsonSerializer.Deserialize<ConfigRequest>(request);
            if (configRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                graph = new Graph(configRequest.Email, configRequest.Secret, configRequest.ClientId, configRequest.TenantId);
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to configure server: {ex.Message}"));
            }

            return JsonSerializer.Serialize(new OkResponse());
        }

        static async Task<string> GetEmailsHandler(string request) {
            if (graph == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("The server is not configured."));
            }

            GetEmailsRequest? getEmialsRequest = JsonSerializer.Deserialize<GetEmailsRequest>(request);
            if (getEmialsRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                List<Message> emails = await graph.GetEmails(getEmialsRequest.Top);

                return JsonSerializer.Serialize(new GetEmailsResponse(Utils.MessagesToEmails(emails.ToArray()).ToArray()));
            } catch(Exception ex) {
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to retrive emails: {ex.Message}"));
            }
        }

        static async Task<string> GetEmailsDetailedHandler(string request)
        {
            if (graph == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("The server is not configured."));
            }

            GetEmailsDetailedRequest? getEmailsDetailedRequest = JsonSerializer.Deserialize<GetEmailsDetailedRequest>(request);
            if (getEmailsDetailedRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                List<Message>? emails = await graph.GetEmailsDetailed(getEmailsDetailedRequest.Emails);

                return JsonSerializer.Serialize(new GetEmailsDetailedResponse(Utils.MessagesToEmails(emails.ToArray()).ToArray()));
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to retrive detailed emails: {ex.Message}"));
            }
            
        }

        static async Task<string> SendEmailHandler(string request)
        {
            if (graph == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("The server is not configured."));
            }

            SendEmailRequest? sendEmailRequest = JsonSerializer.Deserialize<SendEmailRequest>(request);
            if (sendEmailRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                graph.SendEmail(Utils.EmailToMessage(sendEmailRequest.Email));

                return JsonSerializer.Serialize(new OkResponse());
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to send an email: {ex.Message}"));
            }
        }

        static async Task<string> DeleteEmailHandler(string request)
        {
            if (graph == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("The server is not configured."));
            }

            DeleteEmailRequest? deleteEmailRequest = JsonSerializer.Deserialize<DeleteEmailRequest>(request);
            if (deleteEmailRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                graph.DeleteEmail(deleteEmailRequest.Email);

                return JsonSerializer.Serialize(new OkResponse());
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to delete an email from the inbox: {ex.Message}"));
            }
        }

        static void Main(string[] args) {
            try
            {
                server = new Server("MicrosoftGraphService");

                server.SetRequestHandler(RequestType.CONFIG, ConfigHandler);
                server.SetRequestHandler(RequestType.GET_EMAILS, GetEmailsHandler);
                server.SetRequestHandler(RequestType.GET_EMAILS_DETAILED, GetEmailsDetailedHandler);
                server.SetRequestHandler(RequestType.SEND_EMAIL, SendEmailHandler);
                server.SetRequestHandler(RequestType.DELETE_EMAIL, DeleteEmailHandler);

                Task.Run(() => {
                    try
                    {
                        server.Listen();
                    }
                    catch (Exception)
                    {
                        Environment.Exit(1);
                    }
                });

                Console.ReadLine();
            }
            catch (Exception)
            {
                Environment.Exit(1);
            }
        }
    }
}