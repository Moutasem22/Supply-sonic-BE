using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.IdentityDTO
{
    public class RoleResultDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string Name { get; set; }
        public bool IsMaster { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<PermissionAddEditDto> Permissions { get; set; }
        public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
        public byte[] RowVersion { get; set; }
    }
}
