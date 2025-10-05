using MicrosoftGraphService.Model;
using NUnit.Framework.Legacy;
using System.Text.Json;

namespace MicrosoftGraphServiceServer.Tests
{
    public class ServerHandlersTests
    {
        private Credentials credentials;

        [SetUp]
        public void CreateCredentials()
        {
            credentials = new Credentials(
                "",
                "",
                "",
                ""
            );
        }

        [Test]
        public async Task SendEmailHandler_ValidRequest_ShouldNotThrow()
        {
            try
            {
                Email newEmail = new Email();

                newEmail.Recipients = [credentials.Email];
                newEmail.Subject = "Test email";
                newEmail.Content = "Hello, world!";
                newEmail.ContentType = EmailContentType.TEXT;

                await ServerHandlers.SendEmailHandler(JsonSerializer.Serialize(new SendEmailRequest(
                    credentials,
                    newEmail
                )));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task SendEmailHandler_InvalidRequest_ShouldThrow()
        {
            try
            {
                await ServerHandlers.SendEmailHandler("");

                Assert.Fail("GetEmailsHandler should throw.");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public async Task GetEmailsHandler_ValidRequest_ShouldNotThrow()
        {
            try
            {
                await ServerHandlers.GetEmailsHandler(JsonSerializer.Serialize(new GetEmailsRequest(
                    credentials,
                    100
                )));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task GetEmailsHandler_InvalidRequest_ShouldThrow()
        {
            try
            {
                await ServerHandlers.GetEmailsHandler("");

                Assert.Fail("GetEmailsHandler should throw.");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public async Task GetEmailsDetailedHandler_ValidRequest_ShouldNotThrow()
        {
            try
            {
                GetEmailsResponse? getEmails = JsonSerializer.Deserialize<GetEmailsResponse>(await ServerHandlers.GetEmailsHandler(JsonSerializer.Serialize(new GetEmailsRequest(
                    credentials,
                    100
                ))));
                Assert.That(getEmails, Is.Not.Null);
                Assert.That(getEmails.Emails.Count, Is.AtLeast(1));
                Assert.That(getEmails.Emails[0].Id, Is.Not.Null);

                await ServerHandlers.GetEmailsDetailedHandler(JsonSerializer.Serialize(new GetEmailsDetailedRequest(
                    credentials,
                    [ getEmails.Emails[0].Id ]
                )));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task GetEmailsDetailedHandlerHandler_InvalidRequest_ShouldThrow()
        {
            try
            {
                await ServerHandlers.GetEmailsDetailedHandler("");

                Assert.Fail("GetEmailsDetailedHandler should throw.");
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public async Task DeleteEmailHandler_ValidRequest_ShouldNotThrow()
        {
            try
            {
                GetEmailsResponse? getEmails = JsonSerializer.Deserialize<GetEmailsResponse>(await ServerHandlers.GetEmailsHandler(JsonSerializer.Serialize(new GetEmailsRequest(
                    credentials,
                    100
                ))));
                Assert.That(getEmails, Is.Not.Null); 
                Assert.That(getEmails.Emails.Count, Is.AtLeast(1));
                Assert.That(getEmails.Emails[0].Id, Is.Not.Null);

                await ServerHandlers.DeleteEmailHandler(JsonSerializer.Serialize(new DeleteEmailRequest(
                    credentials,
                    getEmails.Emails[0].Id
                )));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task DeleteEmailHandler_InvalidRequest_ShouldThrow()
        {
            try
            {
                await ServerHandlers.DeleteEmailHandler("");

                Assert.Fail("GetEmailsHandler should throw.");
            }
            catch (Exception)
            {
            }
        }
    }
}
