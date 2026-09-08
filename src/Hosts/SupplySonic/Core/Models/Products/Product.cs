using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class Product : BaseEntity<int>
{
    public string? NameEn { get; private set; }
    public string? NameAr { get; private set; }
    public string? MainDescriptionEn { get; private set; }
    public string? MainDescriptionAr { get; private set; }
    public string? SecondDescriptionEn { get; private set; }
    public string? SecondDescriptionAr { get; private set; }
    public string? AdditionalInfo { get; private set; }

    public int SupplierId { get; private set; }
    public virtual SupplierAppUser? Supplier { get; set; }

    public int ProductMainCategoryId { get; private set; }
    public virtual ProductMainCategory ProductMainCategory { get; set; }

    public int ProductSubCategoryId { get; private set; }
    public virtual ProductSubCategory ProductSubCategory { get; set; }

    public ICollection<ProductAttachment> ProductAttachments { get; set; }
    public ICollection<ProductAttribute> ProductAttributes { get; set; }
    public ICollection<ProductWeight> ProductWeights { get; set; }
    
    public Product()
    {
        ProductAttachments = new HashSet<ProductAttachment>();
        ProductAttributes = new HashSet<ProductAttribute>();
        ProductWeights = new HashSet<ProductWeight>();
    }

    public Product(string? nameEn, string? nameAr, string? mainDescriptionEn, string? mainDescriptionAr,
                   string? secondDescriptionEn, string? secondDescriptionAr, string? additionalInfo,
                   int supplierId, int productSubCategoryId, int productMainCategoryId)
    {
        this.NameEn = nameEn;
        this.NameAr = nameAr;
        this.MainDescriptionEn = mainDescriptionEn;
        this.MainDescriptionAr = mainDescriptionAr;
        this.SecondDescriptionEn = secondDescriptionEn;
        this.SecondDescriptionAr = secondDescriptionAr;
        this.AdditionalInfo = additionalInfo;
        this.SupplierId = supplierId;
        this.ProductMainCategoryId = productMainCategoryId;
        this.ProductSubCategoryId = productSubCategoryId;

    }

    public void Update(string? nameEn, string? nameAr, string? mainDescriptionEn, string? mainDescriptionAr,
        string? secondDescriptionEn, string? secondDescriptionAr, string? additionalInfo,
        int supplierId, int productSubCategoryId, int productMainCategoryId)
    {
        this.NameEn = nameEn;
        this.NameAr = nameAr;
        this.MainDescriptionEn = mainDescriptionEn;
        this.MainDescriptionAr = mainDescriptionAr;
        this.SecondDescriptionEn = secondDescriptionEn;
        this.SecondDescriptionAr = secondDescriptionAr;
        this.AdditionalInfo = additionalInfo;
        this.SupplierId = supplierId;
        this.ProductMainCategoryId = productMainCategoryId;
        this.ProductSubCategoryId = productSubCategoryId;
    }

    public void SaveProductAttachments(List<ProductAttachment> productAttachments)
    {
        this.ProductAttachments.Clear();
        this.ProductAttachments = productAttachments;

    }

    public void SaveProductAttributes(List<ProductAttribute> productAttributes)
    {
        this.ProductAttributes.Clear();
        this.ProductAttributes = productAttributes;

    }

    public void SaveProductWeights(List<ProductWeight> productWeights)
    {
        this.ProductWeights.Clear();
        this.ProductWeights = productWeights;
    }
}
