using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using System.Security.Principal;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Repository;

namespace NaRegua_Api.Providers.Implementations
{
    public class UserProvider : IUserProvider
    {
        UserRepository _database;

        public UserProvider()
        {
            _database = new UserRepository();
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
                Success = true,
                Resources = schedulings
            };
        }

        public Task<ListSaloonsResult> GetUserFavoriteSaloonsAsync(IPrincipal user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersList()
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult> RemoveSalonFromFavoritesAsync(IPrincipal user, string saloonCode)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult> ScheduleAppointmentAsync(IPrincipal user, DateTime dateTime, string document)
        {
            throw new NotImplementedException();
        }
    }
}
