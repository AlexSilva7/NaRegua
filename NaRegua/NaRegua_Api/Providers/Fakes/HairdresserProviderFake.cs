using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Configurations;
using NaRegua_Api.Models.Auth;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Hairdresser;
using NaRegua_Api.Models.Users;
using System.Security.Claims;
using System.Security.Principal;
using Scheduling = NaRegua_Api.Models.Hairdresser.Scheduling;

namespace NaRegua_Api.Providers.Fakes
{
    public class HairdresserProviderFake : IHairdresserProvider
    {
        private readonly ISaloonProvider _saloonProvider;

        public static List<Hairdresser> _hairdressers = new List<Hairdresser>();

        public static Dictionary<string, List<DateTime>> _workAvailabilityHairdressers = 
            new Dictionary<string, List<DateTime>>();

        public static Dictionary<string, List<Scheduling>> _scheduleAppointment = 
            new Dictionary<string, List<Scheduling>>();

        public static Dictionary<string, List<double>> _evaluationAverages =
            new Dictionary<string, List<double>>();

        public static Dictionary<string, Dictionary<string, decimal>> _hairdresserAccountBalance = new Dictionary<string, Dictionary<string, decimal>>();

        public HairdresserProviderFake(ISaloonProvider salonProvider)
        {
            _saloonProvider = salonProvider;
            InsertsFakes();
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
            _hairdresserAccountBalance.Add(hairdresser.Id, new Dictionary<string, decimal>
            {
                { hairdresser.Document, 0 }
            });

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

            var aux = new List<DateTime>();
            for (var x = availability.StartHour; x < availability.EndHour; x = x.AddHours(1))
            {
                aux.Add(x);
            }

            var document = Validations.FindFirstClaimOfType(principal, "Document");
            _workAvailabilityHairdressers.TryGetValue(document, out var list);

            if (list is null)
            {
                list = new List<DateTime>();
            }

            list.Union(aux);

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
            _workAvailabilityHairdressers.TryGetValue(document, out var availability);

            if (availability is null)
            {
                return Task.FromResult(new ProfessionalAvailabilityResult
                {
                    Message = "It was not possible to check the availability of the professional.",
                    Success = false
                });
            }

            availability.RemoveAll(x => x.Date < DateTime.Now.Date);
            
            return Task.FromResult(new ProfessionalAvailabilityResult
            {
                Document = document,
                Resources = availability.OrderBy(x => x.Date).Distinct(),
                Success = true
            });
        }

        public Task<AppointmentsListResult> GetAppointmentsFromTheProfessional(IPrincipal principal)
        {
            var document = Validations.FindFirstClaimOfType(principal, "Document");
            _scheduleAppointment.TryGetValue(document, out var appointments);

            return Task.FromResult(new AppointmentsListResult
            {
                Resources = appointments is not null ? appointments.Select(x => x.ToResult()) : 
                new List<Scheduling>().Select(x => x.ToResult()),
                Success = true
            });
        }

        public Task<GenericResult> SetAppointmentsFromTheProfessional(string orderId, IPrincipal principal, string document, DateTime dateTime)
        {
            var name = Validations.FindFirstClaimOfType(principal, ClaimTypes.Name);
            var phone = Validations.FindFirstClaimOfType(principal, "Phone");

            //Adiciona compromisso para o profissional
            _scheduleAppointment.TryGetValue(document, out var list);

            if(list is null)
            {
                _scheduleAppointment.Add(document, new List<Scheduling>());
            }

            _scheduleAppointment[document].Add(
                new Scheduling
            {
                OrderId = orderId,
                CustomerName = name,
                CustomerPhone = phone,
                DateTime = dateTime
            });

            //Remove horário da lista de disponibilidade
            _workAvailabilityHairdressers.TryGetValue(document, out var availability);
            availability?.Remove(dateTime);

            return Task.FromResult(new GenericResult
            {
                Message = "Scheduling done.",
                Success = true
            });
        }

        public Task<EvaluationAverageResult> GetEvaluationAverageFromTheProfessional(string document)
        {
            if(GetHairdressersFromDocument(document) is null)
            {
                return Task.FromResult(new EvaluationAverageResult
                {
                    Message = "Professional not found.",
                    Success = false
                });
            }

            _evaluationAverages.TryGetValue(document, out var evaluations);
            
            return Task.FromResult(new EvaluationAverageResult
            {
                Average = evaluations is not null && evaluations.Any() ? evaluations.Average() : 0,
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

            if (GetHairdressersFromDocument(evaluation.ProfessionalDocument) is null)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Professional not found.",
                    Success = false
                });
            }

            _evaluationAverages.TryGetValue(evaluation.ProfessionalDocument, out var value);
            if (value is null)
            {
                _evaluationAverages.Add(evaluation.ProfessionalDocument, new List<double>());
            }

            _evaluationAverages[evaluation.ProfessionalDocument].Add(evaluation.Evaluation);

            return Task.FromResult(new GenericResult
            {
                Message = "Review sent.",
                Success = true
            });
        }

