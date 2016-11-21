namespace Reston.Identity.Repository.Identity
{
    using Reston.Identity.Helper;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Runtime.Remoting.Contexts;

    public class User : IdentityUser {

        public bool IsLdapUser { get; set; }
        public string DisplayName { get; set; }
        public string Position { get; set; }
    }
    public class Role : IdentityRole {
        public string RoleDescription { get; set; }
    }
    public class IdentityContext : IdentityDbContext<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {

        public IdentityContext()
            : base(IdLdapConstants.Configuration.IdLdapConnectionString)
        {
        }

    }

    public class MigrationsContextFactory : IDbContextFactory<IdentityContext>
    {
        public IdentityContext Create()
        {
            return new IdentityContext();
        }
    }

    public class UserStore : UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(IdentityContext ctx)
            : base(ctx)
        {
        }
    }

    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireDigit = true,
                RequireUppercase = true,
                RequireLowercase = true,
                RequireNonLetterOrDigit = false
            };
        }

    }

    public class RoleStore : RoleStore<Role>
    {
        public RoleStore(IdentityContext ctx)
            : base(ctx)
        {
        }
    }
    

    
    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }
    }



}