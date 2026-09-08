using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DTO;
using DTO.Product;
using DTO.RequestDto;
using IServiceContractor;
using IServiceContractor.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductOfferController : ControllerBase
{
    private readonly IProductOfferService _service;

    public ProductOfferController(IProductOfferService __Service)
    {
        _service = __Service;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] ProductOfferAddEditDto lockupDto)
    {
        var val = await _service.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll/{ProductId}")]
    public async Task<IActionResult> GetAll(int ProductId, NewQueryViewModel<ProductOfferResultDto> query)
    {
        var val = await _service.GetAll(ProductId, query);
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
    public async Task<IActionResult> Update(ProductOfferAddEditDto lockupDto)
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


    [HttpGet("GetProductOfferAttribues/{id}")]
    public async Task<IActionResult> GetProductOfferAttribues(int id) => Ok(await _service.GetProductOfferAttribues(id));

    [HttpGet("GetProductOfferAttachments/{id}")]
    public async Task<IActionResult> GetProductOfferAttachments(int id) => Ok(await _service.GetProductOfferAttachments(id));


    [HttpPost("SaveProductOfferAttachments/{id}")]
    public async Task<IActionResult> SaveProductOfferAttachments(int id, List<int> ids) => Ok(await _service.SaveProductOfferAttachments(id, ids));

    [HttpPost("SaveProductOfferAttribues/{id}")]
    public async Task<IActionResult> SaveProductOfferAttribues(int id, List<ProductOfferAttribueAddEditDto> ids) => Ok(await _service.SaveProductOfferAttribues(id, ids));



    [HttpGet("GetProductOfferPriceSchedules/{id}")]
    public async Task<IActionResult> GetProductOfferPriceSchedules(int id) => Ok(await _service.GetProductOfferPriceSchedules(id));

    [HttpPost("SaveProductOfferPriceSchedules/{id}")]
    public async Task<IActionResult> SaveProductOfferPriceSchedules(int id, List<ProductOfferPriceScheduleResultDto> ids) => Ok(await _service.SaveProductOfferPriceSchedules(id, ids));



    [HttpPost("GetAllProductOfferView")]
    public async Task<IActionResult> GetAllProductOfferView(NewQueryViewModel<ProductViewDto> query)
    {
        var val = await _service.GetAllProductOfferView(query);
        return Ok(val);
    }

    [HttpPost("SearchOffers")]
    public async Task<IActionResult> SearchOffers(NewQueryViewModel<RequestProductFilterDTO> query)
    {
        var val = await _service.SearchOffer(query);
        return Ok(val);
    }
}


