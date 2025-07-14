using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.NotificationsDTO
{
    public class UserNotificationDTO
    {
        public long? Id { get; set; }
        public string URL { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedDateH { get; set; }
        public EnumNotificationState? NotificationState { get; set; }
        public string MessageEn { get; set; }
        public string SubjectEn { get; set; }
    }
}
