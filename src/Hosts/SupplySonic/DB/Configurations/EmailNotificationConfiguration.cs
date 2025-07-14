using Core.Models.Identity;
using Core.Models.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DB.Configurations
{
    public class EmailNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        { 
            builder.HasOne<Notification>(x => x.Notification)
               .WithMany(y => y.UserNotifications).HasForeignKey(z => z.NotificationId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
