using System;
using System.Collections.Generic;
using System.Text;

namespace DTO;

public class UserProfileEditDto
{
    public int Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string Extension { get; set; }
    public int? ProfileAttachmentId { get; set; }
    public int? ResidenceCountryId { get; private set; }
    public int? CityId { get; private set; }
    public string? Address { get; set; }

}
