using Microsoft.Graph.Models;
using MicrosoftGraphService.Model;

namespace MicrosoftGraphServiceServer
{
    class Utils
    {
        public static Recipient RecipientFromEmail(string email)
        {
            Recipient recipient = new Recipient();
            recipient.EmailAddress = new EmailAddress();
            recipient.EmailAddress.Address = email;

            return recipient;
        }

        public static List<Recipient> RecipientsFromEmails(string[] emails) {
            List<Recipient> recipients = [];
            foreach (string email in emails)
            {
                recipients.Add(RecipientFromEmail(email));
            }

            return recipients;
        }

        public static string? GetEmailFromRecipient(Recipient recipient)
        {
            if (recipient.EmailAddress == null)
            {
                return null;
            }

            return recipient.EmailAddress.Address;
        }

        public static List<string> GetEmailsFromRecipients(Recipient[] recipients)
        {
            List<string> emails = [];
            foreach (Recipient recipient in recipients)
            {
                string? email = GetEmailFromRecipient(recipient);
                if (email == null)
                {
                    continue;
                }

                emails.Add(email);
            }

            return emails;
        }

        public static Email MessageToEmail(Message message) {
            Email email = new Email();

            email.Id = message.Id;
            email.Date = message.SentDateTime;
            email.From = message.From?.EmailAddress?.Address;

            if (message.ToRecipients != null)
            {
                email.Recipients = GetEmailsFromRecipients(message.ToRecipients.ToArray()).ToArray();
            }

            if (message.CcRecipients != null)
            {
                email.CCRecipients = GetEmailsFromRecipients(message.CcRecipients.ToArray()).ToArray();
            }

            if (message.BccRecipients != null)
            {
                email.BCCRecipients = GetEmailsFromRecipients(message.BccRecipients.ToArray()).ToArray();
            }

            email.Subject = message.Subject;

            if (message.Body != null)
            {
                email.Content = message.Body.Content;

                switch (message.Body.ContentType)
                {
                    case BodyType.Text:
                        email.ContentType = EmailContentType.TEXT;
                        break;
                    case BodyType.Html:
                        email.ContentType = EmailContentType.HTML;
                        break;
                }
            }

            return email;
        }

        public static List<Email> MessagesToEmails(Message[] messages)
        {
            List<Email> emails = [];
            foreach (Message message in messages)
            {
                emails.Add(MessageToEmail(message));
            }

            return emails;
        }

        public static Message EmailToMessage(Email email)
        {
            Message message = new Message();

            message.Id = email.Id;
            message.SentDateTime = email.Date;

            if (email.From != null)
            {
                message.From = RecipientFromEmail(email.From);
            }

            if (email.Recipients != null) {
                message.ToRecipients = RecipientsFromEmails(email.Recipients.ToArray());
            }

            if (email.CCRecipients != null)
            {
                message.CcRecipients = RecipientsFromEmails(email.CCRecipients.ToArray());
            }

            if (email.BCCRecipients != null)
            {
                message.BccRecipients = RecipientsFromEmails(email.BCCRecipients.ToArray());
            }

            if (email.Subject != null)
            {
                message.Subject = email.Subject;
            }

            message.Body = new ItemBody();
            message.Body.Content = email.Content;

            switch (email.ContentType)
            {
                case EmailContentType.TEXT:
                    message.Body.ContentType = BodyType.Text;
                    break;
                case EmailContentType.HTML:
                    message.Body.ContentType = BodyType.Html;
                    break;
            }

            return message;
        }

        public static List<Message> EmailsToMessages(Email[] emails)
        {
            List<Message> messages = [];
            foreach (Email email in emails)
            {
                messages.Add(EmailToMessage(email));
            }

            return messages;
        }
    }
}
