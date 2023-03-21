using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Wiwi.ScheduleCenter.Core.QuartzHost.AppStart
{
    public class AppLifetimeHostedService : IHostedService
    {

        private readonly IHostApplicationLifetime _appLifetime;

        private readonly IConfiguration _configuration;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<AppLifetimeHostedService> _logger;

        public AppLifetimeHostedService(IHostApplicationLifetime appLifetime, IConfiguration configuration, IServiceProvider serviceProvider, ILogger<AppLifetimeHostedService> logger)
        {
            _appLifetime = appLifetime;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted service StopAsync");
            return Task.CompletedTask;
        }

        #region private

        private void OnStarted()
        {
            _logger.LogInformation("Hosted service OnStarted");
            QuartzManager.InitScheduler().Wait();
        }

        private void OnStopping()
        {
            _logger.LogInformation("Hosted service OnStopping");
            QuartzManager.Shutdown().Wait();
        }

        private void OnStopped()
        {
            _logger.LogInformation("Hosted service OnStopped");
        }

        #endregion
    }
}
