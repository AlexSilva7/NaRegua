using NaRegua_Api.Database;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Models.Users;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;
using Newtonsoft.Json;

namespace NaRegua_Api.Repository.Firebase
{
    public class UserRepositoryFirebase : FirebaseDatabase, IUserRepository
    {
        private const string COLLECTION = "users";

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
                //verifica se já tem um usuario com este login cadastrado
                var verifyUser = await ExecuteReader(null,
                new Dictionary<string, object>
                {
                    {"collection", COLLECTION},
                    {"username", user.Username }
                });

                if (verifyUser.Any())
                {
                    throw new PrimaryKeyException("User already registered with this username");
                }

                await ExecuteNonQuery("insert",
                    new Dictionary<string, object>
                    {
                        {"collection", COLLECTION},
                        {"documentFirebase", user.Document},
                        {"content", user }
                    });
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("AlreadyExists"))
                {
                    throw new PrimaryKeyException("User already registered with this Document");
                }

                throw ex;
            }
        }

        public async Task<IEnumerable<Scheduling>?> SelectAppointmentsOfUser(string document)
        {
            var schedulings = await ExecuteReader(null,
                new Dictionary<string, object>
                {
                    {"collection", new List<string> {"users", "scheduling"}},
                    {"documentFirebase", document}
                });

            if (!schedulings.Any()) return null;

            var scheduleList = new List<Scheduling?>();

            foreach (var scheduling in schedulings)
            {
                var obj = JsonConvert.DeserializeObject<Scheduling>(scheduling.ToString());
                scheduleList.Add(obj);
            }

            return scheduleList.Distinct();
        }

        public async Task<IEnumerable<Saloon>?> SelectUserFavoriteSaloons(string document)
        {
            var saloons = await ExecuteReader(null,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", document }
                });

            if (!saloons.Any()) return null;

            var saloonsList = new List<Saloon>();
            var count = saloons.Count;

            for (var x = 0; x < (count / 4); x++)
            {
                var saloon = new Saloon
                {
                    SaloonCode = saloons[0].ToString(),
                    Address = saloons[1].ToString(),
                    Name = saloons[2].ToString(),
                    Contact = saloons[3].ToString()
                };

                saloonsList.Add(saloon);
                saloons.RemoveRange(0, 4);
            }

            return saloonsList.Distinct();
        }

        public Task<bool> VerifySaloon(string saloonCode)
        {
            throw new NotImplementedException();
        }
    }
}
