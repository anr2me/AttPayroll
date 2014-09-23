using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class SalaryEmployeeDetailMapping : EntityTypeConfiguration<SalaryEmployeeDetail>
    {
        public SalaryEmployeeDetailMapping()
        {
            HasKey(sed => sed.Id);
            HasRequired(sed => sed.SalaryEmployee)
                .WithMany(ss => ss.SalaryEmployeeDetails)
                .HasForeignKey(sed => sed.SalaryEmployeeId);
            HasRequired(sed => sed.SalaryItem)
                .WithMany()
                .HasForeignKey(sed => sed.SalaryItemId)
                .WillCascadeOnDelete(false);
            Ignore(sed => sed.Errors);
        }
    }
}