using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BindingRoomController : ControllerBase
{

    private readonly IBindingRoomService _citytService;

    public BindingRoomController(IBindingRoomService citytService)
    {
        _citytService = citytService;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] BindingRoomResultDto lockupDto)
    {
        var val = await _citytService.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAll(NewQueryViewModel<BindingRoomFilterDto> query)
    {
        var val = await _citytService.GetAll(query);
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
    public async Task<IActionResult> Update(BindingRoomResultDto lockupDto)
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




    //Request 

    [HttpPost("CreateRequest")]
    public async Task<IActionResult> CreateRequest([System.Web.Http.FromBody] BindingRoomRequestResultDto lockupDto)
    {
        var val = await _citytService.CreateRequest(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAllRequest")]
    public async Task<IActionResult> GetAllRequest(NewQueryViewModel<BindingRoomFilterDto> query)
    {
        var val = await _citytService.GetAllRequest(query);
        return Ok(val);
    }


    [HttpGet("GetOneRequest/{UniqueId}")]
    public async Task<IActionResult> GetOneRequest(string UniqueId)
    {
        var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _citytService.GetOneRequest(id);
        return Ok(val);
    }

    [HttpPost("UpdateRequest")]
    public async Task<IActionResult> UpdateRequest(BindingRoomRequestResultDto lockupDto)
    {
        var val = await _citytService.UpdateRequest(lockupDto);
        return Ok(val);
    }
}
