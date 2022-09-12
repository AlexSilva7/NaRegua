using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Hairdresser;
using NaRegua_Api.Repository.ActiveSessionRepository;
using System.Security.Principal;

namespace NaRegua_Api.Providers.Implementations
{
    public class HairdresserProvider : IHairdresserProvider
    {
        private readonly HairdresserRepositorySqlServer _database;

        public HairdresserProvider()
        {
            _database = new HairdresserRepositorySqlServer();
        }

        public async Task<GenericResult> CreateHairdresserAsync(Hairdresser hairdresser)
        {
            //PRIMARY KEY
            await _database.InsertHairdresser(hairdresser);
            return new GenericResult { Success = true };
        }

        public async Task<AppointmentsListResult> GetAppointmentsFromTheProfessional(IPrincipal principal)
        {
            var document = Validations.FindFirstClaimOfType(principal, "Document");

            var appointments = await _database.SelectAppointmentsOfProfessional(document);

            return new AppointmentsListResult
            {
                Resources = appointments is not null ? appointments.Select(x => x.ToResult()) :
                new List<Scheduling>().Select(x => x.ToResult()),
                Success = true
            };
        }

        public Task<EvaluationAverageResult> GetEvaluationAverageFromTheProfessional(string document)
        {
            throw new NotImplementedException();
        }

        public Hairdresser GetHairdressersFromDocument(string document)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hairdresser> GetHairdressersList()
        {
            throw new NotImplementedException();
        }

        public Task<ListHairdresserResult> GetHairdressersListOfSalon(string salonCode)
        {
            throw new NotImplementedException();
        }

        public Task<ProfessionalAvailabilityResult> GetProfessionalAvailability(string document)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult> SendEvaluationAverageFromTheProfessional(ProfessionalEvaluation evaluation)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability, IPrincipal principal)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResult> SetAppointmentsFromTheProfessional(IPrincipal principal, string document, DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
