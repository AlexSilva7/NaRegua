using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using System.Security.Principal;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Repository.Exceptions;
using NaRegua_Api.Repository.ActiveSessionRepository;

namespace NaRegua_Api.Providers.Implementations
{
    public class UserProvider : IUserProvider
    {
        private readonly UserRepositorySqlServer _database;

        public UserProvider()
        {
            _database = new UserRepositorySqlServer();
        }

        public async Task<GenericResult> AddUserSalonAsFavoriteAsync(IPrincipal user, string saloonCode)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");

            if(await _database.VerifySaloon(saloonCode)) 
                return new GenericResult { Message = "Saloon not found.", Success = false };

            if (await _database.CheckIfItIsAlreadyInTheFavoritesList(document, saloonCode))
                return new GenericResult { Message = "Saloon is already in the favorites list.", Success = false };

            await _database.AddSaloonAsFavorite(document, saloonCode);

            return new GenericResult { Success = true };
        }

        public async Task<GenericResult> CreateUserAsync(User user)
        {
            await _database.InsertUser(user);
            return new GenericResult { Success = true };
        }

        public async Task<SchedulingResult> GetAppointmentAsync(IPrincipal user)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            var schedulings = await _database.SelectAppointmentsOfUser(document);

            return new SchedulingResult
            {
                Resources = schedulings is not null ? schedulings : new List<Scheduling>(),
                Success = true
            };
        }

        public async Task<ListSaloonsResult> GetUserFavoriteSaloonsAsync(IPrincipal user)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            var favorites = await _database.SelectUserFavoriteSaloons(document);

            return new ListSaloonsResult
            {
                Success = true,
                Resources = favorites is not null ? favorites.Select(x => x.ToResult()) : new List<Saloon>().Select(x => x.ToResult()),
            };
        }

        public IEnumerable<User> GetUsersList()
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResult> RemoveSalonFromFavoritesAsync(IPrincipal user, string saloonCode)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            await _database.DeleteSalonFromFavorites(document, saloonCode);

            return new GenericResult { Success = true };
        }

        public async Task<GenericResult> ScheduleAppointmentAsync(IPrincipal user, DateTime dateTime, string documentProfessional)
        {
            if (dateTime < DateTime.Now)
            {
                return new GenericResult
                {
                    Message = "Invalid datetime",
                    Success = false
                };
            }

            var document = Validations.FindFirstClaimOfType(user, "Document");

            try
            {
                await _database.InsertScheduleAppointment(document, dateTime, documentProfessional);
            }
            catch (CannotScheduleException ex)
            {
                return new GenericResult
                {
                    Message = ex.Message,
                    Success = false
                };
            }

            return new GenericResult
            {
                Success = true
            };
        }
    }
}
