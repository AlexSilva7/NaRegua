using NaRegua_Api.Configurations;
using System.Data.SqlClient;

namespace NaRegua_Api.Database
{
    public class SQLServerDatabase : IDatabase
    {
        protected virtual SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(AppSettings.ConnectionString);

            connection.Open();

            return connection;
        }

        public async Task<List<object>> ExecuteReader(string query, Dictionary<string, object> parameters)
        {
            var response = new List<object>();

            try
            {
                using (var conn = CreateConnection())
                using (var cmd = CreateCommand(conn, query, parameters))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        for (var x = 0; x < reader.FieldCount; x++)
                        {
                            var data = reader.GetValue(x);
                            response.Add(data);
                        }
                    }
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ExecuteNonQuery(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var conn = CreateConnection())
                using (var cmd = CreateCommand(conn, query, parameters))
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static SqlCommand CreateCommand(SqlConnection connection, string query, Dictionary<string, object> parameters)
        {
            var command = connection.CreateCommand();

            command.CommandText = query;

            if (parameters == null) { return command; }

            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Key, item.Value);
            }

            return command;
        }
    }
}
