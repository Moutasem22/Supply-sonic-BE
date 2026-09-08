using Core.Enums;
using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFState:BaseEntity<int>
    {
        public string Name { get; private set; }
        public ICollection<WFTransition> WFCurrentTransitions { get; private set; }
        public ICollection<WFTransition> WFNextTransitions { get; private set; }
        public ICollection<WFStateActivity> WFStateActivity { get; private set; }
        public int PageId { get; set; }
        public Page Page { get; private set; }
        public EnumWFLevel WFPageLevel { get; set; }
        public ICollection<WFStateAssignedUser> WFStateAssignedUsers { get; set; }
        public ICollection<WFStateUser> WFStateUsers { get; set; }
        public ICollection<WFRejectReason> WFRejectReasons { get; set; }
        public WFState()
        {
            WFCurrentTransitions = new HashSet<WFTransition>();
            WFNextTransitions = new HashSet<WFTransition>();
            WFStateActivity = new HashSet<WFStateActivity>();
            WFStateAssignedUsers = new HashSet<WFStateAssignedUser>();
            WFStateUsers = new HashSet<WFStateUser>();
        }

    }
}
