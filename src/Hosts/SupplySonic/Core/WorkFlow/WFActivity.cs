using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFActivity:BaseEntity<int>
    {
        public EnumWFActivityType ActivityType { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICollection<WFStateActivity> WFStateActivities { get; private set; }
        public ICollection<WFTransitionActivity> WFTransitionActivities { get; private set; }
        public WFActivity()
        {
            WFStateActivities = new HashSet<WFStateActivity>();
            WFTransitionActivities = new HashSet<WFTransitionActivity>();
        }
    }
}
