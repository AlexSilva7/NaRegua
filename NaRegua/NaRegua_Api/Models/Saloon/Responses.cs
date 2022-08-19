namespace NaRegua_Api.Models.Saloon
{
    public class Responses
    {
        public class ListSaloonsResponse
        {
            public IEnumerable<SaloonResponse> Resources { get; set; }
        }

        public class SaloonResponse
        {
            public string SaloonCode { get; set; }
            public string Address { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
        }
    }
}
