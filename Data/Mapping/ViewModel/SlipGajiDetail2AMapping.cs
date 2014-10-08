using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SlipGajiDetail2AMapping : EntityTypeConfiguration<SlipGajiDetail2A>
    {
        public SlipGajiDetail2AMapping()
        {
            HasKey(s => s.Id);
            HasRequired(s => s.SlipGajiDetail)
                .WithMany()
                .HasForeignKey(s => s.SlipGajiDetailId)
                .WillCascadeOnDelete(true);
            Ignore(s => s.Errors);
        }
    }
}