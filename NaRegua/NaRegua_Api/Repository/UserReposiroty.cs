using NaRegua_Api.Controllers;
using NaRegua_Api.Database;
using NaRegua_Api.Models.Users;

namespace NaRegua_Api.Repository
{
    public class UserRepository : SQLServerDatabase
    {
        const string PRIMARY_KEY_MESSAGE = "Violação da restrição PRIMARY KEY";

        const string INSERT_USER =
            "INSERT INTO [USERS] VALUES (@NAME, @DOCUMENT, @EMAIL, @PHONE, @USERNAME, @PASSWORD, @ISCUSTOMER)";

        const string INSERT_FAVORITE_SALOON =
            "INSERT INTO [USERS_FAVORITE_SALONS] VALUES (@USER_DOCUMENT, @SALOON_CODE)";

        const string SELECT_FAVORITE_SALOON =
            "SELECT DISTINCT SALOON_CODE FROM USERS_FAVORITE_SALONS WHERE USER_DOCUMENT = @USER_DOCUMENT AND SALOON_CODE = @SALOON_CODE";

        const string SELECT_SALOONS_CODE =
            "SELECT DISTINCT CODE FROM SALOONS WHERE CODE = @CODE";

        const string SELECT_APPOINTMENTS =
            "SELECT * FROM SCHEDULING_USERS WHERE USER_DOCUMENT = @USER_DOCUMENT AND DATETIME >= @DATETIME";

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

            if (!saloon.Any()) return true;

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
                    throw new Exception(PRIMARY_KEY_MESSAGE);
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
    }
}