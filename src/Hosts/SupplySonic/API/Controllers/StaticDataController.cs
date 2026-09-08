using Core.Enums;
using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StaticDataController : ControllerBase
{

    private readonly ICityService _service;
    public StaticDataController(ICityService service)
    {
        _service = service;
    }
    [HttpGet("GetGenders")]
    public async Task<IActionResult> GetGenders()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumGender)).Cast<EnumGender>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }

    [HttpGet("GetRelationShips")]
    public async Task<IActionResult> GetRelationShips()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumRelationShip)).Cast<EnumRelationShip>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }

    [HttpGet("GetIdentityTypes")]
    public async Task<IActionResult> GetIdentityTypes()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumIdentityType)).Cast<EnumIdentityType>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }

    [HttpGet("GetMediaType")]
    public async Task<IActionResult> GetMediaType()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumMediaType)).Cast<EnumMediaType>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }

    [HttpGet("GetEnumReason")]
    public async Task<IActionResult> GetEnumReason()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumReason)).Cast<EnumReason>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }

    [HttpGet("GetEnumDiscountType")]
    public async Task<IActionResult> GetEnumDiscountType()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumDiscountType)).Cast<EnumDiscountType>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }


    [HttpGet("GetEnumRequestStatus")]
    public async Task<IActionResult> GetEnumRequestStatus()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumRequestStatus)).Cast<EnumRequestStatus>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }

    [HttpGet("GetEnumProviderStatus")]
    public async Task<IActionResult> GetEnumProviderStatus()
    {
        List<StaticDataDto> items = Enum.GetValues(typeof(EnumProviderStatus)).Cast<EnumProviderStatus>().Select(
           item => new StaticDataDto() { Id = (int)item, Name = item.ToString() }).ToList();

        ResultViewModel<List<StaticDataDto>> result = new() { Data = items, IsSuccess = true, Total = items.Count() };

        return Ok(result);
    }


    [HttpGet("GetPrivacyPolicy")]
    public async Task<IActionResult> GetPrivacyPolicy()
    {
        var val = await _service.GetPrivacyPolicy();
        return Ok(val);
    }

    [HttpGet("GetTermsAndConditions")]
    public async Task<IActionResult> GetTermsAndConditions()
    {
        var val = await _service.GetTermsAndConditions();
        return Ok(val);
    }

}

