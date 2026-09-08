using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface ICountryService
{
    Task<ResultViewModel<CountryResultDto>> Add(CountryAddEditDto lockupDto);
    Task<ResultViewModel<List<CountryResultDto>>> GetAll(QueryViewModel<CountryAddEditDto> queryViewModel);
    Task<ResultViewModel<CountryResultDto>> Getone(int id);
    Task<ResultViewModel<CountryResultDto>> Update(CountryAddEditDto lockupDto);
    Task<ResultViewModel<CountryResultDto>> Delete(int id);
    Task<ResultViewModel<List<CountryResultDto>>> GetAllWithCities(QueryViewModel<CountryAddEditDto> queryViewModel);

}
