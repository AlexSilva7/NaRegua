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
            public string Phone { get; set; }
            public string Email { get; set; }
            public string SaloonCode { get; set; }
            public bool IsCustomer = false;
        }

        public class ProfessionalAvailabilityResponse
        {
            public string Document { get; set; }
            public IEnumerable<DateTime> Resources { get; set; }
        }

        public class AppointmentsListResponse
        {
            public IEnumerable<AppointmentsResponse> Resources { get; set; }
        }

        public class AppointmentsResponse
        {
            public string CustomerName { get; set; }
            public string CustomerDocument { get; set; }
            public string CustomerPhone { get; set; }
            public DateTime Scheduling { get; set; }
        }

        public class EvaluationAverageResponse
        {
            public double Average { get; set; }
        }
    }
}
