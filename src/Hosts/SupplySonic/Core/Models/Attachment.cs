using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Attachment : BaseEntity<int>
    {
        public string FileName { get; private set; }
        //DocumentName
        public string FileDownloadName { get; private set; }
        //DocumentType
        public string FileType { get; private set; }
        //DocumentSize
        public long FileSize { get; private set; }
        //DocumentData
        //public byte[] FileData { get; private set; }
        public string Path { get; private set; }
        public Attachment(string FileName, string FileDownloadName, string FileType, long FileSize,/* byte[] FileData,*/ string path)
        {
            this.FileName = FileName;
            this.FileDownloadName = FileDownloadName;
            this.FileType = FileType;
            this.FileSize = FileSize;
            //this.FileData = FileData;
            this.Path = path;
         }
    }
}
