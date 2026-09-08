using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserAddressController : ControllerBase
{
    private readonly IUserAddressService _citytService;

    public UserAddressController(IUserAddressService citytService)
    {
        _citytService = citytService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] UserAddressResultDto lockupDto)
    {
        var val = await _citytService.Add(lockupDto);
        return Ok(val);
    }

    [HttpGet("GetAll/{UserId}")]
    public async Task<IActionResult> GetAll(int UserId)
    {
        var val = await _citytService.GetAll(UserId);
        return Ok(val);
    }

    [HttpGet("GetOne/{UniqueId}")]
    public async Task<IActionResult> GetOne(string UniqueId)
    {
        var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _citytService.Getone(id);
        return Ok(val);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(UserAddressResultDto lockupDto)
    {
        var val = await _citytService.Update(lockupDto);
        return Ok(val);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var val = await _citytService.Delete(id);
        return Ok(val);
    }
}
