namespace Wiwi.ScheduleCenter.Common.Configs
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; }
        public List<string> SlaveNodes { get; set; }
        public string Password { get; set; }
        public string PrefixKey { get; set; }
        public int DbNum { get; set; }
    }
}
