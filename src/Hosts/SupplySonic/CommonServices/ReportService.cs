using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using DB;
using DTO;
using Helpers;
using IServiceContractor.ICommonService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Service;

public class ReportService
{
    private DBContext _dbcontext;
    private readonly IHostEnvironment _env;
    private readonly IBaseService _baseService;
    public ReportService(IHostEnvironment env, IBaseService baseService)
    {
        _baseService = baseService;
        _dbcontext = _baseService.Context;
        _env = env;
    }
    public async Task<ReportResult> DownloadReport<T>(T dto, string ReportName, RenderType ReportrType, string DataSetName = "DataSet1", Dictionary<string, string> Params = null)
    {
        try
        {

            string Path = _env.ContentRootPath + "/wwwroot/Reports/" + ReportName + ".rdlc";
            LocalReport report = new LocalReport(Path);
            report.AddDataSource(DataSetName, dto);

            //Get Logo from AppearancSettings Table 
            var logoImage = "";
         
            var appearance = _dbcontext.AppearancSettings.FirstOrDefault();
            var logoAttachment = _dbcontext.Attachments.FirstOrDefault(x => appearance != null && x.Id == appearance.LogoAttachmentId && x.IsActive && !x.IsDeleted);

            Dictionary<string, string> param = new Dictionary<string, string>();
            if (logoAttachment != null)
            {
                logoImage = _env.ContentRootPath + "/wwwroot" + logoAttachment.Path;

                if (logoAttachment.Path != null || logoAttachment.Path != "")
                {
                    //Convert Logo() to Base 64 
                    using var b = new Bitmap(logoImage);
                    using MemoryStream stream = new MemoryStream();
                    b.Save(stream, ImageFormat.Bmp);
                    logoImage = Convert.ToBase64String(stream.ToArray());
                }
                param.Add("logo", logoImage);
            }
            else
            {
                param.Add("logo", "");
            }

            if (Params != null)
            {

                foreach (var par in Params)
                {
                    param.Add(par.Key, par.Value);
                }
            }

            var reportResult = report.Execute(ReportrType, 1, param);
            return reportResult;
        }
        catch (BusinessException ex)
        {
            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


}
