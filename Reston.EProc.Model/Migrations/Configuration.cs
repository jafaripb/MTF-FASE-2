using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using Model.Helper;

namespace Reston.Pinata.Model.Migrations
{

    public class Configuration : DbMigrationsConfiguration<Reston.Pinata.Model.JimbisContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;// PengadaanConstants.RunSeeder;//PengadaanConstants.RunSeeder;
        }

        protected override void Seed(Reston.Pinata.Model.JimbisContext context)
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
           // Seeder.Seed(context);
        }
    }
}
