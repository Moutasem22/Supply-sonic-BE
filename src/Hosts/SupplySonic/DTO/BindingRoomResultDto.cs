using Core.Enums;
using Core.Models.Identity;
using Core.Models.Products;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Product;

namespace DTO;

public class BindingRoomResultDto
{
    public int Id { get; set; }

    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
    public byte[] RowVersion { get; set; }
    public int SupplierAppUserId { get; set; }
    public string? CreatorName { get; set; }

    public string? ProductNameEn { get; set; }
    public string? ProductDescription { get; set; }
    public string? ProductName { get; set; }

    public string? ProductNameAr { get; set; }

    public int ProductMainCategoryId { get; set; }


    public int ProductSubCategoryId { get; set; }


    public string? MainDescriptionEn { get; set; }
    public string? MainDescriptionAr { get; set; }

    public List<int>? BindingRoomAttachmentsIds { get; set; }

    public List<AttachmentDto>? BindingRoomAttachments { get; set; }
    public double UnitPrice { get; set; }
    public int Qaunt { get; set; }
    public int UOMId { get; set; }


    public bool? IsNegotiate { get; set; } = false;

    public bool? IsSupplier { get; set; } = false;
    public EnumStatus? Status { get; set; } = EnumStatus.Pending;

    public DateTime? StartDate { get; set; }

    public EnumBindingRoomStatus? BindingRoomStatus { get; set; } = EnumBindingRoomStatus.NotClaim;

    public List<ProductAttribueAddEditDto>? BindingRoomAttributes { get; set; } 


    public int? QauntFrom { get; set; }
    public int? QauntTo { get; set; }
    public string? RejectedReason { get; set; }

    //public List<int>? AttributesIds { get; set; } 

    //public int? TimeOutMin
    //{
    //    get
    //    {
    //        return this.StartDate != null
    //             ? DateTime.UtcNow.Date.Hour - this.StartDate.Value.Hour < 24 ?
    //                                                               DateTime.UtcNow.Date.Hour - this.StartDate.Value.Hour : 0 :0;
    //    }
    //}

}
