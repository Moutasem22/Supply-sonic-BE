using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface IOrderService
{
    Task<ResultViewModel<OrderResultDto>> CreateOrder(OrderResultDto lockupDto);
    Task<ResultViewModel<OrderResultDto>> UpdateOrder(OrderResultDto lockupDto);

    Task<ResultViewModel<List<OrderResultDto>>> GetAllOrder(NewQueryViewModel<OrderFilterDto> queryViewModel, bool? IsSupplyer = null);

    Task<ResultViewModel<OrderResultDto>> GetOneOrder(int Id);

}
