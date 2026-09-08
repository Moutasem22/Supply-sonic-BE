using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class NotificationTemplate:BaseEntity<int>
    {
        public string Name { get; private set; }        
        public string Code { get; private set; }
        public string MobileMsg { get; private set; }
        public string EmailMsg { get; private set; }
        public string WebMsg { get; private set; }
        public string MobileMsgEn { get; private set; }
        public string EmailMsgEn { get; private set; }
        public string WebMsgEn { get; private set; }
        public int? ToPageId { get; private set; }
        public EnumNotificationTemplateType NotificationTemplateType { get; private set; }

        public string Subject { get; set; }//for email
        public string SubjectEn { get; set; }//for email

        public void Update(string name, string mobileMsg, string emailMsg,string webMsg, string subject, string mobileMsgEn, string emailMsgEn, string webMsgEn, string subjectEn,bool isActive)
        {
            Name = name;
            MobileMsg = mobileMsg;
            EmailMsg = emailMsg;
            WebMsg = webMsg;
            Subject = subject;
            MobileMsgEn = mobileMsgEn;
            EmailMsgEn = emailMsgEn;
            WebMsgEn = webMsgEn;
            SubjectEn = subjectEn;
            IsActive = isActive;
        }
    }
}
