using System.Data.Entity.ModelConfiguration;
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Mapping
{
    public class UserAccountMapping : EntityTypeConfiguration<UserAccount>
    {
        public UserAccountMapping()
        {
            HasKey(ua => ua.Id);
            Ignore(ua => ua.Errors);
        }
    }
}