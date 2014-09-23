using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class GeneralLeaveMapping : EntityTypeConfiguration<GeneralLeave>
    {
        public GeneralLeaveMapping()
        {
            HasKey(gl => gl.Id);
            Ignore(gl => gl.Errors);
        }
    }
}