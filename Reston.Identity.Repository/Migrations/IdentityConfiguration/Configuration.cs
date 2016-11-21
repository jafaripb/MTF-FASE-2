namespace Reston.Identity.Repository.Migrations.IdentityConfiguration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Reston.Identity.Helper;
    using System.Threading.Tasks;

    public class Configuration : DbMigrationsConfiguration<Reston.Identity.Repository.Identity.IdentityContext>
    {
        private bool pendingMigrationsExist;
        public Configuration()
        {
            
            AutomaticMigrationsEnabled = IdLdapConstants.Id.RunSeeder;
            MigrationsDirectory = @"Migrations\IdentityConfiguration";
            var dbMigrator = new DbMigrator(this);
            //This is required to detect changes.
            //pendingMigrationsExist = dbMigrator.GetPendingMigrations().Any();

            //if (pendingMigrationsExist)
            //{
            //    dbMigrator.Update();
            //    //this.Seed(new IdSrv.Repository.AspId.Context("EOEntities"));
            //}
        }

        protected override void Seed(Reston.Identity.Repository.Identity.IdentityContext context)
        {
            
            Task.Run(async () => { await Seeder.Seed(context); }).Wait();
        }
    }
}
