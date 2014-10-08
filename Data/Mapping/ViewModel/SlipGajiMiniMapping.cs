using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SlipGajiMiniMapping : EntityTypeConfiguration<SlipGajiMini>
    {
        public SlipGajiMiniMapping()
        {
            HasKey(s => s.Id);
            Ignore(s => s.Errors);
        }
    }

}