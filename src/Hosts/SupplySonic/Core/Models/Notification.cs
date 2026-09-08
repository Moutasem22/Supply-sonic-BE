using Core.Enums;
using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class Notification : BaseEntity<long>
    {
        public string? URL { get; private set; }
        public string? Subject { get;private set; }
        public string? SubjectAr { get; private set; }
        public string? Message { get; private set; }
        public string? MessageAr { get; private set; }
        public int? AttachmentId { get; private set; }

        public string? TableName { get; private set; }
        public EnumMethodType MethodType { get; private set; }
        public EnumNotificationType NotificationType { get; private set;}
        public int RowId { get;  private set; }
        public ICollection<UserNotification> UserNotifications { get; private  set; }
        public EnumPriority Priority { get; private set; }

        private Notification()
        {
            UserNotifications = new HashSet<UserNotification>();
        }
        public Notification(string subject, string subjectAr, string Message, string MessageAr,  EnumNotificationType notificationType,int? attachemntId= null,List<int> users=null,string url = "", EnumPriority priority = EnumPriority.Normal) :this()
        {            
            this.Subject = subject;
            this.Message = Message;
            this.MessageAr = MessageAr;
            this.NotificationType = notificationType;
            this.SubjectAr = subjectAr;
            this.AttachmentId = attachemntId;
            this.Priority = priority;
            this.URL = url;
            if(users!=null)
            {
                users.ForEach(x =>
                {
                    this.UserNotifications.Add(new UserNotification()
                    {
                        AppUserId = x,
                        Email = ""
                    });
                });
            }
        }
    }
}
