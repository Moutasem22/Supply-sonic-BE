using System;
using System.Collections.Generic;
using System.Text;
using Core.Models.Identity;

namespace Core.WorkFlow
{
    public class WFStateAssignedUser:BaseEntity<int>
    {
        public int  UserId { get; set; }
        public AppUser User { get; set; }
        public int WFStateId { get; set; }
        public WFState WFState { get; set; }
        public WFStateUserStatus WFStateUserStatus { get; set; }

    }
}
