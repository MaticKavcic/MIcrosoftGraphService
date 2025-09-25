namespace MicrosoftGraphService.Model
{
    public enum ResponseType
    {
        OK = 0,
        ERR = 1
    }

    public class Response
    {
        public Response()
        {
            Type = ResponseType.OK;
        }

        public ResponseType Type { get; set; }
    }

    public class OkResponse : Response
    {
        public OkResponse()
        {
            Type = ResponseType.OK;
        }
    }

    public class ErrorResponse : Response
    {
        public ErrorResponse(string error)
        {
            Type = ResponseType.ERR;
            Error = error;
        }

        public string Error { get; set; }
    }

    public class GetEmailsResponse : Response
    {
        public GetEmailsResponse(Email[] emails)
        {
            Type = ResponseType.OK;
            Emails = emails;
        }

        public Email[] Emails { get; set; }
    }

    public class GetEmailsDetailedResponse : Response
    {
        public GetEmailsDetailedResponse(Email[] emails)
        {
            Type = ResponseType.OK;
            Emails = emails;
        }

        public Email[] Emails { get; set; }
    }
}
