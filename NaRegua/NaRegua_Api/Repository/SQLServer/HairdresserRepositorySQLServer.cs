using NaRegua_Api.Database;
using NaRegua_Api.Models.Hairdresser;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;

namespace NaRegua_Api.Repository.SQLServer
{
    public abstract class HairdresserRepositorySQLServer : SQLServerDatabase, IHairdresserRepository
    {
        const string PRIMARY_KEY_MESSAGE = "Violação da restrição PRIMARY KEY";
        protected abstract string INSERT_HAIRDRESSER { get; }
        protected abstract string INSERT_HAIRDRESSER_SALOON { get; }
        protected abstract string INSERT_WORK_AVAILABILITY { get; }
        protected abstract string SELECT_HAIRDRESSERS_BY_SALOON { get; }
        protected abstract string SELECT_HAIRDRESSERS_AVAILABILITY { get; }
        protected abstract string SELECT_HAIRDRESSERS_APPOINTMENTS { get; }
        protected abstract string SELECT_HAIRDRESSERS_EVALUATION_AVERAGE { get; }
        protected abstract string INSERT_HAIRDRESSERS_EVALUATION_AVERAGE { get; }

        public async Task InsertHairdresser(Hairdresser hairdresser)
        {
            try
            {
                await ExecuteNonQuery(INSERT_HAIRDRESSER,
                new Dictionary<string, object>
                {
                    {"@NAME", hairdresser.Name },
                    {"@DOCUMENT",  hairdresser.Document },
                    {"@EMAIL", hairdresser.Email },
                    {"@PHONE", hairdresser.Phone },
                    {"@USERNAME", hairdresser.Username },
                    {"@PASSWORD", hairdresser.Password },
                    {"@ISCUSTOMER", hairdresser.IsCustomer ? 1 : 0 }
                });

                await ExecuteNonQuery(INSERT_HAIRDRESSER_SALOON,
                new Dictionary<string, object>
                {
                    {"@DOCUMENT",  hairdresser.Document },
                    {"@SALOON_CODE", hairdresser.SaloonCode },
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(PRIMARY_KEY_MESSAGE))
                {
                    throw new PrimaryKeyException(PRIMARY_KEY_MESSAGE);
                }

                throw;
            }
        }

        public async Task<IEnumerable<Scheduling>?> SelectAppointmentsOfProfessional(string document)
        {
            var schedulings = await ExecuteReader(SELECT_HAIRDRESSERS_APPOINTMENTS,
                new Dictionary<string, object>
                {
                    {"@DOCUMENT_PROFISSIONAL", document }
                });

            if (!schedulings.Any()) return null;

            var scheduleList = new List<Scheduling>();
            var count = schedulings.Count;

            for (var x = 0; x < (count / 4); x++)
            {
                DateTime.TryParse(schedulings[3].ToString(), out var date);
                var schedule = new Scheduling
                {
                    CustomerName = schedulings[1].ToString(),
                    CustomerPhone = schedulings[2].ToString(),
                    DateTime = date,
                };

                scheduleList.Add(schedule);
                schedulings.RemoveRange(0, 4);
            }

            return scheduleList.Distinct();
        }

        public async Task InsertWorkAvailability(string document, IEnumerable<DateTime> dateTimeList)
        {
            foreach(var dateTime in dateTimeList)
            {
                await ExecuteNonQuery(INSERT_WORK_AVAILABILITY,
                new Dictionary<string, object>
                {
                    {"@DOCUMENT_PROFISSIONAL", document },
                    {"@DATETIME",  dateTime }
                });
            }
        }

        public async Task<IEnumerable<Hairdresser>> SelectHairdressersListOfSalon(string salonCode)
        {
            var hairdressers = await ExecuteReader(SELECT_HAIRDRESSERS_BY_SALOON,
                new Dictionary<string, object>
                {
                    {"@CODE", salonCode }
                });

            var hairdressersList = new List<Hairdresser>();
            var count = hairdressers.Count;
            for (var x = 0; x < (count / 5); x++)
            {
                var hairdresser = new Hairdresser
                {
                    Name = hairdressers[0].ToString(),
                    Document = hairdressers[1].ToString(),
                    Phone = hairdressers[2].ToString(),
                    Email = hairdressers[3].ToString(),
                    SaloonCode = hairdressers[4].ToString(),
                };

                hairdressersList.Add(hairdresser);
                hairdressers.RemoveRange(0, 5);
            }

            return hairdressersList;
        }

        public async Task<IEnumerable<DateTime>> SelectProfessionalAvailability(string document)
        {
            var dateTimes = await ExecuteReader(SELECT_HAIRDRESSERS_AVAILABILITY,
                new Dictionary<string, object>
                {
                    {"@DOCUMENT", document }
                });

            var dateTimeListResult = new List<DateTime>();

            foreach (var date in dateTimes)
            {
                DateTime.TryParse(date.ToString(), out var aux);

                if (aux.Date > DateTime.Now.Date)
                {
                    dateTimeListResult.Add(aux);
                }
            }

            return dateTimeListResult;
        }

        public async Task<double> SelectEvaluationAverage(string document)
        {
            var average = await ExecuteReader(SELECT_HAIRDRESSERS_EVALUATION_AVERAGE,
                new Dictionary<string, object>
                {
                    {"@DOCUMENT", document }
                });

            double.TryParse(average?.FirstOrDefault()?.ToString(), out var result);

            return result;
        }

        public async Task InsertEvaluationAverage(string document, double evaluation)
        {
            await ExecuteNonQuery(INSERT_HAIRDRESSERS_EVALUATION_AVERAGE,
                new Dictionary<string, object>
                {
                    {"@DOCUMENT", document },
                    {"@EVALUATION", evaluation }
                });
        }
    }
}
