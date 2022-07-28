using NaRegua_API.Common.Contracts;
using NaRegua_API.Configurations;
using NaRegua_API.Models.Auth;
using System;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class AuthProviderFake : IAuthProvider
    {
        readonly ITokenProvider _tokenProvider;

        public AuthProviderFake(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public Task<AuthResult> SignAsync(Auth auth)
        {
            if(String.IsNullOrWhiteSpace(auth.Login) || String.IsNullOrWhiteSpace(auth.Password))
            {
                return Task.FromResult(new AuthResult
                {
                    Token = "",
                    Resources = null,
                    Message = "Unable to login, incomplete fields",
                    Success = false
                });
            }

            if (UserProviderFake.users == null) return Task.FromResult(
                new AuthResult
                {
                    Token = "",
                    Resources = null,
                    Message = "User not found",
                    Success = false
                });

            var user = UserProviderFake.users.Find(x => x.Username == auth.Login);

            if(user == null) return Task.FromResult(
                new AuthResult
                {
                    Token = "",
                    Resources = null,
                    Message = "User not found",
                    Success = false
                }); 

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