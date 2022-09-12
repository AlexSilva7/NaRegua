using NaRegua_Api.Database;
using NaRegua_Api.Models.Hairdresser;
using NaRegua_Api.Repository.Exceptions;

namespace NaRegua_Api.Repository
{
    public abstract class HairdresserRepository : SQLServerDatabase
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
    }
}
