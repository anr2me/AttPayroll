using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeAttendanceDetailMapping : EntityTypeConfiguration<EmployeeAttendanceDetail>
    {
        public EmployeeAttendanceDetailMapping()
        {
            HasKey(sed => sed.Id);
            HasRequired(sed => sed.EmployeeAttendance)
                .WithMany(ss => ss.EmployeeAttendanceDetails)
                .HasForeignKey(sed => sed.EmployeeAttendanceId);
            HasRequired(sed => sed.SalaryItem)
                .WithMany()
                .HasForeignKey(sed => sed.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(sed => sed.Errors);
        }
    }
}