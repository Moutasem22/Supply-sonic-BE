using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserNotificationController : ControllerBase
    {
        IUserNotificationService _repository;
        private readonly GlobalFormat _globalFormat;
        public UserNotificationController(IUserNotificationService repository, GlobalFormat globalFormat)
        {
            _repository = repository;
            _globalFormat = globalFormat;
        }

        [HttpPost("query")]
        public IActionResult GetAll([FromBody] QueryViewModel<UserNotificationDTO> query)
        {
            var fdata = _globalFormat.LogFormat(HttpContext, query);

            var userId = fdata.UserId;
            var val = _repository.GetAll(query, int.Parse(userId));

            return Ok(val);
        }

        [HttpPost("GetAllNotification")]
        public IActionResult GetAllNotification([FromBody] QueryViewModel<UserNotificationDTO> query)
        {
          
            var val = _repository.GetAllNotification(query);

            return Ok(val);
        }

        [HttpGet("ResetCounter")]
        public IActionResult ResetCounter()
        {
            var fdata = _globalFormat.LogFormat(HttpContext, new { });
            var userId = fdata.UserId;
            var val = _repository.ResetCounter(int.Parse(userId));

            return Ok(val);
        }

        [HttpGet("MakeNotifyRead")]
        public IActionResult MakeNotifyRead(string Id)
        {
            var fdata = _globalFormat.LogFormat(HttpContext, new { Id = Id });
            var userId = fdata.UserId;
            var val = _repository.MakeNotifyRead(int.Parse(userId), int.Parse(Id));
            return Ok(val);
        }
    }
}
