using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Providers.Fakes;
using NaRegua_Api.Providers.Implementations.V1.Token;

namespace NaRegua_Api.Common.Dependency
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
