using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class BindingRoomRequest : BaseEntity<int>
{
    public int BindingRoomId { get; set; }
    public virtual BindingRoom BindingRoom { get; set; }
    public int? SupplierAppUserId { get; set; }
    public virtual SupplierAppUser? SupplierAppUser { get; set; }

    public bool? IsNegotiate { get; set; } = false;


    public BindingRoomRequest()
    {

    }

    public BindingRoomRequest(int BindingRoomId, int? SupplierAppUserId, bool? IsNegotiate)
    {
        this.BindingRoomId = BindingRoomId;
        this.SupplierAppUserId = SupplierAppUserId;
        this.IsNegotiate = IsNegotiate;
    }

    public void Update(int BindingRoomId, int? SupplierAppUserId, bool? IsNegotiate, BindingRoom bindingRoom)
    {
        this.BindingRoomId = BindingRoomId;
        this.SupplierAppUserId = SupplierAppUserId;
        this.IsNegotiate = IsNegotiate;
        this.BindingRoom.UpdateStatus(bindingRoom.Status, bindingRoom.RejectedReason);
        this.BindingRoom.UpdateBindingRoomStatus(bindingRoom.BindingRoomStatus);
    }
}
