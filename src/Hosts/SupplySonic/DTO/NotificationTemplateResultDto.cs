using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class NotificationTemplateResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileMsg { get; set; }
        public string EmailMsg { get; set; }
        public string WebMsg { get; set; }
        public string Subject { get; set; }//for email
        public string MobileMsgEn { get; set; }
        public string EmailMsgEn { get; set; }
        public string WebMsgEn { get; set; }
        public bool IsActive { get; set; }

        public string SubjectEn { get; set; }//for email
        public EnumNotificationTemplateType NotificationTemplateType { get; set; }
        public string NotificationTemplateTypeStr { get { return NotificationTemplateType.ToString(); } }
        public ICollection<NotificationTemplateParamsDto> NotificationTemplateParams { get; set; }
    }
}
