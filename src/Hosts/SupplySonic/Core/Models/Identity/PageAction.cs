using System;
using System.Collections.Generic;
using System.Text;
using Core.Models.Notifications;

namespace Core.Models.Identity
{
    public class PageAction:BaseEntity<int>
    {
        public int PageId { get; set; }
        public Page Page { get; set; }
        public int ActionId { get; set; }
        public Action Action { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<NotificationSetting> NotificationSettings { get; set; }
        public PageAction()
        {
            Permissions = new HashSet<Permission>();
            NotificationSettings = new HashSet<NotificationSetting>();
        }
    }
}
