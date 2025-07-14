using DTO.CommandDTO;
using DTO.IdentityDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor.IdentityInterFaces;

public interface IUserService
{

    Task<ResultViewModel<List<UserResultDto>>> GetAll(QueryViewModel<UserResultDto> queryViewModel);
    Task<ResultViewModel<List<UserResultDto>>> GetAllActiveUsers(QueryViewModel<UserResultDto> queryViewModel);
    Task<ResultViewModel<UserResultDto>> Getone(int Id);
    Task<ResultViewModel<UserResultDto>> Add(UserAddEditDto userDto);
    Task<ResultViewModel<UserResultDto>> Update(UserAddEditDto userDto);
    Task<ResultViewModel<UserResultDto>> UpdateProfile(UserProfileEditDto userDto);
    Task<ResultViewModel<UserResultDto>> Delete(int id);
    Task<ResultViewModel<UserResultDto>> EnableUser(int userID);
    Task<ResultViewModel<UserResultDto>> DisableUser(int userID);
    Task<byte[]> PrintUserReport(QueryViewModel<UserResultDto> queryViewModel, int renderType);
    Task<ResultViewModel<bool>> ImportUserFromExcel(IFormFile formFile);

}
