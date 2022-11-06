using NaRegua_Api.Database;
using NaRegua_Api.Models.Users;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;
using Newtonsoft.Json;

namespace NaRegua_Api.Repository.Firebase
{
    public class AuthRepositoryFirebase : FirebaseDatabase, IAuthRepository
    {
        public async Task<User> LoginCredentialsVerification(string username, string password)
        {
            var infosUser = await ExecuteReader(null,
                new Dictionary<string, object>
                {
                    {"collection", "users"},
                    {"documentFirebase", username},
                    {"Username", username }
                });

            if (!infosUser.Any())
            {
                throw new UserNotFoundException();
            }

            var user = JsonConvert.DeserializeObject<User>(infosUser?[0].ToString());

            if (user.Password != password)
            {
                throw new IncorretPasswordException();
            }
            else
            {
                return user;
            }
        }
    }
}
