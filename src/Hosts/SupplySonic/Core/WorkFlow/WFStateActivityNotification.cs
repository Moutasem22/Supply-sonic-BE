using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFStateActivityNotification:BaseEntity<int>
    {
        public int WFStateActivityId { get; set; }
        public WFStateActivity WFStateActivity { get; set; }
        public string NotificationTemplateAr { get; set; }
        public string NotificationTemplateEn { get; set; }
    }
}
