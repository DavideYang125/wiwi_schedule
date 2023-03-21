using SqlSugar;

namespace Wiwi.ScheduleCenter.Common.SqlSugar
{
    public class BaseQueryDbContext
    {
        public BaseQueryDbContext(ConnectionConfig connectionConfig)
        {
            Db = new SqlSugarClient(connectionConfig);

#if DEBUG
            //打印SQL 
            Db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                                  Db.Utilities.SerializeObject(
                                      pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine("###################SqlSugar#########################");
            };
#endif
        }

        /// <summary>
        /// 用来处理事务多表查询和复杂的操作
        /// </summary>
        public ISqlSugarClient Db { get; }
    }
}
