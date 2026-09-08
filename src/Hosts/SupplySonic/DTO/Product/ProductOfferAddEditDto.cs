using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product;

public class ProductOfferAddEditDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductResultDto? Product { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
    public byte[] RowVersion { get; set; }
}
