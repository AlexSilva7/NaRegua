﻿using NaRegua_Api.Models.Auth;

namespace NaRegua_Api.Common.Contracts
{
    public interface IAuthProvider
    {
        Task<AuthResult> SignAsync(Auth auth);
    }

    public class AuthResult
    {
        public string Token { get; set; }
        public DateTime TimeExpireToken { get; set; }
        public UserAuthenticatedResult Resources { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class UserAuthenticatedResult
    {  
        public string Name { get; set; }
        public string Username { get; set; }
        public string Document { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsCustomer { get; set; }
    }
}
