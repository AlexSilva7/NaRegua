using NaRegua_API.Common.Contracts;
using NaRegua_API.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class AuthProviderFake : IAuthProvider
    {
        public static List<LoggedUsers> loggedUsers;

        public AuthProviderFake()
        {
            loggedUsers = new List<LoggedUsers>();
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

            var user = UserProviderFake.users.Find(x => x.Login == auth.Login);

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

            var token = GetFakeToken();
            loggedUsers.Add(new LoggedUsers
            {
                Name = user.Name,
                Document = user.Document,
                Username = user.Login,
                Token = token,
                ExpireDateTime = DateTime.Now + TimeSpan.FromMinutes(20),
            });

            return Task.FromResult(
                new AuthResult
                {
                    Token = token,
                    TimeExpireToken = DateTime.Now + TimeSpan.FromMinutes(20),
                    Resources = new UserAuthenticatedResult
                    {
                        Name = user.Name,
                        Username = user.Login,
                        Document = user.Document,
                        Email = user.Email,
                        IsCustomer = user.IsCustomer
                    },
                    Message = "Successfully authenticated",
                    Success = true
                });
        }

        private string GetFakeToken()
        {
            StringBuilder myStringBuilder = new StringBuilder("");
            var letters = "AQWSZXCEDVFRVTNHPMJaqzwxevtgminupl1234567890";

            for(var x = 0; x < 500; x++)
            {
                var random = new Random().Next(letters.Length - 1);
                myStringBuilder.Append(letters[random]);
            }

            return myStringBuilder.ToString();
        }
    }

    public class LoggedUsers
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }
}