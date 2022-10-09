using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Models.Auth;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;

namespace NaRegua_Api.Providers.Implementations
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IAuthRepository _database;
        private readonly ITokenProvider _tokenProvider;

        public AuthProvider(ITokenProvider tokenProvider, IAuthRepository authRepository)
        {
            _database = authRepository;
            _tokenProvider = tokenProvider;
        }

        public async Task<AuthResult> SignAsync(Auth auth)
        {
            try
            {
                var user = await _database.LoginCredentialsVerification(auth.Username, Criptograph.HashPass(auth.Password));
                var token = _tokenProvider.BuildToken(user);

                return new AuthResult
                {
                    Token = token,
                    TimeExpireToken = DateTime.Now.AddMinutes(AppSettings.ExpiryDurationMinutes),
                    Resources = new UserAuthenticatedResult
                    {
                        Name = user.Name,
                        Username = user.Username,
                        Document = user.Document,
                        Phone = user.Phone,
                        Email = user.Email,
                        IsCustomer = user.IsCustomer
                    },
                    Success = true
                };

            }
            catch (Exception ex)
            {
                if (ex is UserNotFoundException || ex is IncorretPasswordException)
                {
                    return new AuthResult
                    {
                        Message = ex.Message,
                        Success = false
                    };
                }

                throw new Exception(ex.Message);
            }
        }
    }
}
