using static NaRegua_API.Models.Hairdresser.Requests;

namespace NaRegua_API.Models.Hairdresser
{
    public static class Extensions
    {
        public static Hairdresser ToDomain(this HairdresserRequest input)
        {
            return new Hairdresser
            {
                Name = input.Name,
                Document = input.Document,
                Email = input.Email,
                Username = input.Username,
                Password = input.Password,
                SaloonCode = input.SaloonCode
            };
        }

        public static WorkAvailability ToDomain(this WorkAvailabilityRequest input)
        {
            return new WorkAvailability
            {
                Date = input.Date,
                StartHour = input.StartHour,
                EndHour = input.EndHour
            };
        }
    }
}
