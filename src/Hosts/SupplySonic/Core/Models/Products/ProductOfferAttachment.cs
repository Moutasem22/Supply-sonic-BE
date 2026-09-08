using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductOfferAttachment:BaseEntity<int>
{
    public int AttachmentId { get; private set; }
    public virtual Attachment? Attachment { get; set; }
    public int ProductOfferId { get; private set; }
    public virtual ProductOffer? ProductOffer { get; set; }

    public ProductOfferAttachment()
    {

    }

    public ProductOfferAttachment(int attachmentId)
    {
        this.AttachmentId = attachmentId;
    }

    public ProductOfferAttachment(int attachmentId, int productOfferId)
    {
        this.AttachmentId = attachmentId;
        this.ProductOfferId = productOfferId;
    }
}
