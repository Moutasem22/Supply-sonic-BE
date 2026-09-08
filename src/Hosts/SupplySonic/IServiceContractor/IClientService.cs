using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor
{
    public interface IClientService
    {
        Task<ResultViewModel<ClientResultDto>> Add(ClientAddEditDto lockupDto);
        Task<ResultViewModel<List<ClientResultDto>>> GetAll(QueryViewModel<ClientAddEditDto> queryViewModel);
        Task<ResultViewModel<ClientResultDto>> Getone(int id);
        Task<ResultViewModel<ClientResultDto>> Update(ClientAddEditDto lockupDto);
        Task<ResultViewModel<ClientResultDto>> Delete(int id);
    }
}
