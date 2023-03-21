using Microsoft.Extensions.DependencyInjection;

namespace Wiwi.ScheduleCenter.Core.QuartzHost.AppStart
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHost(this IServiceCollection services)
        {
            services.AddHostedService<AppStart.AppLifetimeHostedService>();
            return services;
        }
    }
}
