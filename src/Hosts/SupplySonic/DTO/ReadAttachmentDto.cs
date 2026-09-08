using System;
using System.Collections.Generic;
using System.Text;
namespace DTO
{
    public class ReadAttachmentDto
    {
        public int Id { get; set; }
        public string UniqueId { get { return EncryptionHelper.EncryptForUrl(Id.ToString()); } }
        public string FileName { get; set; }
        public string FileDownloadName { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public int ThumbNailId { get; set; }
        public string Path { get; set; }

    }
}
