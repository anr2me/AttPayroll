using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class EmployeeLoanDetailMapping : EntityTypeConfiguration<EmployeeLoanDetail>
    {
        public EmployeeLoanDetailMapping()
        {
            HasKey(eld => eld.Id);
            HasRequired(eld => eld.EmployeeLoan)
                .WithMany(el => el.EmployeeLoanDetails)
                .HasForeignKey(eld => eld.EmployeeLoanId);
            Ignore(eld => eld.Errors);
        }
    }
}