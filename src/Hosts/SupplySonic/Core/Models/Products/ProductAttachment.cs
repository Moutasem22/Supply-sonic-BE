using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductAttachment : BaseEntity<int>
{
    public int AttachmentId { get; set; }
    public virtual Attachment? Attachment { get; set; }
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public ProductAttachment()
    {

    }

    public ProductAttachment(int attachmentId)
    {
        this.AttachmentId = attachmentId;
    }

    public ProductAttachment(int attachmentId, int productId)
    {
        this.AttachmentId = attachmentId;
        this.ProductId = productId;
    }
}
