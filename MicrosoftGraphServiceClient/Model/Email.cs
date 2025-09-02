namespace MicrosoftGraphService.Model
{
    enum EmailContentType
    {
        TEXT = 0,
        HTML = 1
    }

    /*
    class EmailAttachment
    {
        public string? Id { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string? Name { get; set; }
        public bool? Inline { get; set; }
        public int? Size { get; set; }
        public string? ContentType { get; set; }
        public string? Content { get; set; }
    }
    */

    class Email
    {
        public string? Id { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string? From { get; set; }
        public string[]? Recipients { get; set; }
        public string[]? CCRecipients { get; set; }
        public string[]? BCCRecipients { get; set; }
        public string? Subject { get; set; }
        public EmailContentType ContentType { get; set; }
        public string? Content { get; set; }
        // public EmailAttachment[]? Attachments { get; set; }
    }
}