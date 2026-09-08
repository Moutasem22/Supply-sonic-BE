using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFAction:BaseEntity<int>
    {
        public EnumWFActionType ActionType { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ActionId { get; private set; }
        public Core.Models.Identity.Action Action { get; private set; }
        public ICollection<WFTransitionAction> WFTransitionActions { get; private set; }
        public ICollection<WFRequestAction> WFRequestActions { get; private set; }
        public ICollection<WFTransitionActivity> WFTransitionActivities { get; private set; }
        public WFAction()
        {
            WFTransitionActions = new HashSet<WFTransitionAction>();
            WFRequestActions = new HashSet<WFRequestAction>();
            WFTransitionActivities = new HashSet<WFTransitionActivity>();
        }
    }
}
