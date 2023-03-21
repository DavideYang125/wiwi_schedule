using Microsoft.Extensions.Configuration;

namespace Wiwi.ScheduleCenter.Core.Base
{
    /// <summary>
    /// 任务抽象基类
    /// </summary>
    public abstract class TaskBase : MarshalByRefObject, IDisposable
    {

        private bool _isRunning = false;

        /// <summary>
        /// 任务id，创建实例时赋值，方便写log或其他操作时跟踪
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 自定义配置信息，使用前必须加载过配置文件
        /// </summary>
        public IConfigurationRoot Configuration { get; private set; }

        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// 这里可以执行一些初始化操作，比如加载自己的配置文件
        /// </summary>
        /// <param name="context"></param>
        public virtual void Initialize()
        {
            ///TODO:
        }

        /// <summary>
        /// 任务执行的方法，由具体任务去重写实现
        /// </summary>
        public abstract void Run(TaskContext context);

        /// <summary>
        /// 停止任务后可能需要的处理
        /// </summary>
        public virtual void Dispose()
        {
            ///TODO:
        }

        /// <summary>
        /// 保证前一次运行完才开始下一次，否则就跳过本次执行
        /// </summary>
        internal void InnerRun(TaskContext context)
        {
            if (!_isRunning)
            {
                _isRunning = true;
                try
                {
                    Run(context);
                }
                //catch (Exception err)
                //{
                //    throw err;//这里不再抛一次，保留原始堆栈信息
                //}
                finally
                {
                    _isRunning = false;
                }
            }
            else
            {
                new RunConflictException("互斥跳过");
            }
        }
    }
}
