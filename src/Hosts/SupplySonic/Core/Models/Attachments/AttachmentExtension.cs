using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Attachments
{
    public class AttachmentExtension : BaseEntity<int>
    {
        public string Extension { get; set; }
        public string MagicNo { get; set; }
        public string ExtFileType { get; set; }
    }
}
