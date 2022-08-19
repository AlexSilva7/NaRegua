using NaRegua_Api.Models.Users;

namespace NaRegua_Api.Common.Contracts
{
    public interface ITokenProvider
    {
        string BuildToken(User user);
    }
}
