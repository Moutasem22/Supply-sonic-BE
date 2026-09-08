using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationSettingsController : ControllerBase
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        public NotificationSettingsController(INotificationSettingsService notificationSettingsService)
        {
            _notificationSettingsService= notificationSettingsService;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(List<NotificationSettingAddEditDto> notificationSettings)
        {
            var val = await _notificationSettingsService.Update(notificationSettings);
            return Ok(val);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var val = await _notificationSettingsService.GetAll();
            return Ok(val);
        }
    }
}
