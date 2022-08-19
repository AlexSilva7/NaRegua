using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Auth;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Hairdresser;
using System.Security.Claims;
using System.Security.Principal;

namespace NaRegua_Api.Providers.Fakes
{
    public class HairdresserProviderFake : IHairdresserProvider
    {
        private readonly ISaloonProvider _saloonProvider;

        public static List<Hairdresser> _hairdressers = new List<Hairdresser>();

        public static List<Dictionary<string, List<DateTime>>> _workAvailabilityHairdressers = 
            new List<Dictionary<string, List<DateTime>>>();

        public static List<Dictionary<string, Scheduling>> _scheduleAppointment = 
            new List<Dictionary<string, Scheduling>>();

        public static List<Dictionary<string, double>> _evaluationAverages =
            new List<Dictionary<string, double>>();

        public HairdresserProviderFake(ISaloonProvider salonProvider)
        {
            _saloonProvider = salonProvider;
        }

        public Hairdresser GetHairdressersFromDocument(string document)
        {
            return _hairdressers.Where(x => x.Document == document).FirstOrDefault();
        }

        public IEnumerable<Hairdresser> GetHairdressersList()
        {
            return _hairdressers;
        }

        public Task<GenericResult> CreateHairdresserAsync(Hairdresser hairdresser)
        {
            if (Validations.ChecksIfIsNullProperty(hairdresser))
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Professional cannot be registered, incomplete fields",
                    Success = false
                });
            }

            var saloon = _saloonProvider.GetSaloon(hairdresser.SaloonCode);

            if (saloon == null)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Professional cannot be registered, saloon not found",
                    Success = false
                });
            }

            var verify = CheckIfAlreadyRegistered(hairdresser.Document, hairdresser.Username, hairdresser.Email);
            if (verify.Registered)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = $"User already registered with this {verify.Message}",
                    Success = false
                });
            }

            hairdresser.Password = Criptograph.HashPass(hairdresser.Password);

            _hairdressers.Add(hairdresser);

            return Task.FromResult(new GenericResult
            {
                Message = "Professional registered successfully",
                Success = true
            });
        }

        public Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability, IPrincipal principal)
        {
            if (availability.EndHour.Date != availability.StartHour.Date)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Incorrect time, different start and end data",
                    Success = false
                });
            }

            if ((availability.EndHour - availability.StartHour).Hours < 0)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Incorrect Time",
                    Success = false
                });
            }

            var document = Validations.FindFirstClaimOfType(principal, "Document");

            var hours = new List<DateTime>();
            for (var x = availability.StartHour; x < availability.EndHour; x = x.AddHours(1))
            {
                hours.Add(x);
            }

            var dictAvailability = new Dictionary<string, List<DateTime>>();
            dictAvailability.Add(document, hours);

            _workAvailabilityHairdressers.Add(dictAvailability);

            return Task.FromResult(new GenericResult
            {
                Message = "Times Registered Successfully",
                Success = true
            });
        }

        public Task<ListHairdresserResult> GetHairdressersListOfSalon(string salonCode)
        {
            var hairdressers = GetHairdressersList().Where(x => x.SaloonCode == salonCode);

            if(hairdressers.Count() == 0)
            {
                return Task.FromResult(new ListHairdresserResult
                {
                    Message = "There are no professionals registered for this salon",
                    Success = false
                });
            }

            return Task.FromResult(new ListHairdresserResult
            {
                Resources = hairdressers.Select(x => x.ToResult()),
                Success = true
            });
        }

        public Task<ProfessionalAvailabilityResult> GetProfessionalAvailability(string document)
        {
            string errorMessage = "It was not possible to check the availability of the professional.";

            var availability = _workAvailabilityHairdressers.Where(x => x.Keys.Contains(document));

            if (availability.Count() == 0)
            {
                return Task.FromResult(new ProfessionalAvailabilityResult
                {
                    Message = errorMessage,
                    Success = false
                });
            }

            var list = new List<DateTime>();

            foreach (var item in availability)
            {
                var listAux = new List<DateTime>();
                item.TryGetValue(document, out listAux);

                var listRemove = listAux.Where(x => x.Date < DateTime.Now.Date).ToList();

                for (var x = 0; x < listRemove.Count; x++)
                {
                    var dateRemove = listRemove[x];
                    listAux.Remove(dateRemove);
                }

                list.AddRange(listAux);
            }

            return Task.FromResult(new ProfessionalAvailabilityResult
            {
                Document = document,
                Resources = list.OrderBy(x => x.Date).Distinct(),
                Success = true
            });
        }

        public Task<AppointmentsListResult> GetAppointmentsFromTheProfessional(IPrincipal principal)
        {
            var document = Validations.FindFirstClaimOfType(principal, "Document");
            var appointments = _scheduleAppointment.FindAll(x => x.Keys.Contains(document));

            var result = new List<AppointmentsResult>();
            foreach(var appointment in appointments)
            {
                var scheduling = appointment.GetValueOrDefault(document);
                result.Add(new AppointmentsResult
                {
                    CustomerName = scheduling.CustomerName,
                    CustomerPhone = scheduling.CustomerPhone,
                    DateTime = scheduling.DateTime
                });
            }

            return Task.FromResult(new AppointmentsListResult
            {
                Resources = result,
                Success = true
            });
        }

        public Task<GenericResult> SetAppointmentsFromTheProfessional(IPrincipal principal, string document, DateTime dateTime)
        {
            var name = Validations.FindFirstClaimOfType(principal, ClaimTypes.Name);
            var phone = Validations.FindFirstClaimOfType(principal, "Phone");

            //Adiciona compromisso para o profissional
            var scheduleHairdresser = new Dictionary<string, Scheduling>();
            scheduleHairdresser.Add(document, new Scheduling
            {
                CustomerName = name,
                CustomerPhone = phone,
                DateTime = dateTime
            });

            _scheduleAppointment.Add(scheduleHairdresser);

            //Remove horário da lista de disponibilidade
            var availabilitys = _workAvailabilityHairdressers.FindAll(x => x.Keys.Contains(document));
            foreach (var availability in availabilitys)
            {
                var value = new List<DateTime>();
                bool hasValue = availability.TryGetValue(document, out value);

                if (hasValue)
                {
                    value.Remove(dateTime);
                }
            }

            return Task.FromResult(new GenericResult
            {
                Message = "Scheduling done.",
                Success = true
            });
        }

        public Task<EvaluationAverageResult> GetEvaluationAverageFromTheProfessional(string document)
        {
            var evaluations = _evaluationAverages.FindAll(x => x.Keys.Contains(document));
            var average = 0.0;
            foreach (var evaluation in evaluations)
            {
                average += evaluation.GetValueOrDefault(document);
            }

            return Task.FromResult(new EvaluationAverageResult
            {
                Average = average,
                Success = true
            });
        }

        public Task<GenericResult> SendEvaluationAverageFromTheProfessional(ProfessionalEvaluation evaluation)
        {
            if (Validations.ChecksIfIsNullProperty(evaluation))
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Incomplete fields.",
                    Success = false
                });
            }

            var dict = new Dictionary<string, double>();
            dict.Add(evaluation.Document, evaluation.Evaluation);
            _evaluationAverages.Add(dict);

            return Task.FromResult(new GenericResult
            {
                Message = "Review sent.",
                Success = true
            });
        }

        private RegisteredResult CheckIfAlreadyRegistered(string document, string username, string email)
        {
            if(GetHairdressersList().Where(x => x.Document == document).Count() != 0) 
            {
                return new RegisteredResult { Message = "Document", Registered = true };
            }

            if (GetHairdressersList().Where(x => x.Username == username).Count() != 0)
            {
                return new RegisteredResult { Message = "Username", Registered = true };
            }

            if (GetHairdressersList().Where(x => x.Email == email).Count() != 0)
            {
                return new RegisteredResult { Message = "Email", Registered = true };
            }

            if (UserProviderFake._users != null && UserProviderFake._users.Where(x => x.Username == username).Count() != 0)
            {
                return new RegisteredResult { Message = "Username", Registered = true };
            }

            return new RegisteredResult { Registered = false };
        }

        private class RegisteredResult
        {
            public string Message { get; set; }
            public bool Registered { get; set; }
        }
    }
}
