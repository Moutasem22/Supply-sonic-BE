using Core.Models.Identity;
using Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class OrderDetails:BaseEntity<int>
{
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    public int ProductOfferId { get; set; }
    public virtual ProductOffer ProductOffer { get; set; }
    public double? Price { get; set; }
    public int? Qaunt { get; set; }
    public double? Discount { get; set; }
    public double? Total { get; set; }

    public OrderDetails()
    {
            
    }

    public OrderDetails( int ProductOfferId, double? Price, int? Qaunt, double? Discount, double? Total)
    {
        this.ProductOfferId = ProductOfferId;
        this.Price = Price;
        this.Qaunt = Qaunt;
        this.Discount = Discount;
        this.Total = Total;

    }

    public void Update (int ProductOfferId, double? Price, int? Qaunt, double? Discount, double? Total)
    {
        this.ProductOfferId = ProductOfferId;
        this.Price = Price;
        this.Qaunt = Qaunt;
        this.Discount = Discount;
        this.Total = Total;

    }
}
