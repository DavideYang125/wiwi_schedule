namespace Wiwi.ScheduleCenter.Core.Base
{
    /// <summary>
    /// 任务运行时的上下文
    /// </summary>
    public class TaskContext
    {
        private readonly TaskBase _instance;

        public TaskContext(TaskBase instance)
        {
            _instance = instance;
        }

        /// <summary>
        /// 运行轨迹
        /// </summary>
        public Guid TraceId { private get; set; }

        /// <summary>
        /// 自定义参数
        /// </summary>
        public Dictionary<string, object> ParamsDict { private get; set; }

        /// <summary>
        /// 前置任务的运行结果
        /// </summary>
        public object PreviousResult { get; set; }

        /// <summary>
        /// 本次运行的返回结果
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// 获取自定义参数字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetArgument<T>(string name)
        {
            if (ParamsDict == null)
            {
                return default;
            }
            try
            {
                object value;
                ParamsDict.TryGetValue(name, out value);
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public void InstanceRun()
        {
            _instance.InnerRun(this);
        }

    }
}
