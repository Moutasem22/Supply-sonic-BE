using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DTO.Product;
using Helpers;
using IServiceContractor;
using IServiceContractor.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService __Service)
    {
        _service = __Service;
    }

    [HttpPost("Add")]
    public async Task<IActionResult> Add([System.Web.Http.FromBody] ProductAddEditDto lockupDto)
    {
        var val = await _service.Add(lockupDto);
        return Ok(val);
    }

    [HttpPost("GetAll/{SupplierId}")]
    public async Task<IActionResult> GetAll(int SupplierId,  NewQueryViewModel<ProductResultDto> query)
    {

        var val = await _service.GetAll(SupplierId,query);
        return Ok(val);
    }

    [HttpGet("GetOne/{UniqueId}")]
    public async Task<IActionResult> GetOne(string UniqueId)
    {
        var id = int.Parse(DTO.EncryptionHelper.DecryptFromUrl(UniqueId));
        var val = await _service.Getone(id);
        return Ok(val);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(ProductAddEditDto lockupDto)
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


    [HttpGet("GetProductAttribues/{id}")]
    public async Task<IActionResult> GetProductAttribues(int id) => Ok(await _service.GetProductAttribues(id));

    [HttpGet("GetProductAttachments/{id}")]
    public async Task<IActionResult> GetProductAttachments(int id) => Ok(await _service.GetProductAttachments(id));

    
    [HttpPost("SaveProductAttachments/{id}")]
    public async Task<IActionResult> SaveProductAttachments(int id, List<int> ids) => Ok(await _service.SaveProductAttachments(id, ids));

    [HttpPost("SaveProductAttribues/{id}")]
    public async Task<IActionResult> SaveProductAttribues(int id, List<ProductAttribueAddEditDto> ids) => Ok(await _service.SaveProductAttribues(id, ids));


    [HttpGet("GetProductWeights/{id}")]
    public async Task<IActionResult> GetProductWeights(int id) => Ok(await _service.GetProductWeights(id));

    [HttpPost("SaveProductWeights/{id}")]
    public async Task<IActionResult> SaveProductWeights(int id, List<ProductWeightResultDto> ids) => Ok(await _service.SaveProductWeights(id, ids));

}


