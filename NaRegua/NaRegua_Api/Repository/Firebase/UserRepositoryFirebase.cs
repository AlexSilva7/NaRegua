using NaRegua_Api.Database;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Models.Users;
using NaRegua_Api.Repository.Contracts;

namespace NaRegua_Api.Repository.Firebase
{
    public class UserRepositoryFirebase : FirebaseDatabase, IUserRepository
    {
        public Task AddSaloonAsFavorite(string userDocument, string saloonCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckIfItIsAlreadyInTheFavoritesList(string userDocument, string saloonCode)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSalonFromFavorites(string document, string saloonCode)
        {
            throw new NotImplementedException();
        }

        public Task InsertScheduleAppointment(string documentUser, string nameUser, string phoneUser, DateTime dateTime, string documentProfessional)
        {
            throw new NotImplementedException();
        }

        public async Task InsertUser(User user)
        {
            try
            {
                await ExecuteNonQuery("insert",
                    new Dictionary<string, object>
                    {
                        {"collection", "users"},
                        {"documentFirebase", user.Username},
                        {"content", user }
                    });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<IEnumerable<Scheduling>?> SelectAppointmentsOfUser(string document)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Saloon>?> SelectUserFavoriteSaloons(string document)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifySaloon(string saloonCode)
        {
            throw new NotImplementedException();
        }
    }
}
