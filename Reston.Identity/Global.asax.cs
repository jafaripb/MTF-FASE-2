using Reston.Identity.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IdLdap
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (IdLdapConstants.Id.RunSeeder)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<IdentityServer3.EntityFramework.ClientConfigurationDbContext, Reston.Identity.Repository.Migrations.ClientConfiguration.Configuration>());
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<IdentityServer3.EntityFramework.OperationalDbContext, Reston.Identity.Repository.Migrations.OperationalConfiguration.Configuration>());
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<IdentityServer3.EntityFramework.ScopeConfigurationDbContext, Reston.Identity.Repository.Migrations.ScopeConfiguration.Configuration>());
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<Reston.Identity.Repository.Identity.IdentityContext, Reston.Identity.Repository.Migrations.IdentityConfiguration.Configuration>());
            }


        }
    }
}
