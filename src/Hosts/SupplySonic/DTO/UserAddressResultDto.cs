using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class UserAddressResultDto
{
    public int Id { get; set; }
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
    public byte[] RowVersion { get; set; }
    public int SupplierAppUserId { get; set; }

    public int? CountryId { get; set; }
    public string? CountryName { get; set; }

    public int? CityId { get; set; }
    public string? CityName { get; set; }

    public string? SecondPhone { get; set; }
    public string? Notes { get; set; }

    public string? Address { get; set; }
    public bool? IsDefalut { get; set; } = false;
}
