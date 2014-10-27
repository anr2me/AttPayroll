using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class THRMapping : EntityTypeConfiguration<THR>
    {
        public THRMapping()
        {
            HasKey(t => t.Id);
            HasOptional(t => t.SalaryItem)
                .WithMany()
                .HasForeignKey(t => t.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(t => t.Errors);
        }
    }
}