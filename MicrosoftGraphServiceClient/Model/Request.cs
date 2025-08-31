using Microsoft.Graph.Models;

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

    class Request
    {
        public Request() {
            Type = RequestType.NONE;
        }

        public RequestType Type { get; set; }
    }

    class GetEmailsRequest : Request
    {
        public GetEmailsRequest(int top)
        {
            Type = RequestType.GET_EMAILS;
            Top = top;
        }

        public int Top { get; set; }
    }

    class GetEmailsDetailedRequest : Request
    {
        public GetEmailsDetailedRequest(string[] emails)
        {
            Type = RequestType.GET_EMAILS_DETAILED;
            Emails = emails;
        }

        public string[] Emails { get; set; }
    }

    class SendEmailRequest : Request
    {
        public SendEmailRequest(Message email)
        {
            Type = RequestType.SEND_EMAIL;
            Email = email;
        }

        public Message Email { get; set; }
    }

    class DeleteEmailRequest : Request
    {
        public DeleteEmailRequest(string email)
        {
            Type = RequestType.DELETE_EMAIL;
            Email = email;
        }

        public string Email { get; set; }
    }
}
