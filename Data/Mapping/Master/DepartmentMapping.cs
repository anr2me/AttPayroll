using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class DepartmentMapping : EntityTypeConfiguration<Department>
    {
        public DepartmentMapping()
        {
            HasKey(d => d.Id);
            HasRequired(d => d.BranchOffice)
                .WithMany(ci => ci.Departments)
                .HasForeignKey(d => d.BranchOfficeId);
            Ignore(d => d.Errors);
        }
    }
}