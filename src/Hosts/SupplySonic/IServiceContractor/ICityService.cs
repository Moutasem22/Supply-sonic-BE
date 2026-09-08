using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface ICityService
{
    Task<ResultViewModel<CityResultDto>> Add(CityAddEditDto lockupDto);
    Task<ResultViewModel<List<CityResultDto>>> GetAll(QueryViewModel<CityAddEditDto> queryViewModel);
    Task<ResultViewModel<CityResultDto>> Getone(int id);
    Task<ResultViewModel<CityResultDto>> Update(CityAddEditDto lockupDto);
    Task<ResultViewModel<CityResultDto>> Delete(int id);
    Task<ResultViewModel<List<CityResultDto>>> GetAllCityByCounryId(int? CounryId);
    Task<ResultViewModel<string>> GetTermsAndConditions();
    Task<ResultViewModel<string>> GetPrivacyPolicy();
}
