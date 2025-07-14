using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Identity
{
    public class Permission:BaseEntity<int>
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int PageActionId { get; set; }
        public PageAction PageAction { get; set; }
    }
}
