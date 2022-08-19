using NaRegua_Api.Models.Saloon;

namespace NaRegua_Api.Common.Contracts
{
    public interface ISaloonProvider
    {
        Task<ListSaloonsResult> GetSaloonsAsync();
        Saloon GetSaloon(string saloonCode);
    }

    public class ListSaloonsResult
    {
        public IEnumerable<SaloonResult> Resources { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class SaloonResult
    {
        public string SaloonCode { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
    }
}
