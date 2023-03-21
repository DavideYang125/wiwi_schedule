using Microsoft.AspNetCore.Mvc.Filters;

namespace Wiwi.ScheduleCenter.Common.Attributes
{
    /// <summary>
    /// 跳过外层封装
    /// </summary>
    public class SkipResult : Attribute, IFilterMetadata
    {
    }
}
