using NaRegua_Api.Models.Saloon;

namespace NaRegua_Api.Repository.Contracts
{
    public interface ISaloonRepository
    {
        Task<IEnumerable<Saloon>?> SelectAllSaloons();
    }
}
