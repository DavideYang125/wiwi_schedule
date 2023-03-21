using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Wiwi.ScheduleCenter.Common.Exceptions;
using Wiwi.ScheduleCenter.Common.Helper;
using Wiwi.ScheduleCenter.Core.Domain;
using Wiwi.ScheduleCenter.Core.Enums;
using Wiwi.ScheduleCenter.Core.Helper;
using Wiwi.ScheduleCenter.Core.Infrastructure;

namespace Wiwi.ScheduleCenter.Core.QuartzHost
{
    public class QuartzManager
    {
        private static ILogger<QuartzManager> _logger;

        public QuartzManager(ILogger<QuartzManager> logger)
        {
            var logd = LoggerFactory.Create(builder =>
            {
                //builder.AddConsole();
                //builder.AddDebug();
            }).CreateLogger<QuartzManager>();
            _logger = logd;
        }

        private static IScheduler _scheduler = null;

        /// <summary>
        /// 初始化调度系统
        /// </summary>
        /// <returns></returns>
        public static async Task InitScheduler()
        {
            if (_scheduler is null || _scheduler.IsShutdown)
            {
                ISchedulerFactory factory = new StdSchedulerFactory();
                _scheduler = await factory.GetScheduler();
            }
            await _scheduler.Start();
            await _scheduler.Clear();

            //恢复任务
            RuningRecovery();
        }

        /// <summary>
        /// 关闭调度系统
        /// </summary>
        /// <returns></returns>
        public static async Task Shutdown()
        {
            try
            {
                if (_scheduler != null && !_scheduler.IsShutdown)
                {
                    await _scheduler.Clear();
                    await _scheduler.Shutdown();
                }
                Console.WriteLine("任务调度服务已经停止");
                //_logger.LogInformation("任务调度服务已经停止");
            }
            catch (Exception ex)
            {
                //_logger.LogError($"任务调度服务关闭失败", ex.ToString());
            }
        }

        public static async Task<bool> CheckExist(string sid)
        {
            var jobKey = new JobKey(sid.ToLower());
            if (await _scheduler.CheckExists(jobKey))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 启动一个任务
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static async Task<bool> Start(string sid)
        {
            var jobKey = new JobKey(sid.ToLower());
            if (await _scheduler.CheckExists(jobKey))
            {
                return true;
            }
            ScheduleContext context = GetScheduleContext(sid);
            HttpSchedule httpSchedule = new HttpSchedule();
            httpSchedule.Main = context.Schedule;
            httpSchedule.CancellationTokenSource = new System.Threading.CancellationTokenSource();
            httpSchedule.CreateRunnableInstance(context);
            httpSchedule.RunnableInstance.TaskId = context.Schedule.ScheduleId;
            httpSchedule.RunnableInstance.CancellationToken = httpSchedule.CancellationTokenSource.Token;
            httpSchedule.RunnableInstance.Initialize();
            try
            {
                await Start(httpSchedule);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"任务{sid}启动失败。异常：{ex.ToString()}");
                //_logger.LogError($"任务{sid}启动失败。异常：{ex.ToString()}");
                return false;
            }
        }

        #region private

