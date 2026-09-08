using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFTransitionActivityNotification:BaseEntity<int>
    {
        public int WFTransitionActivityId { get; set; }
        public WFTransitionActivity WFTransitionActivity { get; set; }
        public string NotificationTemplateAr { get; set; }
        public string NotificationTemplateEn { get; set; }
    }
}
