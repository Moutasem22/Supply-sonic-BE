
using Core.Models;
using DB;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Core.Models.Notifications;
using DTO.IdentityDTO;
using DTO.CommandDTO;
using IServiceContractor.IdentityInterFaces;

namespace Service.IdentityServices
{
    public class UserConnectionService : IUserConnectionService
    {

        private DBContext _dbcontext;

        public UserConnectionService(DBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ResultViewModel<UserConnectionDTO> CreateUserConnection(int userId, string ConnectId)
        {
            ResultViewModel<UserConnectionDTO> _ResultViewModel = new ResultViewModel<UserConnectionDTO>();
            var existUserConnection = _dbcontext.UserConnections.FirstOrDefault(x => x.AppUserId == userId && x.ConnectionId == ConnectId);

            if (existUserConnection != null)
            {
                return _ResultViewModel;
            }
            var userConnection = new UserConnection(userId, ConnectId);
            _dbcontext.UserConnections.Add(userConnection);
            _dbcontext.SaveChanges();

            _ResultViewModel.Data = userConnection.Adapt<UserConnectionDTO>();
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;

        }

        public ResultViewModel<IEnumerable<UserConnectionDTO>> GetAllByNotificationId(int NotificationID)
        {
            ResultViewModel<IEnumerable<UserConnectionDTO>> _ResultViewModelList = new ResultViewModel<IEnumerable<UserConnectionDTO>>();
            var whereOrder = ("where UserNotifications.NotificationId=" + NotificationID).Replace("'", "''");
            whereOrder = "N' " + whereOrder + " '";

            var q = _dbcontext.UserConnections.FromSqlRaw($"sp_UserConnection {whereOrder}");

            _ResultViewModelList.Data = q.Adapt<IEnumerable<UserConnectionDTO>>();

            return _ResultViewModelList;
        }

        public UserConnectionDTO GetUserConnection(int userId, string ConnectId)
        {
            var userConnection = _dbcontext.UserConnections.FirstOrDefault(x => x.AppUserId == userId && x.ConnectionId == ConnectId);

            return userConnection.Adapt<UserConnectionDTO>();
        }

        public bool RemoveUserConnection(int userId, string ConnectId)
        {
            ResultViewModel<UserConnectionDTO> _ResultViewModel = new ResultViewModel<UserConnectionDTO>();
            var userconnects = _dbcontext.UserConnections.Where(x => x.AppUserId == userId && x.ConnectionId == ConnectId);
            _dbcontext.UserConnections.RemoveRange(userconnects);
            _dbcontext.SaveChanges();
            return true;
        }

        public ResultViewModel<UserConnectionDTO> UpdateUserConnection(int userId, string ConnectId)
        {
            ResultViewModel<UserConnectionDTO> _ResultViewModel = new ResultViewModel<UserConnectionDTO>();
            var userConnection = _dbcontext.UserConnections.FirstOrDefault(x => x.AppUserId == userId);

            if (userConnection == null)
            {
                return _ResultViewModel;
            }
            userConnection.Update(userId, ConnectId);
            _dbcontext.UserConnections.Update(userConnection);
            _dbcontext.SaveChanges();

            _ResultViewModel.Data = userConnection.Adapt<UserConnectionDTO>();
            _ResultViewModel.IsSuccess = true;
            return _ResultViewModel;
        }
    }
}
