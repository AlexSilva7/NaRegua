﻿using NaRegua_API.Models.Users;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IUserProvider
    {
        Task<CreateUserResult> CreateUserAsync(User user);
        Task<GetUserResult> GetUserAsync(User user);
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
