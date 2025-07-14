using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CommandDTO
{
    public class EditAttachmentDto
    {
        //public int Id { get; set; }        
        //public string FileName { get; set; }
        //public string FileType { get; set; }
        public bool IsThumbnail { get; set; }
        public List<IFormFile> Files { get; set; }
        public string AllowedFiles { get; set; }
        //public string FileDownloadName { get; set; }
        //public long FileSize { get; set; }
        //public byte[] FileData { get; set; }
        //public string FilePath { get; set; }
    }
}
