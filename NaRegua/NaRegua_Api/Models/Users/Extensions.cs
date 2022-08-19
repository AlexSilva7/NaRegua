using NaRegua_Api.Common.Contracts;
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
                Name = input.Name,
                Document = input.Document,
                Phone = input.Phone,
                Email = input.Email,
                Username = input.Username,
                Password = input.Password
            };
        }

        public static SchedulingResponse ToResponse(this SchedulingResult input)
        {
            return new SchedulingResponse
            {
                Resource = input.Resource
            };
        }
    }
}
