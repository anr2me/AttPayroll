using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class LastEducationMapping : EntityTypeConfiguration<LastEducation>
    {
        public LastEducationMapping()
        {
            HasKey(le => le.Id);
            HasRequired(le => le.Employee)
                .WithMany()
                .HasForeignKey(le => le.EmployeeId)
                .WillCascadeOnDelete(false);
            Ignore(le => le.Errors);
        }
    }
}