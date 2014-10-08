using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SPKLMapping : EntityTypeConfiguration<SPKL>
    {
        public SPKLMapping()
        {
            HasKey(el => el.Id);
            HasRequired(el => el.Employee)
                .WithMany()
                .HasForeignKey(el => el.EmployeeId)
                .WillCascadeOnDelete(false);
            Ignore(el => el.Errors);
        }
    }
}