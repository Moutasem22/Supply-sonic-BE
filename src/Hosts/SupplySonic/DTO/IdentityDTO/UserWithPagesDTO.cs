using System;
using System.Collections.Generic;
using System.Text;
using DTO.CommandDTO;

namespace DTO.IdentityDTO
{
    public class UserWithPagesDTO
    {
        public UserDTO CurrentUser { get; set; }
        public ResultViewModel<List<PageDTO>> AuthorizedPages { get; set; }
        public ResultViewModel<List<ActionDTO>> AuthorizedCurrentPageAction { get; set; }
        public ResultViewModel<List<PageCategoryDTO>> PageCategories { get; set; }

    }
}
