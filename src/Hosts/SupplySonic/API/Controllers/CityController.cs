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
public class CityController : ControllerBase
{
    private readonly ICityService _citytService;

    public CityController(ICityService citytService)
    {
        _citytService = citytService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] CityAddEditDto lockupDto)
    {
        var val = await _citytService.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(QueryViewModel<CityAddEditDto> query)
    {
        var val = await _citytService.GetAll(query);
        return Ok(val);
    }

    [HttpGet("GetAllCityByCounryId")]
    public async Task<IActionResult> GetAllCityByCounryId(int? CountryId)
    {
        var val = await _citytService.GetAllCityByCounryId(CountryId);
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
    public async Task<IActionResult> Update(CityAddEditDto lockupDto)
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