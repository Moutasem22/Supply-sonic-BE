using Core.Enums;
using Core.Models.Identity;
using DB;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace AppAPI.CommonService
{
    public static class SecurityDependency
    {

        public static IServiceCollection AddSecurityDependency(this IServiceCollection services, AppSettings appSettings)
        {
            // configure jwt authentication  
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddIdentityCore<SupplierAppUser>()
            .AddEntityFrameworkStores<DBContext>()
            .AddDefaultTokenProviders();


            //services.AddIdentityCore<ProviderAppUser>()
            //        .AddEntityFrameworkStores<DBContext>()
            //        .AddDefaultTokenProviders();

            services.AddIdentity<AppUser, Role>()
           .AddEntityFrameworkStores<DBContext>()
           .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;


                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };

                //// for signalR it did not get access token from header, so we pass it by query string using this line  "this.connection = new signalR.HubConnectionBuilder().withUrl(config.URL + "Notification", { accessTokenFactory: () => localStorage.getItem('tokeninfo').replace("\"", "").substring(0, localStorage.getItem("tokeninfo").length - 2)  }).build();"
                x.EventsType = typeof(CustomJwtBearerEvents);

            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Verified", builder => builder.RequireClaim("TokenType", ((int)EnumTokenType.Verified).ToString()));
                options.AddPolicy("LoginOtp", builder => builder.RequireClaim("TokenType", ((int)EnumTokenType.LoginOtp).ToString()));
                options.AddPolicy("SetPassword", builder => builder.RequireClaim("TokenType", ((int)EnumTokenType.SetPassword).ToString()));
                options.AddPolicy("ForgetPassword", builder => builder.RequireClaim("TokenType", ((int)EnumTokenType.ForgetPassword).ToString()));

                //options.AddPolicy("verified", builder => builder.RequireClaim("otp", "true"));
                //options.AddPolicy("guest", builder => builder.RequireClaim("otp", "false"));
                //options.AddPolicy("Admin", builder => builder.RequireClaim("Admin", "true"));
                //options.AddPolicy("externalSystem", builder => builder.RequireClaim("externalSystem", "true"));
                //options.AddPolicy("Shared", policy =>
                //                  policy.RequireAssertion(context =>
                //                      context.User.HasClaim(c =>
                //                          (c.Type == "Admin" && c.Value == "true") ||
                //                           (c.Type == "otp" && c.Value == "true"))));

            });
            return services;

        }

    }
}
