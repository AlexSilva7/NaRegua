using NaRegua_API.Common.Contracts;
using NaRegua_API.Common.Validations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Hairdresser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class HairdresserProviderFake : IHairdresserProvider
    {
        public static List<Hairdresser> _hairdressers;
        public static List<Dictionary<string, List<DateTime>>> _workAvailabilityHairdressers;
        private readonly ISaloonProvider _saloonProvider;

        public HairdresserProviderFake(ISaloonProvider salonProvider)
        {
            _hairdressers = new List<Hairdresser>();
            _saloonProvider = salonProvider;
            _workAvailabilityHairdressers = new List<Dictionary<string, List<DateTime>>>();
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

            var saloons = _saloonProvider.GetSaloonsAsync();
            var saloon = saloons.Result.Resources.Where(x => x.SaloonCode == hairdresser.SaloonCode);

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

        public Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability, IPrincipal user)
        {
            var isCustomer = Validations.FindFirstClaimOfType(user, "IsCustomer");
            if (bool.Parse(isCustomer))
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Only professionals can register availability.",
                    Success = false
                });
            }

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

            var document = Validations.FindFirstClaimOfType(user, "Document");

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

        public IEnumerable<Hairdresser> GetHairdressersList()
        {
            return _hairdressers;
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

            if (availability == null)
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

            var dict = new Dictionary<string, List<DateTime>>();

            return Task.FromResult(new ProfessionalAvailabilityResult
            {
                Document = document,
                Resources = list.OrderBy(x => x.Date),
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
