using NaRegua_API.Common.Contracts;
using NaRegua_API.Common.Validations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class UserProviderFake : IUserProvider
    {
        public static List<User> users;

        public UserProviderFake()
        {
            users = new List<User>();
        }

        public Task<GenericResult> CreateUserAsync(User user)
        {
            if (Validations.ChecksIfIsNullProperty(user))
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "User cannot be registered, incomplete fields",
                    Success = false
                });
            }

            user.Password = Criptograph.HashPass(user.Password);

            users.Add(user);

            return Task.FromResult(new GenericResult
            {
                Message = "User registered successfully",
                Success = true
            });
        }
    }
}
