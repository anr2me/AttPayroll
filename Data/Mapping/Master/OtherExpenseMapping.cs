using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class OtherExpenseMapping : EntityTypeConfiguration<OtherExpense>
    {
        public OtherExpenseMapping()
        {
            HasKey(oe => oe.Id);
            HasRequired(oe => oe.Employee)
                .WithMany()
                .HasForeignKey(oe => oe.EmployeeId)
                .WillCascadeOnDelete(false);
            HasRequired(oe => oe.SalaryItem)
                .WithMany()
                .HasForeignKey(oe => oe.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(oe => oe.Errors);
        }
    }
}