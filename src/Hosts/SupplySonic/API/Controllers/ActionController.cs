using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ActionController : ControllerBase
{
    IActionService _repository;
    public ActionController(IActionService repository)
    {
        _repository = repository;
    }
    [HttpPost("Query")]
    public IActionResult Query(QueryViewModel<ActionDTO> query)
    {
        var val = _repository.GetAll(query);
        return Ok(val);
    }
}
