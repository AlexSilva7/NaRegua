namespace NaRegua_Api.Configurations
{
    public class AppSettings
    {
        public static string ConnectionString => throw new System.NotImplementedException();
        public static bool UseFakeProviders;
        public static string JwtKey;
        public static double ExpiryDurationMinutes;

        public static void SetConfig(IConfiguration configuration)
        {
            UseFakeProviders = bool.Parse(configuration.GetSection("Providers")["UseFakeProviders"]);
            JwtKey = configuration.GetSection("Jwt")["Key"];
            ExpiryDurationMinutes = double.Parse(configuration.GetSection("Jwt")["ExpiryDurationMinutes"]);
        }
    }
}
