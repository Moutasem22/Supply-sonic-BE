using Core.Models;
using DB;
using DTO;
using Helpers;
using IServiceContractor;
using IServiceContractor.ICommonService;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;

public class OrderService : IOrderService
{
    private readonly IBaseService _baseService;

    private readonly DBContext _dBContext;

    public OrderService(IBaseService baseService)
    {
        _baseService = baseService;

        _dBContext = _baseService.Context;

        TypeAdapterConfig<Order, OrderResultDto>.NewConfig()
                .Map(dest => dest.OrdersDetails, src => src.OrderDetails.Adapt<List<OrderDetailsResultDto>>())

       ;
    }

    public async Task<ResultViewModel<OrderResultDto>> CreateOrder(OrderResultDto lockupDto)
    {
        Order lockup = new(lockupDto.SupplierAppUserId, lockupDto.UserAddressId, lockupDto.Total, lockupDto.Discount, lockupDto.TotalAmount, false,
            lockupDto.ArrivalDate, Core.Enums.EnumOrderStatus.Pending, lockupDto.ShippingCost, lockupDto.PaymentType,
            lockupDto.OrdersDetails.Select(item => new OrderDetails(item.ProductOfferId, item.Price, item.Qaunt, item.Discount, item.Total)).ToList());
        await _dBContext.Orders.AddAsync(lockup);
        await _dBContext.SaveChangesAsync();


        ResultViewModel<OrderResultDto> result = new() { Data = lockup.Adapt<OrderResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<OrderResultDto>>> GetAllOrder(NewQueryViewModel<OrderFilterDto> queryViewModel, bool? IsSupplyer = null)
    {
        var query = _dBContext.Orders.Include(s => s.OrderDetails)
            .ThenInclude(s => s.ProductOffer)
            .ThenInclude(s => s.Product).ThenInclude(s => s.Supplier)
            .Where(n => !n.IsDeleted && n.IsActive)



            .WhereByIf(queryViewModel.SearchModel?.OrderStatus != null, p => (p.OrderStatus == queryViewModel.SearchModel.OrderStatus))
            .WhereByIf(queryViewModel.SearchModel?.OrderCode != null, p => (p.OrderCode == queryViewModel.SearchModel.OrderCode))
            .WhereByIf(queryViewModel.SearchModel?.DateFrom != null && queryViewModel.SearchModel?.DateTo != null,
                 p => (p.CreatedDate >= queryViewModel.SearchModel.DateFrom.FromISOString() && p.CreatedDate <= queryViewModel.SearchModel.DateTo.FromISOString()));


        if (IsSupplyer == true)
        {
            query = query.Include(s => s.OrderDetails.Where(s => s.ProductOffer.Product.SupplierId == _baseService.CurrentUserId));
        }
        else if (IsSupplyer == false)
        {
            query = query.Where(s => s.SupplierAppUserId == _baseService.CurrentUserId);
        }



        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);

        ResultViewModel<List<OrderResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<OrderResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<OrderResultDto>> GetOneOrder(int Id)
    {
        Order? lockup = await _dBContext.Orders.Include(s => s.OrderDetails).ThenInclude(s => s.ProductOffer).FirstOrDefaultAsync(n => n.Id == Id && !n.IsDeleted && n.IsActive);

        ResultViewModel<OrderResultDto> PagedDataResult = new()
        {
            Data = lockup.Adapt<OrderResultDto>(),

            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<OrderResultDto>> UpdateOrder(OrderResultDto lockupDto)
    {
        Order? lockup = await _dBContext.Orders.Include(s => s.OrderDetails)
                             .FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (!StructuralComparisons.StructuralEqualityComparer.Equals(lockup.RowVersion, lockupDto.RowVersion))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }


        lockup.Update(lockupDto.SupplierAppUserId, lockupDto.UserAddressId, lockupDto.Total, lockupDto.Discount, lockupDto.TotalAmount, false,
            lockupDto.ArrivalDate, lockupDto.OrderStatus, lockupDto.ShippingCost, lockupDto.PaymentType, lockupDto.OrderCode,
            lockupDto.OrdersDetails.Select(item => new OrderDetails(item.ProductOfferId, item.Price, item.Qaunt, item.Discount, item.Total)).ToList());

        await _dBContext.SaveChangesAsync();

        ResultViewModel<OrderResultDto> result = new() { Data = lockup.Adapt<OrderResultDto>(), IsSuccess = true };
        return result;
    }
}
