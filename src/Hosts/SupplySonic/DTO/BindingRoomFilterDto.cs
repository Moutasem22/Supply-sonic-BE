using Core.Enums;
using DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class BindingRoomFilterDto
{
   
    public int? SupplierAppUserId { get; set; }

    public string? ProductDescription { get; set; }
    public string? ProductName { get; set; }


    public int? ProductMainCategoryId { get; set; }


    public int? ProductSubCategoryId { get; set; }


    public string? MainDescription{ get; set; }


    public bool? IsSupplier { get; set; } 
    public EnumStatus? Status { get; set; } 


    public EnumBindingRoomStatus? BindingRoomStatus { get; set; } 

    public int? QauntFrom { get; set; }
    public int? QauntTo { get; set; }

    public List<int?>? AttributesIds { get; set; } 
}
