using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Reflection;
using Wiwi.ScheduleCenter.Api;
using Wiwi.ScheduleCenter.Common.Extension;
using Wiwi.ScheduleCenter.Common.Filters;
using Wiwi.ScheduleCenter.Common.Helper;
using Wiwi.ScheduleCenter.Core.Infrastructure;
using Wiwi.ScheduleCenter.Core.QuartzHost.AppStart;

var builder = WebApplication.CreateBuilder(args);

#region log

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

#endregion

//配置
Appsettings.Configuration = builder.Configuration;

// Add services to the container.
builder.Services
    .AddCorsSetup()
    .AddAuthorization();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<ResultFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver =
        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(); //json字符串大小写原样输出
});
builder.Services.AddGrpc();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = false;
}).UseAutofac("Wiwi.ScheduleCenter.Api.dll");

var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
builder.Services.AddDbContext<ScheduleDbContext>(options =>
{
    var connection = Appsettings.Configuration["DbConfig:ConnectionString"];
    options.UseMySql(connection, ServerVersion.AutoDetect(connection),
        sql => sql.MigrationsAssembly(migrationAssembly));
});
var workId = Appsettings.GetConfig<long>("workId");
builder.Services.AddSnowFlakeNoGenter(workId);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHost();

builder.Host.UseNLog();

var app = builder.Build();


app.Services.SetServiceProvider();

app.UseSwaggerMiddleware();


app.UseHttpsRedirection();


//Cors
app.UseCorsMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MigrateDbContext<ScheduleDbContext>((_, __) => { }).Run();