using Core.Models.Identity;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class BindingRoomRequestResultDto
{
    public int Id { get; set; }
    public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
    public byte[] RowVersion { get; set; }
    public int BindingRoomId { get; set; }
    public BindingRoomResultDto? BindingRoom { get; set; }
    public int? SupplierAppUserId { get; set; }
    public string? SupplierAppUserName { get; set; }
    public bool? IsNegotiate { get; set; } = false;

    public string? OrderDate { get; set; }
    public string? CreatorName { get; set; }

    
}
