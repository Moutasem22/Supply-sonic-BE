using Core.Models.Products;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Product;

namespace DTO;

public class OrderDetailsResultDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductOfferId { get; set; }
    public ProductOfferResultDto? ProductOffer { get; set; }
    public double? Price { get; set; }
    public int? Qaunt { get; set; }
    public double? Discount { get; set; }
    public double? Total { get; set; }

     
}
