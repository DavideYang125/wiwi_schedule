using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Wiwi.ScheduleCenter.Api.Model;
using Wiwi.ScheduleCenter.Api.Serivces.Schedule;
using Wiwi.ScheduleCenter.Common.Model;

namespace Wiwi.ScheduleCenter.Api.Controllers
{
    /// <summary>
    /// 任务调度
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IScheduleService _service;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleService service)
        {
            _logger = logger;
            _service = service;
        }
        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("detail/{id}")]
        public async Task<ScheduleDetailResult> GetDetailAsync(string id)
        {
            return await _service.GetDetailAsync(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [Description("新增")]
        [HttpPost]
        public async Task<bool> Add(AddScheduleRequest request)
        {
            return await _service.AddAsync(request);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        [Description("更新")]
        [HttpPost]
        public async Task<bool> Edit(UpdateScheduleRequest request)
        {
            return await _service.UpadteAsync(request);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [Description("删除任务")]
        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(string id)
        {
            return await _service.DeleteAsync(id);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <returns></returns>
        [Description("停止任务")]
        [HttpPost]
        public async Task<bool> StopAsync(StopRequest request)
        {
            return await _service.StopAsync(request);
        }

        /// <summary>
        /// 启动任务
        /// </summary>
        /// <returns></returns>
        [Description("启动任务")]
        [HttpPost]
        public async Task<bool> StartAsync(StartRequest request)
        {
            return await _service.StartAsync(request);
        }

        /// <summary>
        /// 分页获取任务列表
        /// </summary>
        /// <returns></returns>
        [Description("分页获取数据")]
        [HttpGet]
        public async Task<GetQueryPageResult<ScheduleDetailResult>> GetPageList(SchedulePageListRequest request)
        {
            return await _service.GetPageListAsync(request);
        }
    }
}