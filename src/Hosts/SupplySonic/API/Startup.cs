using Service.HubConfig;
using Core.Models.Identity;
using DB;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Service;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Hangfire;
using Hangfire.SqlServer;
using Mapster;

using Microsoft.AspNetCore.Http.Connections;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Localization;
using System.Linq;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using System.Reflection;
using DTO;
using Localization;
using System.Threading;
using FluentValidation.AspNetCore;
using FluentValidation;
using Service.Validators;
using IServiceContractor.ICommonService;
using Core.Enums;
using AppAPI.CommonService;

namespace AppAPI
{
    public class ApplicationLog
    {
    }
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string[] origins = { };
            if (!string.IsNullOrEmpty(Configuration.GetValue<string>("AllowedOrigins")))
            {
                origins = Configuration.GetValue<string>("AllowedOrigins").Split(";");
            }
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => (origins.Length > 0 ? builder.WithOrigins(origins) : builder.AllowAnyOrigin())
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            //Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            ////////////Localization//////////
            services.AddLocalization(option => option.ResourcesPath = "");
            services.Configure<RequestLocalizationOptions>(option =>
            {
                var supportedCulture = new List<CultureInfo>
                {
                    new CultureInfo("ar"),
                    new CultureInfo("en")
                };
                option.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(culture: "ar", uiCulture: "ar");
                option.SupportedCultures = supportedCulture;
                option.SupportedUICultures = supportedCulture;
                //option.RequestCultureProviders = new[] { new RouteDataRequestCultureProvider{RouteDataStringKey="ar",UIRouteDataStringKey="ar"} };
                ///to get lang from request header 
                option.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var userLangs = context.Request.Headers["lang"].ToString();
                    var firstLang = userLangs.Split(',').FirstOrDefault();
                    var defaultLang = string.IsNullOrEmpty(firstLang) ? "ar" : firstLang;
                    return Task.FromResult(new ProviderCultureResult(defaultLang, defaultLang));
                }));
            });
            //////////////////////////////////
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(10),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(10),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            //////////////////////////////////////////////////////////////////////
            ///
            services.AddMvc(options =>
            {

            }).AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
               }).AddDataAnnotationsLocalization(options =>
               {
                   options.DataAnnotationLocalizerProvider = (type, factory) =>
                   {
                       var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                       return factory.Create("Translations", assemblyName.Name);
                   };
               });

            services.AddControllers(o =>
            {
                //o.Conventions.Add(new ActionHidingConvention());
            });



            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            IConfigurationSection mailSettings = Configuration.GetSection("EmailSettings");
            services.Configure<EmailSettings>(mailSettings);

            IConfigurationSection smsSettings = Configuration.GetSection("SMSSettings");
            services.Configure<SMSSettings>(smsSettings);


            IConfigurationSection emailServiceSettings = Configuration.GetSection("EmailServiceSettings");
            services.Configure<EmailServiceSettings>(emailServiceSettings);

            // configure strongly typed settings objects
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();

            // Add Security Dependency
            services.AddSecurityDependency(appSettings);

            services.AddSignalR(option =>
            {

            });

            //Add Swagger 
            services.AddSwaggerConfiguration();

            ///https://stackoverflow.com/questions/52921966/unable-to-resolve-ilogger-from-microsoft-extensions-logging
            var serviceProvider = services.BuildServiceProvider();//application.ApplicationServices;//services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<ApplicationLog>>();
            services.AddSingleton(typeof(ILogger), logger);
            ////////////////////////////////////////////////////////////////////////////

            //Add Service Dependency Injection  
            services.AddServiceDependency(this.Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory
            , INotificationService _notificationService)
        {
            //app.UseEnableRequestRewind();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            ///////https://stackoverflow.com/questions/48676152/asp-net-core-web-api-logging-from-a-static-class
            //ExceptionHelper.LoggerFactory = loggerFactory;
            ///////////for IP Address and Also not working
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });



            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHangfireDashboard();

            //RecurringJob.AddOrUpdate(() =>  _notificationService.SendEmailJobHighPriority(EnumPriority.High), Cron.MinuteInterval(5));
            //RecurringJob.AddOrUpdate(() =>  _notificationService.SendEmailJob(EnumPriority.Normal), Cron.MinuteInterval(15));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Application API V1");
                c.RoutePrefix = string.Empty;
            });

            RewriteOptions option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseRequestLocalization();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/Notification", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
            });
        }
    }
}
