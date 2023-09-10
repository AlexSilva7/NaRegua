using Microsoft.Extensions.Hosting;
using NaRegua_Api.Common.Contracts;

namespace NaRegua_Api.Providers
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly IUserProvider _userProvider;

        public MyBackgroundService(ILogger<MyBackgroundService> logger, IUserProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Coloque aqui o código que deseja executar periodicamente

                try
                {
                    await _userProvider.CheckOpenOrdersAndUpdateUserBalances();
                }
                catch (Exception ex)
                {
                    var x = 0;
                }
                
                //_logger.LogInformation("Executando tarefa agendada...");

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken); // Intervalo de 1 hora
            }
        }
    }
}
