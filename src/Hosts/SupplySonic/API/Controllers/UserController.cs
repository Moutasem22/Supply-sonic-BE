using DTO.CommandDTO;
using DTO.IdentityDTO;
using DTO.SettingDTO;
using Helpers;
using IServiceContractor.IdentityInterFaces;
using IServiceContractor.ISettingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _repository;
    IUserSettingService _userSettingRep;
    private readonly GlobalFormat _globalFormat;

    public UserController(IUserService repository, IUserSettingService userSettingRep, GlobalFormat globalFormat)
    {
        this._repository = repository;
        _userSettingRep = userSettingRep;
        _globalFormat = globalFormat;
    }

    [HttpPost("GetAllActiveUsers")]
    public async Task<IActionResult> GetAllActiveUsers(QueryViewModel<UserResultDto> query) => Ok(await _repository.GetAllActiveUsers(query));

    [HttpGet("GetUser")]
    public IActionResult GetUser()
    {

        var fdata = _globalFormat.LogFormat(HttpContext, new { });
        int.TryParse(fdata.UserId, out int userId);
        var val = _repository.Getone(userId);
        return Ok(val);
    }

    [HttpGet("GetOne/{UniqueId}")]
    public async Task<IActionResult> GetOne(string UniqueId) => Ok(await _repository.Getone(int.Parse(DTO.EncryptionHelper.DecryptFromUrl(UniqueId))));

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(QueryViewModel<UserResultDto> query) => Ok(await _repository.GetAll(query));


    [AllowAnonymous]
    [HttpPost("AddUser")]//for admin
    public async Task<IActionResult> AddUser(UserAddEditDto userdto) => Ok(await _repository.Add(userdto));


    [HttpPut("UpdateUser")]//for admin
    public async Task<IActionResult> UpdateUser(UserAddEditDto userdto) => Ok(_repository.Update(userdto));

    [HttpPut("UpdateProfile")]
    public async Task<IActionResult> UpdateProfile(UserProfileEditDto userdto) => Ok(_repository.UpdateProfile(userdto));

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser(int id) => Ok(await _repository.Delete(id));

    [HttpGet("GetUserSetting")]
    public IActionResult GetUserSetting() => Ok(_userSettingRep.GetUserSetting());

    [HttpPost("SetUserSetting")]
    public IActionResult SetUserSetting(UserSettingDto userSettingDto) => Ok(_userSettingRep.SetUserSetting(userSettingDto.Key, userSettingDto.Value));

    [HttpPost("ImportUserFromExcel")]
    public async Task<IActionResult> ImportUserFromExcel(IFormFile postedFile)
    {
        if (postedFile != null)
        {
            var val = await _repository.ImportUserFromExcel(postedFile);
            return Ok(val);
        }
        else return BadRequest();
    }


    [HttpGet("EnableUser/{Id}")]
    public async Task<IActionResult> EnableUser(int Id) => Ok(_repository.EnableUser(Id));

    [HttpGet("DisableUser/{Id}")]
    public async Task<IActionResult> DisableUser(int Id) => Ok(await _repository.DisableUser(Id));


    [HttpPost("PrintUserReport/{renderType}")]
    public async Task<IActionResult> PrintUserReport([FromBody] QueryViewModel<UserResultDto> query, int renderType) => Ok(await _repository.PrintUserReport(query, renderType));



}
