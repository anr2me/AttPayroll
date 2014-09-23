using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PensionCompensationMapping : EntityTypeConfiguration<PensionCompensation>
    {
        public PensionCompensationMapping()
        {
            HasKey(pc => pc.Id);
            HasRequired(pc => pc.Employee)
                .WithMany()
                .HasForeignKey(pc => pc.EmployeeId)
                .WillCascadeOnDelete(false);
            Ignore(pc => pc.Errors);
        }
    }
}