using NaRegua_API.Common.Contracts;
using NaRegua_API.Common.Validations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class UserProviderFake : IUserProvider
    {
        public static List<User> _users;
        public static List<Dictionary<string, Scheduling>> _scheduleAppointment;

        public UserProviderFake()
        {
            _users = new List<User>();
            _scheduleAppointment = new List<Dictionary<string, Scheduling>>();
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
            if (HairdresserProviderFake._hairdressers == null ||
                HairdresserProviderFake._hairdressers.Where(x => x.Document == documentProfessional).Count() == 0 ||
                HairdresserProviderFake._workAvailabilityHairdressers == null ||
                HairdresserProviderFake._workAvailabilityHairdressers.Where(x => x.Keys.Contains(documentProfessional)).Count() == 0)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Professional not found or not exists availability",
                    Success = false
                });
            }

            //Remove horário disponível para o profissional
            var availabilitys = HairdresserProviderFake._workAvailabilityHairdressers
                .Where(x => x.ContainsKey(documentProfessional)).Distinct();

            foreach (var availability in availabilitys)
            {
                var dateTimes = availability.GetValueOrDefault(documentProfessional);
                if (!VerifyIfAvailabilityExistsAndDelete(dateTimes, dateTime))
                {

                }
            }

            var hairdresser = HairdresserProviderFake._hairdressers.Where(x => x.Document == documentProfessional).First();

            var name = Validations.FindFirstClaimOfType(user, ClaimTypes.Name);
            var phone = Validations.FindFirstClaimOfType(user, "Phone");
            var document = Validations.FindFirstClaimOfType(user, "Document");

            //Add marcação para o usuário
            var scheduleUser = new Dictionary<string, Scheduling>();
            scheduleUser.Add(document, new Scheduling
            {
                ProfessionalName = hairdresser.Name,
                ProfessionalPhone = hairdresser.Phone,
                SalonAdress = hairdresser.SaloonCode,
                DateTime = dateTime
            });
            _scheduleAppointment.Add(scheduleUser);

            //Add marcação para o profissional
            var scheduleHairdresser = new Dictionary<string, Models.Hairdresser.Scheduling>();
            scheduleHairdresser.Add(documentProfessional, new Models.Hairdresser.Scheduling
            {
                CustomerName = name,
                CustomerPhone = phone,
                DateTime = dateTime
            });
            HairdresserProviderFake._scheduleAppointment.Add(scheduleHairdresser);

            return Task.FromResult(new GenericResult
            {
                Message = "Scheduling done.",
                Success = true
            });
        }

        private bool VerifyIfAvailabilityExistsAndDelete(List<DateTime> dateTimes, DateTime dateTime)
        {
            var exists = false;

            foreach (var date in dateTimes)
            {
                if (date == dateTime) exists = true;
            }

            if (exists)
            {
                dateTimes.Remove(dateTime);
                return true;
            }

            return false;
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
