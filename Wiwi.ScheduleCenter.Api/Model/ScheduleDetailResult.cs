namespace Wiwi.ScheduleCenter.Api.Model
{
    public class ScheduleDetailResult
    {
        /// <summary>
        ///  
        ///</summary>
        public string ShcheduleId { get; set; }

        /// <summary>
        ///  
        ///</summary>
        public string ShcheduleName { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public string Remark { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public string CronExpression { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public int Status { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public DateTime? LastRunTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public DateTime? NextRunTime { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///  
        ///</summary>
        public string RequestUrl { get; set; }
        /// <summary>
        ///  
        ///</summary>
        public string Body { get; set; }
    }
}
