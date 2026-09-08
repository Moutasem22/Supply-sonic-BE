using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
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
        public List<UserRoleAddEditDTO>? UserRoles { get; set; }
        public string? UserRoleNames { get; set; }
        //public string Department { get; set; }        
        public string Title { get; set; }
        public int? ProfileAttachmentId { get; set; }
        public string Path { get; set; }
        public AttachmentDto ProfileAttachment { get; set; }




        public int? NationalityId { get; set; }


        public EnumIdentityType IdentityType { get; set; }

        public string? IdentityNumber { get; set; }

        public string? ExpireDate { get; set; }

        public int? IdBackAttachmentId { get; set; }
        public AttachmentDto IdBackAttachment { get; set; }

        public int? IdFrontAttachmentId { get; set; }
        public AttachmentDto IdFrontAttachment { get; set; }

        public string? CityName { get; set; }

        public string? CountryName { get; set; }
        public string? NationalityName { get; set; }
        public string? IdentityTypeName { get; set; }

        public EnumProviderStatus? ProviderStatus { get; set; }

        public string? ProviderStatusName { get; set; }

        public string? ProviderStatusReason { get; set; }
        public float? CurrentBalance { get; set; } = 0;

        
        public int? ResidenceCountryId { get; set; }
        public int? CityId { get; set; }
        public string? Address { get; set; }


        public string? StateName { get;  set; }

        public string? Birthdate { get;  set; }

        public EnumGender? Gender { get; set; }

        public string? GenderName { get { return ((Gender is not null ? Gender.ToString() : null)); } }

        public int? Age { get { return (DateTime.UtcNow.Year - (Birthdate != null ? Convert.ToDateTime(Birthdate).Year : DateTime.UtcNow.Year)); } }

        public string? IdentityIssueDate { get; set; }

        public int? UserRoleId { get; set; }

        public string? RejectedReason { get; set; }

    }
}
