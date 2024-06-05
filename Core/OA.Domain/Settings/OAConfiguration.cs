using Microsoft.Extensions.Configuration;

namespace OA.Domain.Settings
{
    public class OAConfiguration
    {
        public static OaSettings GetSettings()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfigurationRoot configuration = configurationBuilder.Build();

            // Retrieve the section
            var oaSettingsSection = configuration.GetSection("OaSettings");
            OaSettings oaSettings = new OaSettings();
            oaSettingsSection.Bind(oaSettings);

            return oaSettings;
        }
    }
}
