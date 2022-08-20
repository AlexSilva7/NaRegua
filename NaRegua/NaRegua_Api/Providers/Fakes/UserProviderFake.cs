using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Models.Auth;
using NaRegua_Api.Models.Generics;
using NaRegua_Api.Models.Saloon;
using NaRegua_Api.Models.Users;
using System.Security.Principal;

namespace NaRegua_Api.Providers.Fakes
{
    public class UserProviderFake : IUserProvider
    {
        private readonly IHairdresserProvider _hairdresserProvider;
        private readonly ISaloonProvider _saloonProvider;

        public static List<User> _users = new List<User>();
        public static List<Dictionary<string, Scheduling>> _scheduleAppointment = new List<Dictionary<string, Scheduling>>();
        public static List<Dictionary<string, Saloon>> _userFavoriteSaloons = new List<Dictionary<string, Saloon>>();

        public UserProviderFake(IHairdresserProvider hairdresserProvider, ISaloonProvider saloonProvider)
        {
            _hairdresserProvider = hairdresserProvider;
            _saloonProvider = saloonProvider;
        }

        public Task<GenericResult> CreateUserAsync(User user)
        {
            if (Validations.ChecksIfIsNullProperty(user))
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "User cannot be registered, incomplete fields",
                    Success = false
                });
            }

            var verify = CheckIfAlreadyRegistered(user.Document, user.Username, user.Email);
            if (verify.Registered)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = $"User already registered with this {verify.Message}",
                    Success = false
                });
            }

            user.Password = Criptograph.HashPass(user.Password);

            _users.Add(user);

            return Task.FromResult(new GenericResult
            {
                Message = "User registered successfully",
                Success = true
            });
        }

        public IEnumerable<User> GetUsersList()
        {
            return _users;
        }

        public Task<GenericResult> ScheduleAppointmentAsync(IPrincipal user, DateTime dateTime, string documentProfessional)
        {
            if (_hairdresserProvider.GetHairdressersList().Count() == 0 ||
                _hairdresserProvider.GetHairdressersFromDocument(documentProfessional) == null ||
                !_hairdresserProvider.GetProfessionalAvailability(documentProfessional).Result.Success)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Professional not found or not exists availability",
                    Success = false
                });
            }
            var x = dateTime.Date.ToLocalTime();
            var a = DateTime.Now.Date.ToLocalTime();
            if (dateTime.Date.ToLocalTime() < DateTime.Now.Date.ToLocalTime())
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Invalid datetime",
                    Success = false
                });
            }

            var verifyAvailability = _hairdresserProvider.GetProfessionalAvailability(documentProfessional)
                .Result.Resources.Contains(dateTime);

            if (!verifyAvailability)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Professional does not have this availability",
                    Success = false
                });
            }

            var document = Validations.FindFirstClaimOfType(user, "Document");
            var schedules = GetAppointmentAsync(user).Result.Resource;

            //Verifica se o usuário já tem 4 marcações em 30 dias
            var countSchedules = schedules.Where(x => x.DateTime.Date <= DateTime.Now.AddDays(30)).Count();
            if(countSchedules >= 4)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "user already has 4 appointments this month, it is not possible to schedule any more appointments.",
                    Success = false
                });
            }

            //verifica se usuário já tem uma marcação naquele dia
            foreach (var schedule in schedules)
            {
                if (schedule.DateTime.Date == dateTime.Date)
                {
                    return Task.FromResult(new GenericResult
                    {
                        Message = "User already has a booking today.",
                        Success = false
                    });
                }
            }

            //Remove horário disponível para o profissional e adiciona o compromisso
            _hairdresserProvider.SetAppointmentsFromTheProfessional(user, documentProfessional, dateTime);

            var hairdresser = _hairdresserProvider.GetHairdressersFromDocument(documentProfessional);
            var saloon = _saloonProvider.GetSaloon(hairdresser.SaloonCode);

            //Adiciona marcação para o usuário
            var scheduleUser = new Dictionary<string, Scheduling>();
            scheduleUser.Add(document, new Scheduling
            {
                ProfessionalName = hairdresser.Name,
                ProfessionalPhone = hairdresser.Phone,
                SalonAdress = saloon.Address,
                DateTime = dateTime
            });
            _scheduleAppointment.Add(scheduleUser);

            return Task.FromResult(new GenericResult
            {
                Message = "Appointment made.",
                Success = true
            });
        }

        public Task<SchedulingResult> GetAppointmentAsync(IPrincipal user)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            var scheduleList = _scheduleAppointment.FindAll(x => x.Keys.Contains(document));

            var result = new List<Scheduling>();
            foreach (var schedule in scheduleList)
            {
                var value = schedule.GetValueOrDefault(document);
                if (value.DateTime.Date >= DateTime.Now.Date)
                {
                    result.Add(value);
                }
            }

            return Task.FromResult(new SchedulingResult
            {
                Resource = result,
                Message = "Appointments",
                Success = true
            });
        }

        public Task<GenericResult> AddUserSalonAsFavoriteAsync(IPrincipal user, string saloonCode)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            var saloon = _saloonProvider.GetSaloon(saloonCode);

            if(saloon is null)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "salon not found.",
                    Success = false
                });
            }

            var favoriteDict = new Dictionary<string, Saloon>();
            favoriteDict.Add(document, saloon);

            _userFavoriteSaloons.Add(favoriteDict);

            return Task.FromResult(new GenericResult
            {
                Message = "Favorite successfully added.",
                Success = true
            });
        }

        public Task<ListSaloonsResult> GetUserFavoriteSaloonsAsync(IPrincipal user)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            var favorites = new List<Saloon>();

            foreach (var saloon in _userFavoriteSaloons)
            {
                saloon.TryGetValue(document, out Saloon value);
                if(value is not null) favorites.Add(value);
            }

            return Task.FromResult(new ListSaloonsResult
            {
                Resources = favorites.Select(x => x.ToResult()),
                Success = true
            });
        }

        private RegisteredResult CheckIfAlreadyRegistered(string document, string username, string email)
        {
            if (GetUsersList().Where(x => x.Document == document).Count() != 0)
            {
                return new RegisteredResult { Message = "Document", Registered = true };
            }

            if (GetUsersList().Where(x => x.Username == username).Count() != 0)
            {
                return new RegisteredResult { Message = "Username", Registered = true };
            }

            if (GetUsersList().Where(x => x.Email == email).Count() != 0)
            {
                return new RegisteredResult { Message = "Email", Registered = true };
            }

            if (HairdresserProviderFake._hairdressers != null && 
                HairdresserProviderFake._hairdressers.Where(x => x.Username == username).Count() != 0)
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
