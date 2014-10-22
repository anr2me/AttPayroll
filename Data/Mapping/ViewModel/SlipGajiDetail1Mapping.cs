using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SlipGajiDetail1Mapping : EntityTypeConfiguration<SlipGajiDetail1>
    {
        public SlipGajiDetail1Mapping()
        {
            HasKey(s => s.Id);
            HasRequired(s => s.SlipGajiDetail)
                .WithMany(s => s.SlipGajiDetail1s)
                .HasForeignKey(s => s.SlipGajiDetailId)
                .WillCascadeOnDelete(true);
            Ignore(s => s.Errors);
        }
    }

}