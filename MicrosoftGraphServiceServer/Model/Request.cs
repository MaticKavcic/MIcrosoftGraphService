namespace MicrosoftGraphService.Model
{
    public enum RequestType
    {
        NONE = 0,
        CONFIG = 1,
        GET_EMAILS = 2,
        GET_EMAILS_DETAILED = 3,
        SEND_EMAIL = 4,
        DELETE_EMAIL = 5
    }

    class Request
    {
        public Request() {
            Type = RequestType.NONE;
        }

        public RequestType Type { get; set; }
    }

    class ConfigRequest : Request
    {
        public ConfigRequest(string email, string secret, string clientId, string tenantId)
        {
            Type = RequestType.CONFIG;
            Email = email;
            Secret = secret;
            ClientId = clientId;
            TenantId = tenantId;
        }

        public string Email { get; set; }
        public string Secret { get; set; }
        public string ClientId { get; set; }
        public string TenantId { get; set; }
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
        public SendEmailRequest(Email email)
        {
            Type = RequestType.SEND_EMAIL;
            Email = email;
        }

        public Email Email { get; set; }
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
