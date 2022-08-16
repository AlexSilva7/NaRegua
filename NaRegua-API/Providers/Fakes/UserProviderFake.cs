using NaRegua_API.Common.Contracts;
using NaRegua_API.Common.Validations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class UserProviderFake : IUserProvider
    {
        private readonly IHairdresserProvider _hairdresserProvider;
        private readonly ISaloonProvider _saloonProvider;
        public static List<User> _users = new List<User>();
        public static List<Dictionary<string, Scheduling>> _scheduleAppointment = new List<Dictionary<string, Scheduling>>();

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

            //verifica se usuário já tem uma marcação naquele dia
            var schedules = _scheduleAppointment.FindAll(x => x.Keys.Contains(document));
            foreach(var schedule in schedules)
            {
                var date = schedule.GetValueOrDefault(document).DateTime.Date;
                if (date == dateTime)
                {
                    return Task.FromResult(new GenericResult
                    {
                        Message = "User already has a booking today.",
                        Success = false
                    });
                }
            }

            //Remove horário disponível para o profissional
            _hairdresserProvider.SetAppointmentsFromTheProfessional(user, documentProfessional, dateTime);

            var hairdresser = _hairdresserProvider.GetHairdressersFromDocument(documentProfessional);
            var saloon = _saloonProvider.GetSaloonAsync(hairdresser.SaloonCode).Result;


            //Add marcação para o usuário
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
                Message = "Appointment made.",
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
