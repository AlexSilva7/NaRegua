using NaRegua_API.Common.Contracts;
using static NaRegua_API.Models.Hairdresser.Requests;
using static NaRegua_API.Models.Hairdresser.Responses;

namespace NaRegua_API.Models.Hairdresser
{
    public static class Extensions
    {
        public static CreateHairdresserResponse ToResponse(this CreateHairdresserResult input)
        {
            return new CreateHairdresserResponse
            {
                Success = input.Success
            };
        }

        public static Hairdresser ToDomain(this HairdresserRequest input)
        {
            return new Hairdresser
            {
                Name = input.Name,
                Document = input.Document,
                Email = input.Email,
                Login = input.Login,
                Password = input.Password,
                SaloonCode = input.SaloonCode
            };
        }
    }
}
