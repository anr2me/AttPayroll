using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalaryStandardDetailMapping : EntityTypeConfiguration<SalaryStandardDetail>
    {
        public SalaryStandardDetailMapping()
        {
            HasKey(ssd => ssd.Id);
            HasRequired(ssd => ssd.SalaryStandard)
                .WithMany(ss => ss.SalaryStandardDetails)
                .HasForeignKey(ssd => ssd.SalaryStandardId);
            HasRequired(ssd => ssd.SalaryItem)
                .WithMany()
                .HasForeignKey(ssd => ssd.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(ssd => ssd.Errors);
        }
    }
}