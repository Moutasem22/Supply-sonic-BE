using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Notifications
{
    public class NotificationTemplateParams : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EnumNotificationTemplateType NotificationTemplateType { get; set; }
    }
}
