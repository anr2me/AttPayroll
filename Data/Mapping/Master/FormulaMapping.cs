using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class FormulaMapping : EntityTypeConfiguration<Formula>
    {
        public FormulaMapping()
        {
            HasKey(f => f.Id);
            HasOptional(f => f.SalarySlipDetail) // SalaryItem
                .WithMany()
                .HasForeignKey(f => f.SalarySlipDetailId) // SalaryItemId
                .WillCascadeOnDelete(true);
            HasRequired(f => f.FirstSalaryItem)
                .WithMany()
                .HasForeignKey(f => f.FirstSalaryItemId)
                .WillCascadeOnDelete(false);
            HasOptional(f => f.SecondSalaryItem)
                .WithMany()
                .HasForeignKey(f => f.SecondSalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(f => f.Errors);
        }
    }
}