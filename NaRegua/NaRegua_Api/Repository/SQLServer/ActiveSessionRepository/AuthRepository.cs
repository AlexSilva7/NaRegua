namespace NaRegua_Api.Repository.SQLServer.ActiveSessionRepository
{
    public class AuthRepository : AuthRepositorySQLServer
    {
        protected override string SELECT_CREDENTIALS_USER =>
            "SELECT DISTINCT * FROM USERS WHERE USERNAME = @USERNAME";
    }
}
