using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class UserAddress : BaseEntity<int>
{
    public int SupplierAppUserId { get; set; }
    public virtual SupplierAppUser SupplierAppUser { get; set; }

    public int? CountryId { get; set; }
    public virtual Country? Country { get; set; }

    public int? CityId { get; set; }
    public virtual City? City { get; set; }

    public string? SecondPhone { get; set; }
    public string? Notes { get; set; }

    public string? Address { get; set; }
    public bool? IsDefalut { get; set; } = false;

    public UserAddress()
    {

    }

    public  UserAddress(int SupplierAppUserId, int? CountryId, int? CityId, string? SecondPhone, string? Notes, string? Address, bool? IsDefalut)
    { 
    
        this.Address = Address;
        this.CityId = CityId;
        this.SecondPhone = SecondPhone;
        this.Notes = Notes;
        this.SupplierAppUserId = SupplierAppUserId;
        this.CountryId = CountryId;
        this.IsDefalut= IsDefalut;


    }

    public void Update(int SupplierAppUserId, int? CountryId, int? CityId, string? SecondPhone, string? Notes, string? Address, bool? IsDefalut)
    {

        this.Address = Address;
        this.CityId = CityId;
        this.SecondPhone = SecondPhone;
        this.Notes = Notes;
        this.SupplierAppUserId = SupplierAppUserId;
        this.CountryId = CountryId;
        this.IsDefalut = IsDefalut;


    }
}
