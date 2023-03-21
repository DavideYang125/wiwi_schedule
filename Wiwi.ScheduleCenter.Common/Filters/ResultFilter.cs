using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wiwi.ScheduleCenter.Common.Attributes;
using Wiwi.ScheduleCenter.Common.Enums;
using Wiwi.ScheduleCenter.Common.Model.Result;

namespace Wiwi.ScheduleCenter.Common.Filters
{
    /// <summary>
    /// 结果过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ResultFilter : Attribute, IResultFilter
    {
        private readonly ILogger<ResultFilter> _logger;

        public ResultFilter(ILogger<ResultFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 执行完成
        /// </summary>
        /// <param name="context"></param>
        public async void OnResultExecuted(ResultExecutedContext context)
        {
            context.HttpContext.Request.EnableBuffering();
            var requestInfo = await ReadRequest(context.HttpContext);
            _logger.LogInformation("请求日志：" + requestInfo);

            string responseInfo = context.Result != null ? JsonConvert.SerializeObject(context.Result) : "";
            _logger.LogInformation("响应日志：" + responseInfo);
        }

        /// <summary>
        /// 即将执行
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuting(ResultExecutingContext context)
        {
            bool isSkipResult = context.ActionDescriptor.FilterDescriptors.Any(o => o.Filter is SkipResult);
            if (isSkipResult)
                return;

            context.HttpContext.Request.EnableBuffering();

            #region 数据格式处理

            if (context.Result is BadRequestObjectResult jsonResult)
            {
                //不做处理
            }
            else
            {
                if (context.Result is ObjectResult objectResult)
                {
                    context.Result = new ObjectResult(new MessageOutput<object>()
                    {
                        response = objectResult.Value,
                        status = MessageStatus.Success
                    });
                }
                else if (context.Result is ContentResult contentResult)
                {
                    context.Result = new ObjectResult(
                        new MessageOutput<object>()
                        {
                            status = MessageStatus.Success,
                            response = contentResult.Content,
                        });
                }
                else if (context.Result is StatusCodeResult statusCodeResult)
                {
                    context.Result = new ObjectResult(
                        new MessageOutput()
                        {
                            status = MessageStatus.Success,
                        });
                }
            }

            #endregion
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string> ReadRequest(HttpContext context)
        {
            var request = context.Request;
            var url = request.GetEncodedUrl();
            var message = $"[请求地址]:{url}";
            if (!(request.ContentLength > 0)) return message;

            request.Body.Position = 0;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer).Replace("\r\n", "\n").Replace("\n", "");

            message += $@";[请求参数]:{bodyAsText}";
            return message;
        }
    }
}