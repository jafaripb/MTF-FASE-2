using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Reston.Identity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Identity.Repository.Id
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
                {
 
                    // identity scopes

                    StandardScopes.OpenId,
                    StandardScopes.Profile,
                    StandardScopes.OfflineAccess,
                     new Scope
                    {
                        Enabled = true,
                        Name = IdLdapConstants.Scope.Roles,
                        DisplayName = "Roles",
                        Description = "The roles you belong to.",
                        Type = ScopeType.Identity,
                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Role)
                        }
                    },
                    


                    new Scope
                    {
                        Name = IdLdapConstants.Scope.MgtAPI,
                        DisplayName = "Mgt API Scope",
                        Type = ScopeType.Resource,
                        Emphasize = false,
                         Enabled = true,
                           Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Role),
                            new ScopeClaim(IdLdapConstants.Claims.UniqueUserKey),
                            new ScopeClaim(Constants.ClaimTypes.PreferredUserName)
                        }
                       
                    },

                    new Scope
                    {
                        Name = IdLdapConstants.Scope.IdentityManager,
                        DisplayName = "IdentityManager Scope",
                        Type = ScopeType.Resource,
                        Emphasize = false,
                         Enabled = true,
                           Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Role),
                            new ScopeClaim(Constants.ClaimTypes.Subject),
                            new ScopeClaim(Constants.ClaimTypes.PreferredUserName)
                        }
                       
                    },


                 };

            return scopes;
        }

    }
}
