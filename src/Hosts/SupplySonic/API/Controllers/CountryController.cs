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
public class CountryController : ControllerBase
{
    private readonly ICountryService _countrytService;

    public CountryController(ICountryService countrytService)
    {
        _countrytService = countrytService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] CountryAddEditDto lockupDto)
    {
        var val = await _countrytService.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(QueryViewModel<CountryAddEditDto> query)
    {
        var val = await _countrytService.GetAll(query);
        return Ok(val);
    }

    [HttpPost("GetAllWithCities")]
    public async Task<IActionResult> GetAllWithCities(QueryViewModel<CountryAddEditDto> query)
    {
        var val = await _countrytService.GetAllWithCities(query);
        return Ok(val);
    }


    [HttpGet("GetOne/{UniqueId}")]
    public async Task<IActionResult> GetOne(string UniqueId)
    {
        var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _countrytService.Getone(id);
        return Ok(val);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(CountryAddEditDto lockupDto)
    {
        var val = await _countrytService.Update(lockupDto);
        return Ok(val);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var val = await _countrytService.Delete(id);
        return Ok(val);
    }
}