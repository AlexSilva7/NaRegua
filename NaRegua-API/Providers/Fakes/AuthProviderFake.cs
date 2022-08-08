using NaRegua_API.Common.Contracts;
using NaRegua_API.Common.Validations;
using NaRegua_API.Configurations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class AuthProviderFake : IAuthProvider
    {
        readonly ITokenProvider _tokenProvider;
        readonly IUserProvider _userProvider;
        readonly IHairdresserProvider _hairdresserProvider;

        public AuthProviderFake(ITokenProvider tokenProvider, IUserProvider userProvider, IHairdresserProvider hairdresserProvider)
        {
            _tokenProvider = tokenProvider;
            _userProvider = userProvider;
            _hairdresserProvider = hairdresserProvider;
        }

        public Task<AuthResult> SignAsync(Auth auth)
        {
            if(Validations.ChecksIfIsNullProperty(auth))
            {
                return Task.FromResult(new AuthResult
                {
                    Token = "",
                    Resources = null,
                    Message = "Unable to login, incomplete fields",
                    Success = false
                });
            }

            if (_userProvider.GetUsersList().Count() == 0 && _hairdresserProvider.GetHairdressersList().Count() == 0)
            {
                return Task.FromResult(
                new AuthResult
                {
                    Token = "",
                    Resources = null,
                    Message = "User not found",
                    Success = false
                });
            }

            var user = _userProvider.GetUsersList().Where(x => x.Username == auth.Login).FirstOrDefault();

            if (user == null)
            {
                var hairdressers =
                    _hairdresserProvider.GetHairdressersList().Where(x => x.Username == auth.Login).FirstOrDefault();

                if (hairdressers == null)
                {
                    return Task.FromResult(
                    new AuthResult
                    {
                        Token = "",
                        Resources = null,
                        Message = "User not found",
                        Success = false
                    });
                }

                user = new User
                {
                    Name = hairdressers.Name,
                    Username = hairdressers.Username,
                    Password = hairdressers.Password,
                    Document = hairdressers.Document,
                    Email = hairdressers.Email,
                    IsCustomer = hairdressers.IsCustomer
                };
            }

            if(user.Password != Criptograph.HashPass(auth.Password)) return Task.FromResult(
                new AuthResult
                {
                    Token = "",
                    Resources = null,
                    Message = "Incorrect password",
                    Success = false
                });

            var token = _tokenProvider.BuildToken(user);

            return Task.FromResult(
                new AuthResult
                {
                    Token = token,
                    TimeExpireToken = DateTime.UtcNow.AddMinutes(AppSettings.ExpiryDurationMinutes),
                    Resources = new UserAuthenticatedResult
                    {
                        Name = user.Name,
                        Username = user.Username,
                        Document = user.Document,
                        Email = user.Email,
                        IsCustomer = user.IsCustomer
                    },
                    Message = "Successfully authenticated",
                    Success = true
                });
        }
    }
}