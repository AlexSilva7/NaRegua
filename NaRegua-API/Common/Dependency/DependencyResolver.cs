using Microsoft.Extensions.DependencyInjection;
using NaRegua_API.Common.Contracts;
using NaRegua_API.Configurations;
using NaRegua_API.Providers.Fakes;
using NaRegua_API.Providers.Implementations.V1.Token;

namespace NaRegua_API.Common.Dependency
{
    public class DependencyResolver
    {
        public static void SetDependency(IServiceCollection services)
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();
            if (AppSettings.UseFakeProviders)
            {
                services.AddSingleton<IUserProvider, UserProviderFake>();
                services.AddSingleton<IAuthProvider, AuthProviderFake>();
                services.AddSingleton<IHairdresserProvider, HairdresserProviderFake>();
                services.AddSingleton<ISaloonProvider, SaloonProviderFake>();
            }
        }
    }
}
