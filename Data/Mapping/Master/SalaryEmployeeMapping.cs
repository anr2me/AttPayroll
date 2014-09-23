using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalaryEmployeeMapping : EntityTypeConfiguration<SalaryEmployee>
    {
        public SalaryEmployeeMapping()
        {
            HasKey(se => se.Id);
            HasRequired(se => se.Employee)
                .WithMany()
                .HasForeignKey(se => se.EmployeeId)
                .WillCascadeOnDelete(false);
            Ignore(se => se.Errors);
        }
    }
}