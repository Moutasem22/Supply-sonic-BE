using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface IUnitService
{
    Task<ResultViewModel<LockupResultDto>> Add(LockupResultDto lockupDto);
    Task<ResultViewModel<List<LockupResultDto>>> GetAll(NewQueryViewModel<LockupResultDto> queryViewModel);
    Task<ResultViewModel<LockupResultDto>> Getone(int id);
    Task<ResultViewModel<LockupResultDto>> Update(LockupResultDto lockupDto);
    Task<ResultViewModel<LockupResultDto>> Delete(int id);
    
}
