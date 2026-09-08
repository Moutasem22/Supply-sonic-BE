using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppearanceSettingsController : ControllerBase
    {
        private readonly IAppearanceSettingsService _IAppearanceSettingsService;
        public AppearanceSettingsController(IAppearanceSettingsService iAppearanceSettingsService)
        {
            _IAppearanceSettingsService=iAppearanceSettingsService;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(AppearanceAddEditDto AppearanceDto)
        {
            var val = await _IAppearanceSettingsService.Update(AppearanceDto);
            return Ok(val);
        }
        [HttpGet("GetCurrent")]
        public async Task<IActionResult> GetCurrent()
        {
            var val = await _IAppearanceSettingsService.GetCurrent();
            return Ok(val);
        }
    }
}
