using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SlipGajiDetailMapping : EntityTypeConfiguration<SlipGajiDetail>
    {
        public SlipGajiDetailMapping()
        {
            HasKey(s => s.Id);
            //HasOptional(s => s.SlipGajiDetail2A)
            //    .WithMany()
            //    .HasForeignKey(s => s.SlipGajiDetail2AId)
            //    .WillCascadeOnDelete(true);
            HasRequired(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .WillCascadeOnDelete(false);
            HasMany(s => s.SlipGajiDetail2As);
            HasMany(s => s.SlipGajiDetail1s);
            Ignore(s => s.Errors);
        }
    }

}