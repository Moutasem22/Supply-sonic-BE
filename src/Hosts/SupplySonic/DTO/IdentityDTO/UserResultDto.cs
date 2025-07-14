using System;
using System.Collections.Generic;
using System.Text;
using DTO.CommandDTO;

namespace DTO.IdentityDTO
{
    public class UserResultDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? VerificationCode { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public bool? IsActive { get; set; }
        public int TotalNotifications { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Extension { get; set; }
        public string? URN { get; set; }
        public bool IsSelected { get { return false; } }
        public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
        //public bool IsSuperAdmin { get; set; }
        //public int UserType { get; set; }
        public bool IsExternal { get; set; }
        public string? UserId { get; set; }
        public int? TitleId { get; set; }
        //public bool? IsLicensedUser { get; set; }
        //public bool? IsSystemAdmin { get; set; }
        //public bool? IsPMO { get; set; }
        //public bool? IsInitiativesManager { get; set; }
        //public bool? IsStrategyManager { get; set; } 
        public ICollection<UserRoleAddEditDTO> UserRoles { get; set; }
        public string? UserRoleNames { get; set; }
        //public string Department { get; set; }        
        public string Title { get; set; }
        public int? ProfileAttachmentId { get; set; }
        public string Path { get; set; }
        public AttachmentDto ProfileAttachment { get; set; }
    }
}
