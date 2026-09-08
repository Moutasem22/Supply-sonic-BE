using Core.Enums;
using Core.Models;
using DTO;
using FluentValidation;
using Helpers;
using IServiceContractor.ICommonService;
using IServiceContractor;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace Service;

public class UserAddressService : IUserAddressService
{
    private readonly DBContext _dBContext;
    private readonly IBaseService _baseService;

    public UserAddressService(IBaseService baseService)
    {

        _baseService = baseService;
        _dBContext = _baseService.Context;

        TypeAdapterConfig<UserAddress, UserAddressResultDto>.NewConfig()
            .Map(dest => dest.CountryName, src => src.Country != null ? _baseService.Language == "ar" ? src.Country.NameAr : src.Country.NameEn ?? null : null)
            .Map(dest => dest.CityName, src => src.City != null ? _baseService.Language == "ar" ? src.City.NameAr : src.City.NameEn ?? null : null);

    }
    public async Task<ResultViewModel<UserAddressResultDto>> Add(UserAddressResultDto lockupDto)
    {



        if (lockupDto.IsDefalut == true)
        {
            var address = await _dBContext.UserAddresss.Where(s => s.SupplierAppUserId == lockupDto.SupplierAppUserId).ToListAsync();
            foreach (var item in address)
            {
                item.IsDefalut = false;
            }
        }

        UserAddress lockup = new(lockupDto.SupplierAppUserId, lockupDto.CountryId, lockupDto.CityId, lockupDto.SecondPhone, lockupDto.Notes,
           lockupDto.Address, lockupDto.IsDefalut);
        await _dBContext.UserAddresss.AddAsync(lockup);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<UserAddressResultDto> result = new() { Data = lockup.Adapt<UserAddressResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<UserAddressResultDto>> Delete(int id)
    {
        var lockup = _dBContext.UserAddresss.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
        if (lockup == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "UserAddress" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        lockup?.Delete();
        await _dBContext.SaveChangesAsync();

        ResultViewModel<UserAddressResultDto> result = new() { Data = lockup.Adapt<UserAddressResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<List<UserAddressResultDto>>> GetAll(int userId)
    {
        var query = await _dBContext.UserAddresss.Include(s => s.Country).Include(s => s.City).AsNoTracking().Where(s => s.IsActive && !s.IsDeleted && s.SupplierAppUserId == userId).ToListAsync();

        ResultViewModel<List<UserAddressResultDto>> PagedDataResult = new()
        {
            Data = query.Adapt<List<UserAddressResultDto>>(),
            Total = query.Count(),
            IsSuccess = true
        };
        return PagedDataResult;
    }

    public async Task<ResultViewModel<UserAddressResultDto>> Getone(int id)
    {
        var query = await _dBContext.UserAddresss.Include(s => s.Country).Include(s => s.City).Where(n => n.Id == id && !n.IsDeleted).FirstOrDefaultAsync();

        if (query == null)
        {
            ErrorMessageDto errorMessage = new ErrorMessageDto() { ErrorMessage = "ItemNotExist", PropertyName = "id" };
            _baseService.GetExceptionMessages().ReturnExceptionMessages(errorMessage);
        }
        ResultViewModel<UserAddressResultDto> result = new() { Data = query.Adapt<UserAddressResultDto>(), IsSuccess = true };
        return result;
    }

    public async Task<ResultViewModel<UserAddressResultDto>> Update(UserAddressResultDto lockupDto)
    {
        if (lockupDto.IsDefalut == true)
        {
            var address = await _dBContext.UserAddresss.Where(s => s.SupplierAppUserId == lockupDto.SupplierAppUserId).ToListAsync();
            foreach (var item in address)
            {
                item.IsDefalut = false;
            }
        }
        var lockup = await _dBContext.UserAddresss.FirstOrDefaultAsync(n => n.Id == lockupDto.Id && !n.IsDeleted && n.IsActive);

        lockup?.Update(lockupDto.SupplierAppUserId, lockupDto.CountryId, lockupDto.CityId, lockupDto.SecondPhone, lockupDto.Notes,
            lockupDto.Address, lockupDto.IsDefalut);
        await _dBContext.SaveChangesAsync();

        ResultViewModel<UserAddressResultDto> result = new() { Data = lockup.Adapt<UserAddressResultDto>(), IsSuccess = true };
        return result;

    }
}

