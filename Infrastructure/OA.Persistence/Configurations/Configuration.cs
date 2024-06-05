using OA.Persistence.Settings;
using Microsoft.Extensions.Configuration;

namespace OA.Persistence.Configurations
{ 
    static class Configuration
    {
        static public MongoDBSettings ConnectionString()
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../BvysAPI.API"));
            configurationManager.AddJsonFile("appsettings.json");
            return configurationManager.GetSection("MongoDBSettings").Get<MongoDBSettings>();
        }
    }
}
