using NaRegua_Api.Common.Contracts;
using NaRegua_Api.Configurations;
using NaRegua_Api.Providers.Fakes;
using NaRegua_Api.Providers.Implementations;
using NaRegua_Api.Repository.Contracts;
using NaRegua_Api.Repository.Firebase;
using NaRegua_Api.Repository.SQLServer.ActiveSessionRepository;

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
            else
            {
                if (AppSettings.Database.ToLower() == "sqlserver")
                {
                    services.AddSingleton<IAuthRepository, AuthRepository>();
                    services.AddSingleton<IUserRepository, UserRepository>();
                    services.AddSingleton<IHairdresserRepository, HairdresserRepository>();
                    services.AddSingleton<ISaloonRepository, SaloonRepository>();
                }
                else
                {
                    services.AddSingleton<IAuthRepository, AuthRepositoryFirebase>();
                    services.AddSingleton<IUserRepository, UserRepositoryFirebase>();
                    services.AddSingleton<IHairdresserRepository, HairdresserRepository>();
                    services.AddSingleton<ISaloonRepository, SaloonRepository>();
                }

                services.AddSingleton<IUserProvider, UserProvider>();
                services.AddSingleton<IAuthProvider, AuthProvider>();
                services.AddSingleton<IHairdresserProvider, HairdresserProvider>();
                services.AddSingleton<ISaloonProvider, SaloonProvider>();
            }
        }
    }
}
