using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product;

public class ProductResultDto
{
    public int Id { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string? Name { get; set; }
    public string? MainDescriptionEn { get; set; }
    public string? MainDescriptionAr { get; set; }
    public string? SecondDescriptionEn { get; set; }
    public string? SecondDescriptionAr { get; set; }
    public string? AdditionalInfo { get; set; }
    public int SupplierId { get; set; }
    public int ProductMainCategoryId { get; set; }
    public int ProductSubCategoryId { get; set; }
    public List<AttachmentDto> ProductAttachments { get; set; }
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }

}
