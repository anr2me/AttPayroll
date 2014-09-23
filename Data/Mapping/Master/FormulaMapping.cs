using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class FormulaMapping : EntityTypeConfiguration<Formula>
    {
        public FormulaMapping()
        {
            HasKey(f => f.Id);
            Ignore(f => f.Errors);
        }
    }
}