using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;

namespace Wiwi.ScheduleCenter.Common.SqlSugar
{
    public static class SqlSugarExtensions
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, string ConnectionString)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new ArgumentNullException("sqlsugar config is null");

            services.TryAddSingleton(new ConnectionConfig
            {
                ConnectionString = ConnectionString,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true
            });

            services.TryAddTransient<IBaseQuery, BaseQuery>();
            return services;
        }
    }
}
