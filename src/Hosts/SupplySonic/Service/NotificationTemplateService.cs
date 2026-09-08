
using Core.Models;
using Core.Models.Identity;
using DB;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapster;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Localization;

namespace Service
{
    public class NotificationTemplateService : INotificationTemplateService
    {
        private DBContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string lang { get; set; }
        private readonly IStringLocalizer<SharedResource> localizer;

        public NotificationTemplateService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor, IStringLocalizer<SharedResource> localizer)
        {
            this._dbcontext = dbcontext;            
            _httpContextAccessor = httpContextAccessor;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";   
            this.localizer = localizer;
        }

        public ResultViewModel<List<NotificationTemplateResultDto>> GetAll(QueryViewModel<NotificationTemplateResultDto> queryViewModel)
        {
            var PagedDataResult = new ResultViewModel<List<NotificationTemplateResultDto>>();

            var query = _dbcontext.NotificationTemplates.Where(r => r.IsDeleted != true) as IEnumerable<NotificationTemplate>;

            var Total = query.Count();

            var NotificationTemplates = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

            List<NotificationTemplateResultDto> notificationTemplateDtos = NotificationTemplates.Adapt<List<NotificationTemplateResultDto>>();//Mapper.Map<List<NotificationTemplateResultDto>>(NotificationTemplates);

            PagedDataResult.Data = notificationTemplateDtos;
            PagedDataResult.PageSize = queryViewModel.PageSize;
            PagedDataResult.PageNumber = queryViewModel.PageNumber;
            PagedDataResult.Total = Total;
            PagedDataResult.IsSuccess = true;
            return PagedDataResult;
          
        }

        public ResultViewModel<NotificationTemplateResultDto> Getone(int id)
        {
            var PagedDataResult = new ResultViewModel<NotificationTemplateResultDto>();

            var query = _dbcontext.NotificationTemplates.Where(r => r.Id == id).FirstOrDefault();
                var templateParams = _dbcontext.NotificationTemplateParams.Where(r => r.NotificationTemplateType == query.NotificationTemplateType).ToList();


                NotificationTemplateResultDto notificationTemplateDtos = query.Adapt<NotificationTemplateResultDto>(); //Mapper.Map<NotificationTemplateResultDto>(query);
                List<NotificationTemplateParamsDto> templateDtos = templateParams.Adapt<List<NotificationTemplateParamsDto>>();// Mapper.Map<List<NotificationTemplateParamsDto>>(templateParams);

                notificationTemplateDtos.NotificationTemplateParams = templateDtos;
                PagedDataResult.Data = notificationTemplateDtos;
                PagedDataResult.IsSuccess = true;
                return PagedDataResult;
          

        }

        
        public ResultViewModel<NotificationTemplateResultDto> Update(NotificationTemplateAddEditDto notificationTemplateDto)
        {
            ResultViewModel<NotificationTemplateResultDto> _ResultViewModel = new ResultViewModel<NotificationTemplateResultDto>();

            var notificationTemplate = _dbcontext.NotificationTemplates.FirstOrDefault(x => x.Id == notificationTemplateDto.Id);
            if (notificationTemplate == null)
            {
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(new ValidationResult(localizer["ItemNotExist"], new List<string>() { nameof(notificationTemplateDto.Id) })));

            }

            notificationTemplate.Update(notificationTemplateDto.Name, notificationTemplateDto.MobileMsg, notificationTemplateDto.EmailMsg, 
                notificationTemplateDto.WebMsg, notificationTemplateDto.Subject,notificationTemplateDto.MobileMsgEn, notificationTemplateDto.EmailMsgEn,notificationTemplateDto.WebMsgEn,notificationTemplateDto.SubjectEn,notificationTemplateDto.IsActive);
            _dbcontext.SaveChanges();

            _ResultViewModel.Data = notificationTemplate.Adapt<NotificationTemplateResultDto>();// Mapper.Map<NotificationTemplateResultDto>(notificationTemplate);
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;
            
        }
        
    }
}
