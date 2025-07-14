using DTO.CommandDTO;
using DTO.IdentityDTO;
using IServiceContractor.IdentityInterFaces;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PageController : ControllerBase
    {
        IPageService _repository;
        public PageController(IPageService repository)
        {
            _repository = repository;
        }
        [HttpPost("Query")]
        public IActionResult Query(QueryViewModel<PageDTO> query)
        {

            var val = _repository.GetAll(query);
            return Ok(val);

        }

        [HttpPost("GetAllPageCategory")]
        public IActionResult GetAllPageCategory(QueryViewModel<PageCategoryDTO> query)
        {
            var val = _repository.GetAllPageCategory(query);
            return Ok(val);

        }

        [HttpGet("EnablePageWF/{Id}")]
        public IActionResult EnablePageWF(int Id)
        {
            var result = _repository.EnablePageWF(Id);
            return Ok(result);
        }


        [HttpGet("DisablePageWF/{Id}")]
        public IActionResult DisablePageWF(int Id)
        {
            var result = _repository.DisablePageWF(Id);
            return Ok(result);
        }
    }
