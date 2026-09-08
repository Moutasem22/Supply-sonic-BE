using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class BindingRoomAttachment:BaseEntity<int>
{
    public int AttachmentId { get; set; }
    public virtual Attachment? Attachment { get; set; }
    public int BindingRoomId { get; set; }
    public virtual BindingRoom? BindingRoom { get; set; }

    public BindingRoomAttachment()
    {

    }

    public BindingRoomAttachment(int attachmentId)
    {
        this.AttachmentId = attachmentId;
    }

    public BindingRoomAttachment(int attachmentId, int productId)
    {
        this.AttachmentId = attachmentId;
        this.BindingRoomId = productId;
    }
}
