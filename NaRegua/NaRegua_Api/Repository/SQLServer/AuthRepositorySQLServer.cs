using NaRegua_Api.Database;
using NaRegua_Api.Models.Users;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;

namespace NaRegua_Api.Repository.SQLServer
{
    public abstract class AuthRepositorySQLServer : SQLServerDatabase, IAuthRepository
    {
        protected abstract string SELECT_CREDENTIALS_USER { get; }
        
        public async Task<User> LoginCredentialsVerification(string username, string password)
        {
            var infosUser = await ExecuteReader(SELECT_CREDENTIALS_USER,
                new Dictionary<string, object>
                {
                    {"@USERNAME", username }
                });

            if (!infosUser.Any())
            {
                throw new UserNotFoundException();
            }

            var user = new User
            {
                Name = infosUser[0].ToString(),
                Document = infosUser[1].ToString(),
                Email = infosUser[2].ToString(),
                Phone = infosUser[3].ToString(),
                Username = infosUser[4].ToString(),
                Password = infosUser[5].ToString(),
                IsCustomer = bool.Parse(infosUser[6].ToString()),
            };

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
