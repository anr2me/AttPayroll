using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeWorkingTimeMapping : EntityTypeConfiguration<EmployeeWorkingTime>
    {
        public EmployeeWorkingTimeMapping()
        {
            HasKey(ew => ew.Id);
            HasRequired(ew => ew.WorkingTime)
                .WithMany(wt => wt.EmployeeWorkingTimes)
                .HasForeignKey(ew => ew.WorkingTimeId)
                .WillCascadeOnDelete(false);
            HasRequired(ew => ew.Employee)
                .WithMany(e => e.EmployeeWorkingTimes)
                .HasForeignKey(ew => ew.EmployeeId)
                .WillCascadeOnDelete(true);
            Ignore(ew => ew.Errors);
        }
    }
}