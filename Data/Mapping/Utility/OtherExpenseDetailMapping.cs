using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class OtherExpenseDetailMapping : EntityTypeConfiguration<OtherExpenseDetail>
    {
        public OtherExpenseDetailMapping()
        {
            HasKey(oed => oed.Id);
            HasRequired(oed => oed.Employee)
                .WithMany()
                .HasForeignKey(oed => oed.EmployeeId)
                .WillCascadeOnDelete(false);
            HasRequired(oed => oed.OtherExpense)
                .WithMany(oe => oe.OtherExpenseDetails)
                .HasForeignKey(oed => oed.OtherExpenseId)
                .WillCascadeOnDelete(false);
            Ignore(oed => oed.Errors);
        }
    }
}