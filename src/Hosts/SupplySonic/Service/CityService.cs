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
public class CityService : ICityService
{
    private readonly IBaseService _baseService;
    private readonly IValidator<CityAddEditDto> _validator;
    private readonly DBContext _dBContext;
    public CityService(IBaseService baseService, IValidator<CityAddEditDto> validator)
    {
        _baseService = baseService;
        _validator = validator;
        _dBContext = baseService.Context;
        TypeAdapterConfig<City, CityResultDto>.NewConfig()
        .Map(dest => dest.Name, src => _baseService.Language == "ar" ? src.NameAr : src.NameEn)
     .AfterMapping((src, dest) =>
     {
         dest.CountryName = src.Country != null ? _baseService.Language == "ar" ? src.Country.NameAr : src.Country.NameEn : null;

     });
    }

    public async Task<ResultViewModel<CityResultDto>> Add(CityAddEditDto cityDto)
    {
        _baseService.CheckValidation(cityDto, _validator);

        City city = new(cityDto.NameEn, cityDto.NameAr, cityDto.NameUzbek, cityDto.NameRussian, cityDto.CountryId);
        await _dBContext.Cities.AddAsync(city);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<CityResultDto> result = new() { Data = city.Adapt<CityResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<CityResultDto>> Delete(int id)
    {
        var city = _dBContext.Cities.FirstOrDefault(n => n.Id == id && !n.IsDeleted && n.IsActive);
        if (city == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "City" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }

        city.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<CityResultDto> result = new() { Data = city.Adapt<CityResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<CityResultDto>>> GetAll(QueryViewModel<CityAddEditDto> queryViewModel)
    {
        IQueryable<City> query = _dBContext.Cities.Include(s => s.Country).AsNoTracking().Where(n => !n.IsDeleted && n.IsActive);

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
        ResultViewModel<List<CityResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<CityResultDto>>(),
            PageSize = queryViewModel.PageSize,
            PageNumber = queryViewModel.PageNumber,
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<List<CityResultDto>>> GetAllCityByCounryId(int? CounryId)
    {
        IQueryable<City> query = _dBContext.Cities.AsNoTracking().Where(n => !n.IsDeleted && n.IsActive & n.CountryId == CounryId);

        var lockup = query.ToList();
        ResultViewModel<List<CityResultDto>> PagedDataResult = new()
        {
            Data = lockup.Adapt<List<CityResultDto>>(),
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<CityResultDto>> Getone(int id)
    {
        var query = await _dBContext.Cities.Include(s => s.Country).AsNoTracking().FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted && n.IsActive);

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<CityResultDto> result = new() { Data = query.Adapt<CityResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<CityResultDto>> Update(CityAddEditDto cityDto)
    {
        _baseService.CheckValidation(cityDto, _validator);

        City? city = await _dBContext.Cities.FirstOrDefaultAsync(n => n.Id == cityDto.Id && !n.IsDeleted && n.IsActive);
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
        city.Update(cityDto.NameEn, cityDto.NameAr, cityDto.NameUzbek, cityDto.NameRussian, cityDto.CountryId);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<CityResultDto> result = new() { Data = city.Adapt<CityResultDto>(), IsSuccess = true };
        return result;
    }


    public async Task<ResultViewModel<string>> GetPrivacyPolicy()
    {
        var query = await _dBContext.SysSettings.FirstOrDefaultAsync(s => s.SysKey == "privacypolicy");
        ResultViewModel<string> result = new() { Data = query.SysValue.Adapt<string>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<string>> GetTermsAndConditions()
    {
        var query = await _dBContext.SysSettings.FirstOrDefaultAsync(s => s.SysKey == "termsandconditions");
        ResultViewModel<string> result = new() { Data = query.SysValue.Adapt<string>(), IsSuccess = true };
        return result;
    }

}
