using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductMainCategory : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }

    public virtual ICollection<ProductSubCategory> ProductSubCategories { get; set; }

    public ProductMainCategory()
    {
        ProductSubCategories = new HashSet<ProductSubCategory>();
    }

    public ProductMainCategory(string? nameEn, string? nameAr)
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
