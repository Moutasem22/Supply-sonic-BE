using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class Attribute : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }

    public virtual ICollection<SubAttribute> SubAttributes { get; set; }

    public Attribute()
    {
        SubAttributes = new HashSet<SubAttribute>();
    }

    public Attribute(string? nameEn, string? nameAr)
    {
        NameEn = nameEn;
        NameAr = nameAr;

    }
    public void Update(string? nameEn, string? nameAr)
    {
        NameEn = nameEn;
        NameAr = nameAr;
    }
}
