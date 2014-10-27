using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class THRDetailMapping : EntityTypeConfiguration<THRDetail>
    {
        public THRDetailMapping()
        {
            HasKey(td => td.Id);
            HasRequired(td => td.Employee)
                .WithMany()
                .HasForeignKey(td => td.EmployeeId)
                .WillCascadeOnDelete(false);
            HasRequired(td => td.THR)
                .WithMany(t => t.THRDetails)
                .HasForeignKey(td => td.THRId)
                .WillCascadeOnDelete(false);
            Ignore(td => td.Errors);
        }
    }
}