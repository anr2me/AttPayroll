using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class FPUserMapping : EntityTypeConfiguration<FPUser>
    {
        public FPUserMapping()
        {
            HasKey(f => f.Id);
            HasOptional(f => f.Employee)
                .WithMany()
                .HasForeignKey(f => f.EmployeeId)
                .WillCascadeOnDelete(false);
            HasMany(f => f.FPTemplates);
            HasMany(f => f.FPAttLogs);
            Ignore(f => f.Errors);
        }
    }
}