using Core.Models.Identity;
using DB;
using DTO;
using FluentValidation;
using FluentValidation.Results;
using Helpers;
using IServiceContractor.ICommonService;
using Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Core.Enums;
using IServiceContractor.INotificationServices;

namespace AppAPI.CommonService;

public class BaseService : IBaseService
{
    private readonly DBContext _dbcontext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IExceptionMessages _exceptionMessages;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IHostEnvironment _env;
    private readonly ValidationHelper _validationHelper;
    private readonly INotificationSettingsService _notificationSettingsService;
    public BaseService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor, IExceptionMessages exceptionMessages,
                       IStringLocalizer<SharedResource> localizer
        , ValidationHelper validationHelper
        , IHostEnvironment env, INotificationSettingsService notificationSettingsService)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbcontext = dbcontext;
        _localizer = localizer;
        _exceptionMessages = exceptionMessages;
        _env = env;
        _validationHelper = validationHelper;
        _notificationSettingsService = notificationSettingsService;
    }

    public DBContext Context => _dbcontext;

    public IStringLocalizer<SharedResource> Localizer => _localizer;

    public string Language => _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";

    public IExceptionMessages ExceptionMessages => _exceptionMessages;

    public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

    public IHostEnvironment HostEnvironment => _env;

    public int CurrentUserId => GetUserId(_httpContextAccessor.HttpContext.User);

    public string CurrentUrlPath => _httpContextAccessor.HttpContext.Request.Scheme.ToString() + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "";


    public void CheckValidation<T>(T dto, IValidator<T> validator) => _validationHelper.Validate(dto, validator);


    public void Validate<T>(T dto, IValidator<T> validator)
    {
        List<ErrorMessageDto> results = new();

        var validationResult = validator.Validate(dto);

        if (!validationResult.IsValid)
        {
            foreach (ValidationFailure failure in validationResult.Errors)
                results.Add(new ErrorMessageDto() { PropertyName = failure.PropertyName, ErrorMessage = failure.ErrorMessage });

            if (results.Count > 0)
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
        }

      
    }

    private int GetUserId(ClaimsPrincipal principal)
    {
        if (principal != null && principal.HasClaim(c => c.Type.ToLower() == "UserId".ToLower()))
        {
            var activeUserId = principal.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value;
            return Convert.ToInt32(activeUserId);
        }
        return 0;
    }

    public void sendActionNotification(EnumPageCode pageCode, EnumActionCode actionCode, int objId = 0)
    {
        _notificationSettingsService.sendActionNotification(pageCode, actionCode, objId);
    }
}

