using System;

namespace NaRegua_API.Models.Hairdresser
{
    public class Requests
    {
        public class HairdresserRequest
        {
            public string Name { get; set; }
            public string Document { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string SaloonCode { get; set; }
        }

        public class WorkAvailabilityRequest
        {
            public DateTime Date { get; set; }
            public TimeSpan StartHour { get; set; }
            public TimeSpan EndHour { get; set; }
        }
    }
}
