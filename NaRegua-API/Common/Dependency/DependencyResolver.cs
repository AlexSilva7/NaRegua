using Microsoft.Extensions.DependencyInjection;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Configurations;
using NaRegua_API.Providers.Fakes;
using NaRegua_API.Providers.Implementations.V1.Token;

namespace NaRegua_API.Common.Dependency
{
    public class DependencyResolver
    {
        private readonly IServiceCollection _services;

        public DependencyResolver(IServiceCollection services)
        {
            _services = services;
            SetDependency();
        }

        private void SetDependency()
        {
            _services.AddSingleton<ITokenProvider, TokenProvider>();

            if (AppSettings.UseFakeProviders)
            {
                _services.AddSingleton<IUserProvider, UserProviderFake>();
                _services.AddSingleton<IAuthProvider, AuthProviderFake>();
                _services.AddSingleton<IHairdresserProvider, HairdresserProviderFake>();
                _services.AddSingleton<ISaloonProvider, SaloonProviderFake>();
            }
        }
    }
}
