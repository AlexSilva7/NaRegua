using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Hairdresser;
using System.Security.Principal;

namespace NaRegua_Api.Common.Contracts
{
    public interface IHairdresserProvider
    {
        IEnumerable<Hairdresser> GetHairdressersList();
        Hairdresser GetHairdressersFromDocument(string document);
        Task<GenericResult> CreateHairdresserAsync(Hairdresser hairdresser);
        Task<ListHairdresserResult> GetHairdressersListOfSalon(string salonCode);
        Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability, IPrincipal principal);
        Task<ProfessionalAvailabilityResult> GetProfessionalAvailability(string document);
        Task<AppointmentsListResult> GetAppointmentsFromTheProfessional(IPrincipal principal);
        Task<GenericResult> SetAppointmentsFromTheProfessional(IPrincipal principal, string document, DateTime dateTime);
        Task<EvaluationAverageResult> GetEvaluationAverageFromTheProfessional(string document);
        Task<GenericResult> SendEvaluationAverageFromTheProfessional(ProfessionalEvaluation evaluation);
    }

    public class AppointmentsListResult
    {
        public IEnumerable<AppointmentsResult> Resources { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class AppointmentsResult
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class ListHairdresserResult
    {
        public IEnumerable<HairdresserResult> Resources { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class HairdresserResult
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string SaloonCode { get; set; }
        public bool IsCustomer = false;
    }

    public class ProfessionalAvailabilityResult
    {
        public string Document { get; set; }
        public IEnumerable<DateTime> Resources { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class EvaluationAverageResult
    {
        public double Average { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
