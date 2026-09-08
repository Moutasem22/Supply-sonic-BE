using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFUserPage: BaseEntity<int>
    {
        public int WFStateId { get; set; }
        public WFState WFState { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public bool? IsFinal { get; set; }


        public WFUserPage(int userId, int wFStateId, bool? isFinal) 
        {
            WFStateId = wFStateId;
            UserId = userId;
            IsFinal = isFinal;
            IsActive = true;
            IsDeleted = false;
        }
    }
}
