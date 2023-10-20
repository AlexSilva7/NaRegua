using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using StackExchange.Redis;

namespace NaRegua_Api.CacheService
{
    public class RedisService : ICacheService
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

        public CacheInfo GetConnectionInfo()
        {
            try
            {
                if (_redisConnection is not null)
                {
                    return new CacheInfo
                    {
                        CacheProvider = _redisConnection.GetEndPoints()[0].ToString(),
                        IsConnected = _redisConnection.IsConnected,
                    };
                }
            }
            catch { }

            return new CacheInfo
            {
                IsConnected = false,
            };
        }
    }
}
