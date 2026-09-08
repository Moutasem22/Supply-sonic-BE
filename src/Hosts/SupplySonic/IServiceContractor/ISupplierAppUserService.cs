using DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface ISupplierAppUserService
{
    Task<ResultViewModel<List<UserResultDto>>> GetAll(QueryViewModel<UserResultDto> queryViewModel);
    Task<ResultViewModel<List<UserResultDto>>> GetAllActiveUsers(QueryViewModel<UserResultDto> queryViewModel);
    Task<ResultViewModel<UserResultDto>> Getone(int Id);

    Task<ResultViewModel<UserResultDto>> GetoneAdmin(int Id);

    
    Task<ResultViewModel<UserInfo>> Add(SupplierAppUserAddEditDto userDto);
    Task<ResultViewModel<UserResultDto>> Update(SupplierAppUserAddEditDto userDto);
    Task<ResultViewModel<UserResultDto>> UpdateProfile(SupplierAppUserAddEditDto userDto);
    Task<ResultViewModel<UserResultDto>> Delete(int id);
    Task<ResultViewModel<UserResultDto>> EnableUser(int userID);
    Task<ResultViewModel<UserResultDto>> DisableUser(int userID);
    Task<byte[]> PrintUserReport(QueryViewModel<UserResultDto> queryViewModel, int renderType);
    ResultViewModel<List<UserResultDto>> GetAllAdmin(QueryViewModel<UserResultDto> queryViewModel);
    ResultViewModel<List<UserResultDto>> GetAllUsesrWithSameRole(int user);
    Task<ResultViewModel<bool>> ImportUserFromExcel(IFormFile formFile);
    Claim[] generateOTPClaims(string otp);

    Task<ResultViewModel<UserResultDto>> AddUserAddress(SupplierAppUserAddEditDto userDto);

    Task<ResultViewModel<bool>> UpdateStatus(SupplierAppUserAddEditDto userDto);
}
