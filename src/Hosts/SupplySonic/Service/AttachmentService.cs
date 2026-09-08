using Core.Models;
using DB;
using DTO;
using Helpers;
using IServiceContractor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Mapster;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using Localization;
using Microsoft.AspNetCore.StaticFiles;
using Xabe.FFmpeg;

namespace Service
{
    public class AttachmentService : IAttachmentService
    {
        private DBContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _hostingEnvironment;
        private readonly RestHelper _restHelper;
        private readonly IHostingEnvironment _env;
        public string lang { get; set; }
        private readonly IStringLocalizer<SharedResource> localizer;

        public AttachmentService(DBContext dbcontext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env, IHostingEnvironment hostingEnvironment, RestHelper restHelper, IStringLocalizer<SharedResource> localizer)
        {
            _httpContextAccessor = httpContextAccessor;
            lang = _httpContextAccessor.HttpContext.Request.Headers["lang"].ToString() ?? "ar";
            this._dbcontext = dbcontext;
            _hostingEnvironment = hostingEnvironment;
            _restHelper = restHelper;
            this.localizer = localizer;
            _env = env;
        }

        public ResultViewModel<List<ReadAttachmentDto>> GetAll(QueryViewModel<ReadAttachmentDto> queryViewModel)
        {
            var _ResultViewModel = new ResultViewModel<List<ReadAttachmentDto>>();

            try
            {
                var query = _dbcontext.Attachments.Where(r => r.IsActive && r.IsDeleted != true).OrderByDescending(a => a.Id);
                var Total = query.Count();

                var attachments = queryViewModel.PageSize == 0 ? query : query.Skip((queryViewModel.PageNumber - 1) * queryViewModel.PageSize).Take(queryViewModel.PageSize);

                List<ReadAttachmentDto> attachmentDtos = attachments.Adapt<List<ReadAttachmentDto>>().ToList(); //Mapper.Map<List<ReadAttachmentDto>>(attachments.ToList());

                _ResultViewModel.Data = attachmentDtos;
                _ResultViewModel.PageSize = queryViewModel.PageSize;
                _ResultViewModel.PageNumber = queryViewModel.PageNumber;
                _ResultViewModel.Total = Total;
                _ResultViewModel.IsSuccess = true;
                return _ResultViewModel;
            }
            catch
            {
                return _ResultViewModel;
            }

        }
        byte[] MakeThumbnail(byte[] myImage, int thumbWidth, int thumbHeight)
        {
            using (MemoryStream ms = new MemoryStream())
            using (Image thumbnail = Image.FromStream(new MemoryStream(myImage)).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }


        private string GetFileMagicNumber(byte[] content)
        {
            string fileMagicNo = string.Empty;
            try
            {
                string hexContent = BitConverter.ToString(content);
                fileMagicNo = hexContent.Substring(0, 11).Replace("-", " ");
            }
            catch
            {
            }
            return fileMagicNo;
        }

        private string GetFileType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        private void GenerateThumbnail(string filepath, int thumbWidth, int thumbHeight, string thumbnailpath)
        {
            using (Image thumbnail = Image.FromFile(filepath).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(thumbnailpath, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private async Task GetScreenShotFromVedioAsync(string EXECPATH, string video, string thumbnail)
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

        private void validateAttachment(IFormFile FileDto, string AllowedFiles)
        {
            var results = new List<ValidationResult>();
            if (string.IsNullOrWhiteSpace(FileDto.FileName))
            {
                results.Add(new ValidationResult("InvalidFileNameLength", new List<string>() { }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }

            var maxFileName = _dbcontext.SysSettings.FirstOrDefault(s => s.IsActive && s.SysKey == "AttachmentMaxFileName");
            int fileName = int.Parse(maxFileName != null ? maxFileName.SysValue : "100");
            if (FileDto.FileName.Length > fileName)
            {
                results.Add(new ValidationResult("InvalidFileNameLength", new List<string>() { }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }

            if (string.IsNullOrWhiteSpace(Path.GetExtension(FileDto.FileName).Replace(".","")))
            {
                results.Add(new ValidationResult("FileExtNotAllowed", new List<string>() { }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }

            if (FileDto.Length < 1)
            {
                results.Add(new ValidationResult("InvalidFileSize", new List<string>() { }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }

            var maxFileSize = _dbcontext.SysSettings.FirstOrDefault(s => s.IsActive && s.SysKey == "AttachmentMaxFileSize");
            long fileSize = long.Parse(maxFileSize != null ? maxFileSize.SysValue : "10") * 1024 * 1024;
            if (FileDto.Length > fileSize)
            {
                results.Add(new ValidationResult("InvalidFileSize", new List<string>() { }));
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
            }

            var fileExt = Path.GetExtension(FileDto.FileName.ToLower()).Replace(".", "");
            using (var ms = new MemoryStream())
            {
                FileDto.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string fileMagicNo = GetFileMagicNumber(fileBytes);
                var attExtension = _dbcontext.AttachmentExtension.FirstOrDefault(e => e.IsActive && e.Extension == fileExt && e.MagicNo == fileMagicNo);
                if (attExtension == null)
                {
                    results.Add(new ValidationResult("FileExtNotAllowed", new List<string>() { }));
                    throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
                }
                else if ( !string.IsNullOrWhiteSpace(AllowedFiles) && !(AllowedFiles.ToLower().Split(",").Contains(attExtension.Extension.ToLower())|| AllowedFiles.ToLower().Split(",").Contains("."+attExtension.Extension.ToLower())))
                {
                    results.Add(new ValidationResult("InvalidFileType", new List<string>() { }));
                    throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(results));
                }
            }
        }

        private async Task<int> CreateThumbnail(IFormFile FileDto, string relativePath, string filepath)
        {
            //EditAttachmentDto thumbnailAttach = new EditAttachmentDto();
            string thumbRelativePath = "/temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + Guid.NewGuid() + ".png";
            string thumbPath = _env.WebRootPath + thumbRelativePath;
            if (FileDto.ContentType != "video/mp4" && FileDto.ContentType != "video/avi" &&
                FileDto.ContentType != "video/quicktime" && FileDto.ContentType != "video/x-ms-wmv" && FileDto.ContentType != "image/svg+xml"
               && FileDto.ContentType != "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
               && FileDto.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
               && FileDto.ContentType != "application/pdf"
               && FileDto.ContentType != "application/vnd.openxmlformats-officedocument.presentationml.presentation")
            {
                //thumbnailAttach.FileType = string.IsNullOrWhiteSpace(FileDtoType) ? GetFileType(FileDto.FileName) : FileDtoType;
                GenerateThumbnail(filepath, 200, 125, thumbPath);
            }
            else if (FileDto.ContentType == "image/svg+xml")
            {
                //thumbnailAttach.FileType = "image/svg+xml";
                thumbRelativePath = relativePath;
            }
            else if (FileDto.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                   || FileDto.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                   || FileDto.ContentType == "application/pdf"
                   || FileDto.ContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
            {
                //thumbnailAttach.FileType = string.IsNullOrWhiteSpace(FileDtoType) ? GetFileType(FileDto.FileName) : FileDtoType;
                thumbRelativePath = relativePath;
            }
            else
            {
                var screenshot = _env.WebRootPath + "/temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + Guid.NewGuid() + ".png";
                await GetScreenShotFromVedioAsync(_env.WebRootFileProvider.GetFileInfo("temp/ffmpeg.exe")?.PhysicalPath, filepath, screenshot);
                //thumbnailAttach.FileType = GetFileType(screenshot);
                GenerateThumbnail(screenshot, 200, 125, thumbPath);

            }
            var ThumbFileName = "ThumbNail" + FileDto.FileName;

            var fileExt = Path.GetExtension(FileDto.FileName.ToLower()).Replace(".", "");
            var thumbAttachment = new Attachment(ThumbFileName, ThumbFileName, fileExt, 0, thumbRelativePath);
            _dbcontext.Attachments.Add(thumbAttachment);
            _dbcontext.SaveChanges();
            return thumbAttachment.Id;
        }



        //public async Task<ResultViewModel<List<ReadAttachmentDto>>> Add(List<EditAttachmentDto> attachmentsList)
        //{
        //    foreach (var file in attachmentsList)
        //        validateAttachment(file);

        //    if (!Directory.Exists(_env.WebRootPath + "/temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month))
        //        Directory.CreateDirectory(_env.WebRootPath + "/temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month);

        //    var result = new ResultViewModel<List<ReadAttachmentDto>>();
        //    var attList = new List<ReadAttachmentDto>();
        //    foreach (var file in attachmentsList)
        //    {
        //        var relativePath = "/temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + Guid.NewGuid() + Path.GetExtension(file.File.FileName);
        //        var filepath = _env.WebRootPath + relativePath;

        //        using (Stream fileStream = new FileStream(filepath, FileMode.Create))
        //            await file.File.CopyToAsync(fileStream);

        //        file.FileName = string.IsNullOrWhiteSpace(file.FileName) ? file.File.FileName : file.FileName;
        //        file.FileType = string.IsNullOrWhiteSpace(file.FileType) ? GetFileType(file.File.FileName) : file.FileType;

        //        var fileExt = Path.GetExtension(file.FileName.ToLower()).Replace(".", "");
        //        var attachment = new Attachment(file.FileName, file.FileName, fileExt, file.File.Length, relativePath);
        //        _dbcontext.Attachments.Add(attachment);
        //        attList.Add( attachment.Adapt<ReadAttachmentDto>());
        //        if (file.IsThumbnail == true)
        //            attList[attList.Count - 1].ThumbNailId = await CreateThumbnail(file, relativePath, filepath);
        //    }

        //    _dbcontext.SaveChanges();
        //    result.Data = attList;
        //    result.IsSuccess = true;
        //    return result;
        //}

        public async Task<ResultViewModel<List<ReadAttachmentDto>>> Add(EditAttachmentDto editAttachmentDto)
        {
            List<ReadAttachmentDto> readAttachments = new List<ReadAttachmentDto>();

            //foreach (var file in editAttachmentDto.Files)
            //            validateAttachment(file,editAttachmentDto.AllowedFiles);

            foreach (var filedto in editAttachmentDto.Files)
            {
                var FolderPath = "/temp/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/";
                var relativePath = FolderPath + Guid.NewGuid() + Path.GetExtension(filedto.FileName);
                var filepath = _env.WebRootPath + relativePath;
                if (!Directory.Exists(_env.WebRootPath + FolderPath))
                {
                    Directory.CreateDirectory(_env.WebRootPath + FolderPath);
                }

                using (Stream fileStream = new FileStream(filepath, FileMode.Create))
                    await filedto.CopyToAsync(fileStream);

                //editAttachmentDto.FileName = string.IsNullOrWhiteSpace(editAttachmentDto.FileName) ? editAttachmentDto.File.FileName : editAttachmentDto.FileName;
                //editAttachmentDto.FileType = string.IsNullOrWhiteSpace(editAttachmentDto.FileType) ? GetFileType(editAttachmentDto.File.FileName) : editAttachmentDto.FileType;

                var fileExt = Path.GetExtension(filedto.FileName.ToLower()).Replace(".", "");
                var attachment = new Attachment(filedto.FileName, filedto.FileName, fileExt, filedto.Length, relativePath);
                _dbcontext.Attachments.Add(attachment);
                _dbcontext.SaveChanges();

                var readAttachment = attachment.Adapt<ReadAttachmentDto>();
                if (editAttachmentDto.IsThumbnail == true)
                {
                    readAttachment.ThumbNailId = await CreateThumbnail(filedto, relativePath, filepath);
                }
                readAttachments.Add(readAttachment);
            } 
            
            var result = new ResultViewModel<List<ReadAttachmentDto>>();
            result.Data = readAttachments;
            result.IsSuccess = true;        
            return result;
        }

        public ResultViewModel<ReadAttachmentDto> Delete(int id)
        {
            ResultViewModel<ReadAttachmentDto> _ResultViewModel = new ResultViewModel<ReadAttachmentDto>();

            try
            {
                var attcahment = _dbcontext.Attachments.FirstOrDefault(x => x.Id == id);
                if (attcahment == null)
                {
                    //  throw new BusinessException(ExceptionType.RecordMissed, "item Not Exist");

                }
                attcahment.Delete();
                _dbcontext.SaveChanges();

                _ResultViewModel.Data = attcahment.Adapt<ReadAttachmentDto>();//Mapper.Map<ReadAttachmentDto>(attcahment);
                _ResultViewModel.IsSuccess = true;
                return _ResultViewModel;
            }
            catch (Exception ex)
            {
                //_ResultViewModel.Messages.Add(ex.ExceptionLog());
                return _ResultViewModel;
            }

        }

        public ResultViewModel<AttachmentDto> Get(int id)
        {
            ResultViewModel<AttachmentDto> _ResultViewModel = new ResultViewModel<AttachmentDto>();

            var entity = _dbcontext.Attachments.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                throw new BusinessException(Newtonsoft.Json.JsonConvert.SerializeObject(new ValidationResult(localizer["ItemNotExist"], new List<string>() { nameof(id) })));
            }
            _ResultViewModel.Data = entity.Adapt<AttachmentDto>();//Mapper.Map<AttachmentDto>(entity);
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;
        }

    }
}
