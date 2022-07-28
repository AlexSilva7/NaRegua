using Microsoft.IdentityModel.Tokens;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Configurations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Users;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NaRegua_API.Providers.Implementations.V1.Token
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
