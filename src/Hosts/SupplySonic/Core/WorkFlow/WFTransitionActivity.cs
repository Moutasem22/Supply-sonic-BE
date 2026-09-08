using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFTransitionActivity:BaseEntity<int>
    {
        public int WFTransitionId { get; private set; }
        public WFTransition WFTransition { get; private set; }
        public int WFActivityId { get; private set; }
        public WFActivity WFActivity { get; private set; }
        public int PrevWFActionId { get; set; }
        public WFAction PrevWFAction { get; set; }
        public ICollection<WFTransitionActivityNotification> WFTransitionActivityNotifications { get; set; }
        public WFTransitionActivity()
        {
            WFTransitionActivityNotifications = new HashSet<WFTransitionActivityNotification>();
        }
    }
}
