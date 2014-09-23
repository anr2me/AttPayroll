using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeLoanMapping : EntityTypeConfiguration<EmployeeLoan>
    {
        public EmployeeLoanMapping()
        {
            HasKey(el => el.Id);
            HasRequired(el => el.Employee)
                .WithMany()
                .HasForeignKey(el => el.EmployeeId)
                .WillCascadeOnDelete(false);
            HasRequired(el => el.SalaryItem)
                .WithMany()
                .HasForeignKey(el => el.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(el => el.Errors);
        }
    }
}