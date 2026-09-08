using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFStateUser : BaseEntity<int>
    {
        public int WFStateId { get; set; }
        public WFState WFState { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public bool? IsFinal { get; set; }

        public WFStateUser(int userId, int wFStateId, bool? isFinal, int roleId)
        {
            WFStateId = wFStateId;
            UserId = userId;
            IsFinal = isFinal;
            IsActive = true;
            IsDeleted = false;
            RoleId = roleId;
        }
    }
}
