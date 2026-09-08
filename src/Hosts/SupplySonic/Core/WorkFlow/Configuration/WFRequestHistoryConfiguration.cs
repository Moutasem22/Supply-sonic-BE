using Core.WorkFlow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.WorkFlow.Configuration
{
    public class WFRequestHistoryConfiguration : IEntityTypeConfiguration<WFRequestHistory>
    {
        public void Configure(EntityTypeBuilder<WFRequestHistory> builder)
        {
            builder.HasMany<WFRejectReason>(x => x.WFRejectReasons)
              .WithOne(y => y.WFRequestHistory).HasForeignKey(z => z.WFRequestHistoryId).OnDelete(DeleteBehavior.NoAction);
           
        }
    }
}
