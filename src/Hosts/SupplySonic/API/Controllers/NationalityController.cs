using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NationalityController : ControllerBase
{
    private readonly INationalityService _Service;

    public NationalityController(INationalityService Service)
    {
        _Service = Service;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] NationalityAddEditDto lockupDto)
    {
        var val = await _Service.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(QueryViewModel<NationalityAddEditDto> query)
    {
        var val = await _Service.GetAll(query);
        return Ok(val);
    }

    [HttpGet("GetOne/{UniqueId}")]
    public async Task<IActionResult> GetOne(string UniqueId)
    {
        var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _Service.Getone(id);
        return Ok(val);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(NationalityAddEditDto lockupDto)
    {
        var val = await _Service.Update(lockupDto);
        return Ok(val);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var val = await _Service.Delete(id);
        return Ok(val);
    }
}