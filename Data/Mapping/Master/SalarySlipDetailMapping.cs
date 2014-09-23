using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalarySlipDetailMapping : EntityTypeConfiguration<SalarySlipDetail>
    {
        public SalarySlipDetailMapping()
        {
            HasKey(ssd => ssd.Id);
            HasRequired(ssd => ssd.SalarySlip)
                .WithMany(ss => ss.SalarySlipDetails)
                .HasForeignKey(ssd => ssd.SalarySlipId);
            HasRequired(ssd => ssd.SalaryItem)
                .WithMany()
                .HasForeignKey(ssd => ssd.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(ssd => ssd.Errors);
        }
    }
}