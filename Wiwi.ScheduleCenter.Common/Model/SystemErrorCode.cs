using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiwi.ScheduleCenter.Common.Model
{
    /// <summary>
    /// 系统错误码
    /// </summary>
    public enum SystemErrorCode
    {
        /// <summary>
        /// 登录超时
        /// </summary>
        [Description("登录超时")] LoginExpress = 10001,

        /// <summary>
        /// 没有操作权限
        /// </summary>
        [Description("没有操作权限")] NoOperateAuth = 10003,

        /// <summary>
        /// 参数验证错误
        /// </summary>
        [Description("参数验证错误")] ModelValidateError = 90001,

        /// <summary>
        /// 请求失败
        /// </summary>
        [Description("请求失败")] Fail = 001000,

        /// <summary>
        /// 系统异常
        /// </summary>
        [Description("系统异常")] SystemError = 90002
    }
}
