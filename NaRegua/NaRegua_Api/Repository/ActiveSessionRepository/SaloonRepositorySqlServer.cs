namespace NaRegua_Api.Repository.ActiveSessionRepository
{
    public class SaloonRepositorySqlServer : SaloonRepository
    {
        protected override string SELECT_SALOONS => "SELECT DISTINCT * FROM SALOONS";

        protected override string SELECT_SALOON_BY_CODE => "SELECT DISTINCT * FROM SALOONS WHERE CODE = @CODE";
    }
}
