using Core.Enums;
using Core.Models;
using Core.Models.Identity;
using DB;
using DocumentFormat.OpenXml.Wordprocessing;
using DTO;
using Helpers;
using IServiceContractor;
using IServiceContractor.ICommonService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using Service.HubConfig;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class NotificationService : INotificationService
    {

        private IHubContext<NotificationHub> _hub;
        private readonly IHostEnvironment _env;
        private readonly EmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly DB.DBContext _dbcontext;
        private readonly IRabbitMQProducer _messagePublisher;
        private readonly EmailServiceSettings _emailServiceSettings;
        public NotificationService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor, IHubContext<NotificationHub> hub,
           IOptions<EmailSettings> emailSettings, IHostEnvironment env, EmailSender mailSender, IRabbitMQProducer messagePublisher, IOptions<EmailServiceSettings> emailServiceSettings)
        {
            _emailSettings = emailSettings.Value;
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

                Execute(users.FirstOrDefault(), Subject, emailWithTemp);

               // _messagePublisher.SendMessage(email);

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
                        Byte[] attBytes = File.ReadAllBytes(_env.ContentRootPath + "/wwwroot/" + att.Path);
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
            return this.SendEmailJob(priority);
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
                    await _hub.Clients.Clients(userConnections.ToArray<string>()).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(notifyObj));
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

        private async Task<bool> Execute(string email, string subject, string message, Print_pdfDTO attachmentFile = null)
        {
            try
            {

                //var tempFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/EmailTemplate/TempEn.html", FileMode.Open, FileAccess.Read);
                //string emailWithTemp = "";
                //using (StreamReader reader = new StreamReader(tempFileStream))
                //{
                //    emailWithTemp = reader.ReadToEnd();
                //}
              var  emailWithTemp = "{{message}}"+ message;


                // Prepare an email message to be sent
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                mimeMessage.To.Add(MailboxAddress.Parse(email));

                if (!string.IsNullOrWhiteSpace(_emailSettings.MailBBC))
                {
                    mimeMessage.Bcc.Add(MailboxAddress.Parse(_emailSettings.MailBBC));
                }

                mimeMessage.Subject = "No subject";



                // Add email body and file attachments
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailWithTemp
                };
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                //if (attachmentFile != null)
                //{
                //    var builder = new BodyBuilder();
                //    builder.HtmlBody = emailWithTemp;
                //    //base46Attachment = GetBase64(base46Attachment);
                //    var bytes = attachmentFile.Data;
                //    //MimeEntity.Load(new ContentType("application", "pdf"), new MemoryStream(bytes));
                //    builder.Attachments.Add(attachmentFile.FileName, bytes);
                //    mimeMessage.Body = builder.ToMessageBody();
                //}


                //   // Connect and authenticate with the SMTP server
                var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                // Send email message
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
                return true;
            }
            catch
            {
                // TODO: handle exception
                //throw new InvalidOperationException(ex.Message);
            }
            return false;
        }


    }
}
