using DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AppAPI.CommonService
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private DBContext _ldbcontext;
       
        public override Task MessageReceived(MessageReceivedContext context)
        {
            var accessToken = context.Request.Query["access_token"];
            if (string.IsNullOrEmpty(accessToken) == false)
            {
                context.Token = accessToken;
            }          

            /////////////////////////////////////////////
            return System.Threading.Tasks.Task.CompletedTask;
        }
        public override async Task TokenValidated(TokenValidatedContext context)
        {
            _ldbcontext = context.HttpContext.RequestServices
                    .GetRequiredService<DBContext>();
            int? userId = null;
            var principal = ((JwtSecurityToken)context.SecurityToken).Claims;
            if (principal != null && principal.FirstOrDefault(c => c.Type.ToLower() == "UserId".ToLower()) != null)
            {
                userId = int.Parse(principal.FirstOrDefault(c => c.Type.ToLower() == "UserId".ToLower()).Value);
               
                
                //var activeUserId = int.Parse(principal.FirstOrDefault(c => c.Type.ToLower() == "UserId".ToLower()).Value);
                //var user = await _ldbcontext.Users.FirstOrDefaultAsync(x => x.Id == activeUserId);
                //var token = ((JwtSecurityToken)context.SecurityToken).RawData;
                //if (user.Token != token && user.UserType != Core.Enums.EnumUserType.AdminUser)
                //{
                //    //throw new Exception("401");
                //    context.Fail(new Exception("401"));
                //    //context.NoResult();
                //}
            }
            var parameterReturn = new SqlParameter
            {
                ParameterName = "ReturnValue",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,
            };
            if (context.Request.Path.Value != "/Notification/negotiate" && context.Request.Path.Value != "/Notification")
            {
                string controllername = context.Request.RouteValues["controller"].ToString();
                string actionname = context.Request.RouteValues["action"].ToString();
                var result = await _ldbcontext.Database.ExecuteSqlRawAsync($"exec @returnValue  = CheckApiPermission @userId= {userId},@controllername='{controllername}' ,@actionname='{actionname}'", parameterReturn);

                if (int.Parse(parameterReturn.Value.ToString()) > 0)
                {
                    context.Success();
                }
                else
                {
                    context.Fail(new Exception("401"));
                }
            }
            return;
        }
    }
}
