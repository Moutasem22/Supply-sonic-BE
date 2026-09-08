using Core.Models.Products;
using Core.Models.Identity;
using DB;
using DTO;
using FluentValidation;
using Helpers;
using IServiceContractor;
using IServiceContractor.ICommonService;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service;
public class ProductSubCategoryService : IProductSubCategoryService
{
    private readonly IBaseService _baseService;
    //  private readonly IValidator<ChildLockupResultDto> _validator;
    private readonly DBContext _dBContext;
    public ProductSubCategoryService(IBaseService baseService
        //, IValidator<ChildLockupResultDto> validator
        )
    {
        _baseService = baseService;
        // _validator = validator;
        _dBContext = baseService.Context;
        TypeAdapterConfig<ProductSubCategory, ChildLockupResultDto>.NewConfig()
        .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn)
        .Map(dest => dest.ParentId, src => src.ProductMainCategoryId !=null? src.ProductMainCategoryId:0)
        //.Map(dest => dest.ParentName, src => src.ProductMainCategory != null ? _baseService.Language == "ar" ? src.ProductMainCategory.NameAr : src.ProductMainCategory.NameEn : "")
        ;
    }

    public async Task<ResultViewModel<ChildLockupResultDto>> Add(ChildLockupResultDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        ProductSubCategory city = new(lockDto.NameEn, lockDto.NameAr, lockDto.ParentId);
        await _dBContext.ProductSubCategories.AddAsync(city);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ChildLockupResultDto> result = new() { Data = null, IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<ChildLockupResultDto>> Delete(int id)
    {
        var city = _dBContext.ProductSubCategories.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (city == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "ProductSubCategory" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        city?.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ChildLockupResultDto> result = new() { Data = city.Adapt<ChildLockupResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<ChildLockupResultDto>>> GetAll(NewQueryViewModel<ChildLockupResultDto> queryViewModel)
    {
        IQueryable<ProductSubCategory> query = _dBContext.ProductSubCategories.Include(s=>s.ProductMainCategory)
                                          .AsNoTracking().OrderByDescending(x => x.Id)
                                         .WhereByIf(!string.IsNullOrEmpty(queryViewModel.SearchModel?.Name),
                                           p => (p.NameAr.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())
                                               || p.NameEn.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())))

                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameAr), x => x.NameAr, queryViewModel.SortBy?.NameAr)
                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameEn), x => x.NameEn, queryViewModel.SortBy?.NameEn);

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<ChildLockupResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<ChildLockupResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<List<ChildLockupResultDto>>> GetAllByCategoryId(int? CategoryId)
    {
        IQueryable<ProductSubCategory> query = _dBContext.ProductSubCategories.Include(s => s.ProductMainCategory)

            .AsNoTracking().Where(n => !n.IsDeleted && n.IsActive & n.ProductMainCategoryId == CategoryId);

        var lockup = query.ToList();
        ResultViewModel<List<ChildLockupResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<ChildLockupResultDto>>(),
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<ChildLockupResultDto>> Getone(int id)
    {
        var query = await _dBContext.ProductSubCategories.Include(s => s.ProductMainCategory).AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<ChildLockupResultDto> result = new() { Data = query.Adapt<ChildLockupResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<ChildLockupResultDto>> Update(ChildLockupResultDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        ProductSubCategory? city = await _dBContext.ProductSubCategories.FirstOrDefaultAsync(n => n.Id == lockDto.Id && !n.IsDeleted && n.IsActive);
        if (city == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (!StructuralComparisons.StructuralEqualityComparer.Equals(city.RowVersion, lockDto.RowVersion))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        city.Update(lockDto.NameEn, lockDto.NameAr, lockDto.ParentId);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<ChildLockupResultDto> result = new() { Data = city.Adapt<ChildLockupResultDto>(), IsSuccess = true };
        return result;
    }

}
