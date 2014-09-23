using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeMapping : EntityTypeConfiguration<Employee>
    {
        public EmployeeMapping()
        {
            HasKey(e => e.Id);
            HasRequired(e => e.Division)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DivisionId);
            HasRequired(e => e.TitleInfo)
                .WithMany()
                .HasForeignKey(e => e.TitleInfoId)
                .WillCascadeOnDelete(false);
            HasRequired(e => e.LastEducation)
                .WithMany()
                .HasForeignKey(e => e.LastEducationId)
                .WillCascadeOnDelete(false);
            HasOptional(e => e.LastEmployment)
                .WithMany()
                .HasForeignKey(e => e.LastEmploymentId)
                .WillCascadeOnDelete(false);
            Ignore(e => e.Errors);
        }
    }
}