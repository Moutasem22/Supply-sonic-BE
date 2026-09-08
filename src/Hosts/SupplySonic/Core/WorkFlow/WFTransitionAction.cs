using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFTransitionAction:BaseEntity<int>
    {
        public int WFTransitionId{ get; private set; }
        public WFTransition WFTransition { get; private set; }
        public int WFActionId{ get; private set; }
        public WFAction WFAction { get; private set; }
    }
}
