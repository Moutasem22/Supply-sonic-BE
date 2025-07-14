using Core.Enums;
using Core.Models.Identity;
using Core.Models.Notifications;
using DB;
using DTO;
using DTO.CommandDTO;
using DTO.NotificationsDTO;
using Helpers;
using IServiceContractor.ICommonService;
using IServiceContractor.INotificationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Service.HubConfig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.NotificationServices
{
    public class NotificationService : INotificationService
    {

        private IHubContext<NotificationHub> _hub;
        private readonly IHostEnvironment _env;
        private readonly EmailSender _emailSender;

        private readonly DBContext _dbcontext;
        private readonly IRabbitMQProducer _messagePublisher;
        private readonly EmailServiceSettings _emailServiceSettings;
        public NotificationService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hub,
            IHostEnvironment env, EmailSender mailSender, IRabbitMQProducer messagePublisher, IOptions<EmailServiceSettings> emailServiceSettings)
        {
            //  _baseService = baseService;
            _dbcontext = dbcontext;
            _hub = hub;
            _env = env;
            _emailSender = mailSender;
            _emailServiceSettings = emailServiceSettings.Value;
            _messagePublisher = messagePublisher;
        }
        private string ImageToBase64(string Path)
        {
            using (Image image = Image.FromFile(Path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        public async Task<bool> CreateEmailNotification(string Subject, string Message, int? AttachmentId, List<string> users = null, EnumPriority priority = EnumPriority.Normal)
        {
            var result = true;
            try
            {
                var tempFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/EmailTemplate/TempEn.html", FileMode.Open, FileAccess.Read);
                string emailWithTemp = "";
                using (StreamReader reader = new StreamReader(tempFileStream))
                {
                    emailWithTemp = reader.ReadToEnd();
                }

                var appearance = _dbcontext.AppearancSettings.FirstOrDefault();
                var logoAttachment = _dbcontext.Attachments.FirstOrDefault(x => appearance != null && x.Id == appearance.LogoAttachmentId && x.IsActive && !x.IsDeleted);
                var logo = "";
                if (logoAttachment != null)
                    logo = ImageToBase64(@"" + _env.ContentRootPath + "/wwwroot" + logoAttachment.Path);

                emailWithTemp = emailWithTemp.Replace("{{message}}", Message).Replace("{{logo}}", logo);
                var email = new MailRequestDto()
                {
                    Subject = Subject == null ? "" : Subject,
                    Body = emailWithTemp,
                    ToEmails = users,
                    ProjectCode = _emailServiceSettings.ProjectCode
                };


                _messagePublisher.SendMessage(email);

                //var notificationEmail = new Notification(Subject, Subject, Message, Message, EnumNotificationType.Email, AttachmentId, users, priority);
                //_baseService.Context.Notifications.Add(notificationEmail);
                //_dbcontext.SaveChanges();

            }
            catch
            {
                return false;
            }

            return result;
        }

        public async Task<bool> SendEmailJob(EnumPriority priority)
        {
            try
            {
                var MaxRetryNumber = int.Parse(_dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "SendEmailMaxRetry")?.SysValue ?? "3");

                var unsent = _dbcontext.UserNotifications.Include(x => x.Notification).Where(x => x.IsActive == true && x.IsDeleted != true &&
                    x.IsSent != true && x.RetryCount < MaxRetryNumber && x.Notification.NotificationType == EnumNotificationType.Email && x.Notification.Priority == priority);

                var attachmentFile = new Print_pdfDTO();
                unsent.ToList().ForEach(async n =>
                {
                    var att = _dbcontext.Attachments.FirstOrDefault(a => a.Id == n.Notification.AttachmentId);
                    if (att != null)
                    {
                        byte[] attBytes = File.ReadAllBytes(_env.ContentRootPath + "/wwwroot/" + att.Path);
                        attachmentFile = new Print_pdfDTO()
                        {
                            Data = attBytes,
                            FileName = att.FileName
                        };
                    }


                    //var sent = _emailSender.SendEmailAsync(n.Email, n.Notification.Subject, n.Notification.MessageAr, attachmentFile).Result;
                    //n.Update(sent, n.RetryCount + 1);
                });
                _dbcontext.SaveChangesForHangFire();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public Task<bool> SendEmailJobHighPriority(EnumPriority priority)
        {
            return SendEmailJob(priority);
        }

        public async Task<bool> CreateWebNotification(string SubjectEn, string SubjectAr, string MessageEn, string MessageAr, List<int> users = null, string URL = "")
        {
            try
            {
                UserNotificationDTO notifyObj = null;
                Notification notificationWeb = null;
                notificationWeb = new Notification(SubjectEn, SubjectAr, MessageEn, MessageAr, EnumNotificationType.Web, null, users, URL);
                _dbcontext.Notifications.Add(notificationWeb);
                _dbcontext.SaveChanges();

                if (notificationWeb != null)
                {
                    notifyObj = new UserNotificationDTO()
                    {
                        Id = notificationWeb.Id,
                        URL = URL,
                        SubjectEn = SubjectEn,
                        Subject = SubjectAr,
                        Message = MessageAr,
                        MessageEn = MessageEn,
                        CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm"),
                        NotificationState = EnumNotificationState.New
                    };
                    var userConnections = _dbcontext.UserConnections.Where(x => users.Contains(x.AppUserId)).Select(x => x.ConnectionId.ToString());
                    await _hub.Clients.Clients(userConnections.ToArray()).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(notifyObj));
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        //public bool CreateNewRequestWebNotification(string PageCode, string UniqueId, string NationalId)
        //{ 
        //    var result = true;
        //    try
        //    {               
        //        var page = _dbcontext.Pages.FirstOrDefault(x => x.Code == PageCode);
        //        var adminURL = _dbcontext.SysSettings.FirstOrDefault(x => x.SysKey == "AdminURL").SysValue;
        //        string URL = adminURL + page.Path + "?UniqueId=" + UniqueId;


        //        var usersList = _dbcontext.Users.Where(x => x.UserRoles.Count(y => y.Role.Permissions.Count(p => p.PageAction.Page.Code == PageCode) > 0) > 0).Select(x => x.Id).ToList();

        //        FileStream fileStream = new FileStream(@"" + _env.ContentRootPath + "/wwwroot/NotificationWebTemplate/NotificationAR.txt", FileMode.Open, FileAccess.Read);
        //        string NotificationAR = "";
        //        using (StreamReader reader = new StreamReader(fileStream))
        //        {
        //            NotificationAR = reader.ReadToEnd();
        //        }

        //        fileStream = new FileStream(@"" + _env.ContentRootPath + "/wwwroot/NotificationWebTemplate/NotificationEN.txt", FileMode.Open, FileAccess.Read);
        //        string NotificationEN = "";
        //        using (StreamReader reader = new StreamReader(fileStream))
        //        {
        //            NotificationEN = reader.ReadToEnd();
        //        }
        //        string webMsg = NotificationAR.Replace("{NationalId}", NationalId)
        //                        .Replace("{requestType}", page.NameAr);

        //        string webMsgEn = NotificationEN.Replace("{NationalId}", NationalId)
        //                        .Replace("{requestType}", page.NameEn);
        //        var notificationWeb = new Notification(URL, "طلب جديد", "New Request", webMsgEn, webMsg, EnumNotificationType.Web, usersList);
        //        _dbcontext.Notifications.Add(notificationWeb);
        //        _dbcontext.SaveChanges();
        //        var notifyObj = new UserNotificationDTO()
        //        {
        //            Id = notificationWeb.Id,
        //            URL = URL,
        //            Subject = "طلب جديد",
        //            SubjectEn = "New Request",
        //            Message = webMsg,
        //            MessageEn = webMsgEn,
        //            CreatedDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm"),
        //            NotificationState = DTO.EnumsDTO.EnumNotificationStateDTO.New
        //        };
        //        var userConnections = _dbcontext.UserConnections.Where(x => usersList.Contains(x.AppUserId)).Select(x => x.ConnectionId.ToString());
        //        _hub.Clients.Clients(userConnections.ToArray<string>()).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(notifyObj));

        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //    return result;
        //}

    }
}
