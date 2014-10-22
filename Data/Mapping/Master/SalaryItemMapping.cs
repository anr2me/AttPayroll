using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalaryItemMapping : EntityTypeConfiguration<SalaryItem>
    {
        public SalaryItemMapping()
        {
            HasKey(si => si.Id);
            //HasOptional(si => si.Formula)
            //    .WithMany()
            //    .HasForeignKey(si => si.FormulaId)
            //    .WillCascadeOnDelete(false);
            Ignore(si => si.Errors);
        }
    }
}