        public Task<AccountBalanceResult> GetAccountBalanceAsync(IPrincipal user)
        {
            var accountId = Validations.FindFirstClaimOfType(user, "AccountId");
            var document = Validations.FindFirstClaimOfType(user, "Document");

            var value = _hairdresserAccountBalance[accountId][document];

            return Task.FromResult(new AccountBalanceResult
            {
                Balance = value,
                Success = true
            });
        }

        public Task<GenericResult> MakePayment(string document, decimal value)
        {
            try
            {
                var hairdresser = GetHairdressersFromDocument(document);
                _hairdresserAccountBalance[hairdresser.Id][document] += value;

                return Task.FromResult(new GenericResult
                {
                    Success = true
                });
            }
            catch(Exception ex)
            {
                return Task.FromResult(new GenericResult
                {
                    Success = false
                });
            }
        }

        private RegisteredResult CheckIfAlreadyRegistered(string document, string username, string email)
        {
            if(GetHairdressersList().Where(x => x.Document == document).Count() != 0) 
            {
                return new () { Message = "Document", Registered = true };
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

        private void InsertsFakes()
        {
            RegisterProfessionalFake("Jose Ribeiro Gomes", "48280663061", "teste@gmail.com", "(21) 99999-9999",
                "teste1", "teste", "045B79");

            RegisterProfessionalFake("Willian Souza", "92849257036", "teste1@gmail.com", "(21) 00000-0000",
                "teste2", "teste", "045B79");

            RegisterProfessionalFake("Carlos Alberto Dominguez", "64730188080", "teste2@gmail.com", "(21) 11111-1111",
                "teste3", "teste", "045B79");

            RegisterProfessionalFake("Jefferson Laureano", "34125497036", "teste3@gmail.com", "(21) 99999-9999",
                "teste4", "teste", "045B79");

            RegisterProfessionalFake("Damiao Neves", "56599247008", "teste4@gmail.com", "(21) 99999-9999", "teste5",
                "teste", "978C50");

            RegisterProfessionalFake("Lucas Silva", "33587139032", "teste5@gmail.com", "(21) 99999-9999",
                "teste6", "teste", "978C50");

            RegisterProfessionalFake("Roberto Goncalves", "70623372002", "teste6@gmail.com", "(21) 99999-9999", "teste7",
                "teste", "978C50");

            RegisterProfessionalFake("Aldamir Ribeiro", "99005221097", "teste7@gmail.com", "(21) 99999-9999", "teste8",
                "teste", "022D79");

            RegisterProfessionalFake("Alex Silva", "12699068012", "teste8@gmail.com", "(21) 99999-9999", "teste9",
                "teste", "022D79");

            RegisterProfessionalFake("Otto Monteiro", "86379180001", "teste9@gmail.com", "(21) 99999-9999", "teste10",
                "teste", "022D79");

            RegisterProfessionalFake("Ricardo Souza", "94838570074", "teste10@gmail.com", "(21) 99999-9999", "teste11",
                "teste", "022D79");

            RegisterProfessionalFake("Joao Davi", "51542937035", "teste11@gmail.com", "(21) 99999-9999", "teste12",
                "teste", "022D79");

            RegisterProfessionalFake("Tiago Alves", "59321302026", "teste12@gmail.com", "(21) 99999-9999", "teste13",
                "teste", "022D79");

            RegisterProfessionalFake("Dj Ed", "90376065044", "teste13@gmail.com", "(21) 99999-9999", "teste14",
                "teste", "011Z79");

            RegisterAvailabilityProfessionalsFake("48280663061");
            RegisterAvailabilityProfessionalsFake("92849257036");
            RegisterAvailabilityProfessionalsFake("34125497036");
            RegisterAvailabilityProfessionalsFake("64730188080");
            RegisterAvailabilityProfessionalsFake("56599247008");
            RegisterAvailabilityProfessionalsFake("33587139032");
            RegisterAvailabilityProfessionalsFake("90376065044");
            RegisterAvailabilityProfessionalsFake("59321302026");
            RegisterAvailabilityProfessionalsFake("94838570074");
            RegisterAvailabilityProfessionalsFake("86379180001");
            RegisterAvailabilityProfessionalsFake("51542937035");
            RegisterAvailabilityProfessionalsFake("70623372002");
            RegisterAvailabilityProfessionalsFake("99005221097");
            RegisterAvailabilityProfessionalsFake("12699068012");
        }

        private async void RegisterProfessionalFake(string name, string document, string email, string phone, 
            string username, string password, string code)
        {
            var hairdresser = new Hairdresser
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Document = document,
                Phone = phone,
                Email = email,
                Username = username,
                Password = password,
                SaloonCode = code,
                IsCustomer = false
            };

            await CreateHairdresserAsync(hairdresser);
        }

        private void RegisterAvailabilityProfessionalsFake(string document)
        {
            _workAvailabilityHairdressers.TryGetValue(document, out var aux);

            if (aux is null) aux = new List<DateTime>();

            for (var x = 1; x < 80; x++)
            {
                var tomorrow = DateTime.Now.AddDays(x);

                var startTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 08, 00, 00);
                var endTime = startTime.AddHours(9);

                for (var y = startTime; y <= endTime; y = y.AddHours(1))
                {
                    aux.Add(y);
                }
            }

            _workAvailabilityHairdressers.Add(document, aux);
        }
    }
}
