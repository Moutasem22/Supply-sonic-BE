using Core.Models;
using Core.Models.Identity;
using DB;
using DTO;
using FluentValidation;
using Helpers;
using IServiceContractor;
using IServiceContractor.ICommonService;
using IServiceContractor.Products;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Product;
using Core.Models.Products;
using DTO.RequestDto;

namespace Service.Products;

public class ProductOfferService : IProductOfferService
{
    private readonly IBaseService _baseService;
    private readonly IValidator<ProductOfferAddEditDto> _validator;
    private readonly DBContext _dBContext;
    public ProductOfferService(IBaseService baseService
       , IValidator<ProductOfferAddEditDto> validator
        )
    {
        _baseService = baseService;
        _validator = validator;
        _dBContext = baseService.Context;
        MapigToDto();
    }

    public async Task<ResultViewModel<ProductOfferResultDto>> Add(ProductOfferAddEditDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        ProductOffer lockup = new(lockDto.ProductId, lockDto.IsActive, lockDto.StartDate.FromISOString(), lockDto.EndDate.FromISOString());
        await _dBContext.ProductOffers.AddAsync(lockup);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ProductOfferResultDto> result = new() { Data = lockup.Adapt<ProductOfferResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<ProductOfferResultDto>> Delete(int id)
    {
        var lockup = _dBContext.ProductOffers.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "ProductOffer" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        lockup?.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ProductOfferResultDto> result = new() { Data = lockup?.Adapt<ProductOfferResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ProductOfferResultDto>>> GetAll(int ProductId, NewQueryViewModel<ProductOfferResultDto> queryViewModel)
    {
        IQueryable<ProductOffer> query = _dBContext.ProductOffers
             .Include(s => s.ProductOfferAttachments).ThenInclude(s => s.Attachment)
            .Include(s => s.Product).Where(n => n.ProductId == ProductId && !n.IsDeleted)
            .AsNoTracking().OrderByDescending(x => x.Id);
        //.WhereByIf(queryViewModel.SearchModel?.ProductOfferMainCategoryId != null,
        //  p => (p.ProductOfferMainCategoryId == queryViewModel.SearchModel.ProductOfferMainCategoryId
        //      || p.ProductOfferSubCategoryId == queryViewModel.SearchModel.ProductOfferSubCategoryId
        //      || p.NameAr.ToLower().Contains(queryViewModel.SearchModel.NameAr.Trim().ToLower())
        //      || p.NameEn.ToLower().Contains(queryViewModel.SearchModel.NameEn.Trim().ToLower())
        //      ))

        //    .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameAr), x => x.NameAr, queryViewModel.SortBy?.NameAr)
        //    .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameEn), x => x.NameEn, queryViewModel.SortBy?.NameEn);

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<ProductOfferResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<ProductOfferResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;


    }

    public async Task<ResultViewModel<ProductOfferResultDto>> Getone(int id)
    {
        var query = await _dBContext.ProductOffers.Include(s => s.Product).AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<ProductOfferResultDto> result = new() { Data = query.Adapt<ProductOfferResultDto>(), IsSuccess = true };
        return result;
    }



    public async Task<ResultViewModel<ProductOfferResultDto>> Update(ProductOfferAddEditDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        ProductOffer? lockup = await _dBContext.ProductOffers.FirstOrDefaultAsync(n => n.Id == lockDto.Id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (!StructuralComparisons.StructuralEqualityComparer.Equals(lockup.RowVersion, lockDto.RowVersion))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        lockup.Update(lockDto.ProductId, lockDto.IsActive, lockDto.StartDate.FromISOString(), lockDto.EndDate.FromISOString());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ProductOfferResultDto> result = new() { Data = lockup.Adapt<ProductOfferResultDto>(), IsSuccess = true };
        return result;
    }



    #region ProductOffer Tabs

    public async Task<ResultViewModel<bool>> SaveProductOfferAttachments(int ProductOfferId, List<int> ids)
    {
        ProductOffer? lockup = await _dBContext.ProductOffers.Include(s => s.ProductOfferAttachments).FirstOrDefaultAsync(n => n.Id == ProductOfferId && !n.IsDeleted && n.IsActive);
        lockup?.SaveProductOfferAttachments(ids.Select(s => new ProductOfferAttachment(s)).ToList());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<AttachmentDto>>> GetProductOfferAttachments(int id)
    {
        var lockup = await _dBContext.ProductOffers.Include(s => s.ProductOfferAttachments).ThenInclude(s => s.Attachment).FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        ResultViewModel<List<AttachmentDto>> result = new() { Data = lockup.ProductOfferAttachments.Select(s => s.Attachment).Adapt<List<AttachmentDto>>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<bool>> SaveProductOfferAttribues(int ProductOfferId, List<ProductOfferAttribueAddEditDto> dto)
    {

        ProductOffer? lockup = await _dBContext.ProductOffers.Include(s => s.ProductOfferAttributes).FirstOrDefaultAsync(n => n.Id == ProductOfferId && !n.IsDeleted && n.IsActive);
        lockup?.SaveProductOfferAttributes(dto.Select(s => new ProductOfferAttribute(s.AttributeId, s.SubAttributeId)).ToList());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ProductOfferAttribueAddEditDto>>> GetProductOfferAttribues(int id)
    {
        var lockup = await _dBContext.ProductOffers.Include(s => s.ProductOfferAttributes).ThenInclude(s => s.Attribute)
            .Include(s => s.ProductOfferAttributes).ThenInclude(s => s.SubAttribute).FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        ResultViewModel<List<ProductOfferAttribueAddEditDto>> result = new() { Data = lockup.ProductOfferAttributes.Adapt<List<ProductOfferAttribueAddEditDto>>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ProductOfferPriceScheduleResultDto>>> GetProductOfferPriceSchedules(int id)
    {
        var lockup = await _dBContext.ProductOffers.Include(s => s.ProductOfferPriceSchedules).FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        ResultViewModel<List<ProductOfferPriceScheduleResultDto>> result = new() { Data = lockup.ProductOfferPriceSchedules.Adapt<List<ProductOfferPriceScheduleResultDto>>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<bool>> SaveProductOfferPriceSchedules(int ProductOfferId, List<ProductOfferPriceScheduleResultDto> dto)
    {
        ProductOffer? lockup = await _dBContext.ProductOffers.Include(s => s.ProductOfferPriceSchedules).FirstOrDefaultAsync(n => n.Id == ProductOfferId && !n.IsDeleted && n.IsActive);
        lockup?.SaveProductOfferPriceSchedules(dto.Select(s => new ProductOfferPriceSchedule(s.UnitPrice,
            s.QuantityOption1From, s.QuantityOption1To, s.QuantityOption1UnitPrice,
             s.QuantityOption2From, s.QuantityOption2To, s.QuantityOption2UnitPrice,
             s.QuantityOption3From, s.QuantityOption3To, s.QuantityOption3UnitPrice)).ToList());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }




    #endregion


    #region Home View Products

    public async Task<ResultViewModel<List<ProductViewDto>>> GetAllProductOfferView(NewQueryViewModel<ProductViewDto> queryViewModel)
    {

        var query = _dBContext.ProductOffers
             .Include(s => s.Product).ThenInclude(s => s.ProductWeights).ThenInclude(s => s.UOM)
             .Include(s => s.ProductOfferAttachments).ThenInclude(s => s.Attachment)
             .Include(s => s.ProductOfferPriceSchedules)

             .Where(n => n.IsActive && !n.IsDeleted && n.ProductOfferAttachments.Any() && n.ProductOfferPriceSchedules.Any()
             // && n.StartDate.Value.Date >= DateTime.UtcNow.Date && n.EndDate.Value.Date <= DateTime.UtcNow.Date
             )
             .WhereByIf(queryViewModel.SearchModel?.ProductName != null,
                  p =>
                       p.Product.NameAr.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                     || p.Product.NameEn.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
        )
             .AsNoTracking();

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<ProductViewDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<ProductViewDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }



    public async Task<ResultViewModel<List<ProductViewDto>>> SearchOffer(NewQueryViewModel<RequestProductFilterDTO> queryViewModel)
    {
        var query = _dBContext.ProductOffers
            .Include(s => s.Product).ThenInclude(s => s.ProductWeights).ThenInclude(s => s.UOM)
           .Include(s => s.ProductOfferAttributes)
            .Include(s => s.ProductOfferAttachments).ThenInclude(s => s.Attachment)
            .Include(s => s.ProductOfferPriceSchedules)

            .Where(n => n.IsActive && !n.IsDeleted && n.ProductOfferAttachments.Any() && n.ProductOfferPriceSchedules.Any()
            // && n.StartDate.Value.Date >= DateTime.UtcNow.Date && n.EndDate.Value.Date <= DateTime.UtcNow.Date
            )
               .WhereByIf(queryViewModel.SearchModel?.ProductDescription != null,
                 p =>
                      p.Product.MainDescriptionAr.ToLower().Contains(queryViewModel.SearchModel.ProductDescription.Trim().ToLower())
                    || p.Product.MainDescriptionEn.ToLower().Contains(queryViewModel.SearchModel.ProductDescription.Trim().ToLower()))

               .WhereByIf(queryViewModel.SearchModel?.ProductName != null,
                  p =>
                       p.Product.NameAr.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                     || p.Product.NameEn.ToLower().Contains(queryViewModel.SearchModel.ProductName.Trim().ToLower())
                 )

              .WhereByIf(queryViewModel.SearchModel?.ProductMainCategoryId != null, p => (p.Product.ProductMainCategoryId == queryViewModel.SearchModel.ProductMainCategoryId))

               .WhereByIf(queryViewModel.SearchModel?.ProductSubCategoryId != null, p => (p.Product.ProductSubCategoryId == queryViewModel.SearchModel.ProductSubCategoryId))

               .WhereByIf(queryViewModel.SearchModel?.AttributesIds != null ,
                 p => (p.ProductOfferAttributes.Any(s => queryViewModel.SearchModel.AttributesIds.Contains(s.SubAttributeId))))

                 .WhereByIf(queryViewModel.SearchModel?.UOMFrom != null,
                  p => (p.ProductOfferPriceSchedules.Any(s => (s.QuantityOption1From >= queryViewModel.SearchModel.UOMFrom
                                                             && s.QuantityOption1To <= queryViewModel.SearchModel.UOMFrom)
                                                             || (s.QuantityOption2From >= queryViewModel.SearchModel.UOMFrom
                                                             && s.QuantityOption2To <= queryViewModel.SearchModel.UOMFrom)
                                                             || (s.QuantityOption3From >= queryViewModel.SearchModel.UOMFrom
                                                             && s.QuantityOption3To <= queryViewModel.SearchModel.UOMFrom)
                                                             )))



            .AsNoTracking();

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<ProductViewDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<ProductViewDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    #endregion
    #region Helper Method 

    void MapigToDto()
    {

        TypeAdapterConfig<ProductOfferPriceSchedule, ProductOfferPriceScheduleResultDto>.NewConfig();
        TypeAdapterConfig<Product, ProductResultDto>.NewConfig()
        .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);

        TypeAdapterConfig<ProductOffer, ProductOfferResultDto>.NewConfig()

               .Map(dest => dest.ProductOfferAttachments, src => src.ProductOfferAttachments.Select(s => s.Attachment).Adapt<List<AttachmentDto>>())
             ;
        // .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn)
        // .Map(dest => dest.ParentId, src => src.ProductOfferMainCategoryId)
        // .Map(dest => dest.ParentName, src => src.ProductOfferMainCategory != null ? _baseService.Language == "ar" ? src.ProductOfferMainCategory.NameAr : src.ProductOfferMainCategory.NameEn : "");


        TypeAdapterConfig<ProductOfferAttribute, ProductOfferAttribueAddEditDto>.NewConfig()
         .Map(dest => dest.AttributeName, src => src.Attribute != null ? _baseService.Language == "ar" ? src.Attribute.NameAr : src.Attribute.NameEn : "")
         .Map(dest => dest.SubAttributeName, src => src.SubAttribute != null ? _baseService.Language == "ar" ? src.SubAttribute.NameAr : src.SubAttribute.NameEn : "");


        TypeAdapterConfig<ProductOffer, ProductViewDto>.NewConfig()
             .Map(dest => dest.ProductName, src => _baseService.Language == "ar" ? src.Product.NameAr : src.Product.NameEn)
             .Map(dest => dest.ProductPrice, src => src.ProductOfferPriceSchedules.FirstOrDefault().UnitPrice)
             .Map(dest => dest.UOM, src => _baseService.Language == "ar" ? src.Product.ProductWeights.FirstOrDefault().UOM.NameAr :
             src.Product.ProductWeights.FirstOrDefault().UOM.NameEn)
                .Map(dest => dest.ProductOfferAttachments, src => src.ProductOfferAttachments.Select(s => s.Attachment).Adapt<List<AttachmentDto>>())
                ;

        TypeAdapterConfig<Attachment, AttachmentDto>.NewConfig()
             .Map(dest => dest.FilePath, src => _baseService.CurrentUrlPath + src.Path);

    }




    #endregion
}

