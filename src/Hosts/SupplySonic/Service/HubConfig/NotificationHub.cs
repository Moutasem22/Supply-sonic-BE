using IServiceContractor;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.HubConfig
{
    public class NotificationHub : Hub
    {
        private IUserConnectionService _userConnectionService;
        public NotificationHub(IUserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }
        public override Task OnConnectedAsync()
        {
            var connectionid = Context.ConnectionId;
            if (Context.User.FindFirst(x => x.Type == "UserId") != null)
            {
                var userId = int.Parse(Context.User.FindFirst(x => x.Type == "UserId").Value);                
                if (_userConnectionService.GetUserConnection(userId, connectionid) != null)
                {
                    _userConnectionService.UpdateUserConnection(userId, connectionid);
                }
                else
                {
                    _userConnectionService.CreateUserConnection(userId, connectionid);

                }
            }           
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionid = Context.ConnectionId;

            if (Context.User.FindFirst(x => x.Type == "UserId") != null)
            {
                var userId = int.Parse(Context.User.FindFirst(x => x.Type == "UserId").Value);
                
                if (_userConnectionService.GetUserConnection(userId, connectionid) != null)
                {
                    _userConnectionService.RemoveUserConnection(userId, connectionid);
                }               
            }

            return base.OnDisconnectedAsync(exception);
        }     

    }
}
