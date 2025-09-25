using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceClient.Tests
{
    public class ClientTest
    {
        private Client client;

        [SetUp]
        public void Setup()
        {
            client = new Client(
                "MicrosoftGraphService",
                new Credentials(
                    "",
                    "",
                    "",
                    ""
                )
            );
        }

        [Test]
        public async Task GetEmails_DoesntThrow()
        {
            try
            {
                await client.GetEmails(100);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task GetEmailsDetailed_DoesntThrow()
        {
            try
            {
                GetEmailsResponse getEmails = await client.GetEmails(100);

                List<String> emails = [];
                foreach (Email email in getEmails.Emails)
                {
                    if (email.Id == null)
                    {
                        continue;
                    }

                    emails.Add(email.Id);
                }

                await client.GetEmailsDetailed(emails.ToArray());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task SendEmail_DoesntThrow()
        {
            try
            {
                Email newEmail = new Email();

                newEmail.Recipients = ["st3@infrax.si"];
                newEmail.ContentType = EmailContentType.HTML;
                newEmail.Content = "<p>Hello, world!</p>";

                await client.SendEmail(newEmail);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task DeleteEmail_DoesntThrow()
        {
            try
            {
                GetEmailsResponse getEmails = await client.GetEmails(100);

                Assert.Multiple(() =>
                {
                    Assert.That(getEmails.Emails.Length, Is.GreaterThan(0));
                    Assert.That(getEmails.Emails[0].Id, Is.Not.Null);
                });

                await client.DeleteEmail(getEmails.Emails[0].Id);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
