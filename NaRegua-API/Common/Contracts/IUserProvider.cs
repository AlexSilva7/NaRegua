using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Users;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IUserProvider
    {
        Task<GenericResult> CreateUserAsync(User user);
    }

    public class CreateUserResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class GetUserResult
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool IsCustomer { get; set; }
    }
}
