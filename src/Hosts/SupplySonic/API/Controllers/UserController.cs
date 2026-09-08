using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        public async Task<IActionResult> GetAllActiveUsers(QueryViewModel<UserResultDto> query)
        {
            var val = await _repository.GetAllActiveUsers(query);
            return Ok(val);
        }


        [HttpGet("GetUser")]
        public IActionResult GetUser()
        {
            var fdata = _globalFormat.LogFormat(HttpContext, new { });
            int.TryParse(fdata.UserId, out int userId);
            var val = _repository.Getone(userId);

            return Ok(val);

        }

        [HttpGet("GetOne/{UniqueId}")]
        public async Task<IActionResult> GetOne(string UniqueId)
        {
            var Id = int.Parse(DTO.EncryptionHelper.DecryptFromUrl(UniqueId));

            var val = await _repository.Getone(Id);

            return Ok(val);

        }


        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(QueryViewModel<UserResultDto> query)
        {

            var val = await _repository.GetAll(query);
            return Ok(val);

        }

        [HttpGet("GetAllUsesrWithSameRole")]
        public IActionResult GetAllUsesrWithSameRole()
        {
            var fdata = _globalFormat.LogFormat(HttpContext, new { });

            int.TryParse(fdata.UserId, out int userId);
            var val = _repository.GetAllUsesrWithSameRole(userId);
            return Ok(val);

        }

        [HttpPost("GetAllAdmin")]
        public IActionResult GetAllAdmin(QueryViewModel<UserResultDto> query)
        {
            var val = _repository.GetAllAdmin(query);
            return Ok(val);

        }
        [AllowAnonymous]
        [HttpPost("AddUser")]//for admin
        public async Task<IActionResult> AddUser(UserAddEditDto userdto)
        {
            var val = await _repository.Add(userdto);
            return Ok(val);
        }

        [HttpPost("UpdateUser")]//for admin
        public async Task<IActionResult> UpdateUser(UserAddEditDto userdto)
        {
            var val = await _repository.Update(userdto);
            return Ok(val);
        }
        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UserProfileEditDto userdto)
        {


            var val = await _repository.UpdateProfile(userdto);
            return Ok(val);

        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var val = await _repository.Delete(id);
            return Ok(val);
        }

        [HttpGet("GetUserSetting")]
        public IActionResult GetUserSetting()
        {

            var val = _userSettingRep.GetUserSetting();
            return Ok(val);

        }

        [HttpPost("SetUserSetting")]
        public IActionResult SetUserSetting(UserSettingDto userSettingDto)
        {

            var val = _userSettingRep.SetUserSetting(userSettingDto.Key, userSettingDto.Value);
            return Ok(val);

        }

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
        public async Task<IActionResult> EnableUser(int Id)
        {
            var result = await _repository.EnableUser(Id);
            return Ok(result);
        }


        [HttpGet("DisableUser/{Id}")]
        public async Task<IActionResult> DisableUser(int Id)
        {
            var result = await _repository.DisableUser(Id);
            return Ok(result);
        }

        [HttpPost("PrintUserReport/{renderType}")]
        public async Task<IActionResult> PrintUserReport([FromBody] QueryViewModel<UserResultDto> query, int renderType)
        {
            var val = await _repository.PrintUserReport(query, renderType);
            return Ok(val);
        }

        [HttpPost("GetAllDeletedUser")]
        public async Task<IActionResult> GetAllDeletedUser(QueryViewModel<UserResultDto> query)
        {
            var val = await _repository.GetAllDeletedUser(query);
            return Ok(val);
        }

        [HttpGet("RecoveryUser/{Id}")]
        public async Task<IActionResult> RecoveryUser(int Id)
        {
            var result = await _repository.RecoveryUser(Id);
            return Ok(result);
        }

    }
}
