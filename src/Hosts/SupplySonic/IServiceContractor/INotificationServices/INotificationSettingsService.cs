using Core.Enums;
using DTO.CommandDTO;
using DTO.NotificationsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor.INotificationServices
{
    public interface INotificationSettingsService
    {
        Task<ResultViewModel<List<NotificationSettingResultDto>>> GetAll();
        Task<ResultViewModel<bool>> Update(List<NotificationSettingAddEditDto> notificationSettings);
        void sendActionNotification(EnumPageCode pageCode, EnumActionCode actionCode, int objId = 0);
    }
}
