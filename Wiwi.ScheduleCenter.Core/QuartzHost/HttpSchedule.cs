using Wiwi.ScheduleCenter.Core.Base;
using Wiwi.ScheduleCenter.Core.Domain;
using Wiwi.ScheduleCenter.Core.Helper;

namespace Wiwi.ScheduleCenter.Core.QuartzHost
{
    public class HttpSchedule
    {
        public ScheduleModel Main { get; set; }
        public Dictionary<string, object> CustomParams { get; set; }

        public TaskBase RunnableInstance { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public void CreateRunnableInstance(ScheduleContext context)
        {
            RunnableInstance = new HttpTask(context.Schedule);
        }

        public Type GetQuartzJobType()
        {
            return typeof(HttpJob);
        }

        public void Dispose()
        {
            RunnableInstance.Dispose();
            RunnableInstance = null;
        }
    }

    public class HttpTask : TaskBase
    {
        private readonly ScheduleOption _option;

        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private const string HEADER_TIMEOUT = "sm-timeout";

        private readonly Dictionary<string, object> _headers = new Dictionary<string, object>();


        public HttpTask(ScheduleModel httpOption)
        {
            if (httpOption != null)
            {
                _option = new ScheduleOption();

                string requestBody = string.Empty;
                string url = httpOption.RequestUrl;
                //if (httpOption.ContentType == "application/json")
                //{
                //    requestBody = httpOption.Body?.Replace("\r\n", "");
                //}
                requestBody = httpOption.Body?.Replace("\r\n", "");
                _option.RequestUrl = url;
                _option.Body = requestBody;
            }
        }


        public override void Run(TaskContext context)
        {
            if (_option == null) return;
            //context.WriteLog($"即将请求：{_option.RequestUrl}");

            DoRequest(context).Wait(CancellationToken);

        }

        private async Task DoRequest(TaskContext context)
        {
            using (var scope = new ScopeDbContext())
            {
                var httpClient = scope.GetService<IHttpClientFactory>().CreateClient();

                foreach (var item in _headers)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value.ToString());
                }

                httpClient.Timeout = _timeout;

                var httpRequest = new HttpRequestMessage
                {
                    Content = new StringContent(_option.Body ?? string.Empty, System.Text.Encoding.UTF8, "application/json"),
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(_option.RequestUrl)
                };

                var response = await httpClient.SendAsync(httpRequest, CancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"任务执行完成，响应码：{response.StatusCode.GetHashCode().ToString()}");
                }
                response.Dispose();
            }
        }
    }
}
