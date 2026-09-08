using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFActionConfiguration : IEntityTypeConfiguration<WFAction>
    {
        public void Configure(EntityTypeBuilder<WFAction> builder)
        {
            builder.HasMany<WFTransitionAction>(x => x.WFTransitionActions)
              .WithOne(y => y.WFAction).HasForeignKey(z => z.WFActionId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Core.Models.Identity.Action>(x => x.Action)
              .WithOne(y => y.WFAction).HasForeignKey<WFAction>(x=>x.ActionId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<WFRequestAction>(x => x.WFRequestActions)
             .WithOne(y => y.WFAction).HasForeignKey(x => x.WFActionId).OnDelete(DeleteBehavior.NoAction);
            
            builder.HasMany<WFTransitionActivity>(x => x.WFTransitionActivities)
             .WithOne(y => y.PrevWFAction).HasForeignKey(x => x.PrevWFActionId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}


