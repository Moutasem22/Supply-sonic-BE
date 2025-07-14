using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.IdentityDTO
{
    public class UserRoleAddEditDTO
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
