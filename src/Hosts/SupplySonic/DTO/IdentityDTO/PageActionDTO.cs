using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.IdentityDTO
{
    public class PageActionDTO
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int ActionId { get; set; }
        public ICollection<PermissionDTO> Permissions { get; set; }
        public PageActionDTO()
        {
            Permissions = new HashSet<PermissionDTO>();
        }
    }
}
