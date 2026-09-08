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

public class UnitService : IUnitService
{
    private readonly IBaseService _baseService;
    //private readonly IValidator<LockupResultDto> _validator;
    private readonly DBContext _dBContext;

    public UnitService(IBaseService baseService
        //, IValidator<LockupResultDto> validator
        )
    {
        _baseService = baseService;
        //_validator = validator;
        _dBContext = baseService.Context;
        TypeAdapterConfig<Unit, LockupResultDto>.NewConfig()
             .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
        TypeAdapterConfig<ProductSubCategory, ChildLockupResultDto>.NewConfig()
         .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
    }

    public async Task<ResultViewModel<LockupResultDto>> Add(LockupResultDto lockDto)
    {
        // _baseService.CheckValidation(lockDto, _validator);

        Unit city = new(lockDto.NameEn, lockDto.NameAr);
        await _dBContext.Units.AddAsync(city);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<LockupResultDto> result = new() { Data = city.Adapt<LockupResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<LockupResultDto>> Delete(int id)
    {
        var city = _dBContext.ProductMainCategories.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (city == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "Unit" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        city.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<LockupResultDto> result = new() { Data = city.Adapt<LockupResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<LockupResultDto>>> GetAll(NewQueryViewModel<LockupResultDto> queryViewModel)
    {
        IQueryable<Unit> query = _dBContext.Units.AsNoTracking().OrderByDescending(x => x.Id)
                                         .WhereByIf(!string.IsNullOrEmpty(queryViewModel.SearchModel?.Name),
                                           p => (p.NameAr.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())
                                               || p.NameEn.ToLower().Contains(queryViewModel.SearchModel.Name.Trim().ToLower())))

                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameAr), x => x.NameAr, queryViewModel.SortBy?.NameAr)
                                             .SortingByIf(!string.IsNullOrEmpty(queryViewModel.SortBy?.NameEn), x => x.NameEn, queryViewModel.SortBy?.NameEn);

        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<LockupResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<LockupResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;

    }

    public async Task<ResultViewModel<LockupResultDto>> Getone(int id)
    {
        var query = await _dBContext.ProductMainCategories.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<LockupResultDto> result = new() { Data = query.Adapt<LockupResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<LockupResultDto>> Update(LockupResultDto lockDto)
    {
        //_baseService.CheckValidation(lockDto, _validator);

        Unit? city = await _dBContext.Units.FirstOrDefaultAsync(n => n.Id == lockDto.Id && !n.IsDeleted && n.IsActive);
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
        city.Update(lockDto.NameEn, lockDto.NameAr);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<LockupResultDto> result = new() { Data = city.Adapt<LockupResultDto>(), IsSuccess = true };
        return result;
    }
}

