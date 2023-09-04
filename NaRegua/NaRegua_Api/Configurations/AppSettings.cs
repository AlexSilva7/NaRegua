﻿namespace NaRegua_Api.Configurations
{
    public class AppSettings
    {
        public static string ConnectionString;
        public static bool UseFakeProviders;
        public static string JwtKey;
        public static double ExpiryDurationMinutes;
        public static string Database;
        public static string Redis;

        public static void SetConfig(IConfiguration configuration)
        {
            UseFakeProviders = bool.Parse(configuration.GetSection("Providers")["UseFakeProviders"]);
            JwtKey = configuration.GetSection("Jwt")["Key"];
            ExpiryDurationMinutes = double.Parse(configuration.GetSection("Jwt")["ExpiryDurationMinutes"]);
            ConnectionString = configuration.GetSection("ConnectionStrings")["SqlServer"];
            Database = configuration.GetSection("Database")["UseDatabase"];
            Redis = configuration.GetSection("ConnectionStrings")["Redis"];
        }
    }
}
