using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{

    private readonly IOrderService _citytService;

    public OrderController(IOrderService citytService)
    {
        _citytService = citytService;
    }

    //Order 

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([System.Web.Http.FromBody] OrderResultDto lockupDto)
    {
        var val = await _citytService.CreateOrder(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders(NewQueryViewModel<OrderFilterDto> query)
    {
        var val = await _citytService.GetAllOrder(query,null);
        return Ok(val);
    }

    [HttpPost("GetAllSupplierOrders")]
    public async Task<IActionResult> GetAllSupplierOrders(NewQueryViewModel<OrderFilterDto> query)
    {
        var val = await _citytService.GetAllOrder(query, true);
        return Ok(val);
    }

    [HttpPost("GetAllDemanderOrders")]
    public async Task<IActionResult> GetAllDemanderOrders(NewQueryViewModel<OrderFilterDto> query)
    {
        var val = await _citytService.GetAllOrder(query, false);
        return Ok(val);
    }

    [HttpGet("GetOneOrder/{UniqueId}")]
    public async Task<IActionResult> GetOneOrder(string UniqueId)
    {
        var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _citytService.GetOneOrder(id);
        return Ok(val);
    }

    [HttpPost("UpdateOrder")]
    public async Task<IActionResult> UpdateOrder(OrderResultDto lockupDto)
    {
        var val = await _citytService.UpdateOrder(lockupDto);
        return Ok(val);
    }

}
