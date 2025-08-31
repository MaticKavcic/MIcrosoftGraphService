using Microsoft.Extensions.Configuration;

namespace MicrosoftGraphServiceServer
{
    class Settings
    {
        public string? Email { get; set; }
        public string? Secret { get; set; }
        public string? ClientId { get; set; }
        public string? TenantId { get; set; }

        public static Settings LoadSettings()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("settings.json", optional: false)
                .Build();

            Settings? settings = config.GetRequiredSection("settings").Get<Settings>();
            if (settings == null)
            {
                throw new Exception("Failed to load settings.json.");
            }

            return settings;
        }
    }
}
