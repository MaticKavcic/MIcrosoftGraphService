namespace MicrosoftGraphService.Model
{
    class Credentials
    {
        public Credentials(string email, string secret, string clientId, string tenantId)
        {
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
}