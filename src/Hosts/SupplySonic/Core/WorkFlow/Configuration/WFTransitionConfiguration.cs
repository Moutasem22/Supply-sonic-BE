using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFTransitionConfiguration : IEntityTypeConfiguration<WFTransition>
    {
        public void Configure(EntityTypeBuilder<WFTransition> builder)
        {
            builder.HasMany<WFTransitionActivity>(x => x.WFTransitionActivities)
               .WithOne(y => y.WFTransition).HasForeignKey(z => z.WFTransitionId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<WFTransitionAction>(x => x.WFTransitionActions)
              .WithOne(y => y.WFTransition).HasForeignKey(z => z.WFTransitionId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<WFRequestAction>(x => x.WFRequestActions)
              .WithOne(y => y.WFTransition).HasForeignKey(z => z.WFTransitionId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}


