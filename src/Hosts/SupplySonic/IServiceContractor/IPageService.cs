using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor
{
    public interface IPageService
    {
        ResultViewModel<List<PageDTO>> GetAll(QueryViewModel<PageDTO> queryViewModel);
        ResultViewModel<List<PageCategoryDTO>> GetAllPageCategory(QueryViewModel<PageCategoryDTO> queryViewModel);
        ResultViewModel<PageDTO> EnablePageWF(int pageID);

        ResultViewModel<PageDTO> DisablePageWF(int pageID);
    }
}
