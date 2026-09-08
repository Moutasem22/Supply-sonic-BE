using Core.Models;
using Core.Models.Products;
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

namespace Service.Products;

public class ProductService : IProductService
{
    private readonly IBaseService _baseService;
    private readonly IValidator<ProductAddEditDto> _validator;
    private readonly DBContext _dBContext;
    public ProductService(IBaseService baseService
       , IValidator<ProductAddEditDto> validator
        )
    {
        _baseService = baseService;
        _validator = validator;
        _dBContext = baseService.Context;
        MapigToDto();
    }

    public async Task<ResultViewModel<ProductAddEditDto>> Add(ProductAddEditDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        Product lockup = new(lockDto.NameEn, lockDto.NameAr, lockDto.MainDescriptionEn, lockDto.MainDescriptionAr, lockDto.SecondDescriptionEn,
             lockDto.SecondDescriptionAr, lockDto.AdditionalInfo, lockDto.SupplierId, lockDto.ProductSubCategoryId, lockDto.ProductMainCategoryId);
        await _dBContext.Products.AddAsync(lockup);
        await _dBContext.SaveChangesAsync();

        lockDto.Id = lockup.Id;
        lockDto.RowVersion = lockup.RowVersion;

        ResultViewModel<ProductAddEditDto> result = new() { Data = lockDto, IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<ProductResultDto>> Delete(int id)
    {
        var lockup = _dBContext.Products.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "Product" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        lockup?.Delete();

        var offers = _dBContext.ProductOffers.Where(s => s.ProductId == id).ToList();
        foreach (var item in offers)
        {
            item.Deactivate();
        }

        await _dBContext.SaveChangesAsync();

        ResultViewModel<ProductResultDto> result = new() { Data = lockup?.Adapt<ProductResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ProductResultDto>>> GetAll(int SupplierId, NewQueryViewModel<ProductResultDto> queryViewModel)
    {
        IQueryable<Product> query = _dBContext.Products.Include(s => s.ProductMainCategory)
            .Include(s => s.ProductAttachments).ThenInclude(s => s.Attachment)
            .Where(n => n.SupplierId == SupplierId && n.IsActive && !n.IsDeleted)
            //.Where(n => n.SupplierId == 1006)
            .AsNoTracking().OrderByDescending(x => x.Id)
                                         .WhereByIf(queryViewModel.SearchModel?.ProductMainCategoryId != null,
                                           p => (p.ProductMainCategoryId == queryViewModel.SearchModel.ProductMainCategoryId
                                               || p.ProductSubCategoryId == queryViewModel.SearchModel.ProductSubCategoryId
                                               || p.NameAr.ToLower().Contains(queryViewModel.SearchModel.NameAr.Trim().ToLower())
                                               || p.NameEn.ToLower().Contains(queryViewModel.SearchModel.NameEn.Trim().ToLower())
                                               ))

                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameAr), x => x.NameAr, queryViewModel.SortBy?.NameAr)
                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameEn), x => x.NameEn, queryViewModel.SortBy?.NameEn);

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<ProductResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<ProductResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;


    }

    public async Task<ResultViewModel<ProductResultDto>> Getone(int id)
    {
        var query = await _dBContext.Products
            .Include(s => s.ProductMainCategory)
            .Include(s => s.ProductAttachments).ThenInclude(s => s.Attachment)
            .AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<ProductResultDto> result = new() { Data = query.Adapt<ProductResultDto>(), IsSuccess = true };
        return result;
    }



    public async Task<ResultViewModel<ProductResultDto>> Update(ProductAddEditDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        Product? lockup = await _dBContext.Products.FirstOrDefaultAsync(n => n.Id == lockDto.Id && !n.IsDeleted && n.IsActive);
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
        lockup.Update(lockDto.NameEn, lockDto.NameAr, lockDto.MainDescriptionEn, lockDto.MainDescriptionAr, lockDto.SecondDescriptionEn,
             lockDto.SecondDescriptionAr, lockDto.AdditionalInfo, _baseService.CurrentUserId, lockDto.ProductSubCategoryId, lockDto.ProductMainCategoryId);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ProductResultDto> result = new() { Data = lockup.Adapt<ProductResultDto>(), IsSuccess = true };
        return result;
    }



