using Core.Enums;
using Core.Models.Notifications;
using Core.Models.StoredProcedures;
using DB;
using DTO.CommandDTO;
using DTO.NotificationsDTO;
using FluentValidation;
using Helpers;
using IServiceContractor.ICommonService;
using IServiceContractor.INotificationServices;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Service.NotificationServices;
public class NotificationSettingsService : INotificationSettingsService
{
    //private readonly IBaseService _baseService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly INotificationService _notificationService;
    private readonly IHostEnvironment _env;
    private readonly DBContext _dbContext;
    public NotificationSettingsService(
        DBContext dbcontext,
        //IBaseService baseService,
        IHttpContextAccessor httpContextAccessor, INotificationService notificationService, IHostEnvironment env)
    {
        // _baseService = baseService;
        _dbContext = dbcontext;
        _httpContextAccessor = httpContextAccessor;
        _notificationService = notificationService;
        _env = env;
    }

    private int CurrentUserId()
    {
        var principal = _httpContextAccessor.HttpContext.User;
        if (principal != null && principal.HasClaim(c => c.Type.ToLower() == "UserId".ToLower()))
        {
            var activeUserId = principal.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value;
            return Convert.ToInt32(activeUserId);
        }
        return 0;
    }

    public async Task<ResultViewModel<List<NotificationSettingResultDto>>> GetAll()
    {
        var notificationSettings = _dbContext.NotificationSettings.Where(n => n.UserId == CurrentUserId() && n.IsActive && !n.IsDeleted).ToList();

        var PagesWithActionsName = _dbContext.Set<SPAuthorizePagesWithAction>().FromSqlRaw($"exec sp_GetAuthorizePagesWithAction @userId= {CurrentUserId()}").ToList();


        if (notificationSettings.Count == 0)
        {
            notificationSettings.AddRange(PagesWithActionsName.Select(p => new NotificationSetting(p.NameEn, p.NameAr, p.PageCategoryId, p.PageActionId, false, false, CurrentUserId())));
            await _dbContext.NotificationSettings.AddRangeAsync(notificationSettings);
        }
        else
        {
            // To delete items
            List<string> deletedNames = notificationSettings.Select(x => x.NameEn).Except(PagesWithActionsName.Select(x => x.NameEn)).ToList();
            var deletedItems = notificationSettings.Where(x => deletedNames.Contains(x.NameEn)).ToList();
            _dbContext.NotificationSettings.RemoveRange(deletedItems);

            var newNotificationSettings = notificationSettings.Except(deletedItems).ToList();

            //Save New  Items
            var addedNames = PagesWithActionsName.Where(x => !newNotificationSettings.Any(y => y.NameEn == x.NameEn)).ToList();
            var addedItems = addedNames.Select(p => new NotificationSetting(p.NameEn, p.NameAr, p.PageCategoryId, p.PageActionId, false, false, CurrentUserId())).ToList();
            newNotificationSettings.AddRange(addedItems);
            await _dbContext.NotificationSettings.AddRangeAsync(addedItems);

            notificationSettings = newNotificationSettings;
        }
        await _dbContext.SaveChangesAsync();

        ResultViewModel<List<NotificationSettingResultDto>> result = new() { Data = notificationSettings.Adapt<List<NotificationSettingResultDto>>(), IsSuccess = true };

        return result;
    }


    public async Task<ResultViewModel<bool>> Update(List<NotificationSettingAddEditDto> notificationSettings)
    {

        foreach (var n in notificationSettings)
        {
            var notificationSetting = _dbContext.NotificationSettings.FirstOrDefault(x => x.Id == n.Id);

            if (notificationSetting == null)
                continue;

            notificationSetting.Update(n.NameEn, n.NameAr, n.PageCategoryId, n.PageActionId, n.ByEmail, n.BySystem, n.UserId);
        }

        await _dbContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }

