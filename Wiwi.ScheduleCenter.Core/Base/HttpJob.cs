namespace Wiwi.ScheduleCenter.Core.Base
{
    /// <summary>
    /// http任务的入口
    /// </summary>
    public class HttpJob : RootJob
    {
        /// <summary>
        /// 执行任务ing
        /// </summary>
        /// <param name="context"></param>
        public override void OnExecuting(TaskContext context)
        {
            context.InstanceRun();
        }

        /// <summary>
        /// 执行完成
        /// </summary>
        /// <param name="context"></param>
        public override void OnExecuted(TaskContext context)
        {

        }
    }
}
