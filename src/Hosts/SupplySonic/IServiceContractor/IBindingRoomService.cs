using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor;

public interface IBindingRoomService
{

    Task<ResultViewModel<BindingRoomResultDto>> Add(BindingRoomResultDto lockupDto);
    Task<ResultViewModel<List<BindingRoomResultDto>>> GetAll(NewQueryViewModel<BindingRoomFilterDto> queryViewModel);
    Task<ResultViewModel<BindingRoomResultDto>> Getone(int id);
    Task<ResultViewModel<BindingRoomResultDto>> Update(BindingRoomResultDto lockupDto);
    Task<ResultViewModel<BindingRoomResultDto>> Delete(int id);


    //request
    Task<ResultViewModel<BindingRoomRequestResultDto>> CreateRequest(BindingRoomRequestResultDto lockupDto);
    Task<ResultViewModel<BindingRoomRequestResultDto>> UpdateRequest(BindingRoomRequestResultDto lockupDto);

    Task<ResultViewModel<List<BindingRoomRequestResultDto>>> GetAllRequest(NewQueryViewModel<BindingRoomFilterDto> queryViewModel);

    Task<ResultViewModel<BindingRoomRequestResultDto>> GetOneRequest(int Id);
}
