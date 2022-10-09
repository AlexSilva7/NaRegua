using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Models.Users;

namespace NaRegua_Api.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<bool> VerifySaloon(string saloonCode);
        Task<bool> CheckIfItIsAlreadyInTheFavoritesList(string userDocument, string saloonCode);
        Task InsertUser(User user);
        Task<IEnumerable<Scheduling>?> SelectAppointmentsOfUser(string document);
        Task<IEnumerable<Saloon>?> SelectUserFavoriteSaloons(string document);
        Task DeleteSalonFromFavorites(string document, string saloonCode);
        Task InsertScheduleAppointment(string documentUser, DateTime dateTime, string documentProfessional);
        Task AddSaloonAsFavorite(string userDocument, string saloonCode);
    }
}
