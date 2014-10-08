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
            HasRequired(f => f.SalaryItem)
                .WithMany()
                .HasForeignKey(f => f.SalaryItemId)
                .WillCascadeOnDelete(false);
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