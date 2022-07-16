using NaRegua_API.Common.Contracts;
using NaRegua_API.Models.Hairdresser;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class HairdresserProviderFake : IHairdresserProvider
    {
        public Task<CreateHairdresserResult> CreateHairdresserAsync(Hairdresser hairdresser)
        {
            throw new System.NotImplementedException();
        }
    }
}
