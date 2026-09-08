using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO;

public class MailRequestDto
{
    public List<string?>? ToEmails { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? ProjectCode { get; set; }

    // public List<IFormFile>? Attachments { get; set; }
    //public List<MailRequestAttachmentDto?>? AttachmentsRequest { get; set; }
}

public class MailRequestAttachmentDto
{
    public byte[]? _Attachments { get; set; }
    public string? FileName { get; set; }
}
