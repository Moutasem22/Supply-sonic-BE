using DTO;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ClientAddEditDto lockupDto)
        {
            var val = await _clientService.Add(lockupDto);
            return Ok(val);
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(QueryViewModel<ClientAddEditDto> query)
        {
            var val = await _clientService.GetAll(query);
            return Ok(val);
        }

        [HttpGet("GetOne/{UniqueId}")]
        public async Task<IActionResult> GetOne(string UniqueId)
        {
            var id = int.Parse(EncryptionHelper.DecryptFromUrl(UniqueId));
            var val = await _clientService.Getone(id);
            return Ok(val);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(ClientAddEditDto lockupDto)
        {
            var val = await _clientService.Update(lockupDto);
            return Ok(val);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var val = await _clientService.Delete(id);
            return Ok(val);
        }
    }
}
