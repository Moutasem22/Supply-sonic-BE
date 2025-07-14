using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.IdentityDTO
{
    public class UserAddEditDto
    {
        public int Id { get; set; }
        public string VerificationCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public bool IsExternal { get; set; }
        public List<int> UserRoles { get; set; }
    }
}
