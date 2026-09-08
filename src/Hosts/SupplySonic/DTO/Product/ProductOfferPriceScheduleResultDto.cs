using Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product;

public class ProductOfferPriceScheduleResultDto
{
    public int Id { get; set; }
    public int ProductOfferId { get;  set; }
    public double UnitPrice { get;  set; }

    //Option1
    public int QuantityOption1From { get;  set; }
    public int QuantityOption1To { get;  set; }
    public double QuantityOption1UnitPrice { get;  set; }

    //Option2
    public int? QuantityOption2From { get;  set; }
    public int? QuantityOption2To { get;  set; }
    public double? QuantityOption2UnitPrice { get;  set; }

    //Option3
    public int? QuantityOption3From { get;  set; }
    public int? QuantityOption3To { get;  set; }
    public double? QuantityOption3UnitPrice { get;  set; }


}
