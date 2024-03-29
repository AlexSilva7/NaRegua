﻿namespace NaRegua_Api.Models.Hairdresser
{
    public class Requests
    {
        public class HairdresserRequest
        {
            public string Name { get; set; }
            public string Document { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string SaloonCode { get; set; }
        }

        public class WorkAvailabilityRequest
        {
            public DateTime StartHour { get; set; }
            public DateTime EndHour { get; set; }
        }

        public class ProfessionalEvaluationRequest
        {
            public string Document { get; set; }
            public double Evaluation { get; set; }
        }
    }
}
