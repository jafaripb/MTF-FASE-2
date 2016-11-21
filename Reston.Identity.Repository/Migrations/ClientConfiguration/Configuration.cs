namespace Reston.Identity.Repository.Migrations.ClientConfiguration
{
    using Reston.Identity.Helper;
    using Reston.Identity.Repository.Id;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class Configuration : DbMigrationsConfiguration<IdentityServer3.EntityFramework.ClientConfigurationDbContext>
    {
        private bool pendingMigrationsExist;
        public Configuration()
        {
            AutomaticMigrationsEnabled = IdLdapConstants.Id.RunSeeder;
            MigrationsDirectory = @"Migrations\ClientConfiguration";
            ContextKey = "IdentityServer3.EntityFramework.ClientConfigurationDbContext";

            var dbMigrator = new DbMigrator(this);

            //This is required to detect changes.
           // pendingMigrationsExist = dbMigrator.GetPendingMigrations().Any();

            if (pendingMigrationsExist)
            {
                //dbMigrator.Update();
                this.Seed(new IdentityServer3.EntityFramework.ClientConfigurationDbContext(IdLdapConstants.Configuration.IdLdapConnectionString));
            }
        }

        protected override void Seed(IdentityServer3.EntityFramework.ClientConfigurationDbContext context)
        {
            Task.Run(async () => { await Seeder.Seed(Clients.Get(), context); }).Wait();
        }
    }
}
