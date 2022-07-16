using NaRegua_API.Models.Hairdresser;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IHairdresserProvider
    {
        Task<CreateHairdresserResult> CreateHairdresserAsync(Hairdresser hairdresser);
    }

    public class CreateHairdresserResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
