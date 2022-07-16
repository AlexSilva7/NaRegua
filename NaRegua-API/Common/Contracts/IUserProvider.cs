using NaRegua_API.Models.Users;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IUserProvider
    {
        Task<CreateUserResult> CreateUserAsync(User user);
    }

    public class CreateUserResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
