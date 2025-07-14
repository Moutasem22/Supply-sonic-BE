using DTO.CommandDTO;
using DTO.NotificationsDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor.INotificationServices
{
    public interface IUserNotificationService
    {
        ResultViewModel<List<UserNotificationDTO>> GetAll(QueryViewModel<UserNotificationDTO> queryViewModel, int userId);
        bool ResetCounter(int userId);
        bool MakeNotifyRead(int userId, int UserNotificationId);
    }
}
