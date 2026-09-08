using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;
public class ProductSubCategory : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }


    public int? ProductMainCategoryId { get; private set; }
    public ProductMainCategory ProductMainCategory { get; private set; }

    public ProductSubCategory(string? nameEn, string? nameAr,  int? productMainCategoryId)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        ProductMainCategoryId = productMainCategoryId;
    }

    public void Update(string? nameEn, string? nameAr,  int? productMainCategoryId)
    {
        NameEn = nameEn;
        NameAr = nameAr;
        ProductMainCategoryId = productMainCategoryId;
    }
}
