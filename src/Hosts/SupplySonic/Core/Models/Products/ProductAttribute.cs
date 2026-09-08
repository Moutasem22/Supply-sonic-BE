using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductAttribute : BaseEntity<int>
{
    public int AttributeId { get;private set; }
    public virtual Attribute? Attribute { get; set; }

    public int SubAttributeId { get; set; }
    public virtual SubAttribute? SubAttribute { get; set; }
    public int ProductId { get; set; }
    public virtual Product? Product { get; private set; }

    public ProductAttribute()
    {

    }

    public ProductAttribute(int attributeId, int subAttributeId)
    {
        this.AttributeId = attributeId;
        this.SubAttributeId = subAttributeId;
    }

    public ProductAttribute(int attributeId, int subAttributeId, int productId)
    {
        this.AttributeId = attributeId;
        this.SubAttributeId = subAttributeId;
        this.ProductId = productId;
    }
}
