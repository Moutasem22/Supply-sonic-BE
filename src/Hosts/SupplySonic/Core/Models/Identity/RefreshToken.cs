using System;
using System.Collections.Generic;
using System.Text;
using Core;
namespace Core.Models.Identity
{
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; private set; }
        public DateTime Expires { get; private set; }
        public int AppUserId { get; private set; }       
        public string? RemoteIpAddress { get; private set; }

        public RefreshToken(string token, DateTime expires, int appUserId, string remoteIpAddress)
        {
            Token = token;
            Expires = expires;
            AppUserId = appUserId;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
