using Core.Enums;
using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Notifications
{
    public class UserNotification : BaseEntity<long>
    {
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public string Email { get; set; }
        public long NotificationId { get; set; }
        public Notification Notification { get; set; }
        public EnumNotificationState NotificationState { get; set; }
        public DateTime? ReadDate { get; set; }
        //for email
        public bool IsSent { get; set; }
        public int RetryCount { get; set; }
        public void Update(bool issent, int retryCount)
        {
            IsSent = issent;
            RetryCount = retryCount;
        }
    }
}
