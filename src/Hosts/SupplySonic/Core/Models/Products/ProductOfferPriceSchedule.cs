using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Products;

public class ProductOfferPriceSchedule:BaseEntity<int>
{
    public int ProductOfferId { get; private set; }
    public virtual ProductOffer? ProductOffer { get;  set; }

    public double UnitPrice { get; private set; }

    //Option1
    public int QuantityOption1From { get; private set; }
    public int QuantityOption1To { get; private set; }
    public double QuantityOption1UnitPrice { get; private set; }

    //Option2
    public int? QuantityOption2From { get; private set; }
    public int? QuantityOption2To { get; private set; }
    public double? QuantityOption2UnitPrice { get; private set; }

    //Option3
    public int? QuantityOption3From { get; private set; }
    public int? QuantityOption3To { get; private set; }
    public double? QuantityOption3UnitPrice { get; private set; }

    public ProductOfferPriceSchedule()
    {
                
    }

    public ProductOfferPriceSchedule(int productOfferId , double unitPrice,
         int quantityOption1From, int quantityOption1To, double quantityOption1UnitPrice,
         int? quantityOption2From, int? quantityOption2To, double? quantityOption2UnitPrice,
         int? quantityOption3From, int? quantityOption3To, double? quantityOption3UnitPrice)
    {
        this.ProductOfferId = productOfferId;
        this.UnitPrice = unitPrice;
        this.QuantityOption1From= quantityOption1From;
        this.QuantityOption1To= quantityOption1To;
        this.QuantityOption1UnitPrice= quantityOption1UnitPrice;

        this.QuantityOption2From = quantityOption2From;
        this.QuantityOption2To = quantityOption2To;
        this.QuantityOption2UnitPrice = quantityOption2UnitPrice;

        this.QuantityOption3From = quantityOption3From;
        this.QuantityOption3To = quantityOption3To;
        this.QuantityOption3UnitPrice = quantityOption3UnitPrice;

    }

    public ProductOfferPriceSchedule( double unitPrice,
        int quantityOption1From, int quantityOption1To, double quantityOption1UnitPrice,
        int? quantityOption2From, int? quantityOption2To, double? quantityOption2UnitPrice,
        int? quantityOption3From, int? quantityOption3To, double? quantityOption3UnitPrice)
    {
        this.UnitPrice = unitPrice;
        this.QuantityOption1From = quantityOption1From;
        this.QuantityOption1To = quantityOption1To;
        this.QuantityOption1UnitPrice = quantityOption1UnitPrice;

        this.QuantityOption2From = quantityOption2From;
        this.QuantityOption2To = quantityOption2To;
        this.QuantityOption2UnitPrice = quantityOption2UnitPrice;

        this.QuantityOption3From = quantityOption3From;
        this.QuantityOption3To = quantityOption3To;
        this.QuantityOption3UnitPrice = quantityOption3UnitPrice;

    }
}
