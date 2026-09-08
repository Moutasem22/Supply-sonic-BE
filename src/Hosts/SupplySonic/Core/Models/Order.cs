using Core.Enums;
using Core.Models.Identity;
using Core.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Order:BaseEntity<int>
{
    public int? SupplierAppUserId { get; set; }
    public virtual SupplierAppUser? SupplierAppUser { get; set; }

    public int? UserAddressId { get; set; }
    public virtual UserAddress? UserAddress { get; set; }

    public double? Total { get; set; }
    public double? Discount { get; set; }
    public double? TotalAmount { get; set; }
    public bool? ConfirmOrder { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public EnumPaymentType? PaymentType { get; set; }
    public EnumOrderStatus? OrderStatus { get; set; }
    public double? ShippingCost { get; set; }
    public string? OrderCode { get; set; }

    public List<OrderDetails> OrderDetails { get; set;}

    public Order()
    {
        OrderDetails = new List<OrderDetails>();
    }


     string GeneratCode()
    {
        var digit = 4;
        var otp = new Random().Next((int)Math.Pow(10, digit - 1), (int)Math.Pow(10, digit) - 1).ToString("####");
        //if (otp[0] == '0')
        //    return GeneratOTP(user);
        //user.Token = GetGuestToken(new UserDTO()
        //{
        //    Id = user.Id,
        //    UserName = user.UserName
        //}, otp);
        return otp;
    }
    public Order(int? SupplierAppUserId, int? UserAddressId , double? Total, double? Discount, double? TotalAmount, bool? ConfirmOrder, DateTime? ArrivalDate
        , EnumOrderStatus? OrderStatus, double? ShippingCost, EnumPaymentType? PaymentType, List<OrderDetails> OrderDetails)
    {
            this.SupplierAppUserId = SupplierAppUserId;
        this.UserAddressId = UserAddressId;
        this.Total = Total;
        this.Discount = Discount;
        this.TotalAmount = TotalAmount;
        this.ConfirmOrder = ConfirmOrder;
        this.ArrivalDate = ArrivalDate;
        this.OrderStatus = OrderStatus;
        this.PaymentType= PaymentType;
        this.ShippingCost = ShippingCost;
        this.OrderCode =  GeneratCode();
        this.OrderDetails = OrderDetails;

    }
    public void Update(int? SupplierAppUserId, int? UserAddressId, double? Total, double? Discount, double? TotalAmount, bool? ConfirmOrder, DateTime? ArrivalDate
      , EnumOrderStatus? OrderStatus, double? ShippingCost, EnumPaymentType? PaymentType, string? OrderCode, List<OrderDetails> OrderDetails)
    {
        this.SupplierAppUserId = SupplierAppUserId;
        this.UserAddressId = UserAddressId;
        this.Total = Total;
        this.Discount = Discount;
        this.TotalAmount = TotalAmount;
        this.ConfirmOrder = ConfirmOrder;
        this.ArrivalDate = ArrivalDate;
        this.OrderStatus = OrderStatus;
        this.PaymentType = PaymentType;
        this.ShippingCost = ShippingCost;
        this.OrderCode = OrderCode;
        this.OrderDetails = OrderDetails;
    }

}
