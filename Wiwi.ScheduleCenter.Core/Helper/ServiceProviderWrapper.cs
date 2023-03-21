using Microsoft.Extensions.DependencyInjection;
using Wiwi.ScheduleCenter.Common.Helper;
using Wiwi.ScheduleCenter.Core.Infrastructure;

namespace Wiwi.ScheduleCenter.Core.Helper
{
    public class ServiceProviderWrapper : IDisposable
    {
        protected IServiceScope scope;
        protected IServiceProvider provider;

        public ServiceProviderWrapper()
        {
            scope = ServiceProviderHelper.provider.CreateScope();
            provider = scope.ServiceProvider;
        }

        public T GetService<T>()
        {
            return provider.GetRequiredService<T>();
        }

        public virtual void Dispose()
        {
            scope.Dispose();
        }
    }

    public class ScopeDbContext : ServiceProviderWrapper
    {
        private ScheduleDbContext dbContext;

        public ScopeDbContext()
        {
            dbContext = scope.ServiceProvider.GetRequiredService<ScheduleDbContext>();
        }

        public ScheduleDbContext GetDbContext()
        {
            return dbContext;
        }

        public override void Dispose()
        {
            base.Dispose();
            dbContext.Dispose();
        }
    }
}
