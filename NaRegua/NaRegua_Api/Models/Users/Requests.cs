using NaRegua_Api.Common.Enums;

namespace NaRegua_Api.Models.Users
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

        public class AddFavoriteRequest
        {
            public string saloonCode { get; set; }
        }

        public class DepositsFundsRequests
        {
            public decimal Value { get; set; }
            public PaymentType PaymentType { get; set; }

            public string? CardNumber { get; set; } = string.Empty;
        }
    }
}
