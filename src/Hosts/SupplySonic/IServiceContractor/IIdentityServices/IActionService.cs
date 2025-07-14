using DTO.CommandDTO;
using DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor.IdentityInterFaces
{
    public interface IActionService
    {
        ResultViewModel<List<ActionDTO>> GetAll(QueryViewModel<ActionDTO> queryViewModel);
    }
}
