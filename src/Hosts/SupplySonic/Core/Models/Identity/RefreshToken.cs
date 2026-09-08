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
        public int? AppUserId { get; private set; }
        public string? RemoteIpAddress { get; private set; }
        public int? SupplierAppUserId { get; private set; }

        public RefreshToken()
        {

        }
        public RefreshToken(string token, DateTime expires, int? appUserId, string remoteIpAddress, int? type = 1)
        {
            Token = token;
            Expires = expires;
            RemoteIpAddress = remoteIpAddress;
            if (type == 1)
            {
                AppUserId = appUserId;
            }
            else if (type == 2)
            {
                SupplierAppUserId = appUserId;
               
            }
           
        }



    }
}
