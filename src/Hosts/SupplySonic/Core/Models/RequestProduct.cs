using Core.Models.Identity;
using Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class RequestProduct:BaseEntity<int>
{
    public int RequesterId { get; set; }
    public virtual SupplierAppUser? Requester { get; set; }
    public int ProductMainCategoryId { get; private set; }
    public virtual ProductMainCategory ProductMainCategory { get; set; }
    public int ProductSubCategoryId { get; private set; }
    public virtual ProductSubCategory ProductSubCategory { get; set; }
    public string Address { get; set; }
    public string? AdditionalInfo { get;  set; }
    public string? BuildingNumber { get; set; }
    public string? LandMark { get; set; }
    public int CountryId { get; set; }
    public virtual Country? Country { get; set; }
    public int CityId { get; set; }
    public virtual City? City { get; set; }


}
