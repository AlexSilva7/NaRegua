using StackExchange.Redis;

namespace NaRegua_Api.RedisService
{
    public interface IRedisService
    {
        IDatabase GetDatabase();
        void SetString(string key, string value, TimeSpan? expiry = null);
        string GetString(string key);
    }
}
