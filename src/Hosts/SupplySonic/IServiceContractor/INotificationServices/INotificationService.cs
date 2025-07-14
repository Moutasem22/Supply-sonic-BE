using Core.Enums;
using DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor.INotificationServices
{
    public interface INotificationService
    {
        Task<bool> CreateEmailNotification(string Subject, string Message, int? AttachmentId, List<string> users = null, EnumPriority priority = EnumPriority.Normal);
        Task<bool> CreateWebNotification(string SubjectEn, string SubjectAr, string MessageEn, string MessageAr, List<int> users = null, string URL = "");
        Task<bool> SendEmailJob(EnumPriority priority);
        Task<bool> SendEmailJobHighPriority(EnumPriority priority);
    }
}
