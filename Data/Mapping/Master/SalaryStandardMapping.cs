using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalaryStandardMapping : EntityTypeConfiguration<SalaryStandard>
    {
        public SalaryStandardMapping()
        {
            HasKey(ss => ss.Id);
            HasRequired(ss => ss.TitleInfo)
                .WithMany()
                .HasForeignKey(ss => ss.TitleInfoId)
                .WillCascadeOnDelete(false);
            Ignore(ss => ss.Errors);
        }
    }
}