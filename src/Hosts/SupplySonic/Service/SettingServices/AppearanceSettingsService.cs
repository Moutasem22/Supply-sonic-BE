using Core.Models.Attachments;
using Core.Models.Setting;
using DB;
using DTO.CommandDTO;
using DTO.SettingDTO;
using FluentValidation;
using Helpers;
using IServiceContractor.ICommonService;
using IServiceContractor.ISettingServices;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SettingServices;
public class AppearanceSettingsService : IAppearanceSettingsService
{
    private readonly IBaseService _baseService;
    private readonly IValidator<AppearanceAddEditDto> _validator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DBContext _dBContext;

    public AppearanceSettingsService(IBaseService baseService, IValidator<AppearanceAddEditDto> validator, IHttpContextAccessor httpContextAccessor)
    {
        _baseService = baseService;
        _validator = validator;
        _dBContext = baseService.Context;
        _httpContextAccessor = httpContextAccessor;
        var url = _httpContextAccessor.HttpContext.Request.Scheme.ToString() + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "";
        TypeAdapterConfig<Attachment, AttachmentDto>.NewConfig()
        .Map(dest => dest.FilePath, src => url + src.Path);

    }

    public async Task<ResultViewModel<AppearanceResultDto>> GetCurrent()
    {
        // var url = _httpContextAccessor.HttpContext.Request.Scheme.ToString() + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + "";
        AppearancSetting query = await _dBContext.AppearancSettings.Where(n => n.IsActive && !n.IsDeleted)?.Include(n => n.LogoAttachment).FirstOrDefaultAsync();

        if (query == null)
        {
            //add new row 
            AppearancSetting appearanceSettings = new AppearancSetting("", "", "", "", "", "", "", null);

            _dBContext.Add(appearanceSettings);
            await _dBContext.SaveChangesAsync();
            query = appearanceSettings;
        }

        ResultViewModel<AppearanceResultDto> result = new() { Data = query.Adapt<AppearanceResultDto>(), IsSuccess = true };
        //result.Data.LogoAttachment.FilePath = url+query.LogoAttachment.Path;
        return result;
    }


    public async Task<ResultViewModel<AppearanceResultDto>> Update(AppearanceAddEditDto AppearanceDto)
    {
        _baseService.CheckValidation(AppearanceDto, _validator);
        var Appearance = _dBContext.AppearancSettings.FirstOrDefault();

        if (Appearance == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Appearance" };
           _baseService.ExceptionMessages.ReturnExceptionMessages(errorMessage);
        }

        var result = new ResultViewModel<AppearanceResultDto>();
        Appearance.Update(AppearanceDto.SystemColorAlpha, AppearanceDto.SystemColorHexa, AppearanceDto.SystemColorHex, AppearanceDto.ButtonsColorsAlpha, AppearanceDto.ButtonsColorsHexa, AppearanceDto.ButtonsColorsHex, AppearanceDto.Logo, AppearanceDto.LogoAttachmentId);
        await _dBContext.SaveChangesAsync();

        result.Data = Appearance.Adapt<AppearanceResultDto>();
        result.IsSuccess = true;
        return result;
    }
}
