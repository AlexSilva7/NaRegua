using Microsoft.IdentityModel.Tokens;
using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Models.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NaRegua_Api.Providers.Implementations
{
    public class TokenProvider : ITokenProvider
    {
        public string BuildToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Phone", user.Phone),
                    new Claim("Document", user.Document),
                    new Claim("Username", user.Username),
                    new Claim("IsCustomer", user.IsCustomer.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(AppSettings.ExpiryDurationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
