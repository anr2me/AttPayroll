namespace Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Core.DomainModel;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.Context.AttPayrollEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Data.Context.AttPayrollEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            int? compId = null;
            CompanyInfo comp = context.CompanyInfos.FirstOrDefault();
            if (comp != null) compId = comp.Id;
            foreach (var x in context.BranchOffices)
            {
                x.CompanyInfoId = compId;
            }
            //context.SaveChanges();
        }
    }
}
