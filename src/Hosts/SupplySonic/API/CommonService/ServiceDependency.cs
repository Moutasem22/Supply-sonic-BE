using DB;
using DTO;
using DTO.IdentityDTO;
using DTO.SettingDTO;
using FluentValidation;
using Helpers;
using IServiceContractor;
using IServiceContractor.IAttachmentServices;
using IServiceContractor.ICommonService;
using IServiceContractor.IdentityInterFaces;
using IServiceContractor.INotificationServices;
using IServiceContractor.ISettingServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Service;
using Service.AttachmentServices;
using Service.IdentityServices;
using Service.NotificationServices;
using Service.SettingServices;
using Service.Validators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator.IdentityValidator;

namespace AppAPI.CommonService
{
    public static class ServiceDependency
    {
        public static IServiceCollection AddServiceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            // Add ConnectionString From  appsetting.json
            services.AddDbContext<DBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(true);
            });

            #region Inject Services
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<CustomJwtBearerEvents>();
            services.AddScoped<DBContext>();
            services.AddScoped<RestHelper>();
            services.AddScoped<EmailSender>();
            services.AddSingleton<GlobalFormat>();
            services.AddScoped<JwtSecurityTokenHandler>();
            services.TryAddTransient<SysSettingsService>();
            services.TryAddTransient<SysSettingsRep>();
            services.TryAddTransient<IUserService, UserService>();
            services.TryAddTransient<IAuthenticationService, AuthenticationService>();
            services.TryAddTransient<IUserSettingService, UserSettingService>();
            services.TryAddTransient<ITokenFactory, TokenFactory>();
            services.TryAddTransient<IRoleService, RoleService>();
            services.TryAddTransient<IActionService, ActionService>();
            services.TryAddTransient<IPageService, PageService>();
            services.TryAddTransient<IJwtTokenHandler, JwtTokenHandler>();
            services.TryAddTransient<IJwtTokenValidator, JwtTokenValidator>();
            services.TryAddTransient<IAttachmentService, AttachmentService>();
            services.AddScoped<IUserConnectionService, UserConnectionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<INotificationTemplateService, NotificationTemplateService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<IBaseService, BaseService>();   
            services.AddTransient<IAppearanceSettingsService, AppearanceSettingsService>();
            services.AddTransient<ReportService>();
            services.AddScoped<IExceptionMessages, ExceptionMessages>();
            services.AddScoped<ValidationHelper>();
            services.AddTransient<IRabbitMQProducer, RabbitMQProducer>();
            services.AddTransient<INotificationSettingsService, NotificationSettingsService>();
            services.AddTransient<IUserNotificationService, UserNotificationService>();


            services.AddTransient<IClientService, ClientService>();

            
            #endregion

            #region Inject Validator

            services.AddScoped<IValidator<UserAddEditDto>, UserValidator>();
            services.AddScoped<IValidator<UserProfileEditDto>, UserProfileValidator>();
            services.AddScoped<IValidator<AppearanceAddEditDto>, AppearanceSettingsValidator>();
            services.AddScoped<IValidator<PasswordDto>, ChangePasswordValidator>();
            services.AddScoped<IValidator<SetPasswordDto>, SetPasswordValidator>();
            services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordValidator>();
          
            services.AddScoped<IValidator<ClientAddEditDto>, ClientValidator>();
            #endregion

            return services;
        }
    }
}
