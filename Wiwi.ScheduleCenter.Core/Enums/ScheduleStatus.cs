using System.ComponentModel;

namespace Wiwi.ScheduleCenter.Core.Enums
{
    /// <summary>
    /// 任务状态  已删除:-1  已停止:0  运行中:1  已停止:0
    /// </summary>
    public enum ScheduleStatus
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = -1,

        /// <summary>
        /// 已停止
        /// </summary>
        [Description("已停止")]
        Stop = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        [Description("运行中")]
        Running = 1,

        /// <summary>
        /// 已暂停
        /// </summary>
        [Description("已暂停")]
        Paused = 2
    }
}
