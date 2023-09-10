using NaRegua_Api.Configurations;
using StackExchange.Redis;

namespace NaRegua_Api.RedisService
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redisConnection;
        public RedisService()
        {
            _redisConnection = ConnectionMultiplexer.Connect(AppSettings.Redis);
        }
        public IDatabase GetDatabase()
        {
            return _redisConnection.GetDatabase();
        }

        public void SetString(string key, string value, TimeSpan? expiry = null)
        {
            var database = GetDatabase();
            database.StringSet(key, value, expiry);
        }

        public string? GetString(string key)
        {
            try
            {
                return GetDatabase().StringGet(key);
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                return x;
            }
        }
    }
}
