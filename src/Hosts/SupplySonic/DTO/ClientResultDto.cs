using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ClientResultDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
        public byte[] RowVersion { get; set; }
        public bool IsActive { get; set; }
    }
}
