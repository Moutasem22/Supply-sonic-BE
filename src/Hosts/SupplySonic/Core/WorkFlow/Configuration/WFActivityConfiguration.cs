using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFActivityConfiguration : IEntityTypeConfiguration<WFActivity>
    {
        public void Configure(EntityTypeBuilder<WFActivity> builder)
        {
            builder.HasMany<WFStateActivity>(x => x.WFStateActivities)
              .WithOne(y => y.WFActivity).HasForeignKey(z => z.WFActivityId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<WFTransitionActivity>(x => x.WFTransitionActivities)
              .WithOne(y => y.WFActivity).HasForeignKey(x => x.WFActivityId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
