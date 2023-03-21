using System.ComponentModel.DataAnnotations.Schema;

namespace Wiwi.ScheduleCenter.Core.Base
{
    public class ScheduleOption
    {
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
    }
}
