using NaRegua_Api.Common.Contracts;

namespace NaRegua_Api.Providers
{
    public class CheckOrderBackgroundService : BackgroundService
    {
        private readonly ILogger<CheckOrderBackgroundService> _logger;
        private readonly IUserProvider _userProvider;

        public CheckOrderBackgroundService(ILogger<CheckOrderBackgroundService> logger, IUserProvider userProvider)
        {
            _logger = logger;
            _userProvider = userProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _userProvider.CheckOpenOrdersAndUpdateUserBalances();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"CheckOrderBackgroundService :: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
