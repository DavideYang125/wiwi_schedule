using Microsoft.EntityFrameworkCore;
using SqlSugar;
using Wiwi.ScheduleCenter.Api.Model;
using Wiwi.ScheduleCenter.Common.Exceptions;
using Wiwi.ScheduleCenter.Common.Model;
using Wiwi.ScheduleCenter.Core.Domain;
using Wiwi.ScheduleCenter.Core.Enums;
using Wiwi.ScheduleCenter.Core.Infrastructure;
using Wiwi.ScheduleCenter.Core.QuartzHost;

namespace Wiwi.ScheduleCenter.Api.Serivces.Schedule
{
    /// <summary>
    /// ScheduleService
    ///</summary>
    public class ScheduleService : IScheduleService
    {
        private readonly ILogger<ScheduleService> _logger;
        private readonly ScheduleDbContext _db;

        public ScheduleService(ILogger<ScheduleService> logger,
             ScheduleDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        /// <summary>
        /// 根据主键获取实体
        ///</summary>
        public async Task<ScheduleDetailResult> GetDetailAsync(string id)
        {
            var schedule = await _db.Schedules.Where(x => x.ScheduleId == id && !x.Deleted).FirstOrDefaultAsync();
            if (schedule is null)
            {
                throw new CustomException("任务不存在或已被删除");
            }
            var result = new ScheduleDetailResult()
            {
                Body = schedule.Body,
                CronExpression = schedule.CronExpression,
                EndDate = schedule.EndDate,
                LastRunTime = schedule.LastRunTime,
                NextRunTime = schedule.NextRunTime,
                Remark = schedule.Remark,
                RequestUrl = schedule.RequestUrl,
                ShcheduleName = schedule.ShcheduleName,
                StartDate = schedule.StartDate,
                Status = schedule.Status,
            };
            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AddScheduleRequest request)
        {
            if (await _db.Schedules.AnyAsync(x => x.ShcheduleName == request.ShcheduleName && !x.Deleted))
            {
                _logger.LogError($"任务名称已存在，请更换名称");
                throw new CustomException("任务名称已存在，请更换名称");
            }

            var schedule = new ScheduleModel(request.ShcheduleName, request.Remark, request
                .CronExpression, (int)ScheduleStatus.Stop, request.StartDate, request.EndDate, request.RequestUrl, request.Body);
            schedule.SetCreater(string.Empty, string.Empty);
            await _db.Schedules.AddAsync(schedule);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"任务添加异常:{ex.ToString()}");
                throw new CustomException("任务添加异常");
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> UpadteAsync(UpdateScheduleRequest request)
        {
            var schedule = await _db.Schedules.Where(x => x.ScheduleId == request.ShcheduleId && !x.Deleted).FirstOrDefaultAsync();
            if (schedule is null)
            {
                throw new CustomException("任务不存在或已被删除");
            }

            if (schedule.Status != (int)ScheduleStatus.Stop)
            {
                throw new CustomException("在停止状态下才能编辑任务信息");
            }

            if (await _db.Schedules.AnyAsync(x => x.ScheduleId != request.ShcheduleId && x.ShcheduleName == request.ShcheduleName && !x.Deleted))
            {
                _logger.LogError($"任务名称已存在，请更换名称");
                throw new CustomException("任务名称已存在，请更换名称");
            }

            schedule.ShcheduleName = request.ShcheduleName;
            schedule.Remark = request.Remark;
            schedule.CronExpression = request.CronExpression;
            schedule.Body = request.Body;
            _db.Schedules.Update(schedule);
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"任务修改异常:{ex.ToString()}");
                throw new CustomException("任务修改异常");
            }
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> DeleteAsync(string id)
        {
            var schedule = await _db.Schedules.Where(x => x.ScheduleId == id && !x.Deleted).FirstOrDefaultAsync();
            if (schedule is null)
            {
                throw new CustomException("任务不存在或已被删除");
            }
            if (schedule.Status != (int)ScheduleStatus.Stop)
            {
                throw new CustomException("只有停止状态下的任务可以删除");
            }

            schedule.SetDelete();
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"任务删除异常:{ex.ToString()}");
                throw new CustomException("任务删除异常");
            }
            return true;
        }

        public async Task<bool> StopAsync(StopRequest request)
        {
            var schedule = await _db.Schedules.Where(x => x.ScheduleId == request.ScheduleId && !x.Deleted).FirstOrDefaultAsync();
            if (schedule is null)
            {
                throw new CustomException("任务不存在或已被删除");
            }
            if (schedule.Status == (int)ScheduleStatus.Stop)
            {
                throw new CustomException("任务已经处于停止状态");
            }
            bool success = await QuartzManager.Stop(request.ScheduleId);
            schedule.SetStop();

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"任务停止异常:{ex.ToString()}");
                throw new CustomException("任务停止异常");
            }
            return true;
        }

        public async Task<bool> StartAsync(StartRequest request)
        {
            bool exist = await QuartzManager.CheckExist(request.ScheduleId);

            var schedule = await _db.Schedules.Where(x => x.ScheduleId == request.ScheduleId && !x.Deleted).FirstOrDefaultAsync();
            if (schedule is null)
            {
                throw new CustomException("任务不存在或已被删除");
            }
            if (schedule.Status != (int)ScheduleStatus.Stop)
            {
                throw new CustomException("任务在停止状态下才能启动");
            }

            if (schedule.EndDate.HasValue && schedule.EndDate < DateTime.Now)
            {
                throw new CustomException("任务结束时间不能小于当前时间");
            }

            bool success = await QuartzManager.Start(request.ScheduleId);
            schedule.SetStart();

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"任务启动异常:{ex.ToString()}");
                throw new CustomException("任务启动异常");
            }
            return true;
        }

        /// <summary>
        /// 分页获取数据 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetQueryPageResult<ScheduleDetailResult>> GetPageListAsync(SchedulePageListRequest request)
        {
            if (request.PageIndex <= 0) request.PageIndex = 1;
            var totalCount = _db.Schedules.Where(x => !x.Deleted)
                .WhereIF(!string.IsNullOrEmpty(request.ShcheduleName), x => x.ShcheduleName.Contains(request.ShcheduleName))
                .WhereIF(request.Status != -2, x => x.Status == request.Status).Count();

            var schedules = _db.Schedules.Where(x => !x.Deleted)
                .WhereIF(!string.IsNullOrEmpty(request.ShcheduleName), x => x.ShcheduleName.Contains(request.ShcheduleName))
                .WhereIF(request.Status != -2, x => x.Status == request.Status)
                .Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageIndex).Select(x => new ScheduleDetailResult()
                {
                    ShcheduleId = x.ScheduleId,
                    Body = x.Body,
                    CronExpression = x.CronExpression,
                    EndDate = x.EndDate,
                    LastRunTime = x.LastRunTime,
                    NextRunTime = x.NextRunTime,
                    Remark = x.Remark,
                    RequestUrl = x.RequestUrl,
                    ShcheduleName = x.ShcheduleName,
                    StartDate = x.StartDate,
                    Status = x.Status,
                }).ToList();

            var result = new GetQueryPageResult<ScheduleDetailResult>()
            {
                PageInfo = new PageInfoResult() { PageSize = request.PageSize, PageIndex = request.PageIndex, TotalCount = totalCount },
                Rows = schedules
            };
            return result;
        }
    }
}