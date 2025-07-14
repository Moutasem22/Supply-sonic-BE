using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using System.IO;

using MailKit.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using System.Drawing;


namespace Helpers
{
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostEnvironment _env;
        //private DBContext _dbcontext;
        //private readonly IBaseService baseService;

        public EmailSender(IOptions<EmailSettings> emailSettings, IHostEnvironment env
            //, IBaseService baseService
            )
        {
            _emailSettings = emailSettings.Value;
            _env = env;
          //  this.baseService = baseService;
           // _dbcontext = baseService.Context;   
        }

        //public async Task<bool> SendEmailAsync(string email, string subject, string message, Print_pdfDTO attachmentFile = null)
        //{            
        //    return await Execute(email, subject, message, attachmentFile);
        //}
        //public async Task<bool> SendEmailAsync(string subject, string message)
        //{
        //    string email = _emailSettings.ToEmail;
        //    return await Execute(email, subject, message);
        //}

        //private string ImageToBase64(string Path)
        //{
        //    using (Image image = Image.FromFile(Path))
        //    {
        //        using (MemoryStream m = new MemoryStream())
        //        {
        //            image.Save(m, image.RawFormat);
        //            byte[] imageBytes = m.ToArray();

        //            // Convert byte[] to Base64 String
        //            string base64String = Convert.ToBase64String(imageBytes);
        //            return base64String;
        //        }
        //    }
        //}

        //private async Task<bool> Execute(string email, string subject, string message, Print_pdfDTO attachmentFile = null)
        //{
        //    try
        //    {

        //        var tempFileStream = new FileStream(_env.ContentRootPath + "/wwwroot/EmailTemplate/TempEn.html", FileMode.Open, FileAccess.Read);
        //        string emailWithTemp = "";
        //        using (StreamReader reader = new StreamReader(tempFileStream))
        //        {
        //            emailWithTemp = reader.ReadToEnd();
        //        }



        //        var appearance = _dbcontext.AppearancSettings.FirstOrDefault();
        //        var logoAttachment = _dbcontext.Attachments.FirstOrDefault(x=> appearance != null && x.Id == appearance.LogoAttachmentId && x.IsActive && !x.IsDeleted);
        //        var logo = "";
        //        if (logoAttachment != null)
        //            logo = ImageToBase64(@"" + _env.ContentRootPath + "/wwwroot" + logoAttachment.Path);


        //        emailWithTemp = emailWithTemp.Replace("{{message}}", message).Replace("{{logo}}", logo);


        //        // Prepare an email message to be sent
        //        var mimeMessage = new MimeMessage();
        //        mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
        //        mimeMessage.To.Add(MailboxAddress.Parse(email));

        //        if (!string.IsNullOrWhiteSpace(_emailSettings.MailBBC))
        //        {
        //            mimeMessage.Bcc.Add(MailboxAddress.Parse(_emailSettings.MailBBC));
        //        }

        //        mimeMessage.Subject = subject;



        //        // Add email body and file attachments
        //        var bodyBuilder = new BodyBuilder
        //        {
        //            HtmlBody = emailWithTemp
        //        };
        //        mimeMessage.Body = bodyBuilder.ToMessageBody();

        //        if (attachmentFile != null && attachmentFile.Data != null && attachmentFile.FileName != null)

        //        {
        //            var builder = new BodyBuilder();
        //            builder.HtmlBody = emailWithTemp;
        //            //base46Attachment = GetBase64(base46Attachment);
        //            var bytes = attachmentFile.Data;
        //            //MimeEntity.Load(new ContentType("application", "pdf"), new MemoryStream(bytes));
        //            builder.Attachments.Add(attachmentFile.FileName, bytes);
        //            mimeMessage.Body = builder.ToMessageBody();
        //        }


        //        //   // Connect and authenticate with the SMTP server
        //      //  var smtpClient = new SmtpClient();
        //      //  await smtpClient.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, SecureSocketOptions.StartTls);
        //     //   await smtpClient.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

        //        // Send email message
        //       // await smtpClient.SendAsync(mimeMessage);
        //       // await smtpClient.DisconnectAsync(true);
        //      //  smtpClient.Dispose();
        //        return true;
        //    }
        //    catch
        //    {
        //        // TODO: handle exception
        //        //throw new InvalidOperationException(ex.Message);
        //    }
        //    return false;
        //}       
    }
}
