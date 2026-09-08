using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnitController : ControllerBase
{
    private readonly IUnitService _service;

    public UnitController(IUnitService __Service)
    {
        _service = __Service;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] LockupResultDto lockupDto)
    {
        var val = await _service.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(NewQueryViewModel<LockupResultDto> query)
    {
        var val = await _service.GetAll(query);
        return Ok(val);
    }


    [HttpGet("GetOne/{UniqueId}")]
    public async Task<IActionResult> GetOne(string UniqueId)
    {
        var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _service.Getone(id);
        return Ok(val);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(LockupResultDto lockupDto)
    {
        var val = await _service.Update(lockupDto);
        return Ok(val);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var val = await _service.Delete(id);
        return Ok(val);
    }
}
