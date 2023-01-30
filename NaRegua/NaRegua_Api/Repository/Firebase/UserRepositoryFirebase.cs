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

        public async Task AddSaloonAsFavorite(string userDocument, string saloonCode)
        {
            var saloonQuery = await ExecuteReader(null,
                new Dictionary<string, object>
                {
                    {"collection", "saloons"},
                    {"documentFirebase", saloonCode}
                });

            var saloon = JsonConvert.DeserializeObject<Saloon>(saloonQuery[0].ToString());
            saloon.SaloonCode = saloonCode;

            var favorites = await SelectUserFavoriteSaloons(userDocument);

            var saloons = new SaloonList();
            saloons.Saloons = new List<Saloon>();

            var favoritesToList = favorites?.ToList() ?? saloons.Saloons;
            favoritesToList.Add(saloon);

            await ExecuteNonQuery("update",
                new Dictionary<string, object>
                {
                    {"collection", "users_favorites_saloons"},
                    {"documentFirebase", userDocument},
                    {"content", favoritesToList.ToHashSet()}
                });
        }

        public async Task<bool> CheckIfItIsAlreadyInTheFavoritesList(string userDocument, string saloonCode)
        {
            var favorites = await SelectUserFavoriteSaloons(userDocument);

            if (favorites is null) return false;

            foreach (var saloon in favorites)
            {
                if (saloon.SaloonCode == saloonCode)
                {
                    return true;
                }
            }

            return false;
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
                    {"collection", "user_favorite_saloons" },
                    {"documentFirebase", document}
                });

            if (!saloons.Any()) return null;

            var obj = JsonConvert.DeserializeObject<SaloonCodeParser>(saloons[0].ToString());
            var response = obj?.Saloons;

            return response;
        }

        public async Task<bool> VerifySaloon(string saloonCode)
        {
            var saloon = await ExecuteReader(null,
                new Dictionary<string, object>
                {
                    {"collection", "saloons" },
                    {"documentFirebase", saloonCode}
                });

            if(saloon.Any()) return true;

            return false;
        }

        private class SaloonCodeParser
        {
            public IEnumerable<Saloon>? Saloons { get; set; }
        }
    }
}
