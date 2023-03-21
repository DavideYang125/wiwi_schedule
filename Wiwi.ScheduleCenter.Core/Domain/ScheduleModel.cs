using System.ComponentModel.DataAnnotations.Schema;
using Wiwi.ScheduleCenter.Common.Extension;
using Wiwi.ScheduleCenter.Core.Domain.Base;
using Wiwi.ScheduleCenter.Core.Enums;

namespace Wiwi.ScheduleCenter.Core.Domain
{
    public class ScheduleModel : EntityWithOperatorBase
    {
        public ScheduleModel()
        {

        }

        public ScheduleModel( string shcheduleName, string remark, string cronExpression, int status, DateTime? startDate, DateTime? endDate, string requestUrl, string body) : base()
        {
            ShcheduleName = shcheduleName;
            Remark = remark;
            CronExpression = cronExpression;
            Status = status;
            StartDate = startDate;
            EndDate = endDate;
            RequestUrl = requestUrl;
            Body = body;
        }

        /// <summary>
        /// 任务Id
        /// </summary>
        [Column("schedule_id")]
        public string ScheduleId { get; set; } = StringExtensions.GetSnowFlakeIdString();

        /// <summary>
        /// 任务名称
        /// </summary>
        [Column("schedule_name")]
        public string ShcheduleName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [Column("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        [Column("cron_expression")]
        public string CronExpression { get; set; }

        /// <summary>
        /// 任务状态
        /// 任务状态  已删除:-1  已停止:0  运行中:1  已停止:0
        /// ScheduleStatus
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 上次运行时间
        /// </summary>
        [Column("last_run_time")]
        public DateTime? LastRunTime { get; set; }

        /// <summary>
        /// 下次运行时间
        /// </summary>
        [Column("next_run_time")]
        public DateTime? NextRunTime { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 失效日期
        /// </summary>
        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Column("deleted")]
        public bool Deleted { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [Column("request_url")]
        public string RequestUrl { get; set; }

        /// <summary>
        /// 数据内容（json格式）
        /// </summary>
        [Column("body")]
        public string Body { get; set; }

        public void SetDelete()
        {
            Status = (int)ScheduleStatus.Deleted;
            NextRunTime = null;
        }

        public void SetStop()
        {
            Status = (int)ScheduleStatus.Stop;
            NextRunTime = null;
        }
        public void SetStart()
        {
            Status = (int)ScheduleStatus.Running;
            NextRunTime = null;
        }
    }
}
