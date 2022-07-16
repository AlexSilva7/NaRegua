using System;

namespace NaRegua_API.Models.Auth
{
    public class Responses
    {
        public class AuthResponse
        {
            public string Token { get; set; }
            public DateTime TimeExpireToken { get; set; }
            public UserAuthenticated Resources { get; set; }
        }

        public class UserAuthenticated
        {
            public string Name { get; set; }
            public string Username { get; set; }
            public string Document { get; set; }
            public string Email { get; set; }
            public bool IsCustomer { get; set; }
        }
    }
}
