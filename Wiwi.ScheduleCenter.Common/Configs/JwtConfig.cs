namespace Wiwi.ScheduleCenter.Common.Configs
{
    public class JwtConfig
    {
        private string authPath;
        /// <summary>
        /// id4路径
        /// </summary>
        public string AuthPath
        {
            get
            {
                if (!string.IsNullOrEmpty(authPath) && authPath.EndsWith('/'))
                {
                    authPath = authPath.TrimEnd('/');
                }
                return authPath;
            }
            set { authPath = value; }

        }
        /// <summary>
        /// 客户端
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 是否需要SSL
        /// </summary>
        public bool IsSSL { get; set; } = false;
    }
}
