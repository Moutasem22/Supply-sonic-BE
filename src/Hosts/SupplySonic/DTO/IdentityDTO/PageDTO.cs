using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.IdentityDTO
{
    public class PageDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public int Index { get; set; }
        public string Path { get; set; }
        public int? PageCategoryId { get; set; }
        public ICollection<PageActionDTO> PageActions { get; set; }
        public bool HasDataPermissions { get; set; }

        public PageDTO()
        {
            PageActions = new HashSet<PageActionDTO>();

        }
    }
}
