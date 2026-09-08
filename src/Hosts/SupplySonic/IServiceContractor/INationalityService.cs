using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface INationalityService
{
    Task<ResultViewModel<NationalityResultDto>> Add(NationalityAddEditDto lockupDto);
    Task<ResultViewModel<List<NationalityResultDto>>> GetAll(QueryViewModel<NationalityAddEditDto> queryViewModel);
    Task<ResultViewModel<NationalityResultDto>> Getone(int id);
    Task<ResultViewModel<NationalityResultDto>> Update(NationalityAddEditDto lockupDto);
    Task<ResultViewModel<NationalityResultDto>> Delete(int id);
}
