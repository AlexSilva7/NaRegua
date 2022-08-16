using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Users;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IUserProvider
    {
        Task<GenericResult> ScheduleAppointmentAsync(IPrincipal user, DateTime dateTime, string document);
        Task<GenericResult> CreateUserAsync(User user);
        IEnumerable<User> GetUsersList();
        Task<SchedulingResult> GetAppointmentAsync(IPrincipal user);
    }

    public class SchedulingResult
    {
        public IEnumerable<Scheduling> Resource { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
