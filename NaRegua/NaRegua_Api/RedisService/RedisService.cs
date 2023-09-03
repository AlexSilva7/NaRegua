using StackExchange.Redis;

namespace NaRegua_Api.RedisService
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redisConnection;

        public RedisService(string connectionString)
        {
            _redisConnection = ConnectionMultiplexer.Connect(connectionString);
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

        public string GetString(string key)
        {
            var database = GetDatabase();
            return database.StringGet(key);
        }
    }
}
