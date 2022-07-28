using Microsoft.Extensions.Configuration;

namespace NaRegua_API.Configurations
{
    public class AppSettings
    {
        public static bool UseFakeProviders;
        public static string ConnectionString;
        public static string JwtKey;
        public static double ExpiryDurationMinutes;

        public AppSettings(IConfiguration configuration)
        {
            UseFakeProviders = bool.Parse(configuration.GetSection("Providers").GetSection("UseFakeProviders").Value);
            //ConnectionString = configuration.GetSection("Providers").GetSection("UseFakeProviders").Value;
            JwtKey = configuration.GetSection("Jwt").GetSection("Key").Value;
            ExpiryDurationMinutes = double.Parse(configuration.GetSection("Jwt").GetSection("ExpiryDurationMinutes").Value);
        }
        
    }
}
