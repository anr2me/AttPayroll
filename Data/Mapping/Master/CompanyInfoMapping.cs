using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class CompanyInfoMapping : EntityTypeConfiguration<CompanyInfo>
    {
        public CompanyInfoMapping()
        {
            HasKey(ci => ci.Id);
            Ignore(ci => ci.Errors);
        }
    }
}