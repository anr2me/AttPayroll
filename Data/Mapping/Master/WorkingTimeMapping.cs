using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WorkingTimeMapping : EntityTypeConfiguration<WorkingTime>
    {
        public WorkingTimeMapping()
        {
            HasKey(wt => wt.Id);
            Ignore(wt => wt.Errors);
        }
    }
}