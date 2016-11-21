namespace Reston.Identity.Repository.Migrations.OperationalConfiguration
{
    using Reston.Identity.Helper;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class Configuration : DbMigrationsConfiguration<IdentityServer3.EntityFramework.OperationalDbContext>
    {
        private bool pendingMigrationsExist;
        public Configuration()
        {
            AutomaticMigrationsEnabled = IdLdapConstants.Id.RunSeeder;
            MigrationsDirectory = @"Migrations\OperationalConfiguration";
            ContextKey = "IdentityServer3.EntityFramework.OperationalDbContext";

            var connectionString = new AppConfiguration().DefaultConnectionString;
            TargetDatabase = new DbConnectionInfo(connectionString.ConnectionString, "System.Data.SqlClient");
            var dbMigrator = new DbMigrator(this);
            dbMigrator.Configuration.TargetDatabase = new DbConnectionInfo(connectionString.ConnectionString, "System.Data.SqlClient");

            //// This is required to detect changes.
            //pendingMigrationsExist = dbMigrator.GetPendingMigrations().Any();

            //if (pendingMigrationsExist)
            //{
            //    dbMigrator.Update();
            //}
        }

        protected override void Seed(IdentityServer3.EntityFramework.OperationalDbContext context)
        {
            Task.Run(async () => { await Seeder.Seed(context); }).Wait();
        }
    }
}
