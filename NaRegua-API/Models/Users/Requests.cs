using System;

namespace NaRegua_API.Models.Users
{
    public class Requests
    {
        public class UserRequest
        {
            public string Name { get; set; }
            public string Document { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class ScheduleAppointmentRequest
        {
            public string DocumentProfessional { get; set; }
            public DateTime DateTime { get; set; }
        }
    }
}
