using NaRegua_API.Common.Contracts;
using NaRegua_API.Models.Auth;
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

        public Task<CreateUserResult> CreateUserAsync(User user)
        {
            if (ChecksIfIsNullProperty(user))
            {
                return Task.FromResult(new CreateUserResult
                {
                    Message = "User cannot be registered, incomplete fields",
                    Success = false
                });
            }

            user.IsCustomer = true;
            user.Password = Criptograph.HashPass(user.Password);

            users.Add(user);

            return Task.FromResult(new CreateUserResult
            {
                Message = "User registered successfully",
                Success = true
            });
        }

        public Task<GetUserResult> GetUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        private bool ChecksIfIsNullProperty(User user)
        {
            var properties = user.GetType().GetProperties();
            foreach (var property in properties)
            {
                if(property.PropertyType == typeof(string))
                {
                    var value = property.GetValue(user);

                    if (value == null) return true;
                }
            }
           
            return false;
        }
    }
}
