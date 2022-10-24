using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Hairdresser;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Exceptions;
using System.Security.Principal;

namespace NaRegua_Api.Providers.Implementations
{
    public class HairdresserProvider : IHairdresserProvider
    {
        private readonly IHairdresserRepository _database;

        public HairdresserProvider(IHairdresserRepository hairdresserRepository)
        {
            _database = hairdresserRepository;
        }

        public async Task<GenericResult> CreateHairdresserAsync(Hairdresser hairdresser)
        {
            try
            {
                if (!await _database.VerifySaloon(hairdresser.SaloonCode))
                    return new GenericResult { Message = "Saloon not found.", Success = false };

                await _database.InsertHairdresser(hairdresser);
            }
            catch (PrimaryKeyException ex)
            {
                return new GenericResult
                {
                    Message = ex.Message,
                    Success = false
                };
            }

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

        public async Task<EvaluationAverageResult> GetEvaluationAverageFromTheProfessional(string document)
        {
            var average = await _database.SelectEvaluationAverage(document);
            return new EvaluationAverageResult
            {
                Average = average,
                Success = true
            };
        }

        public Hairdresser GetHairdressersFromDocument(string document)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hairdresser> GetHairdressersList()
        {
            throw new NotImplementedException();
        }

        public async Task<ListHairdresserResult> GetHairdressersListOfSalon(string salonCode)
        {
            var hairdresserList = await _database.SelectHairdressersListOfSalon(salonCode);

            return new ListHairdresserResult
            {
                Resources = hairdresserList is not null ? hairdresserList.Select(x => x.ToResult()) : 
                new List<Hairdresser>().Select(x => x.ToResult()),
                Success = true
            };
        }

        public async Task<ProfessionalAvailabilityResult> GetProfessionalAvailability(string document)
        {
            var availabilitys = await _database.SelectProfessionalAvailability(document);

            return new ProfessionalAvailabilityResult
            {
                Document = document,
                Resources = availabilitys,
                Success = true
            };
        }

        public async Task<GenericResult> SendEvaluationAverageFromTheProfessional(ProfessionalEvaluation evaluation)
        {
            await _database.InsertEvaluationAverage(evaluation.ProfessionalDocument, evaluation.Evaluation);

            return new GenericResult
            {
                Success = true
            };
        }

        public async Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability, IPrincipal principal)
        {
            if (availability.StartHour.Date < DateTime.Now.Date)
            {
                return new GenericResult
                {
                    Message = "Incorrect date",
                    Success = false
                };
            }

            if (availability.EndHour.Date != availability.StartHour.Date)
            {
                return new GenericResult
                {
                    Message = "Incorrect time, different start and end data",
                    Success = false
                };
            }

            if ((availability.EndHour - availability.StartHour).Hours < 0)
            {
                return new GenericResult
                {
                    Message = "Incorrect Time",
                    Success = false
                };
            }

            var aux = new List<DateTime>();
            for (var x = availability.StartHour; x < availability.EndHour; x = x.AddHours(1))
            {
                aux.Add(x);
            }

            var document = Validations.FindFirstClaimOfType(principal, "Document");
            await _database.InsertWorkAvailability(document, aux);

            return new GenericResult
            {
                Success = true
            };
        }

        public Task<GenericResult> SetAppointmentsFromTheProfessional(IPrincipal principal, string document, DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
