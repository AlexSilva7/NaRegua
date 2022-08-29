namespace NaRegua_Api.Database
{
    public interface IDatabase
    {
        Task<List<object>> ExecuteReader(string query, Dictionary<string, object>? parameters);
        Task ExecuteNonQuery(string query, Dictionary<string, object> parameters);
    }
}
