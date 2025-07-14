using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public AppUser AppUser { get; set; }
        public Role Role { get; set; }
    }
}
