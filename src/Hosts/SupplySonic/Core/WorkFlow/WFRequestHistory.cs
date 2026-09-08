using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFRequestHistory:BaseEntity<int>
    {
        public Guid WFRequestId { get; private set; }
        public WFRequest WFRequest { get; private set; }
        public int WFCurrentStateId { get; private set; }
        public WFState WFCurrentState { get; private set; }
        public int? WFPrevStateId { get; private set; }
        public EnumWFActionType LastActionType { get; private set; }
        public HashSet<WFRejectReason> WFRejectReasons { get; private set;}
        public WFRequestHistory(int wFCurrentStateId, int? wFPrevStateId, EnumWFActionType lastActionType, WFRejectReason wFRejectReason=null) :this()
        {
            WFCurrentStateId = wFCurrentStateId;
            WFPrevStateId = wFPrevStateId;
            LastActionType = lastActionType;
            if(wFRejectReason !=null)
            {
                WFRejectReasons.Add(wFRejectReason);
            }
        }
        private WFRequestHistory()
        {
            WFRejectReasons = new HashSet<WFRejectReason>();
        }
    }
}
