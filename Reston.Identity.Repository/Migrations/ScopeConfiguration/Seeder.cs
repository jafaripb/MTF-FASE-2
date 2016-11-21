using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Reston.Identity.Repository.Migrations.ScopeConfiguration
{
    public class Seeder
    {
        public static async Task Seed(IEnumerable<IdentityServer3.Core.Models.Scope> scopes, IdentityServer3.EntityFramework.ScopeConfigurationDbContext context)
        {
            if (!context.Scopes.Any())
            {
                foreach (var s in scopes)
                {
                    var e = s.ToEntity();
                    context.Scopes.Add(e);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
