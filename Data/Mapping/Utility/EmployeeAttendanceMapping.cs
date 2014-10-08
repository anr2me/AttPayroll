using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeAttendanceMapping : EntityTypeConfiguration<EmployeeAttendance>
    {
        public EmployeeAttendanceMapping()
        {
            HasKey(ma => ma.Id);
            HasRequired(ma => ma.Employee)
                .WithMany()
                .HasForeignKey(ma => ma.EmployeeId)
                .WillCascadeOnDelete(false);
            Ignore(ma => ma.Errors);
        }
    }
}