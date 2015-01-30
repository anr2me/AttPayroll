using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class FPAttLogMapping : EntityTypeConfiguration<FPAttLog>
    {
        public FPAttLogMapping()
        {
            HasKey(f => f.Id);
            HasRequired(f => f.FPUser)
                .WithMany(u => u.FPAttLogs)
                .HasForeignKey(f => f.FPUserId)
                .WillCascadeOnDelete(false);
            Ignore(f => f.Errors);
        }
    }
}