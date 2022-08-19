using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using System.Security.Principal;

namespace NaRegua_Api.Common.Contracts
{
    public interface IUserProvider
    {
        IEnumerable<User> GetUsersList();
        Task<GenericResult> ScheduleAppointmentAsync(IPrincipal user, DateTime dateTime, string document);
        Task<GenericResult> CreateUserAsync(User user);
        Task<SchedulingResult> GetAppointmentAsync(IPrincipal user);
        Task<GenericResult> AddUserSalonAsFavoriteAsync(IPrincipal user, string saloonCode);
    }

    public class SchedulingResult
    {
        public IEnumerable<Scheduling> Resource { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
