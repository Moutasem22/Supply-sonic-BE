using Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class BindingRoomAttribute : BaseEntity<int>
{
    public int AttributeId { get; private set; }
    public virtual Products.Attribute? Attribute { get; set; }

    public int SubAttributeId { get; private set; }
    public virtual SubAttribute? SubAttribute { get; set; }
    public int BindingRoomId { get; private set; }
    public virtual BindingRoom? BindingRoom { get; set; }

    public BindingRoomAttribute()
    {

    }

    public BindingRoomAttribute(int attributeId, int subAttributeId)
    {
        this.AttributeId = attributeId;
        this.SubAttributeId = subAttributeId;
    }

    public BindingRoomAttribute(int attributeId, int subAttributeId, int bindingRoomId)
    {
        this.AttributeId = attributeId;
        this.SubAttributeId = subAttributeId;
        this.BindingRoomId = bindingRoomId;
    }
}

