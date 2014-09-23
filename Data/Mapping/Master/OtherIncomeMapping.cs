using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class OtherIncomeMapping : EntityTypeConfiguration<OtherIncome>
    {
        public OtherIncomeMapping()
        {
            HasKey(oi => oi.Id);
            HasRequired(oi => oi.Employee)
                .WithMany()
                .HasForeignKey(oi => oi.EmployeeId)
                .WillCascadeOnDelete(false);
            HasRequired(oi => oi.SalaryItem)
                .WithMany()
                .HasForeignKey(oi => oi.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(oi => oi.Errors);
        }
    }
}