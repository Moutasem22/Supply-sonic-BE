using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserConnection:BaseEntity<long>
    {
        public int AppUserId { get; private set; }
        public string ConnectionId { get; private set; }
        public UserConnection(int appUserId, string connectionId)
        {
            AppUserId = appUserId;
            ConnectionId = connectionId;
        }
        public void Update(int appUserId, string connectionId)
        {
            AppUserId = appUserId;
            ConnectionId = connectionId;
        }

    }
}
