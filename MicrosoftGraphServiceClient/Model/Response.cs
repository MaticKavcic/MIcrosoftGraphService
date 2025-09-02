namespace MicrosoftGraphService.Model
{
    public enum ResponseType
    {
        OK = 0,
        ERR = 1
    }

    class Response
    {
        public Response()
        {
            Type = ResponseType.OK;
        }

        public ResponseType Type { get; set; }
    }

    class OkResponse : Response
    {
        public OkResponse()
        {
            Type = ResponseType.OK;
        }
    }

    class ErrorResponse : Response
    {
        public ErrorResponse()
        {
            Type = ResponseType.ERR;
            Errors = [];
        }

        public ErrorResponse(string error)
        {
            Type = ResponseType.ERR;
            Errors = [
                error
            ];
        }

        public ErrorResponse(string[] errors)
        {
            Type = ResponseType.ERR;
            Errors = errors;
        }

        public string[] Errors { get; set; }
    }

    class GetEmailsResponse : Response
    {
        public GetEmailsResponse(Email[] emails)
        {
            Type = ResponseType.OK;
            Emails = emails;
        }

        public Email[] Emails { get; set; }
    }

    class GetEmailsDetailedResponse : Response
    {
        public GetEmailsDetailedResponse(Email[] emails)
        {
            Type = ResponseType.OK;
            Emails = emails;
        }

        public Email[] Emails { get; set; }
    }
}
