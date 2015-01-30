using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class FPTemplateMapping : EntityTypeConfiguration<FPTemplate>
    {
        public FPTemplateMapping()
        {
            HasKey(f => f.Id);
            HasRequired(f => f.FPUser)
                .WithMany(u => u.FPTemplates)
                .HasForeignKey(f => f.FPUserId)
                .WillCascadeOnDelete(false);
            Ignore(f => f.Errors);
        }
    }
}