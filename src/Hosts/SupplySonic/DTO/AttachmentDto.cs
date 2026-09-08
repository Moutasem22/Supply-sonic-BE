using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }//{ get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
        public string FileName { get; set; }
        public string FileDownloadName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public string FilePath { get; set; }

        public string? Path { get; set; }

    }
}
