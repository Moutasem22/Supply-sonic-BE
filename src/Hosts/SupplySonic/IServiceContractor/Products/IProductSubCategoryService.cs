using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface IProductSubCategoryService
{
    Task<ResultViewModel<ChildLockupResultDto>> Add(ChildLockupResultDto lockupDto);
    Task<ResultViewModel<List<ChildLockupResultDto>>> GetAll(NewQueryViewModel<ChildLockupResultDto> queryViewModel);
    Task<ResultViewModel<ChildLockupResultDto>> Getone(int id);
    Task<ResultViewModel<ChildLockupResultDto>> Update(ChildLockupResultDto lockupDto);
    Task<ResultViewModel<ChildLockupResultDto>> Delete(int id);
    Task<ResultViewModel<List<ChildLockupResultDto>>> GetAllByCategoryId(int? CategoryId);
}
