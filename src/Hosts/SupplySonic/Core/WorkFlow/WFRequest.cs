using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFRequest:BaseEntity<Guid>
    {
        public int WorkFlowId { get; private set; }
        public WorkFlow WorkFlow { get; private set; }
        public int WFCurrentStateId { get; private set; }
        public WFState WFCurrentState { get; private set; }
        public ICollection<WFRequestAction> WFRequestActions { get; private set; }
        public ICollection<WFRequestHistory> WFRequestHistories { get; private set; }
        public WFRequestPosition WFRequestPosition { get; private set; }
        public EnumWFRequestSituation WFRequestSituation { get; private set; }
        public EnumWFActionType LastActionType { get; private set; }
        public int? WFPrevStateId { get; set; }
        public WFRequest()
        {
            WFRequestActions = new HashSet<WFRequestAction>();
            WFRequestHistories = new HashSet<WFRequestHistory>();
        }
        public WFRequest(Guid Id, int WorkFlowId) :this()
        {
            this.Id = Id;
            this.WorkFlowId = WorkFlowId;
        }
        public void UpdateState(int CurrentStateId, int? PrevStateId, EnumWFActionType LastActionType, EnumWFRequestSituation WFRequestSituation, WFRejectReason RejectReason)
        {
            this.WFCurrentStateId = CurrentStateId;
            this.WFPrevStateId = PrevStateId;
            this.LastActionType = LastActionType;
            this.WFRequestSituation = WFRequestSituation;
            this.WFRequestHistories.Add(new WFRequestHistory(CurrentStateId, PrevStateId, LastActionType,RejectReason));
        }
    }
}
