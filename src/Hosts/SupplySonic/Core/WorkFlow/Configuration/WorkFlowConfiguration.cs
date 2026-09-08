using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WorkFlowConfiguration : IEntityTypeConfiguration<WorkFlow>
    {
        public void Configure(EntityTypeBuilder<WorkFlow> builder)
        {
            builder.HasMany<WFTransition>(x => x.WFTransitions)
               .WithOne(y => y.WorkFlow).HasForeignKey(z => z.WorkFlowId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany<WFRequest>(x => x.WFRequests)
              .WithOne(y => y.WorkFlow).HasForeignKey(z => z.WorkFlowId).OnDelete(DeleteBehavior.Cascade);
            
        }        
    }
}

