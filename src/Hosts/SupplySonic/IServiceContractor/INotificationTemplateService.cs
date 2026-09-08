using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor
{
    public interface INotificationTemplateService
    {
        ResultViewModel<List<NotificationTemplateResultDto>> GetAll(QueryViewModel<NotificationTemplateResultDto> queryViewModel);
        ResultViewModel<NotificationTemplateResultDto> Getone(int id);
        ResultViewModel<NotificationTemplateResultDto> Update(NotificationTemplateAddEditDto notificationTemplateDto);
    }
}
