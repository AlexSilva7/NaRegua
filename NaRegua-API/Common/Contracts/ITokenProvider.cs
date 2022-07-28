using NaRegua_API.Models.Users;

namespace NaRegua_API.Common.Contracts
{
    public interface ITokenProvider
    {
        string BuildToken(User user);
    }
}
