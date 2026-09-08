using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.WorkFlow
{
    public class WFRejectReason : BaseEntity<int>
    {
        public WFRejectReason(string reason,int stateId, int rejectedBy) : this()
        {            
            this.Reason = reason;
            this.StateId = stateId;
            this.RejectedBy = rejectedBy;
        }       
        public string Reason { get; private set; }
        public int StateId { get; private set; }
        public int RejectedBy { get; private set; }
        public int WFRequestHistoryId { get; private set; }
        public WFRequestHistory WFRequestHistory { get; private set; }
        private WFRejectReason()
        {
        }

    }
}
