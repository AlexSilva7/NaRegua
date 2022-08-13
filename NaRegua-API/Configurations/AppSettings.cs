using Microsoft.Extensions.Configuration;

namespace NaRegua_API.Configurations
{
    public class AppSettings
    {
        public static string ConnectionString => throw new System.NotImplementedException();
        public static bool UseFakeProviders;
        public static string JwtKey;
        public static double ExpiryDurationMinutes;

        public static void SetConfig(IConfiguration configuration)
        {
            UseFakeProviders = bool.Parse(configuration.GetSection("Providers").GetSection("UseFakeProviders").Value);
            JwtKey = configuration.GetSection("Jwt").GetSection("Key").Value;
            ExpiryDurationMinutes = double.Parse(configuration.GetSection("Jwt").GetSection("ExpiryDurationMinutes").Value);
        }
    }
}
