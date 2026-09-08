using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor
{
    public interface IUserNotificationService
    {
        ResultViewModel<List<UserNotificationDTO>> GetAll(QueryViewModel<UserNotificationDTO> queryViewModel,int userId);
        bool ResetCounter(int userId);
        bool MakeNotifyRead(int userId, int UserNotificationId);
        ResultViewModel<List<UserNotificationDTO>> GetAllNotification(QueryViewModel<UserNotificationDTO> queryViewModel);
        
    }
}
