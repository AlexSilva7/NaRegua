using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.Models.Auth;
using static NaRegua_Api.Models.Users.Requests;
using static NaRegua_Api.Models.Users.Responses;

namespace NaRegua_Api.Models.Users
{
    public static class Extensions
    {
        public static User ToDomain(this UserRequest input)
        {
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = input.Name,
                Document = input.Document,
                Phone = input.Phone,
                Email = input.Email,
                Username = input.Username,
                Password = Criptograph.HashPass(input.Password)
            };
        }

        public static SchedulingResponse ToResponse(this SchedulingResult input)
        {
            return new SchedulingResponse
            {
                Resources = input.Resources
            };
        }

        public static DepositInfo ToDomain(this DepositsFundsRequests input)
        {
            return new DepositInfo
            {
                Value = input.Value,
                PaymentType = input.PaymentType,
                CardNumber = input.CardNumber
            };
        }
    }
}
