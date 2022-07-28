using System;

namespace NaRegua_API.Models.Token
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