    public void sendActionNotification(EnumPageCode pageCode, EnumActionCode actionCode, int objId = 0)
    {
        try
        {
            var actioncode_ = (int)actionCode;
            var page = _dbContext.Pages.AsNoTracking().
                Include(s => s.PageActions.Where(z => z.ActionId == actioncode_)).ThenInclude(f => f.Action).
                Include(s => s.PageActions.Where(z => z.ActionId == actioncode_)).ThenInclude(f => f.NotificationSettings).
                ThenInclude(x => x.User).FirstOrDefault(x => x.Code == pageCode.ToString());

            var notificationSettings = page.PageActions.FirstOrDefault(x => x.Action != null && x.Action.Code == Enum.GetName(typeof(EnumActionCode), actionCode)).NotificationSettings.ToList();

            var emails = notificationSettings.Where(x => x.ByEmail == true).Select(x => x.User.Email).ToList();
            if (emails.Count > 0)
                SendActionEmailNotification(emails, actionCode, page.NameEn);

            var userIds = notificationSettings.Where(x => x.BySystem == true).Select(x => x.User.Id).ToList();
            if (userIds.Count > 0)
                SendActionWebNotification(userIds, actionCode, page.NameAr, page.NameEn, page.Path, objId);
        }
        catch (Exception ex)
        {

        }

    }

    private async void SendActionEmailNotification(List<string> emails, EnumActionCode actionCode, string pageName)
    {
        string emailSubject, emailBody, actionName;
        FileStream emailFileStream;
        actionName = Enum.GetName(typeof(EnumActionCode), actionCode);
        emailSubject = GetSysSettings(actionName + "SubjectEn") != null ? GetSysSettings(actionName + "SubjectEn").Replace("{{pageNameEn}}", pageName) : "";
        emailFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/" + actionName + "Email/" + actionName + "EmailEn.html", FileMode.Open, FileAccess.Read);
        emailBody = "";
        using (StreamReader reader = new StreamReader(emailFileStream))
        {
            emailBody = reader.ReadToEnd();
        }
        emailBody = emailBody.Replace("{{pageName}}", pageName);

        foreach (var email in emails)
        {
            await _notificationService.CreateEmailNotification(emailSubject, emailBody, null, new List<string>() { email });
        }
    }


    private async void SendActionWebNotification(List<int> userIds, EnumActionCode actionCode, string pageNameAr, string pageNameEn, string path, int objId = 0)
    {
        string subjectEn, subjectAr, msgEn, msgAr, actionName, url;
        FileStream fileStreamEn, fileStreamAr;
        actionName = Enum.GetName(typeof(EnumActionCode), actionCode);

        subjectAr = GetSysSettings(actionName + "SubjectAr") != null ? GetSysSettings(actionName + "SubjectAr").Replace("{{pageNameAr}}", pageNameAr) : "";
        subjectEn = GetSysSettings(actionName + "SubjectEn") != null ? GetSysSettings(actionName + "SubjectEn").Replace("{{pageNameEn}}", pageNameEn) : "";

        fileStreamEn = new FileStream(_env.ContentRootPath + "/wwwroot/" + actionName + "Web/" + actionName + "WebEn.html", FileMode.Open, FileAccess.Read);
        msgEn = "";
        using (StreamReader reader = new StreamReader(fileStreamEn))
        {
            msgEn = reader.ReadToEnd();
        }
        msgEn = msgEn.Replace("{{pageName}}", pageNameEn);

        fileStreamAr = new FileStream(_env.ContentRootPath + "/wwwroot/" + actionName + "Web/" + actionName + "WebAr.html", FileMode.Open, FileAccess.Read);
        msgAr = "";
        using (StreamReader reader = new StreamReader(fileStreamAr))
        {
            msgAr = reader.ReadToEnd();
        }
        msgAr = msgAr.Replace("{{pageName}}", pageNameAr);


        if (objId != 0)
        {
            if (actionCode == EnumActionCode.add || actionCode == EnumActionCode.update)
            {
                string UniqueId = EncryptionHelper.EncryptForUrl(objId.ToString());
                url = path + "Edit/" + UniqueId + "/1";
            }
            else
                url = path;
        }
        else
            url = path;

        await _notificationService.CreateWebNotification(subjectEn, subjectAr, msgEn, msgAr, userIds, url);
    }
    private string GetSysSettings(string sysKey)
    {
        return _dbContext.SysSettings.AsNoTracking().FirstOrDefault(x => x.SysKey == sysKey)?.SysValue;
    }
}
