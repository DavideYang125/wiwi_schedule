namespace Wiwi.ScheduleCenter.Common.Model
{
    public class PageInfoRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 10;
    }


    public abstract class PageInfo
    {
        /// <summary>
        /// 每页多少条记录
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 总共多少页
        /// </summary>
        public long TotalPage { get; set; }
    }

    public class PageInfoResult : PageInfo
    {
        public PageInfoResult()
        {
        }

        public PageInfoResult(int pageIndex, int pageSize, long totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPage = totalCount == 0 ? 0 : (long)Math.Ceiling(totalCount * 1.0 / pageSize);
        }

        /// <summary>
        /// 一共多少条记录
        /// </summary>
        public long TotalCount { get; set; }
    }

    public class GetQueryPageResult<T>
    {
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<T> Rows { get; set; } = new List<T>(0);

        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfoResult PageInfo { get; set; } = new PageInfoResult();
    }
}
