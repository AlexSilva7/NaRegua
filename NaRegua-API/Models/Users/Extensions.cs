using NaRegua_API.Common.Contracts;
using static NaRegua_API.Models.Users.Requests;
using static NaRegua_API.Models.Users.Responses;

namespace NaRegua_API.Models.Users
{
    public static class Extensions
    {
        public static CreateUserResponse ToResponse(this CreateUserResult input)
        {
            return new CreateUserResponse
            {
                Success = input.Success
            };
        }

        public static User ToDomain(this UserRequest input)
        {
            return new User
            {
                Name = input.Name,
                Document = input.Document,
                Email = input.Email,
                Login = input.Login,
                Password = input.Password
            };
        }
    }
}
