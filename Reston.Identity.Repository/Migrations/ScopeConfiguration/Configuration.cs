namespace Reston.Identity.Repository.Migrations.ScopeConfiguration
{
    using Reston.Identity.Helper;
    using Reston.Identity.Repository.Id;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class Configuration : DbMigrationsConfiguration<IdentityServer3.EntityFramework.ScopeConfigurationDbContext>
    {
        private bool pendingMigrationsExist;
        public Configuration()
        {
            AutomaticMigrationsEnabled = IdLdapConstants.Id.RunSeeder;
            MigrationsDirectory = @"Migrations\ScopeConfiguration";
            ContextKey = "IdentityServer3.EntityFramework.ScopeConfigurationDbContext";

            var dbMigrator = new DbMigrator(this);
            pendingMigrationsExist = dbMigrator.GetPendingMigrations().Any();

            if (pendingMigrationsExist)
            {
                this.Seed(new IdentityServer3.EntityFramework.ScopeConfigurationDbContext(IdLdapConstants.Configuration.IdLdapConnectionString));
            }
        }

        protected override void Seed(IdentityServer3.EntityFramework.ScopeConfigurationDbContext context)
        {
            Task.Run(async () => { await Seeder.Seed(Scopes.Get(), context); }).Wait();
        }
    }
}
