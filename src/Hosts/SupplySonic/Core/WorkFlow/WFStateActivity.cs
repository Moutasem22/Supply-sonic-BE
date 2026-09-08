using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFStateActivity:BaseEntity<int>
    {
        public int WFStateId { get; private set; }
        public WFState WFState { get; private set; }
        public int WFActivityId { get; private set; }
        public WFActivity WFActivity { get; private set; }
        public int PrevWFActionId { get; set; }
        public WFAction PrevWFAction { get; set; }
        public ICollection<WFStateActivityNotification> WFStateActivityNotifications { get; set; }
        public WFStateActivity()
        {
            WFStateActivityNotifications = new HashSet<WFStateActivityNotification>();
        }
    }
}
