using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFTransition:BaseEntity<int>
    {
        public int WorkFlowId { get; private set; }
        public WorkFlow WorkFlow { get; private set; }
        public int WFCurrentStateId { get; private set; }
        public WFState WFCurrentState { get; private set; }
        public int? WFNextStateId { get; private set; }
        public WFState WFNextState { get; private set; }
        public ICollection<WFTransitionActivity> WFTransitionActivities { get; private set; }
        public ICollection<WFTransitionAction> WFTransitionActions { get; private set; }
        public ICollection<WFRequestAction> WFRequestActions { get; private set; }
        public bool InitPoint { get; private set; }
        public WFTransition()
        {
            WFTransitionActivities = new HashSet<WFTransitionActivity>();
            WFTransitionActions = new HashSet<WFTransitionAction>();
            WFRequestActions = new HashSet<WFRequestAction>();
        }
    }
}
