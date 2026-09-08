using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFStateConfiguration : IEntityTypeConfiguration<WFState>
    {
        public void Configure(EntityTypeBuilder<WFState> builder)
        {
            builder.HasMany<WFTransition>(x => x.WFCurrentTransitions)
              .WithOne(y => y.WFCurrentState).HasForeignKey(z => z.WFCurrentStateId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<WFTransition>(x => x.WFNextTransitions)
              .WithOne(y => y.WFNextState).HasForeignKey(z => z.WFNextStateId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<WFStateActivity>(x => x.WFStateActivity)
              .WithOne(y => y.WFState).HasForeignKey(z => z.WFStateId).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<WFStateAssignedUser>(x => x.WFStateAssignedUsers)
              .WithOne(y => y.WFState).HasForeignKey(z => z.WFStateId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
