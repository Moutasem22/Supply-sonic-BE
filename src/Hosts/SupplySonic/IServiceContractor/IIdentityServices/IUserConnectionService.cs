using DTO.CommandDTO;
using DTO.IdentityDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace IServiceContractor.IdentityInterFaces
{
    public interface IUserConnectionService
    {
        ResultViewModel<IEnumerable<UserConnectionDTO>> GetAllByNotificationId(int NotificationID);
        ResultViewModel<UserConnectionDTO> CreateUserConnection(int userId, string ConnectId);
        bool RemoveUserConnection(int userId, string ConnectId);
        ResultViewModel<UserConnectionDTO> UpdateUserConnection(int userId, string ConnectId);
        UserConnectionDTO GetUserConnection(int userId, string ConnectId);
    }
}
