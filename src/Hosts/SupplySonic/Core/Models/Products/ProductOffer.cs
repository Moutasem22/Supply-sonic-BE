using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductOffer:BaseEntity<int>
{
    public int ProductId { get; private set; }
    public virtual Product? Product { get;  set; }

    public ICollection<ProductOfferAttachment> ProductOfferAttachments { get; set; }
    public ICollection<ProductOfferAttribute> ProductOfferAttributes { get; set; }

    public ICollection<ProductOfferPriceSchedule> ProductOfferPriceSchedules { get; set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }


    public ProductOffer()
    {
        ProductOfferAttachments = new HashSet<ProductOfferAttachment>();
        ProductOfferAttributes = new HashSet<ProductOfferAttribute>();
        ProductOfferPriceSchedules = new HashSet<ProductOfferPriceSchedule>();
    }

    public ProductOffer(int productId, bool isActive, DateTime? StartDate, DateTime? EndDate)
    {
        this.ProductId = productId;
        this.IsActive = isActive;
        this.StartDate = StartDate;
        this.EndDate = EndDate;

    }

    public void Update(int productId, bool isActive, DateTime? StartDate, DateTime? EndDate)
    {
        this.ProductId = productId;
        this.IsActive = isActive;
        this.StartDate = StartDate;
        this.EndDate = EndDate;

    }

    public void SaveProductOfferAttachments(List<ProductOfferAttachment> productAttachments)
    {
        this.ProductOfferAttachments.Clear();
        this.ProductOfferAttachments = productAttachments;

    }

    public void SaveProductOfferAttributes(List<ProductOfferAttribute> productAttributes)
    {
        this.ProductOfferAttributes.Clear();
        this.ProductOfferAttributes = productAttributes;

    }
    public void SaveProductOfferPriceSchedules(List<ProductOfferPriceSchedule> productOfferPriceSchedule)
    {
        this.ProductOfferPriceSchedules.Clear();
        this.ProductOfferPriceSchedules = productOfferPriceSchedule;

    }
}
