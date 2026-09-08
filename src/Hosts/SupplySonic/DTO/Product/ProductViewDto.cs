using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product;

public class ProductViewDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductUniqueId { get { return EncryptionHelper.EncryptForUrl(ProductId.ToString()); } }
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }

    public string ProductName { get; set; } = string.Empty;

    public float ProductPrice { get; set; }

    public List<AttachmentDto>? ProductOfferAttachments { get; set; } = new List<AttachmentDto>();

    public float? ProductRate { get; set; } = 5;

    public string UOM { get; set; }

    public bool IsActive { get; set; } = false;
}
