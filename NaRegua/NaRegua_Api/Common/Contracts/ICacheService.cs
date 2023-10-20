using StackExchange.Redis;

namespace NaRegua_Api.Common.Contracts
{
    public interface ICacheService
    {
        IDatabase GetDatabase();
        void SetString(string key, string value, TimeSpan? expiry = null);
        string GetString(string key);
        CacheInfo GetConnectionInfo();
    }
}
