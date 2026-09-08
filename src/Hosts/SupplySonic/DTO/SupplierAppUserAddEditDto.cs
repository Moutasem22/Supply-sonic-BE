using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class SupplierAppUserAddEditDto
{
    public int Id { get; set; }

    public string? VerificationCode { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public int? ProfileAttachmentId { get; set; }

    public string? StateName { get;  set; }

    public DateTime? Birthdate { get;  set; }

    public EnumGender? Gender { get; set; }

    public int? ResidenceCountryId { get; set; }

    public int? CityId { get; set; }

    public string? Address { get; set; }

    public bool? IsSupplier { get;  set; } 
    public EnumStatus? Status { get;  set; }
   // public string? GenderName { get { return ((Gender is not null ? Gender.ToString() : null)); } }
    public string? RejectedReason { get; set; }
   // public int? Age { get { return (DateTime.UtcNow.Year - (Birthdate != null ? Convert.ToDateTime(Birthdate).Year : DateTime.UtcNow.Year)); } }
}
