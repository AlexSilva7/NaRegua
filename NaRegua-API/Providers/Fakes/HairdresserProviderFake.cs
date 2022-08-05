using NaRegua_API.Common.Contracts;
using NaRegua_API.Common.Validations;
using NaRegua_API.Models.Auth;
using NaRegua_API.Models.Generics;
using NaRegua_API.Models.Hairdresser;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaRegua_API.Providers.Fakes
{
    public class HairdresserProviderFake : IHairdresserProvider
    {
        public static List<Hairdresser> hairdressers;
        private readonly ISaloonProvider _saloonProvider;

        public HairdresserProviderFake(ISaloonProvider salonProvider)
        {
            hairdressers = new List<Hairdresser>();
            _saloonProvider = salonProvider;
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

            hairdresser.Password = Criptograph.HashPass(hairdresser.Password);

            hairdressers.Add(hairdresser);

            return Task.FromResult(new GenericResult
            {
                Message = "Professional registered successfully",
                Success = true
            });
        }

        public Task<GenericResult> SendWorkAvailabilityAsync(WorkAvailability availability)
        {
            throw new System.NotImplementedException();
        }
    }
}
