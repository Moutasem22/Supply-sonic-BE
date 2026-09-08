using DTO;
using Helpers;
using Microsoft.Extensions.Hosting;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Service
{
    public class ExportHelper
    {
        private readonly IHostEnvironment _env;
        private readonly SysSettingsService _sysSettingsService;
        private string AdminUrl { get; set; }
        public ExportHelper(IHostEnvironment env, SysSettingsService sysSettingsService)
        {
            _env = env;
            _sysSettingsService = sysSettingsService;
        }

        public byte[] Convert(ExportModel model, string path, PageSize size= NReco.PdfGenerator.PageSize.A4, 
            PageOrientation orientation= PageOrientation.Portrait, int pageWidth = 210, int pageHeight = 297)
        {
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates/Reports.html");
            var pdf = $"{model.HTML}";
            using (var reader = new StreamReader(templatePath))
            {
                var template = reader.ReadToEnd();

                pdf = template.Replace("{{HTML}}", model.HTML).Replace("{{DIR}}", model.Dir).Replace("{{DOMAIN}}", AdminUrl);
            }
            var converter = new HtmlToPdfConverter();
            converter.PdfToolPath = path;
            var stream = new MemoryStream();
            converter.Grayscale = false;
            converter.Size = size; //NReco.PdfGenerator.PageSize.A4;
            converter.Orientation = orientation;//PageOrientation.Portrait;
            converter.GenerateToc = false;
            converter.PageWidth = pageWidth;
            converter.PageHeight = pageHeight;
            //pdf = File.ReadAllText(HttpContext.Current.Server.MapPath("~/temp/temp-pdf.html"), new UTF8Encoding());
            var data = converter.GeneratePdf(pdf);
            //System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/temp/temp-pdf.html"), pdf, new UTF8Encoding());
            return data;
        }
    }
}
