using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class FPMachineMapping : EntityTypeConfiguration<FPMachine>
    {
        public FPMachineMapping()
        {
            HasKey(f => f.Id);
            HasRequired(f => f.CompanyInfo)
                .WithMany()
                .HasForeignKey(f => f.CompanyInfoId)
                .WillCascadeOnDelete(false);
            //HasMany(f => f.FPAttLogs);
            Ignore(f => f.Errors);
            //Ignore(f => f.fpDevice);
            Ignore(f => f.IsConnected);
            Ignore(f => f.IsInSync);
        }
    }
}