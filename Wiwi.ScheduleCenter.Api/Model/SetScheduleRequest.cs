namespace Wiwi.ScheduleCenter.Api.Model
{
    public class AddScheduleRequest
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string ShcheduleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 时间表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 请求url
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Body { get; set; }
    }

    public class UpdateScheduleRequest
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public string ShcheduleId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string ShcheduleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 时间表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Body { get; set; }
    }
}
