using NaRegua_API.Common.Contracts;
using static NaRegua_API.Models.Auth.Requests;
using static NaRegua_API.Models.Auth.Responses;

namespace NaRegua_API.Models.Auth
{
    public static class Extensions
    {
        public static Auth ToDomain(this AuthRequest input)
        {
            return new Auth
            {
                Login = input.Login,
                Password = input.Password
            };
        }

        public static AuthResponse ToResponse(this AuthResult input)
        {
            return new AuthResponse
            {
                Token = input.Token,
                TimeExpireToken = input.TimeExpireToken,
                Resources = new UserAuthenticated
                {
                    Name = input.Resources.Name,
                    Username = input.Resources.Username,
                    Document = input.Resources.Document,
                    Email = input.Resources.Email,
                    IsCustomer = input.Resources.IsCustomer
                }
            };
        }
    }
}
