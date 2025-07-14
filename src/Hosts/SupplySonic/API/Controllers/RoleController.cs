using DTO.CommandDTO;
using DTO.IdentityDTO;
using Helpers;
using IServiceContractor.IdentityInterFaces;
using Microsoft.AspNetCore.Mvc;


namespace AppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]

public class RoleController : ControllerBase
    {
        IRoleService _repository;
        public RoleController(IRoleService repository)
        {
            _repository = repository;
        }
        [HttpGet("GetRole/{UniqueId}")]
        public IActionResult GetRole(string UniqueId)
        {
            var id = int.Parse(DTO.EncryptionHelper.DecryptFromUrl(UniqueId));
            var val = _repository.Getone(id);
            return Ok(val);

        }

        [HttpPost("query")]
        public IActionResult query(QueryViewModel<RoleResultDto> query)
        {
            var val = _repository.GetAll(query);
            return Ok(val);

        }

        [HttpPost("GetAllExceptMaster")]
        public IActionResult GetAllExceptMaster(QueryViewModel<RoleResultDto> query)
        {
            var val = _repository.GetAllExceptMaster(query);
            return Ok(val);

        }

        [HttpPost("AddRole")]
        public IActionResult AddRole(RoleAddEditDto roledto)
        {

            var val = _repository.Add(roledto);
            return Ok(val);

        }

        [HttpPut("UpdateRole")]
        public IActionResult UpdateRole(RoleAddEditDto roledto)
        {
            
            var val = _repository.Update(roledto);
            return Ok(val);
           
        }

        [HttpDelete("DeleteRole/{id}")]
        public IActionResult DeleteRole(int id)
        {          
            var val = _repository.Delete(id);
            return Ok(val);
          
        }

    }
