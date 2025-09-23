using System.Text.Json;
using Microsoft.Graph.Models;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceServer
{
    static class Program
    {
        private static Server? server;

        static async Task<string> GetEmailsHandler(string request) {
            GetEmailsRequest? getEmialsRequest = JsonSerializer.Deserialize<GetEmailsRequest>(request);
            if (getEmialsRequest == null)
            {
                return JsonSerializer.Serialize(new ErrorResponse("Invalid request."));
            }

            try
            {
                Graph graph = new Graph(getEmialsRequest.Credentials);

                List<Message> emails = await graph.GetEmails(getEmialsRequest.Top);

                return JsonSerializer.Serialize(new GetEmailsResponse(Utils.MessagesToEmails(emails.ToArray()).ToArray()));
            } catch(Exception ex) {
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to retrive emails: {ex.Message}"));
            }
        }

        static async Task<string> GetEmailsDetailedHandler(string request)
        {
            GetEmailsDetailedRequest? getEmailsDetailedRequest = JsonSerializer.Deserialize<GetEmailsDetailedRequest>(request);
            if (getEmailsDetailedRequest == null)
            {
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
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to retrive detailed emails: {ex.Message}"));
            }
            
        }

        static async Task<string> SendEmailHandler(string request)
        {
            SendEmailRequest? sendEmailRequest = JsonSerializer.Deserialize<SendEmailRequest>(request);
            if (sendEmailRequest == null)
            {
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
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to send an email: {ex.Message}"));
            }
        }

        static async Task<string> DeleteEmailHandler(string request)
        {
            DeleteEmailRequest? deleteEmailRequest = JsonSerializer.Deserialize<DeleteEmailRequest>(request);
            if (deleteEmailRequest == null)
            {
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
                return JsonSerializer.Serialize(new ErrorResponse($"Failed to delete an email from the inbox: {ex.Message}"));
            }
        }

        static void Main(string[] args) {
            try
            {
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