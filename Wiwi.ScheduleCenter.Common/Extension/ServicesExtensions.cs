using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.XPath;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;
using Wiwi.ScheduleCenter.Common.Configs;
using Wiwi.ScheduleCenter.Common.Enums;
using Wiwi.ScheduleCenter.Common.Helper;
using Wiwi.ScheduleCenter.Common.Model.Result;

namespace Wiwi.ScheduleCenter.Common.Extension
{
    public static class ServicesExtensions
    {
        private static Logger logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        #region Swagger
        /// <summary>
        /// 添加Swagger
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var config = Appsettings.GetConfig<ApiConfig>("ApiConfig");

            if (config == null || !config.Enable)
            {
                return services;
            }

            var basePath = AppContext.BaseDirectory;
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(config.ApiVersion, new OpenApiInfo
                {
                    Version = config.ApiVersion,
                    Title = $"{config.ApiName} 接口文档——{RuntimeInformation.FrameworkDescription}",
                });
                c.OrderActionsBy(o => o.RelativePath);


                var directoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                var fileInfos = directoryInfo.GetFiles();
                var xmlFiles = fileInfos.Where(x => x.Name.EndsWith(".xml"));
                foreach (var file in xmlFiles)
                {
                    if (!File.Exists(file.FullName))
                        continue;

                    var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.Name);
                    c.IncludeXmlComments(() =>
                    {
                        var xmlDoc = XDocument.Load(filePath);
                        var crefMembers = xmlDoc.XPathSelectElements("/doc/members/member[@name and not(inheritdoc)]")
                            .ToDictionary(x => x.Attribute("name")?.Value ?? string.Empty);

                        var realDocMembers = xmlDoc.XPathSelectElements("/doc/members/member[inheritdoc[@cref]]");
                        foreach (var real in realDocMembers)
                        {
                            var element = real.Element("inheritdoc");
                            if (element == null)
                                continue;

                            var cref = element.Attribute("cref")?.Value;
                            if (string.IsNullOrWhiteSpace(cref))
                                continue;

                            if (crefMembers.TryGetValue(cref, out var realDocMember))
                                element.Parent?.ReplaceNodes(realDocMember.Nodes());
                        }

                        return new XPathDocument(xmlDoc.CreateReader());
                    }, true);
                }

                c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            var config = Appsettings.GetConfig<ApiConfig>("ApiConfig");

            if (config == null || !config.Enable)
            {
                return app;
            }
            app.UseSwagger();
            //app.UseSwaggerUI();

            app.UseKnife4UI(c =>
            {
                c.RoutePrefix = "";
                c.DocExpansion(DocExpansion.None);
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "web api");
            });
            return app;
        }
        #endregion

        #region Cors
        /// <summary>
        /// Cors
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var config = Appsettings.GetConfig<CorsConfig>("CorsConfig");

            services.AddCors(c =>
            {
                if (!config.EnableAllIPs)
                {
                    c.AddPolicy(config.PolicyName,

                        policy =>
                        {
                            policy
                            .WithOrigins(config.IPs)
                            .AllowAnyHeader()//Ensures that the policy allows any header.
                            .AllowAnyMethod();
                        });
                }
                else
                {
                    //允许任意跨域请求
                    c.AddPolicy(config.PolicyName,
                    policy =>
                    {
                        policy
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                    });
                }
            });

            return services;
        }

        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder app)
        {
            var config = Appsettings.GetConfig<CorsConfig>("CorsConfig");
            // CORS跨域
            app.UseCors(config.PolicyName);

            return app;
        }
        #endregion
    }
}
