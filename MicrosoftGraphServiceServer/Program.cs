using System.Text.Json;
using Microsoft.Graph.Models;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceServer
{
    static class Program
    {
        private static Graph? graph;
        private static Server? server;

        static async Task<string> GetEmailsHandler(string request) {
            GetEmailsRequest? getEmialsRequest = JsonSerializer.Deserialize<GetEmailsRequest>(request);
            if (getEmialsRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            List<Message>? emails = await graph.GetEmails(getEmialsRequest.Top);
            if (emails == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Failed to retrive emails."));
            }

            return JsonSerializer.Serialize(new GetEmailsResponse(emails.ToArray()));
        }

        static async Task<string> GetEmailsDetailedHandler(string request)
        {
            GetEmailsDetailedRequest? getEmailsDetailedRequest = JsonSerializer.Deserialize<GetEmailsDetailedRequest>(request);
            if (getEmailsDetailedRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            List<Message>? emails = await graph.GetEmailsDetailed(getEmailsDetailedRequest.Emails);
            if (emails == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Failed to retrive detailed emails."));
            }

            return JsonSerializer.Serialize(new GetEmailsDetailedResponse(emails.ToArray()));
        }

        static async Task<string> SendEmailHandler(string request)
        {
            SendEmailRequest? sendEmailRequest = JsonSerializer.Deserialize<SendEmailRequest>(request);
            if (sendEmailRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            if (!await graph.SendEmail(sendEmailRequest.Email))
            {
                return JsonSerializer.Serialize(new ErrorResponse("Failed to send an email."));
            }

            return JsonSerializer.Serialize(new OkResponse());
        }

        static async Task<string> DeleteEmailHandler(string request)
        {
            DeleteEmailRequest? deleteEmailRequest = JsonSerializer.Deserialize<DeleteEmailRequest>(request);
            if (deleteEmailRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            if (!await graph.DeleteEmail(deleteEmailRequest.Email))
            {
                return JsonSerializer.Serialize(new ErrorResponse("Failed to delete an email from the inbox."));
            }

            return JsonSerializer.Serialize(new OkResponse());
        }

        static void Main(string[] args) {
            try
            {
                graph = new Graph(Settings.LoadSettings());
                server = new Server("MicrosoftGraphService");

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