using Wiwi.ScheduleCenter.Common.Helper;

namespace Wiwi.ScheduleCenter.Common.Model.Result
{
    /// <summary>
    /// api 返回结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }

        public ApiResult()
        {
        }

        public ApiResult(SystemErrorCode code)
        {
            Code = code.GetHashCode();
            Message = EnumHelper.GetEnumDesc(code);
        }

        public ApiResult(SystemErrorCode code, string message)
        {
            Code = code.GetHashCode();
            Message = message;
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// api 返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}