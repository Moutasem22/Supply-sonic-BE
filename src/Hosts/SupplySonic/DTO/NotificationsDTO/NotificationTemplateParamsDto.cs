using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.NotificationsDTO
{
    public class NotificationTemplateParamsDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumNotificationTemplateType NotificationTemplateType { get; set; }
    }
}
