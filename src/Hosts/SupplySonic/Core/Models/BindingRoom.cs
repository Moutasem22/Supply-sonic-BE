using Core.Enums;
using Core.Models.Identity;
using Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class BindingRoom : BaseEntity<int>
{
    public int SupplierAppUserId { get; set; }
    public virtual SupplierAppUser SupplierAppUser { get; set; }

    public string? ProductNameEn { get; set; }

    public string? ProductNameAr { get; set; }

    public int ProductMainCategoryId { get; private set; }
    public virtual ProductMainCategory ProductMainCategory { get; set; }

    public int ProductSubCategoryId { get; private set; }
    public virtual ProductSubCategory ProductSubCategory { get; set; }

    public string? MainDescriptionEn { get; private set; }
    public string? MainDescriptionAr { get; private set; }

    public ICollection<BindingRoomAttachment> BindingRoomAttachments { get; set; }

    public double UnitPrice { get; private set; }
    public int Qaunt { get; private set; }
    public int? UOMId { get; private set; }
    public virtual Unit? UOM { get; set; }


    public bool? IsNegotiate { get; set; } = false;

    public bool? IsSupplier { get; set; } = false;
    public EnumStatus? Status { get; set; } = EnumStatus.Pending;

    public DateTime? StartDate { get; set; }

    public EnumBindingRoomStatus? BindingRoomStatus { get; set; } = EnumBindingRoomStatus.NotClaim;

    // List Of Attributtes 

    public ICollection<BindingRoomAttribute> BindingRoomAttributes { get; set; }

    public ICollection<BindingRoomRequest> BindingRoomRequests { get; set; }

    public string? RejectedReason { get; set; }

    public BindingRoom()
    {
        BindingRoomAttributes = new HashSet<BindingRoomAttribute>();
        BindingRoomAttachments = new HashSet<BindingRoomAttachment>();

        BindingRoomRequests = new HashSet<BindingRoomRequest>();
    }

    public BindingRoom(int supplierAppUserId, string? productNameEn, string? productNameAr, int productMainCategoryId,
                        int productSubCategoryId, double unitPrice, string? mainDescriptionEn, string? mainDescriptionAr,
                        int qaunt, int? uOMId, bool? isNegotiate, bool? isSupplier, EnumStatus? status, DateTime? startDate,
                         EnumBindingRoomStatus? bindingRoomStatus, List<BindingRoomAttribute> bindingRoomAttributes,
                         List<BindingRoomAttachment> bindingRoomAttachments)
    {
        this.SupplierAppUserId = supplierAppUserId;
        this.ProductNameEn = productNameEn;
        this.ProductNameAr = productNameAr;
        this.UnitPrice = unitPrice;
        this.MainDescriptionEn = mainDescriptionEn;
        this.MainDescriptionAr = mainDescriptionAr;
        this.UOMId = uOMId;
        this.IsNegotiate = isNegotiate;
        this.Status = status;
        this.ProductMainCategoryId = productMainCategoryId;
        this.ProductSubCategoryId = productSubCategoryId;
        this.IsSupplier = isSupplier;
        this.StartDate = startDate;
        this.BindingRoomStatus = bindingRoomStatus;
        this.BindingRoomAttributes = bindingRoomAttributes;
        this.BindingRoomAttachments = bindingRoomAttachments;
        this.Qaunt = qaunt;
    }

    public void Update(int supplierAppUserId, string? productNameEn, string? productNameAr, int productMainCategoryId,
                       int productSubCategoryId, double unitPrice, string? mainDescriptionEn, string? mainDescriptionAr,
                       int qaunt, int? uOMId, bool? isNegotiate, bool? isSupplier, EnumStatus? status, DateTime? startDate,
                        EnumBindingRoomStatus? bindingRoomStatus, List<BindingRoomAttribute> bindingRoomAttributes,
                        List<BindingRoomAttachment> bindingRoomAttachments)
    {
        this.BindingRoomAttributes.Clear();
        this.BindingRoomAttachments.Clear();

        this.SupplierAppUserId = supplierAppUserId;
        this.ProductNameEn = productNameEn;
        this.ProductNameAr = productNameAr;
        this.UnitPrice = unitPrice;
        this.MainDescriptionEn = mainDescriptionEn;
        this.MainDescriptionAr = mainDescriptionAr;
        this.UOMId = uOMId;
        this.IsNegotiate = isNegotiate;
        this.Status = status;
        this.ProductMainCategoryId = productMainCategoryId;
        this.ProductSubCategoryId = productSubCategoryId;
        this.IsSupplier = isSupplier;
        this.StartDate = startDate;
        this.BindingRoomStatus = bindingRoomStatus;
        this.BindingRoomAttributes = bindingRoomAttributes;
        this.BindingRoomAttachments = bindingRoomAttachments;
        this.Qaunt = qaunt;
    }

    public void UpdateStatus(EnumStatus? status,string? rejectedReason)
    {
        this.Status = status;
        this.RejectedReason= rejectedReason;
    }
    public void UpdateBindingRoomStatus(EnumBindingRoomStatus? BindingRoomStatus)
    {
        this.BindingRoomStatus = BindingRoomStatus;
    }

}
