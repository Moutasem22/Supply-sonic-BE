
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO
{
    public class RoleAddEditDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage  = "Required")]
        public string NameAr { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public ICollection<PermissionAddEditDto> Permissions { get; set; }
        public ICollection<PageActionModel> PageActions { get; set; }
        public byte[] RowVersion { get; set; }
    }
    public class PageActionModel
    {
        public int? Id { get; set; }
        public int? PageId { get; set; }
        public int? ActionId { get; set; }
        public bool? selected { get; set; }
        public int? PageActionId { get; set; }
    }
}
