using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class OtherIncomeDetailMapping : EntityTypeConfiguration<OtherIncomeDetail>
    {
        public OtherIncomeDetailMapping()
        {
            HasKey(oid => oid.Id);
            HasRequired(oid => oid.Employee)
                .WithMany()
                .HasForeignKey(oid => oid.EmployeeId)
                .WillCascadeOnDelete(false);
            HasRequired(oid => oid.OtherIncome)
                .WithMany(oi => oi.OtherIncomeDetails)
                .HasForeignKey(oid => oid.OtherIncomeId)
                .WillCascadeOnDelete(false);
            Ignore(oid => oid.Errors);
        }
    }
}