        private static async Task Start(HttpSchedule schedule)
        {
            JobDataMap map = new JobDataMap
            {
                new KeyValuePair<string, object> ("instance",schedule),
            };
            string jobKey = schedule.Main.ScheduleId.ToString();
            try
            {
                IJobDetail job = JobBuilder.Create().OfType(schedule.GetQuartzJobType()).WithIdentity(jobKey).UsingJobData(map).Build();

                //添加监听器
                var listener = new JobRunListener(jobKey);
                listener.OnSuccess += StartedEvent;
                _scheduler.ListenerManager.AddJobListener(listener, KeyMatcher<JobKey>.KeyEquals(new JobKey(jobKey)));

                var trigger = GetTrigger(schedule.Main);
                await _scheduler.ScheduleJob(job, trigger, schedule.CancellationTokenSource.Token);

                using (var scope = new ScopeDbContext())
                {
                    var db = scope.GetDbContext();
                    var task = await db.Schedules.Where(x => x.ScheduleId == schedule.Main.ScheduleId).FirstOrDefaultAsync();
                    if (task != null)
                    {
                        task.NextRunTime = TimeZoneInfo.ConvertTimeFromUtc(trigger.GetNextFireTimeUtc().Value.UtcDateTime, TimeZoneInfo.Local);
                        await db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex) { throw new SchedulerException(ex); }
            try
            {
                Console.WriteLine($"任务[{schedule.Main.ShcheduleName}]启动成功，任务Id：{schedule.Main.ScheduleId}！");
                //_logger.LogInformation($"任务[{schedule.Main.ShcheduleName}]启动成功，任务Id：{schedule.Main.ScheduleId}！");
            }
            catch (Exception ex) { 
            
            }
        }

        private static void RuningRecovery()
        {
            using (var scope = ServiceProviderHelper.provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ScheduleDbContext>();
                var idList = db.Set<ScheduleModel>().Where(x => x.Status == (int)ScheduleStatus.Running && x.Deleted).Select(x => x.ScheduleId).ToList();
                idList.AsParallel().ForAll(async sid =>
                {
                    try { await Start(sid); } catch { }
                });
            }
        }

        private static void StartedEvent(string sid, DateTime? nextRunTime)
        {
            using (var scope = new ScopeDbContext())
            {
                var db = scope.GetDbContext();
                //每次运行成功后更新任务的运行情况
                var task = db.Schedules.FirstOrDefault(x => x.ScheduleId == sid);
                if (task == null) return;
                task.LastRunTime = DateTime.Now;
                task.NextRunTime = nextRunTime;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 停止一个任务
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        public static async Task<bool> Stop(string sid)
        {
            JobKey jk = new JobKey(sid.ToLower());
            var job = await _scheduler.GetJobDetail(jk);
            if (job != null)
            {
                CancellationToken token = default;
                var instance = job.JobDataMap["instance"] as HttpSchedule;
                if (instance != null)
                {
                    instance.RunnableInstance?.Dispose();
                    instance.Dispose();
                    token = instance.CancellationTokenSource.Token;
                }
                var trigger = new TriggerKey(sid.ToString());
                await _scheduler.PauseTrigger(trigger, token);
                await _scheduler.UnscheduleJob(trigger, token);
                await _scheduler.DeleteJob(jk, token);
                _scheduler.ListenerManager.RemoveJobListener(sid.ToString());
                instance?.CancellationTokenSource.Cancel();
            }
            return true;
        }

        private static ITrigger GetTrigger(ScheduleModel model)
        {
            string jobKey = model.ScheduleId.ToString();
            if (!CronExpression.IsValidExpression(model.CronExpression))
            {
                throw new Exception("cron表达式验证失败");
            }
            CronTriggerImpl trigger = new CronTriggerImpl
            {
                CronExpressionString = model.CronExpression,
                Name = model.ShcheduleName,
                Key = new TriggerKey(jobKey),
                Description = model.Remark,
                MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing
            };
            if (model.StartDate.HasValue)
            {
                if (model.StartDate.Value < DateTime.Now) model.StartDate = DateTime.Now;
                trigger.StartTimeUtc = TimeZoneInfo.ConvertTimeToUtc(model.StartDate.Value);
            }
            if (model.EndDate.HasValue)
            {
                trigger.EndTimeUtc = TimeZoneInfo.ConvertTimeToUtc(model.EndDate.Value);
            }
            return trigger;
        }

        private static ScheduleContext GetScheduleContext(string sid)
        {
            using (var scope = ServiceProviderHelper.provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ScheduleDbContext>();
                var model = db.Schedules.Where(x => x.ScheduleId == sid && x.Status != (int)ScheduleStatus.Deleted).FirstOrDefault();
                if (model is null)
                {
                    Console.WriteLine($"启动任务时，任务不存在，任务Id:{sid}");
                    //_logger.LogError($"启动任务时，任务不存在，任务Id:{sid}");
                    throw new CustomException($"启动任务时，任务不存在，任务Id:{sid}");
                }
                ScheduleContext context = new ScheduleContext() { Schedule = model };
                return context;
            }
        }

        #endregion
    }

    /// <summary>
    /// 任务监听器
    /// </summary>
    internal class JobRunListener : IJobListener
    {
        public delegate void SuccessEventHandler(string sid, DateTime? nextTime);

        public string Name { get; set; }
        public event SuccessEventHandler OnSuccess;


        public JobRunListener()
        {
        }

        public JobRunListener(string name)
        {
            Name = name;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken)
        {
            IJobDetail job = context.JobDetail;
            var instance = job.JobDataMap["instance"] as HttpSchedule;

            if (jobException == null)
            {
                var utcDate = context.Trigger.GetNextFireTimeUtc();
                DateTime? nextTime = utcDate.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(utcDate.Value.DateTime, TimeZoneInfo.Local) : new DateTime?();
                OnSuccess(job.Key.Name, nextTime);
            }
            return Task.FromResult(0);
        }
    }
}
