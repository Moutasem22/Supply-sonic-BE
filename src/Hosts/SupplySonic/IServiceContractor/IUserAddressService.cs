using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface IUserAddressService
{
    Task<ResultViewModel<UserAddressResultDto>> Add(UserAddressResultDto lockupDto);
    Task<ResultViewModel<List<UserAddressResultDto>>> GetAll(int userId);
    Task<ResultViewModel<UserAddressResultDto>> Getone(int id);
    Task<ResultViewModel<UserAddressResultDto>> Update(UserAddressResultDto lockupDto);
    Task<ResultViewModel<UserAddressResultDto>> Delete(int id);
}
