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

                await client.Config(
                    "",
                    "",
                    "",
                    ""
                );

                /*
                GetEmailsResponse getEmailsResponse = await client.GetEmails(100);

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

                GetEmailsDetailedResponse getEmailsDetailedResponse = await client.GetEmailsDetailed(ids.ToArray());
                foreach (Email email in getEmailsDetailedResponse.Emails)
                {
                    Console.WriteLine(email.Content);
                }
                */

                /*
                GetEmailsResponse getEmailsResponse = await client.GetEmails(100);

                List<string> ids = [];
                foreach (Email email in getEmailsResponse.Emails)
                {
                    if (email.Id == null)
                    {
                        continue;
                    }

                    Console.WriteLine($"ID: {email.Id}");
                    await client.DeleteEmail(email.Id);
                }
                */

                /*
                Email newEmail = new Email();

                newEmail.Recipients = ["st3@infrax.si"];
                newEmail.ContentType = EmailContentType.HTML;
                newEmail.Content = "<p>Hello, world!</p>";

                await client.SendEmail(newEmail);

                GetEmailsResponse getEmailsResponse = await client.GetEmails(100);
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