using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFRequestAction:BaseEntity<int>
    {
        public WFRequest WFRequest { get; private set; }
        public Guid WFRequestId { get; private set; }
        public WFAction WFAction { get; private set; }
        public int WFActionId { get; private set; }
        public WFTransition WFTransition { get; private set; }
        public int WFTransitionId { get; private set; }
        public bool IsReady { get; private set; }// instead of isactive 
        public bool IsComplete { get; private set; }
        public WFRequestAction(Guid WFRequestId,int WFTransitionId ,int WFActionId)
        {
            this.WFRequestId = WFRequestId;
            this.WFTransitionId = WFTransitionId;
            this.WFActionId = WFActionId;
            this.IsReady = true;
            this.IsComplete = false;
        }
        public void Complete()
        {
            this.IsComplete = true;
        }
        public void Disable()
        {
            this.IsReady = false;
        }
    }
}
