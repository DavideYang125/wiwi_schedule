using Coldairarrow.Util;

namespace Wiwi.ScheduleCenter.Common.Extension
{
    public static class StringExtensions
    {
        public static string ToSnowFlakeNoString(this string value)
        {
            return $"{value}{IdHelper.GetId()}";
        }

        public static string GetSnowFlakeIdString()
        {
            return IdHelper.GetId();
        }

        public static long GetSnowFlakeId()
        {
            return IdHelper.GetLongId();
        }
    }
}