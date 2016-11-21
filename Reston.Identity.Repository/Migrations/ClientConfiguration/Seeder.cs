using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;

namespace Reston.Identity.Repository.Migrations.ClientConfiguration
{
    public class Seeder
    {
        public static async Task Seed(IEnumerable<IdentityServer3.Core.Models.Client> clients, IdentityServer3.EntityFramework.ClientConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var c in clients)
                {
                    var e = c.ToEntity();
                    context.Clients.Add(e);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
