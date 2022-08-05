using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Hairdresser;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IHairdresserProvider
    {
        Task<GenericResult> CreateHairdresserAsync(Hairdresser hairdresser);
        Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability);
    }
}
