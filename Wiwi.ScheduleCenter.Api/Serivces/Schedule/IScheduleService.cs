using Wiwi.ScheduleCenter.Api.Model;
using Wiwi.ScheduleCenter.Common.Model;

namespace Wiwi.ScheduleCenter.Api.Serivces.Schedule
{
    public interface IScheduleService
    {
        Task<bool> AddAsync(AddScheduleRequest request);
        Task<bool> DeleteAsync(string id);
        Task<ScheduleDetailResult> GetDetailAsync(string id);
        Task<GetQueryPageResult<ScheduleDetailResult>> GetPageListAsync(SchedulePageListRequest request);
        Task<bool> StartAsync(StartRequest request);
        Task<bool> StopAsync(StopRequest request);
        Task<bool> UpadteAsync(UpdateScheduleRequest request);
    }
}