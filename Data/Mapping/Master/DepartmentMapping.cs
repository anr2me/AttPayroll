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
            HasRequired(d => d.CompanyInfo)
                .WithMany(ci => ci.Departments)
                .HasForeignKey(d => d.CompanyInfoId);
            Ignore(d => d.Errors);
        }
    }
}