using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class PTKPMapping : EntityTypeConfiguration<PTKP>
    {
        public PTKPMapping()
        {
            HasKey(p => p.Id);
            Ignore(p => p.Errors);
        }
    }
}