using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFRequestConfiguration : IEntityTypeConfiguration<WFRequest>
    {
        public void Configure(EntityTypeBuilder<WFRequest> builder)
        {
            builder.HasMany<WFRequestAction>(x => x.WFRequestActions)
              .WithOne(y => y.WFRequest).HasForeignKey(z => z.WFRequestId).OnDelete(DeleteBehavior.NoAction);   
            builder.HasMany<WFRequestHistory>(x => x.WFRequestHistories)
              .WithOne(y => y.WFRequest).HasForeignKey(z => z.WFRequestId).OnDelete(DeleteBehavior.NoAction);          

            builder.HasOne<WFRequestPosition>(x => x.WFRequestPosition)
              .WithOne(y => y.WFRequest).HasForeignKey<WFRequestPosition>(z => z.WFRequestId).OnDelete(DeleteBehavior.NoAction);    
        }
    }
}
