using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor
{
    public interface IActionService
    {
        ResultViewModel<List<ActionDTO>> GetAll(QueryViewModel<ActionDTO> queryViewModel);
    }
}
