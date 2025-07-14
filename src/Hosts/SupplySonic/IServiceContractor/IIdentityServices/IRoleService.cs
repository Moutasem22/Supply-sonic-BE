using DTO.CommandDTO;
using DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor.IdentityInterFaces
{
    public interface IRoleService
    {
        ResultViewModel<List<RoleResultDto>> GetAll(QueryViewModel<RoleResultDto> queryViewModel);
        ResultViewModel<List<RoleResultDto>> GetAllExceptMaster(QueryViewModel<RoleResultDto> queryViewModel);
        ResultViewModel<RoleResultDto> Getone(int id);
        ResultViewModel<RoleResultDto> Add(RoleAddEditDto roleDto);
        ResultViewModel<RoleResultDto> Update(RoleAddEditDto roleDto);
        ResultViewModel<RoleResultDto> Delete(int id);
    }
}
