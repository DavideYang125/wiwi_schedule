namespace Wiwi.ScheduleCenter.Common.Configs
{
    public class CorsConfig
    {
        public string PolicyName { get; set; }
        public bool EnableAllIPs { get; set; }
        public string[] IPs { get; set; }
    }
}
