using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow
{
    public class WFStateRole : BaseEntity<int>
    {
        public int RoleId { get; set; }
        public Role Role { get;  set; }
        public int WFStateId { get; set; }
        public WFState WFState { get;  set; }
    }
}
