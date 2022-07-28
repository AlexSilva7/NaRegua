using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Providers.Fakes;

namespace NaRegua_API.Common.Dependency
{
    public class DependencyResolver
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public DependencyResolver(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
            SetDependency();
        }

        private void SetDependency()
        {
            var fakeProviders = bool.Parse(_configuration.GetSection("Providers").GetSection("UseFakeProviders").Value);
            if (fakeProviders)
            {
                _services.AddSingleton<IUserProvider, UserProviderFake>();
                _services.AddSingleton<IAuthProvider, AuthProviderFake>();
                _services.AddSingleton<IHairdresserProvider, HairdresserProviderFake>();
                _services.AddSingleton<ISaloonProvider, SaloonProviderFake>();
                _services.AddSingleton<ITokenProvider, TokenProviderFake>();
            }
        }
    }
}
