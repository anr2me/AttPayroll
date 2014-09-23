using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class TitleInfoMapping : EntityTypeConfiguration<TitleInfo>
    {
        public TitleInfoMapping()
        {
            HasKey(ti => ti.Id);
            Ignore(ti => ti.Errors);
        }
    }
}