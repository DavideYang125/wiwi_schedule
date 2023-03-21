using Wiwi.ScheduleCenter.Common.Model;

namespace Wiwi.ScheduleCenter.Api.Model
{
    public class SchedulePageListRequest : PageInfoRequest
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string ShcheduleName { get; set; }

        /// <summary>
        /// 任务状态  已删除:-1  已停止:0  运行中:1  已停止:0
        /// </summary>
        public int Status { get; set; }
    }
}
