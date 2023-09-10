using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Users;
using System.Security.Principal;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Repository.Exceptions;
using NaRegua_Api.Repository.Contracts;
using System.Security.Claims;

namespace NaRegua_Api.Providers.Implementations
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _database;
        private string GenericMessage = "We were unable to process your request";

        public UserProvider(IUserRepository userRepository)
        {
            _database = userRepository;
        }

        public async Task<GenericResult> AddUserSalonAsFavoriteAsync(IPrincipal user, string saloonCode)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");

            if (await _database.CheckIfItIsAlreadyInTheFavoritesList(document, saloonCode))
                return new GenericResult { Message = "Saloon is already in the favorites list.", Success = false };

            if (!await _database.VerifySaloon(saloonCode)) 
                return new GenericResult { Message = "Saloon not found.", Success = false };

            await _database.AddSaloonAsFavorite(document, saloonCode);

            return new GenericResult { Success = true };
        }

        public Task CheckOpenOrdersAndUpdateUserBalances()
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResult> CreateUserAsync(User user)
        {
            try
            {
                await _database.InsertUser(user);
            }
            catch (PrimaryKeyException ex)
            {
                return new GenericResult { Success = false, Message = ex.Message};
            }

            return new GenericResult { Success = true };
        }

        public Task<GenericResult> DepositFundsAsync(IPrincipal user, DepositInfo depositInfo)
        {
            throw new NotImplementedException();
        }

        public Task<AccountBalanceResult> GetAccountBalanceAsync(IPrincipal user)
        {
            throw new NotImplementedException();
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
            try
            {
                await _database.DeleteSalonFromFavorites(document, saloonCode);
            }
            catch (Exception ex)
            {
                return new GenericResult { Success = false, Message = ex.Message };
            }

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

            var document = user.FindFirstClaimOfType("Document");
            var name = user.FindFirstClaimOfType(ClaimTypes.Name);
            var phone = user.FindFirstClaimOfType("Phone");

            try
            {
                await _database.InsertScheduleAppointment(document, name, phone, dateTime, documentProfessional);
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
