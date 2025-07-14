using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.IdentityDTO
{
    public class UserProfileEditDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }
        public int? ProfileAttachmentId { get; set; }
    }
}
