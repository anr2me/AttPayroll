using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class DivisionMapping : EntityTypeConfiguration<Division>
    {
        public DivisionMapping()
        {
            HasKey(d => d.Id);
            HasRequired(d => d.Department)
                .WithMany(x => x.Divisions)
                .HasForeignKey(d => d.DepartmentId);
            Ignore(d => d.Errors);
        }
    }
}