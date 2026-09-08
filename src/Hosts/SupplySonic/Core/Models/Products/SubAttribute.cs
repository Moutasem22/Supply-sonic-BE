using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class SubAttribute : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }


    public int? AttributeId { get; private set; }
    public Attribute Attribute { get; private set; }

    public SubAttribute(string? nameEn, string? nameAr,  int? attributeId)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        AttributeId = attributeId;
    }

    public void Update(string? nameEn, string? nameAr,  int? attributeId)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        AttributeId = attributeId;
    }
}
