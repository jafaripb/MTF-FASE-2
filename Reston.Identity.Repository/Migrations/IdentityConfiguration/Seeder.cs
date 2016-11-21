using Reston.Identity.Helper;
using Reston.Identity.Repository.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Reston.Identity.Repository.Migrations.IdentityConfiguration
{
    public class Seeder
    {

        static UserManager userManager;
        static RoleManager roleManager;
        static IdentityManagerService identityManagerService;

        public static async Task Seed(Identity.IdentityContext context)
        {

            userManager = new UserManager(new UserStore(context));
            roleManager = new RoleManager(new RoleStore(context));
            identityManagerService = new IdentityManagerService(userManager, roleManager);

            List<Role> roles = new List<Role>() { 
                new Role { Name = IdLdapConstants.AppConfiguration.IdLdapAdminRole},
                new Role { Name = IdLdapConstants.AppConfiguration.IdLdapUserRole},
                new Role { Name = IdLdapConstants.App.Roles.pRole_procurement_admin},
                new Role { Name = IdLdapConstants.App.Roles.pRole_procurement_head},
                new Role { Name = IdLdapConstants.App.Roles.pRole_procurement_manager},
                new Role { Name = IdLdapConstants.App.Roles.pRole_procurement_user},
                new Role { Name = IdLdapConstants.App.Roles.pRole_procurement_vendor},
                new Role { Name = IdLdapConstants.App.Roles.pRole_procurement_end_user},
                new Role { Name = IdLdapConstants.App.Roles.pRole_compliance}
            };

            

            if (!context.Roles.Any())
            {
                foreach (var c in roles)
                {
                    var roleresult = await roleManager.CreateAsync(c);
                }
                await context.SaveChangesAsync();
            }

            await CreateUser(IdLdapConstants.AppConfiguration.IdLdapAdminUsername,
                IdLdapConstants.AppConfiguration.IdLdapAdminPassword,
                new Guid(IdLdapConstants.AppConfiguration.AdminSubject),
                IdLdapConstants.AppConfiguration.IdLdapAdminRole);
        }

        private static async Task<User> CreateUser(string username, string password, Guid guid,  params string[] roles)
        {
            var newUser = new User()
            {
                IsLdapUser = false,
                UserName = username,
                Email = username + "@.com",
                Id = guid.ToString(),
                
            };
            var result = await userManager.CreateAsync(newUser, password);

            //add role user eoffice
            await identityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, IdLdapConstants.AppConfiguration.IdLdapUserRole);

            //add role lain-lain
            foreach (var role in roles)
            {
                await identityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, role);
            }

            return newUser;
        }


        private static async Task<User> CreateUser(string username, string password, params string[] roles)
        {
            var newUser = new User()
            {
                IsLdapUser = false,
                UserName = username,
                Email = username + "@.com",

            };
            var result = await userManager.CreateAsync(newUser, password);

            //add role user eoffice
            await identityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, IdLdapConstants.AppConfiguration.IdLdapUserRole);

            //add role lain-lain
            foreach (var role in roles)
            {
                await identityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, role);
            }

            return newUser;
        }
    }
}
