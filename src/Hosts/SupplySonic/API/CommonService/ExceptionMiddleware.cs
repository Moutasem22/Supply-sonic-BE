using DTO;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppAPI.CommonService
{
    //https://code-maze.com/global-error-handling-aspnetcore/
    //https://codewithmukesh.com/blog/global-exception-handling-in-aspnet-core/
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly GlobalFormat _globalFormat;
        public ExceptionMiddleware(RequestDelegate next, ILogger logger, GlobalFormat globalFormat)
        {
            _logger = logger;
            _next = next;
            _globalFormat = globalFormat;

        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //httpContext.Request.EnableBuffering();

                await _next(httpContext);
            }
            catch (BusinessException ex)
            {
                var bodyStr = "";
                var queryStr = "";
                using (StreamReader reader
                  = new StreamReader(httpContext.Request.Body))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }
                if (httpContext.Request.QueryString.HasValue)
                {
                    queryStr = httpContext.Request.QueryString.Value;
                }

                var obj = new
                {
                    EXCode = Guid.NewGuid(),
                    LogType = "Business",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Data = ex.Data,
                    RequestBody = bodyStr,
                    RequestQuery = queryStr,
                    Request = httpContext.Request.Headers,

                    ControllerName = httpContext.Request.RouteValues["controller"].ToString(),
                    ActionName = httpContext.Request.RouteValues["action"].ToString()
                };
                var edata = _globalFormat.LogFormat(httpContext, obj);
                _logger.LogWarning($"End Execute by {edata.UserId} from connection={edata.IP} using data={edata.Data}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;//(int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { EXCode = obj.EXCode, Message = obj.Message, type = obj.LogType });
                //await httpContext.Response.WriteAsJsonAsync(new { EXCode = obj.EXCode, Message = new { msg = Newtonsoft.Json.JsonConvert.DeserializeObject(obj.Message) }, type = obj.LogType });
            }
            catch (Exception ex)
            {
                //HttpRequestRewindExtensions.EnableBuffering(httpContext.Request);
                //httpContext.Request.EnableBuffering();
                //httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                var bodyStr = "";
                var queryStr = "";
                using (StreamReader reader
                  = new StreamReader(httpContext.Request.Body))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }
                if (httpContext.Request.QueryString.HasValue)
                {
                    queryStr = httpContext.Request.QueryString.Value;
                }

                var obj = new
                {
                    EXCode = Guid.NewGuid(),
                    LogType = "Error",
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Data = ex.Data,
                    RequestBody = bodyStr,
                    RequestQuery = queryStr,
                    Request = httpContext.Request.Headers,
                    ControllerName = httpContext.Request.RouteValues["controller"].ToString(),
                    ActionName = httpContext.Request.RouteValues["action"].ToString()
                };
                var edata = _globalFormat.LogFormat(httpContext, obj);
                _logger.LogError($"End Execute by {edata.UserId} from connection={edata.IP} using data={edata.Data}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;//(int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { EXCode = obj.EXCode, Message = new { SystemError = "SystemError" }, type = obj.LogType }); //BadRequest(new { EXCode = obj.EXCode, Message = "SystemError", type = obj.type });
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = new { SystemError = "Internal Server Error from the custom middleware." }
            }.ToString());
        }
    }
}
