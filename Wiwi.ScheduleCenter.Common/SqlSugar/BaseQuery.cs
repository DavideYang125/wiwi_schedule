using SqlSugar;

namespace Wiwi.ScheduleCenter.Common.SqlSugar
{
    public class BaseQuery : BaseQueryDbContext, IBaseQuery
    {
        public BaseQuery(ConnectionConfig connectionConfig) : base(connectionConfig)
        {
            //Db.GetDbExternal();
        }

        public IAdo Ado => Db.Ado;
    }
}
