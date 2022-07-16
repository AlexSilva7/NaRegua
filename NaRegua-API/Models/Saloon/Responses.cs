using System.Collections.Generic;

namespace NaRegua_API.Models.Saloon
{
    public class Responses
    {
        public class ListSaloonsResponse
        {
            public IEnumerable<SaloonsResponse> Resources { get; set; }
        }

        public class SaloonsResponse
        {
            public string SaloonCode { get; set; }
            public string Address { get; set; }
            public string Name { get; set; }
            public string Contact { get; set; }
        }
    }
}
