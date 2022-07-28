using NaRegua_API.Models.Users;

namespace NaRegua_API.Common.Contracts
{
    public interface ITokenProvider
    {
        string BuildToken(string key, string issuer, string audience, User user);
        bool ValidateToken(string key, string issuer, string audience, string token);
    }
}
