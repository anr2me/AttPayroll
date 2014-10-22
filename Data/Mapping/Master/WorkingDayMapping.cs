using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class WorkingDayMapping : EntityTypeConfiguration<WorkingDay>
    {
        public WorkingDayMapping()
        {
            HasKey(wd => wd.Id);
            HasOptional(wd => wd.WorkingTime)
                .WithMany(wt => wt.WorkingDays)
                .HasForeignKey(wd => wd.WorkingTimeId)
                .WillCascadeOnDelete(false);
            Ignore(wd => wd.Errors);
        }
    }
}