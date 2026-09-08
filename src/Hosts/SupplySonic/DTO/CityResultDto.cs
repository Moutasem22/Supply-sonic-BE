using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class CityResultDto
{

    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
    public byte[] RowVersion { get; set; }
    public string? NameUzbek { get; set; }
    public string? NameRussian { get; set; }
    public int? CountryId { get; set; }
    public string? Name { get; set; }
    public string? CountryName { get; set; }
}
