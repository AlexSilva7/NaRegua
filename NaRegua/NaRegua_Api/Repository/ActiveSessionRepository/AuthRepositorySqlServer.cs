namespace NaRegua_Api.Repository.ActiveSessionRepository
{
    public class AuthRepositorySqlServer : AuthRepository
    {
        protected override string SELECT_CREDENTIALS_USER =>
            "SELECT DISTINCT * FROM USERS WHERE USERNAME = @USERNAME";
    }
}
