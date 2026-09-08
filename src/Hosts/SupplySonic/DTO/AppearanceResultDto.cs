using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AppearanceResultDto
    {
        public int Id { get; set; }

        public string SystemColors { get; set; }
        public string ButtonsColors { get; set; }
        public string SystemColorAlpha { get; set; }
        public string SystemColorHexa { get; set; }
        public string SystemColorHex { get; set; }

        public string ButtonsColorsAlpha { get; set; }
        public string ButtonsColorsHexa { get; set; }
        public string ButtonsColorsHex { get; set; }

        public string Logo { get; set; }
        public int? LogoAttachmentId { get; set; }
        public string Path { get; set; }
        public AttachmentDto LogoAttachment { get; set; }
        public byte[] RowVersion { get; set; }


    }
}
