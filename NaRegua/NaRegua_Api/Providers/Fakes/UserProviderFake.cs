using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Common.Enums;
using NaRegua_Api.Common.Validations;
using NaRegua_Api.Configurations;
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
        private readonly IOrderProvider _orderProvider;

        public static List<User> _users = new List<User>();
        public static Dictionary<string, List<Scheduling>> _scheduleAppointment = new Dictionary<string, List<Scheduling>>();
        public static Dictionary<string, List<Saloon>> _userFavoriteSaloons = new Dictionary<string, List<Saloon>>();
        public static Dictionary<string, List<OpenDepositOrders>> _userOpenDepositOrders = new Dictionary<string, List<OpenDepositOrders>>();
        public static Dictionary<string, Dictionary<string, decimal>> _userAccountBalance = new Dictionary<string, Dictionary<string, decimal>>();

        public UserProviderFake(IHairdresserProvider hairdresserProvider, ISaloonProvider saloonProvider, IOrderProvider orderProvider)
        {
            _hairdresserProvider = hairdresserProvider;
            _saloonProvider = saloonProvider;
            _orderProvider = orderProvider;
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
            _userAccountBalance.Add(user.Id, new Dictionary<string, decimal>
            {
                { user.Document, 0 }
            });

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
            var accountId = Validations.FindFirstClaimOfType(user, "AccountId");
            var document = Validations.FindFirstClaimOfType(user, "Document");

            if (_userAccountBalance[accountId][document] < AppSettings.CostOfService)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "Insufficient balance to schedule service",
                    Success = false
                });
            }

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

            var schedules = GetAppointmentAsync(user).Result.Resources;

            //Verifica se o usuário já tem 4 marcações em 10 dias
            var countSchedules = schedules.Where(x => x.DateTime.Date <= DateTime.Now.AddDays(10)).Count();
            if(countSchedules >= 4)
            {
                return Task.FromResult(new GenericResult
                {
                    Message = "user already has 10 appointments this month, it is not possible to schedule any more appointments.",
                    Success = false
                });
            }

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

            var hairdresser = _hairdresserProvider.GetHairdressersFromDocument(documentProfessional);
            var saloon = _saloonProvider.GetSaloon(hairdresser.SaloonCode);

            var orderId = Guid.NewGuid().ToString();

            var scheduling = new Scheduling
            {
                OrderId = orderId,
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

            //Remove horário disponível para o profissional e adiciona o compromisso
            _hairdresserProvider.SetAppointmentsFromTheProfessional(orderId, user, documentProfessional, dateTime);

            _scheduleAppointment.TryAdd(document, list);

            _userAccountBalance[accountId][document] -= AppSettings.CostOfService;

            _hairdresserProvider.MakePayment(documentProfessional, AppSettings.CostOfService);

            return Task.FromResult(new GenericResult
            {
                Message = orderId,
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

        public Task<GenericResult> DepositFundsAsync(IPrincipal user, DepositInfo deposit)
        {
            var accountId = Validations.FindFirstClaimOfType(user, "AccountId");
            var document = Validations.FindFirstClaimOfType(user, "Document");

            var orderId = Guid.NewGuid().ToString();

            var depositOrders = new OpenDepositOrders
            {
                AccountId = accountId,
                DocumentUser = document,
                OrderId = orderId,
                PaymentType = deposit.PaymentType,
                CardNumber = deposit.CardNumber,
                Value = deposit.Value,
            };

            if (_userOpenDepositOrders.ContainsKey(document))
            {
                _userOpenDepositOrders[document].Add(depositOrders);
            }
            else
            {
                _userOpenDepositOrders.Add(document, new List<OpenDepositOrders>
                {
                    { depositOrders }
                });
            }
            
            _orderProvider.SetPaymentOrder(orderId, deposit.PaymentType, deposit.CardNumber);

            return Task.FromResult(new GenericResult
            {
                Message = "Order placed, awaiting payment confirmation.",
                Success = true
            });
        }

        public Task<AccountBalanceResult> GetAccountBalanceAsync(IPrincipal user)
        {
            var accountId = Validations.FindFirstClaimOfType(user, "AccountId");
            var document = Validations.FindFirstClaimOfType(user, "Document");

            var value = _userAccountBalance[accountId][document];

            return Task.FromResult(new AccountBalanceResult
            {
                Balance = value,
                Success = true
            });
        }

        public async Task CheckOpenOrdersAndUpdateUserBalances()
        {
            var objRemovePendingPaymentOrders = new List<OpenDepositOrders>();

            foreach(var order in _userOpenDepositOrders)
            {
                foreach (var deposit in order.Value)
                {
                    var accountId = deposit.AccountId;
                    var orderId = deposit.OrderId;
                    var depositValue = deposit.Value;
                    var userDocument = deposit.DocumentUser;

                    var status = await _orderProvider.GetPaymentOrderStatus(orderId);

                    if(status == OrderStatus.Confirmed)
                    {
                        try
                        {
                            _userAccountBalance[accountId][userDocument] += depositValue;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if(status != OrderStatus.PendingPayment)
                    {
                        objRemovePendingPaymentOrders.Add(deposit);
                    }
                }
            }

            foreach (var order in objRemovePendingPaymentOrders)
            {
                _userOpenDepositOrders[order.DocumentUser].Remove(order);
            }
        }

        private class RegisteredResult
        {
            public string Message { get; set; }
            public bool Registered { get; set; }
        }
    }
}
