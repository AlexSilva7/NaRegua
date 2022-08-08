using System;
using System.Collections.Generic;

namespace NaRegua_API.Models.Hairdresser
{
    public class Responses
    {
        public class ListHairdresserResponse
        {
            public IEnumerable<HairdresserResponse> Resources { get; set; }
        }

        public class HairdresserResponse
        {
            public string Name { get; set; }
            public string Document { get; set; }
            public string Email { get; set; }
            public string SaloonCode { get; set; }
            public bool IsCustomer = false;
        }

        public class ProfessionalAvailabilityResponse
        {
            public string Document { get; set; }
            public IEnumerable<DateTime> Resources { get; set; }
        }
    }
}
