using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalarySlipMapping : EntityTypeConfiguration<SalarySlip>
    {
        public SalarySlipMapping()
        {
            HasKey(ss => ss.Id);
            HasOptional(ss => ss.Formula)
                .WithMany()
                .HasForeignKey(ss => ss.FormulaId)
                .WillCascadeOnDelete(false);
            Ignore(ss => ss.Errors);
        }
    }
}