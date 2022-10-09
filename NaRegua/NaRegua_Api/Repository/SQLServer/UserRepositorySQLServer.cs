using NaRegua_Api.Database;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Models.Users;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;

namespace NaRegua_Api.Repository.SQLServer
{
    public abstract class UserRepositorySQLServer : SQLServerDatabase, IUserRepository
    {
        const string PRIMARY_KEY_MESSAGE = "Violação da restrição PRIMARY KEY";
        protected abstract string INSERT_USER { get; }
        protected abstract string INSERT_FAVORITE_SALOON { get; }
        protected abstract string DELETE_FAVORITE_SALOON { get; }
        protected abstract string SELECT_FAVORITE_SALOON { get; }
        protected abstract string SELECT_USER_FAVORITE_SALOON { get; }
        protected abstract string SELECT_SALOONS_CODE { get; }
        protected abstract string SELECT_APPOINTMENTS { get; }
        protected abstract string SELECT_COUNT_SCHEDULING_USER { get; }
        protected abstract string DELETE_AVAILABILITY_FROM_PROFESSIONAL { get; }
        protected abstract string SELECT_AVAILABILITY_AND_INFOs_FROM_PROFESSIONAL { get; }
        protected abstract string INSERT_SCHEDULING_USER { get; }
        protected abstract string SELECT_USER_FROM_USERNAME { get; }

        public async Task<bool> VerifySaloon(string saloonCode)
        {
            var verifySaloon = await ExecuteReader(SELECT_SALOONS_CODE,
                new Dictionary<string, object>
                {
                    {"@CODE", saloonCode }
                });

            if (!verifySaloon.Any()) return true;

            return false;
        }

        public async Task<bool> CheckIfItIsAlreadyInTheFavoritesList(string userDocument, string saloonCode)
        {
            var saloon = await ExecuteReader(SELECT_FAVORITE_SALOON,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", userDocument },
                    {"@SALOON_CODE", saloonCode }
                });

            if (saloon.Any()) return true;

            return false;
        }

        public async Task AddSaloonAsFavorite(string userDocument, string saloonCode)
        {
            await ExecuteNonQuery(INSERT_FAVORITE_SALOON,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", userDocument },
                    {"@SALOON_CODE", saloonCode }
                });
        }

        public async Task InsertUser(User user)
        {
            var username = await ExecuteReader(SELECT_USER_FROM_USERNAME,
                new Dictionary<string, object>
                {
                    {"@USERNAME", user.Username },
                });

            if (username.Any())
            {
                throw new PrimaryKeyException("User already registered with this username");
            }
            
            try
            {
                await ExecuteNonQuery(INSERT_USER,
                new Dictionary<string, object>
                {
                    {"@NAME", user.Name },
                    {"@DOCUMENT",  user.Document },
                    {"@EMAIL", user.Email },
                    {"@PHONE", user.Phone },
                    {"@USERNAME", user.Username },
                    {"@PASSWORD", user.Password },
                    {"@ISCUSTOMER", user.IsCustomer == true ? 1 : 0 }
                });
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains(PRIMARY_KEY_MESSAGE))
                {
                    throw new PrimaryKeyException("User already registered with this document");
                }

                throw;
            }
        }

        public async Task<IEnumerable<Scheduling>?> SelectAppointmentsOfUser(string document)
        {
            var schedulings = await ExecuteReader(SELECT_APPOINTMENTS,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", document },
                    {"@DATETIME", DateTime.Now.Date }
                });

            if (!schedulings.Any()) return null;

            var scheduleList = new List<Scheduling>();
            var count = schedulings.Count;

            for (var x = 0; x < (count / 5); x++)
            {
                DateTime.TryParse(schedulings[4].ToString(), out var Date);
                var schedule = new Scheduling
                {
                    ProfessionalName = schedulings[1].ToString(),
                    ProfessionalPhone = schedulings[2].ToString(),
                    SalonAdress = schedulings[3].ToString(),
                    DateTime = Date
                };

                schedulings.RemoveRange(0, 5);
                scheduleList.Add(schedule);
            }
            
            return scheduleList.Distinct();
        }

        public async Task<IEnumerable<Saloon>?> SelectUserFavoriteSaloons(string document)
        {
            var saloons = await ExecuteReader(SELECT_USER_FAVORITE_SALOON,
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

        public async Task DeleteSalonFromFavorites(string document, string saloonCode)
        {
            await ExecuteNonQuery(DELETE_FAVORITE_SALOON,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", document },
                    {"@SALOON_CODE", saloonCode }
                });
        }

        public async Task InsertScheduleAppointment(string documentUser, DateTime dateTime, string documentProfessional)
        {
            var count = await ExecuteReader(SELECT_COUNT_SCHEDULING_USER,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", "15" }
                });

            if(int.Parse(count.First().ToString()) > 4)
            {
                throw new CannotScheduleException(
                    "user already has 4 appointments this month, it is not possible to schedule any more appointments.");
            }

            //verifica se o professional tem essa disponibilidade
            var verify_availability = await ExecuteReader(SELECT_AVAILABILITY_AND_INFOs_FROM_PROFESSIONAL,
                new Dictionary<string, object>
                {
                    {"@PROFESSIONAL_DOCUMENT", documentProfessional },
                    {"@DATE", dateTime }
                });

            if (verify_availability.Any())
            {
                await ExecuteNonQuery(DELETE_AVAILABILITY_FROM_PROFESSIONAL,
                new Dictionary<string, object>
                {
                    {"@PROFESSIONAL_DOCUMENT", documentProfessional },
                    {"@DATE", dateTime }
                });
                
                await ExecuteNonQuery(INSERT_SCHEDULING_USER,
                new Dictionary<string, object>
                {
                    {"@USER_DOCUMENT", documentUser },
                    {"@PROFESSIONAL_NAME", verify_availability[0].ToString() },
                    {"@PROFESSIONAL_PHONE", verify_availability[1].ToString() },
                    {"@SALOON_ADRESS", verify_availability[2].ToString() },
                    {"@DATE", dateTime }
                });
            }
            else
            {
                throw new CannotScheduleException("Professional does not have this availability");
            }
        }
    }
}