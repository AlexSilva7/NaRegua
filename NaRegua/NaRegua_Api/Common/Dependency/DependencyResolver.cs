using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Providers.Fakes;
using NaRegua_Api.Providers.Implementations;

namespace NaRegua_Api.Common.Dependency
{
    public class DependencyResolver
    {
        public static void SetDependency(IServiceCollection services)
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();
            if (AppSettings.UseFakeProviders)
            {
                services.AddSingleton<IUserProvider, UserProvider>();
                services.AddSingleton<IAuthProvider, AuthProvider>();
                services.AddSingleton<IHairdresserProvider, HairdresserProviderFake>();
                services.AddSingleton<ISaloonProvider, SaloonProviderFake>();
            }
            else
            {
                services.AddSingleton<IUserProvider, UserProvider>();
                services.AddSingleton<IAuthProvider, AuthProvider>();
                //services.AddSingleton<IHairdresserProvider, HairdresserProvider>();
                //services.AddSingleton<ISaloonProvider, SaloonProvider>();
            }
        }
    }
}
