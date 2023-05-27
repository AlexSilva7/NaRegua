using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Validations;
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
        public static Dictionary<string, List<Scheduling>> _scheduleAppointment = new Dictionary<string, List<Scheduling>>();
        public static Dictionary<string, List<Saloon>> _userFavoriteSaloons = new Dictionary<string, List<Saloon>>();

        public UserProviderFake(IHairdresserProvider hairdresserProvider, ISaloonProvider saloonProvider)
        {
            _hairdresserProvider = hairdresserProvider;
            _saloonProvider = saloonProvider;
        }

        public Task<GenericResult> CreateUserAsync(User user)
        {
            var verify = CheckIfAlreadyRegistered(user.Document, user.Username, user.Email);
            if (verify.Registered)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = $"User already registered with this {verify.Message}",
                    Success = false
                });
            }

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

            if (dateTime < DateTime.Now)
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
            var schedules = GetAppointmentAsync(user).Result.Resources;

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

            //Remove horário disponível para o profissional e adiciona o compromisso
            _hairdresserProvider.SetAppointmentsFromTheProfessional(user, documentProfessional, dateTime);

            var hairdresser = _hairdresserProvider.GetHairdressersFromDocument(documentProfessional);
            var saloon = _saloonProvider.GetSaloon(hairdresser.SaloonCode);

            //Adiciona marcação para o usuário
            _scheduleAppointment.TryGetValue(document, out var list);
            if (list is not null)
            {
                foreach (var item in list)
                {
                    if (item.DateTime.Date == dateTime.Date)
                    {
                        return Task.FromResult(new GenericResult
                        {
                            Message = "User already has a booking today.",
                            Success = false
                        });
                    }
                }
            }

            var scheduling = new Scheduling
            {
                ProfessionalName = hairdresser.Name,
                ProfessionalPhone = hairdresser.Phone,
                SalonAdress = saloon.Address,
                DateTime = dateTime
            };

            if (list is null)
            {
                list = new List<Scheduling> { scheduling };
            }
            else
            {
                list.Add(scheduling);
            }

            _scheduleAppointment.TryAdd(document, list);

            return Task.FromResult(new GenericResult
            {
                Message = "Appointment made.",
                Success = true
            });
        }

        public Task<SchedulingResult> GetAppointmentAsync(IPrincipal user)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            _scheduleAppointment.TryGetValue(document, out var schedulings);

            return Task.FromResult(new SchedulingResult
            {
                Resources = schedulings is not null ? schedulings.FindAll(x => x.DateTime.Date >= DateTime.Now.Date) : new List<Scheduling>(),
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
                    Message = "Salon not found.",
                    Success = false
                });
            }

            _userFavoriteSaloons.TryGetValue(document, out var list);
            if(list is not null)
            {
                foreach (var item in list)
                {
                    if (item.SaloonCode == saloonCode)
                    {
                        return Task.FromResult(new GenericResult
                        {
                            Message = "Salon is already in the favorites list.",
                            Success = false
                        });
                    }
                }
            }

            if (list is null)
            {
                list = new List<Saloon> { saloon };
            }
            else
            {
                list.Add(saloon);
            }

            _userFavoriteSaloons.TryAdd(document, list);
            return Task.FromResult(new GenericResult
            {
                Message = "Favorite successfully added.",
                Success = true
            });
        }

        public Task<ListSaloonsResult> GetUserFavoriteSaloonsAsync(IPrincipal user)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            _userFavoriteSaloons.TryGetValue(document, out var favorites);

            return Task.FromResult(new ListSaloonsResult
            {
                Resources = favorites is not null ? favorites.Select(x => x.ToResult()) : new List<Saloon>().Select(x => x.ToResult()),
                Success = true
            });
        }

        public Task<GenericResult> RemoveSalonFromFavoritesAsync(IPrincipal user, string saloonCode)
        {
            var document = Validations.FindFirstClaimOfType(user, "Document");
            var saloon = _saloonProvider.GetSaloon(saloonCode);

            if (saloon is null)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Salon not found.",
                    Success = false
                });
            }

            _userFavoriteSaloons.TryGetValue(document, out var favorites);
            if(favorites is not null)
            {
                if (favorites.Remove(saloon))
                {
                    return Task.FromResult(new GenericResult
                    {
                        Message = "Salon successfully removed.",
                        Success = true
                    });
                }
            }

            return Task.FromResult(new GenericResult
            {
                Message = "Salon not found in favorites list.",
                Success = false
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
