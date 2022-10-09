using NaRegua_Api.Models.Hairdresser;

namespace NaRegua_Api.Repository.Contracts
{
    public interface IHairdresserRepository
    {
        Task InsertHairdresser(Hairdresser hairdresser);
        Task<IEnumerable<Scheduling>?> SelectAppointmentsOfProfessional(string document);
        Task InsertWorkAvailability(string document, IEnumerable<DateTime> dateTimeList);
        Task<IEnumerable<Hairdresser>> SelectHairdressersListOfSalon(string salonCode);
        Task<IEnumerable<DateTime>> SelectProfessionalAvailability(string document);
        Task<double> SelectEvaluationAverage(string document);
        Task InsertEvaluationAverage(string document, double evaluation);
    }
}
