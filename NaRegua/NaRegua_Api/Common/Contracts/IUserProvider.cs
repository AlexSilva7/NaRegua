using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using System.Security.Principal;

namespace NaRegua_Api.Common.Contracts
{
    public interface IUserProvider
    {
        IEnumerable<User> GetUsersList();
        Task<GenericResult> CreateUserAsync(User user);
        Task<GenericResult> ScheduleAppointmentAsync(IPrincipal user, DateTime dateTime, string document);
        Task<SchedulingResult> GetAppointmentAsync(IPrincipal user);
        Task<ListSaloonsResult> GetUserFavoriteSaloonsAsync(IPrincipal user);
        Task<GenericResult> AddUserSalonAsFavoriteAsync(IPrincipal user, string saloonCode);
        Task<GenericResult> RemoveSalonFromFavoritesAsync(IPrincipal user, string saloonCode);
    }

    public class SchedulingResult
    {
        public IEnumerable<Scheduling>? Resources { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
