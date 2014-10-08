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
            HasOptional(s => s.SlipGajiDetail1)
                .WithMany()
                .HasForeignKey(s => s.SlipGajiDetail1Id);
            HasOptional(s => s.SlipGajiDetail2A)
                .WithMany()
                .HasForeignKey(s => s.SlipGajiDetail2AId);
            Ignore(s => s.Errors);
        }
    }

}