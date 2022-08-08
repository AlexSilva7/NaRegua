using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Hairdresser;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NaRegua_API.Common.Contracts
{
    public interface IHairdresserProvider
    {
        IEnumerable<Hairdresser> GetHairdressersList();
        Task<GenericResult> CreateHairdresserAsync(Hairdresser hairdresser);
        Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability, IPrincipal principal);
        Task<ListHairdresserResult> GetHairdressersListOfSalon(string salonCode);
        Task<ProfessionalAvailabilityResult> GetProfessionalAvailability(string document);
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
}
