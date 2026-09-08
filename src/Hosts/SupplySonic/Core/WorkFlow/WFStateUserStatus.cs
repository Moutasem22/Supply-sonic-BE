using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFStateUserStatus:BaseEntity<int>
    {
        public WFStateUserStatus(int WFStateAssignedUserId, Guid WFRequestId, EnumWFCurrentUserState CurrentUserState)
        {
            this.WFStateAssignedUserId = WFStateAssignedUserId;
            this.WFRequestId = WFRequestId;
            this.CurrentUserState = CurrentUserState;
        }
        public int WFStateAssignedUserId { get; set; }
        public WFStateAssignedUser WFStateAssignedUser { get; set; }
        public Guid WFRequestId { get; set; }
        public EnumWFCurrentUserState CurrentUserState { get; set; }
        public void Update(EnumWFCurrentUserState CurrentUserState)
        {
            this.CurrentUserState = CurrentUserState;

        }
    }
}
