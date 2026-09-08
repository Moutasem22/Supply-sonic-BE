using Core.Enums;
using Core.Models.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class OrderResultDto
{
    public int Id { get; set; }
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
    public byte[] RowVersion { get; set; }
    public int? SupplierAppUserId { get; set; }
    public string? CreatorName { get; set; }

    public int? UserAddressId { get; set; }
    public UserAddEditDto? UserAddress { get; set; }

    public double? Total { get; set; }
    public double? Discount { get; set; }
    public double? TotalAmount { get; set; }
    public bool? ConfirmOrder { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public EnumPaymentType? PaymentType { get; set; }
    public EnumOrderStatus? OrderStatus { get; set; }
    public double? ShippingCost { get; set; }
    public string? OrderCode { get; set; }

    public List<OrderDetailsResultDto>? OrdersDetails { get; set; }
}
