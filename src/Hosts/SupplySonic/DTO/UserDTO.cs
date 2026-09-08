using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? Title { get; set; }
        public int? ProfileAttachmentId { get; set; }
        public AttachmentDto?   ProfileAttachment { get; set; }
        public bool? IsEnabled { get; set; }       
    }
}
