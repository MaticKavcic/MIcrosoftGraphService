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

                Union<OkResponse, ErrorResponse> config_response = await client.Config(
                    "",
                    "",
                    "",
                    ""
                );
                if (config_response.Is<ErrorResponse>())
                {
                    ErrorResponse errorResponse = config_response;

                    foreach (string error in errorResponse.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    Environment.Exit(1);
                }

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
                foreach (Email email in getEmailsResponse.Emails)
                {
                    if (email.Id == null)
                    {
                        continue;
                    }

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
                foreach (Email email in getEmailsDetailedResponse.Emails)
                {
                    Console.WriteLine(email.Content);
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
                foreach (Email email in getEmailsResponse.Emails)
                {
                    if (email.Id == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"ID: {email.Id}");

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

                /*
                Email newEmail = new Email();

                newEmail.Recipients = ["st3@infrax.si"];
                newEmail.ContentType = EmailContentType.HTML;
                newEmail.Content = "<p>Hello, world!</p>";

                Union<OkResponse, ErrorResponse> response = await client.SendEmail(newEmail);
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

                foreach (Email email in getEmailsResponse.Emails)
                {
                    Console.WriteLine($"ID: {email.Id}");
                }
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
        }
    }
}