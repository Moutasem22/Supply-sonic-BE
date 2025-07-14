using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.CommandDTO;
using Helpers;
using IServiceContractor.IAttachmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Xabe.FFmpeg;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        IAttachmentService _repository;
        private readonly GlobalFormat _globalFormat;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string[] allowedExt = { ".png", ".pdf", ".jpg", ".jpeg", ".svg", ".mp4", ".avi", ".ogg", ".mkv", ".webm", ".flv", ".gif", ".wmv", ".asf", ".xlsx", ".docx", ".pptx" };
        public string lang { get; set; }
        public AttachmentController(IAttachmentService repository, GlobalFormat globalFormat, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _globalFormat = globalFormat;
            _httpContextAccessor = httpContextAccessor;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";

        }

        bool IsPdf(IFormFile file)
        {
            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;
            var buf = new byte[len];
            var remaining = len;
            var pos = 0;
            using (var f = file.OpenReadStream())
            {
                while (remaining > 0)
                {
                    var amtRead = f.Read(buf, pos, remaining);
                    if (amtRead == 0) return false;
                    remaining -= amtRead;
                    pos += amtRead;
                }
            }
            return pdfBytes.SequenceEqual(buf);
        }
        bool isImag(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return true;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return true;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return true;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return true;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return true;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return true;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return true;

            return false;
        }

        bool IsSvgFile(byte[] bytes)
        {
            try
            {
                //using (var xmlReader = XmlReader.Create(fileStream))
                //{
                //    return xmlReader.MoveToContent() == XmlNodeType.Element && "svg".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase);

                //}
                var str = Encoding.UTF8.GetString(bytes);
                //using (XmlReader xmlReader = XmlReader.Create(new StringReader(str){ XmlResolver = null}))
                //{
                //    //xmlReader.Read();
                //    //return xmlReader.Name.Equals("svg", StringComparison.InvariantCultureIgnoreCase);
                //    return xmlReader.MoveToContent() == XmlNodeType.Element && "svg".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase);
                //}

                return str.StartsWith("<?xml ") || str.StartsWith("<svg ");
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("Query")]
        public IActionResult Query(QueryViewModel<ReadAttachmentDto> query)
        {
            var val = _repository.GetAll(query);
            return Ok(val);

        }
        [HttpPost("mobileAttachment")]
        public IActionResult mobileAttachment(IFormFile file)
        {

            return Ok("");
        }
        [HttpPost("AddAttachmentsList")]
        public async Task<IActionResult> AddAttachmentsList([FromForm] EditAttachmentDto editAttachmentDto)
        {
            var val = await _repository.Add(editAttachmentDto);
            return Ok(val);
        }

        //[HttpPost("AddAttachment")]
        //public async Task<IActionResult> AddAttachment([FromForm] EditAttachmentDto editAttachmentDto)
        //{
        //    var val = await _repository.Add(editAttachmentDto);
        //    return Ok(val);
        //}


        void GenerateThumbnail(string filepath, int thumbWidth, int thumbHeight, string thumbnailpath)
        {
            using (Image thumbnail = Image.FromFile(filepath).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(thumbnailpath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        async Task GetScreenShotFromVedioAsync(string EXECPATH, string video, string thumbnail)
        {
            //var cmd = "ffmpeg  -itsoffset -1  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            //var startInfo = new ProcessStartInfo
            //{
            //    WindowStyle = ProcessWindowStyle.Hidden,
            //    FileName = "cmd.exe",
            //    Arguments = "/C " + cmd
            //};

            //var process = new Process
            //{
            //    StartInfo = startInfo
            //};

            //process.Start();
            //process.WaitForExit(5000);

            //return LoadImage(thumbnail);
            EXECPATH = EXECPATH.Replace("ffmpeg.exe", "");

            string output = thumbnail;//"D:\\Workspaces\\SASCO.Portal\\SASCO.Portal\\AppAPI\\wwwroot\\temp\\ee.png";
            FFmpeg.SetExecutablesPath(EXECPATH);
            //FFmpeg.ExecutablesPath = _env.WebRootFileProvider.GetFileInfo("temp/ffmpeg.exe")?.PhysicalPath;
            IConversion conversion = await FFmpeg.Conversions.FromSnippet.Snapshot(video, output, TimeSpan.FromSeconds(0));
            IConversionResult result = await conversion.Start();
            return;
        }


        //[HttpGet("DownloadProfileFileAttachment")]
        //public async Task<IActionResult> DownloadProfileFileAttachment(string filename)
        //{

        //    var val = await _repository.DownloadProfileFile(filename);
        //    return Ok(val);

        //}

        string GetFileType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpDelete("DeleteAttachment/{id}")]
        public IActionResult DeleteAttachment(int id)
        {

            var val = _repository.Delete(id);

            return Ok(val);

        }

        [HttpGet("GetAttachment/{id}")]
        public IActionResult GetAttachment(string id)
        {

            var attachmentId = int.Parse(Helpers.EncryptionHelper.DecryptFromUrl(id));
            var val = _repository.Get(attachmentId);

            return Ok(val);

        }

        [HttpGet("DownloadAttachment/{id}")]
        [AllowAnonymous]
        public IActionResult DownloadAttachment(string id)
        {
            var attachmentId = int.Parse(Helpers.EncryptionHelper.DecryptFromUrl(id));
            var attachment = _repository.Get(attachmentId);

            var edata = _globalFormat.LogFormat(HttpContext, attachment);

            if (attachment.Data == null)
            {
                return BadRequest("File note found");
            }
            return File(attachment.Data.FilePath, "APPLICATION/octet-stream", attachment.Data.FileDownloadName);

        }

    }
}
