using DTO;
using IServiceContractor;
using IServiceContractor.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttributeController : ControllerBase
    {
        private readonly IAttributeService _service;

        public AttributeController(IAttributeService __Service)
        {
            _service = __Service;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add( LockupResultDto lockupDto)
        {
            var val = await _service.Add(lockupDto);
            return Ok(val);
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(NewQueryViewModel<LockupResultDto> query)
        {
            var val = await _service.GetAll(query);
            return Ok(val);
        }

        [HttpPost("GetAllWithSubs")]
        public async Task<IActionResult> GetAllWithSubs(QueryViewModel<LockupResultDto> query)
        {
            var val = await _service.GetAllWithSubs(query);
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
        public async Task<IActionResult> Update(LockupResultDto lockupDto)
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
    }
}
