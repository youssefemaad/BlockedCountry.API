using Core.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlockedCountriesAPI.Services
{
    public class ExpiredBlocksCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredBlocksCleanupService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public ExpiredBlocksCleanupService(
            IServiceProvider serviceProvider,
            ILogger<ExpiredBlocksCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Expired blocks cleanup service is starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoCleanupAsync();
                    _logger.LogInformation("Expired blocks cleanup completed");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during expired blocks cleanup");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
        private async Task DoCleanupAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IBlockedCountryRepository>();
            await Task.Run(() => repository.CleanupExpired());
        }
    }
}
