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

public class CountryService : ICountryService
{
    private readonly IBaseService _baseService;
    private readonly IValidator<CountryAddEditDto> _validator;
    private readonly DBContext _dBContext;

    public CountryService(IBaseService baseService, IValidator<CountryAddEditDto> validator)
    {
        _baseService = baseService;
        _validator = validator;
        _dBContext = baseService.Context;
        TypeAdapterConfig<Country, CountryResultDto>.NewConfig()
             .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
        TypeAdapterConfig<City, CityResultDto>.NewConfig()
         .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn);
    }

    public async Task<ResultViewModel<CountryResultDto>> Add(CountryAddEditDto cityDto)
    {
        _baseService.CheckValidation(cityDto, _validator);

        Country city = new(cityDto.NameEn, cityDto.NameAr, cityDto.NameUzbek, cityDto.NameRussian);
        await _dBContext.Countries.AddAsync(city);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<CountryResultDto> result = new() { Data = city.Adapt<CountryResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<CountryResultDto>> Delete(int id)
    {
        var city = _dBContext.Countries.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (city == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "Country" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        city.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<CountryResultDto> result = new() { Data = city.Adapt<CountryResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<CountryResultDto>>> GetAll(QueryViewModel<CountryAddEditDto> queryViewModel)
    {
        IQueryable<Country> query = _dBContext.Countries.AsNoTracking().Where(n => !n.IsDeleted && n.IsActive);

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
        ResultViewModel<List<CountryResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<CountryResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<List<CountryResultDto>>> GetAllWithCities(QueryViewModel<CountryAddEditDto> queryViewModel)
    {
        IQueryable<Country> query = _dBContext.Countries.AsNoTracking().Include(s => s.Cities).Where(n => !n.IsDeleted && n.IsActive);
        var lockup = await query.PageDataAsync(queryViewModel.PageSize, queryViewModel.PageNumber);
        ResultViewModel<List<CountryResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<CountryResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<CountryResultDto>> Getone(int id)
    {
        var query = await _dBContext.Countries.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<CountryResultDto> result = new() { Data = query.Adapt<CountryResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<CountryResultDto>> Update(CountryAddEditDto cityDto)
    {
        _baseService.CheckValidation(cityDto, _validator);

        Country? city = await _dBContext.Countries.FirstOrDefaultAsync(n => n.Id == cityDto.Id && !n.IsDeleted && n.IsActive);
        if (city == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ObjectNotFound", PropertyName = "Project" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        if (!StructuralComparisons.StructuralEqualityComparer.Equals(city.RowVersion, cityDto.RowVersion))
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "RowVersionErr", PropertyName = "RowVersion" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        city.Update(cityDto.NameEn, cityDto.NameAr, cityDto.NameUzbek, cityDto.NameRussian);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<CountryResultDto> result = new() { Data = city.Adapt<CountryResultDto>(), IsSuccess = true };
        return result;
    }
}

