using Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class UserAddEditDto
    {
        public int Id { get; set; }
        public string? VerificationCode { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }       
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsExternal { get; set; }
        public List<int>? UserRoles { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public int? UserRoleId { get; set; }

        public int? ResidenceCountryId { get; set; }
        public int? CityId { get; set; }


        public string? StateName { get; set; }

        public string? Birthdate { get; set; }

        public EnumGender? Gender { get; set; }
    }
}
