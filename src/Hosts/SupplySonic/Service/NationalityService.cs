using Core.Models;
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

public class NationalityService : INationalityService
{
    private readonly IBaseService _baseService;
    private readonly IValidator<NationalityAddEditDto> _validator;
    private readonly DBContext _dBContext;

    public NationalityService(IBaseService baseService, IValidator<NationalityAddEditDto> validator)
    {
        _baseService = baseService;
        _validator = validator;
        _dBContext = baseService.Context;
        TypeAdapterConfig<Nationality, NationalityResultDto>.NewConfig()
             .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
    }

    public async Task<ResultViewModel<NationalityResultDto>> Add(NationalityAddEditDto dto)
    {
        _baseService.CheckValidation(dto, _validator);

        Nationality lockup = new(dto.NameEn, dto.NameAr, dto.NameUzbek, dto.NameRussian);
        await _dBContext.Nationalities.AddAsync(lockup);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<NationalityResultDto> result = new() { Data = lockup.Adapt<NationalityResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<NationalityResultDto>> Delete(int id)
    {
        var lockup = _dBContext.Nationalities.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "Nationality" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        lockup.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<NationalityResultDto> result = new() { Data = lockup.Adapt<NationalityResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<NationalityResultDto>>> GetAll(QueryViewModel<NationalityAddEditDto> queryViewModel)
    {
        IQueryable<Nationality> query = _dBContext.Nationalities.AsNoTracking().Where(n => !n.IsDeleted && n.IsActive);

        queryViewModel.Filter.ToList().ForEach(x =>
        {
            switch (x.Operation)
            {
                case (FilterOperation.Equal):
                    if (x.FieldName.ToLower() == "name")
                        query = query.Where(r => (r.NameAr.ToLower().Contains(x.value.Trim().ToLower())
                                                || r.NameEn.ToLower().Contains(x.value.Trim().ToLower())));
                    break;
            }
        });

        if (queryViewModel.Order is not null)
        {
            if (queryViewModel.Order.SortType == SortTypeEnum.ASC)
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("namear"):
                        query = query.OrderBy(x => x.NameAr);
                        break;
                    case ("nameen"):
                        query = query.OrderBy(x => x.NameEn);
                        break;

                    case ("namerussian"):
                        query = query.OrderBy(x => x.NameRussian);
                        break;

                    case ("nameuzbek"):
                        query = query.OrderBy(x => x.NameUzbek);
                        break;

                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                switch (queryViewModel.Order.FieldName.ToLower())
                {
                    case ("namear"):
                        query = query.OrderByDescending(x => x.NameAr);
                        break;
                    case ("nameen"):
                        query = query.OrderByDescending(x => x.NameEn);
                        break;
                    case ("namerussian"):
                        query = query.OrderByDescending(x => x.NameRussian);
                        break;

                    case ("nameuzbek"):
                        query = query.OrderByDescending(x => x.NameUzbek);
                        break;

                    default:
                        query = query.OrderByDescending(x => x.Id);
                        break;
                }
            }
        }
        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<NationalityResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<NationalityResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<NationalityResultDto>> Getone(int id)
    {
        var query = await _dBContext.Nationalities.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<NationalityResultDto> result = new() { Data = query.Adapt<NationalityResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<NationalityResultDto>> Update(NationalityAddEditDto dto)
    {
        _baseService.CheckValidation(dto, _validator);

        Nationality? lockup = await _dBContext.Nationalities.FirstOrDefaultAsync(n => n.Id == dto.Id && !n.IsDeleted && n.IsActive);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (!StructuralComparisons.StructuralEqualityComparer.Equals(lockup.RowVersion, dto.RowVersion))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        lockup.Update(dto.NameEn, dto.NameAr, dto.NameUzbek, dto.NameRussian);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<NationalityResultDto> result = new() { Data = lockup.Adapt<NationalityResultDto>(), IsSuccess = true };
        return result;
    }
}

