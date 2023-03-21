using SqlSugar;

namespace Wiwi.ScheduleCenter.Common.SqlSugar
{
    public interface IBaseQuery
    {
        IAdo Ado { get; }
        ISqlSugarClient Db { get; }
    }
}
