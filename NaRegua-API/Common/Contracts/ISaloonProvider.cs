using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface ISaloonProvider
    {
        Task<ListSaloonsResult> GetSaloonsAsync();
    }

    public class ListSaloonsResult
    {
        public IEnumerable<SaloonsResult> Resources { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class SaloonsResult
    {
        public string SaloonCode { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
    }
}
