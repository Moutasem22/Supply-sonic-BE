using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFTransitionActivityConfiguration : IEntityTypeConfiguration<WFTransitionActivity>
    {
        public void Configure(EntityTypeBuilder<WFTransitionActivity> builder)
        {
            builder.HasMany<WFTransitionActivityNotification>(x => x.WFTransitionActivityNotifications)
               .WithOne(y => y.WFTransitionActivity).HasForeignKey(z => z.WFTransitionActivityId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
