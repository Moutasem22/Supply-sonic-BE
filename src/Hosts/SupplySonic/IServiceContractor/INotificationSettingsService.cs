using Core.Enums;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServiceContractor
{
    public interface INotificationSettingsService
    {
        Task<ResultViewModel<List<NotificationSettingResultDto>>> GetAll();
        Task<ResultViewModel<bool>> Update(List<NotificationSettingAddEditDto> notificationSettings);
        void sendActionNotification(EnumPageCode pageCode, EnumActionCode actionCode, int objId = 0);
    }
}