    #region Product Tabs

    public async Task<ResultViewModel<bool>> SaveProductAttachments(int ProductId, List<int> ids)
    {
        Product? lockup = await _dBContext.Products.Include(s => s.ProductAttachments).FirstOrDefaultAsync(n => n.Id == ProductId && !n.IsDeleted && n.IsActive);
        lockup?.SaveProductAttachments(ids.Select(s => new ProductAttachment(s)).ToList());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<AttachmentDto>>> GetProductAttachments(int id)
    {
        var lockup = await _dBContext.Products.Include(s => s.ProductAttachments).ThenInclude(s => s.Attachment).FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        ResultViewModel<List<AttachmentDto>> result = new() { Data = lockup.ProductAttachments.Select(s => s.Attachment).Adapt<List<AttachmentDto>>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<bool>> SaveProductAttribues(int ProductId, List<ProductAttribueAddEditDto> dto)
    {

        Product? lockup = await _dBContext.Products.Include(s => s.ProductAttributes).FirstOrDefaultAsync(n => n.Id == ProductId && !n.IsDeleted && n.IsActive);
        lockup?.SaveProductAttributes(dto.Select(s => new ProductAttribute(s.AttributeId, s.SubAttributeId)).ToList());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ProductAttribueAddEditDto>>> GetProductAttribues(int id)
    {
        var lockup = await _dBContext.Products.Include(s => s.ProductAttributes).ThenInclude(s => s.Attribute)
            .Include(s => s.ProductAttributes).ThenInclude(s => s.SubAttribute).FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        ResultViewModel<List<ProductAttribueAddEditDto>> result = new() { Data = lockup.ProductAttributes.Adapt<List<ProductAttribueAddEditDto>>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ProductWeightResultDto>>> GetProductWeights(int id)
    {
        var lockup = await _dBContext.Products.Include(s => s.ProductWeights).FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        ResultViewModel<List<ProductWeightResultDto>> result = new() { Data = lockup.ProductWeights.Adapt<List<ProductWeightResultDto>>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<bool>> SaveProductWeights(int ProductId, List<ProductWeightResultDto> dto)
    {
        Product? lockup = await _dBContext.Products.Include(s => s.ProductWeights).FirstOrDefaultAsync(n => n.Id == ProductId && !n.IsDeleted && n.IsActive);
        lockup?.SaveProductWeights(dto.Select(s => new ProductWeight(
            s.UOMId, s.PackingUnitId, s.PackingQuantity, s.LoadingUnitId, s.LoadingQuantity,
            s.MOQUnitId, s.MOQ, s.ProductionQuantity, s.ProductionUnitId

            )).ToList());
        await _dBContext.SaveChangesAsync();

        ResultViewModel<bool> result = new() { Data = true, IsSuccess = true };
        return result;
    }
    #endregion

    #region Helper Method 

    void MapigToDto()
    {

        TypeAdapterConfig<ProductWeight, ProductWeightResultDto>.NewConfig();

        TypeAdapterConfig<Product, ProductResultDto>.NewConfig()

         .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn)
           .Map(dest => dest.ProductAttachments, src => src.ProductAttachments.Any() ? src.ProductAttachments.Select(s => s.Attachment).Adapt<List<AttachmentDto>>() : null)
                ;

        TypeAdapterConfig<Attachment, AttachmentDto>.NewConfig()
             .Map(dest => dest.FilePath, src => _baseService.CurrentUrlPath + src.Path);
        // .Map(dest => dest.ParentId, src => src.ProductMainCategoryId)
        // .Map(dest => dest.ParentName, src => src.ProductMainCategory != null ? _baseService.Language == "ar" ? src.ProductMainCategory.NameAr : src.ProductMainCategory.NameEn : "");


        TypeAdapterConfig<ProductAttribute, ProductAttribueAddEditDto>.NewConfig()
         .Map(dest => dest.AttributeName, src => src.Attribute != null ? _baseService.Language == "ar" ? src.Attribute.NameAr : src.Attribute.NameEn : "")
         .Map(dest => dest.SubAttributeName, src => src.SubAttribute != null ? _baseService.Language == "ar" ? src.SubAttribute.NameAr : src.SubAttribute.NameEn : "");
    }





    #endregion
}
