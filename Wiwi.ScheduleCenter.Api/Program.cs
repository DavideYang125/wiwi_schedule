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
    var connection = Appsettings.Configuration["ConnectionStrings:DefaultConnectionString"];//Configuration.GetConnectionString("DefaultConnectionString");
    options.UseMySql(connection, ServerVersion.AutoDetect(connection),
        sql => sql.MigrationsAssembly(migrationAssembly));
});
var workId = Appsettings.GetConfig<long>("workId");
builder.Services.AddSnowFlakeNoGenter(workId);
//Add CAP
//builder.Services.AddCapRabbitMq(options =>
//{
//    options.UseMySql(mysqlOption =>
//    {
//        mysqlOption.ConnectionString = Appsettings.Configuration["DbConfig:ConnectionString"];
//        mysqlOption.TableNamePrefix = Appsettings.Configuration["CAP:TableNamePrefix"];
//    });
//    options.DefaultGroupName = Appsettings.Configuration["CAP:DefaultGroup"];
//},
//    mqOptions =>
//    {
//        mqOptions.HostName = Appsettings.Configuration["CAP:RabbitMQ:HostName"];
//        mqOptions.UserName = Appsettings.Configuration["CAP:RabbitMQ:UserName"];
//        mqOptions.Password = Appsettings.Configuration["CAP:RabbitMQ:Password"];
//        mqOptions.VirtualHost = Appsettings.Configuration["CAP:RabbitMQ:VirtualHost"];
//    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHost();
//builder.Services.AddJobs();

//builder.Services.AddMediatR(typeof(BaseCommand).Assembly);

// NLog：注册Nlog组件
//builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();


app.Services.SetServiceProvider();
//app.Services.AddSnowFlakeNoGenter("1");
//app.MapGrpcService<GrpcMessageService>();
//app.MapGet("/GrpcMessage", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//CodeFirst
//app.UseCodeFirst();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapGrpcService<GrpcGatewayService>();
//    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
//}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
app.UseSwaggerMiddleware();


app.UseHttpsRedirection();


//Cors
app.UseCorsMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MigrateDbContext<ScheduleDbContext>((_, __) => { }).Run();