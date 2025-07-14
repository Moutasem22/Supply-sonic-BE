using DB;
using Helpers;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class EmailSenderService
    {
        private DBContext _dbcontext;
        private readonly EmailSettings _emailSettings;

        public EmailSenderService(DBContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        //public async Task<bool> SendEmailAsync(string email, string subject, string message)
        //{
        //    return await Execute(email, subject, message);
        //}

        //private async Task<bool> Execute(string email, string subject, string message, Print_pdfDTO printpdf = null)
        //{
        //    // Prepare an email message to be sent
        //    var mimeMessage = new MimeMessage();
        //    mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
        //    mimeMessage.To.Add(new MailboxAddress(email));
        //    //mimeMessage.To.Add(new MailboxAddress("msayed@futureface.sa"));

        //    if (!string.IsNullOrWhiteSpace(_emailSettings.MailBBC))
        //    {
        //        mimeMessage.Bcc.Add(new MailboxAddress(_emailSettings.MailBBC));
        //    }

        //    mimeMessage.Subject = subject;



        //    // Add email body and file attachments
        //    var bodyBuilder = new BodyBuilder
        //    {
        //        HtmlBody = message
        //    };
        //    mimeMessage.Body = bodyBuilder.ToMessageBody();

        //    if (printpdf != null)
        //    {
        //        var builder = new BodyBuilder();
        //        builder.HtmlBody = message;
        //        //base46Attachment = GetBase64(base46Attachment);
        //        var bytes = printpdf.Data;
        //        //MimeEntity.Load(new ContentType("application", "pdf"), new MemoryStream(bytes));
        //        builder.Attachments.Add(printpdf.FileName + ".pdf", bytes, new ContentType("application", "pdf"));
        //        mimeMessage.Body = builder.ToMessageBody();
        //    }


        //    //   // Connect and authenticate with the SMTP server
        //    var smtpClient = new SmtpClient();
        //    await smtpClient.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, MailKit.Security.SecureSocketOptions.StartTls);
        //    await smtpClient.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

        //    // Send email message
        //    await smtpClient.SendAsync(mimeMessage);
        //    await smtpClient.DisconnectAsync(true);
        //    smtpClient.Dispose();
        //    return true;
        //    return false;
        //}

    }
}
