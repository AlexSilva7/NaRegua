namespace NaRegua_Api.Repository.SQLServer.ActiveSessionRepository
{
    public class SaloonRepository : SaloonRepositorySQLServer
    {
        protected override string SELECT_SALOONS => "SELECT DISTINCT * FROM SALOONS";

        protected override string SELECT_SALOON_BY_CODE => "SELECT DISTINCT * FROM SALOONS WHERE CODE = @CODE";
    }
}
