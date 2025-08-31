using Azure;
using Microsoft.Graph.Models;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceClient
{
    static class Program
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args) {
            try
            {
                Client client = new Client("MicrosoftGraphService");

                /*
                Union<GetEmailsResponse, ErrorResponse> response = await client.GetEmails(100);
                if (response.Is<ErrorResponse>()) {
                    ErrorResponse errorResponse = response;

                    foreach (string error in errorResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                }

                GetEmailsResponse getEmailsResponse = response;

                List<string> ids = [];
                foreach (Message email in getEmailsResponse.Emails)
                {
                    ids.Add(email.Id);

                    Console.WriteLine(email.Id);
                }

                Union<GetEmailsDetailedResponse, ErrorResponse> response2 = await client.GetEmailsDetailed(ids.ToArray());
                if (response2.Is<ErrorResponse>())
                {
                    ErrorResponse errorResponse = response2;

                    foreach (string error in errorResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                }

                GetEmailsDetailedResponse getEmailsDetailedResponse = response2;
                foreach (Message email in getEmailsDetailedResponse.Emails)
                {
                    Console.WriteLine(email.Body.Content);
                }
                */

                /*
                Union<GetEmailsResponse, ErrorResponse> response = await client.GetEmails(100);
                if (response.Is<ErrorResponse>())
                {
                    ErrorResponse errorResponse = response;

                    foreach (string error in errorResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                }

                GetEmailsResponse getEmailsResponse = response;

                List<string> ids = [];
                foreach (Message email in getEmailsResponse.Emails)
                {
                    Console.WriteLine(email.Id);

                    Union<OkResponse, ErrorResponse> response2 = await client.DeleteEmail(email.Id);
                    if (response2.Is<ErrorResponse>())
                    {
                        ErrorResponse errorResponse = response2;

                        foreach (string error in errorResponse.Errors)
                        {
                            Console.WriteLine(error);
                        }

                        Environment.Exit(1);
                    }
                }
                */

                var message = new Message();

                message.Subject = "Test email";

                message.Body = new ItemBody();
                message.Body.Content = "<p>Hello, world!</p>";
                message.Body.ContentType = BodyType.Html;

                Recipient recipient = new Recipient();
                recipient.EmailAddress = new EmailAddress();
                recipient.EmailAddress.Address = "st3@infrax.si";

                message.ToRecipients = [ recipient ];
                message.CcRecipients = new List<Recipient>();
                message.BccRecipients = new List<Recipient>();
                message.ReplyTo = new List<Recipient>();
                message.Attachments = new List<Attachment>();

                Union<OkResponse, ErrorResponse> response = await client.SendEmail(message);
                if (response.Is<ErrorResponse>())
                {
                    ErrorResponse errorResponse = response;

                    foreach (string error in errorResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                }

                Union<GetEmailsResponse, ErrorResponse> response2 = await client.GetEmails(100);
                if (response2.Is<ErrorResponse>())
                {
                    ErrorResponse errorResponse = response2;

                    foreach (string error in errorResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                }

                GetEmailsResponse getEmailsResponse = response2;

                foreach (Message email in getEmailsResponse.Emails)
                {
                    Console.WriteLine(email.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}