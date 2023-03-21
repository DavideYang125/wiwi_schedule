using Coldairarrow.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;

namespace Wiwi.ScheduleCenter.Common.Extension
{
    public static class IHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                if(context == null) return webHost;
                
                try
                {
                    logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
                    var retry = Policy.Handle<Exception>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10),
                            TimeSpan.FromSeconds(15),
                        });

                    retry.Execute(() =>
                    {
                        //context.Database.EnsureCreated();
                        context.Database.Migrate();
                        seeder(context, services);
                    });

                    logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
                }
            }
            return webHost;

        }

        public static IServiceCollection AddSnowFlakeNoGenter(this IServiceCollection services, long workId = 1)
        {
            new IdHelperBootstrapper().SetWorkderId(workId).Boot();
            return services;
        }
    }
}
