using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFRequestPosition:BaseEntity<long> //table mohsen for showing current state 
    {
        public Guid WFRequestId { get; set; }
        public WFRequest WFRequest { get;  set; }
        public EnumWFRequestState FirstApproval { get; set; }
        public EnumWFRequestState SecondApproval { get; set; }
        public EnumWFRequestState ThirdApproval { get; set; }
        public EnumWFRequestState FourthApproval { get; set; }
    }
}
