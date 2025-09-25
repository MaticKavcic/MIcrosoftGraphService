namespace MicrosoftGraphService.Model
{
    public enum RequestType
    {
        NONE = 0,
        GET_EMAILS = 1,
        GET_EMAILS_DETAILED = 2,
        SEND_EMAIL = 3,
        DELETE_EMAIL = 4
    }

    public class Request
    {
        public Request()
        {
            Type = RequestType.NONE;
        }

        public RequestType Type { get; set; }
    }

    public class GetEmailsRequest : Request
    {
        public GetEmailsRequest(Credentials credentials, int top)
        {
            Type = RequestType.GET_EMAILS;
            Credentials = credentials;
            Top = top;
        }

        public Credentials Credentials { get; set; }
        public int Top { get; set; }
    }

    public class GetEmailsDetailedRequest : Request
    {
        public GetEmailsDetailedRequest(Credentials credentials, string[] emails)
        {
            Type = RequestType.GET_EMAILS_DETAILED;
            Credentials = credentials;
            Emails = emails;
        }

        public Credentials Credentials { get; set; }
        public string[] Emails { get; set; }
    }

    public class SendEmailRequest : Request
    {
        public SendEmailRequest(Credentials credentials, Email email)
        {
            Type = RequestType.SEND_EMAIL;
            Credentials = credentials;
            Email = email;
        }

        public Credentials Credentials { get; set; }
        public Email Email { get; set; }
    }

    public class DeleteEmailRequest : Request
    {
        public DeleteEmailRequest(Credentials credentials, string email)
        {
            Type = RequestType.DELETE_EMAIL;
            Credentials = credentials;
            Email = email;
        }

        public Credentials Credentials { get; set; }
        public string Email { get; set; }
    }
}
