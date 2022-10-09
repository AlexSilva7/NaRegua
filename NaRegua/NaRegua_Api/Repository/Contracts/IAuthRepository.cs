using NaRegua_Api.Models.Users;

namespace NaRegua_Api.Repository.Contracts
{
    public interface IAuthRepository
    {
        Task<User> LoginCredentialsVerification(string username, string password);
    }
}
