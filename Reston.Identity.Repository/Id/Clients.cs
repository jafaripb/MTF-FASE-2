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
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]{

                //Idm
                new Client{
                    Enabled = true,
                    ClientName = IdLdapConstants.IDM.ClientName,
                    ClientId = IdLdapConstants.IDM.ClientId,
                    ClientSecrets = new List<Secret>
                    { 
                        new Secret(IdLdapConstants.IDM.FirstSecret.Sha256())
                    },
                    Flow = Flows.Hybrid,
                    AccessTokenLifetime = 3600,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Roles,
                        Constants.StandardScopes.OfflineAccess,
                        IdLdapConstants.Scope.IdentityManager
                        
                    },
                    RequireConsent = false,
                    AccessTokenType = AccessTokenType.Reference,
                    LogoUri = IdLdapConstants.IDM.LogoUri,
                    RedirectUris = new List<string>
                    {
                        IdLdapConstants.IDM.Url
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        IdLdapConstants.IDM.Url
                    }
                },

                //mgt
                new Client{
                    Enabled = true,
                    ClientName = IdLdapConstants.Mgt.ClientName,
                    ClientId = IdLdapConstants.Mgt.ClientId,
                    ClientSecrets = new List<Secret>{
                        new Secret(IdLdapConstants.Mgt.FirstSecret.Sha256())
                    },
                    Flow = Flows.Hybrid,
                    AccessTokenLifetime = 3600,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Roles,
                        Constants.StandardScopes.OfflineAccess,
                        IdLdapConstants.Scope.MgtAPI,


                    },
                    RequireConsent = false,
                    AccessTokenType = AccessTokenType.Reference,
                    LogoUri = IdLdapConstants.Mgt.LogoUri,
                    RedirectUris = new List<string>{
                        IdLdapConstants.Mgt.Url
                    },
                    PostLogoutRedirectUris = new List<string>{
                        IdLdapConstants.Mgt.Url
                    }
                },
                //ep
                new Client{
                    Enabled = true,
                    ClientName = IdLdapConstants.Proc.ClientName,
                    ClientId = IdLdapConstants.Proc.ClientId,
                    ClientSecrets = new List<Secret>
                    { 
                        new Secret(IdLdapConstants.Proc.FirstSecret.Sha256())
                    },
                    Flow = Flows.Hybrid,
                    AccessTokenLifetime = 3600,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Roles,
                        Constants.StandardScopes.OfflineAccess,
                        IdLdapConstants.Scope.IdentityManager
                        
                    },
                    RequireConsent = false,
                    AccessTokenType = AccessTokenType.Reference,
                    LogoUri = IdLdapConstants.Proc.LogoUri,
                    RedirectUris = new List<string>
                    {
                        IdLdapConstants.Proc.Url
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        IdLdapConstants.Proc.Url
                    }
                },

                //econtractmanagement
                new Client{
                    Enabled = true,
                    ClientName = IdLdapConstants.Contract.ClientName,
                    ClientId = IdLdapConstants.Contract.ClientId,
                    ClientSecrets = new List<Secret>
                    { 
                        new Secret(IdLdapConstants.Contract.FirstSecret.Sha256())
                    },
                    Flow = Flows.Hybrid,
                    AccessTokenLifetime = 3600,
                    AllowedScopes = new List<string>
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.Roles,
                        Constants.StandardScopes.OfflineAccess,
                        IdLdapConstants.Scope.IdentityManager
                        
                    },
                    RequireConsent = false,
                    AccessTokenType = AccessTokenType.Reference,
                    LogoUri = IdLdapConstants.Contract.LogoUri,
                    RedirectUris = new List<string>
                    {
                        IdLdapConstants.Contract.Url
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        IdLdapConstants.Contract.Url
                    }
                },

                new Client
                {
                    Enabled = true,
                    ClientName = IdLdapConstants.SuperAdmin.ClientName,
                    ClientId = IdLdapConstants.SuperAdmin.ClientId,
                    ClientSecrets = new List<Secret>{
                        new Secret(IdLdapConstants.SuperAdmin.FirstSecret.Sha256())
                    },
                    Claims = new List<System.Security.Claims.Claim>
                    {
                        new System.Security.Claims.Claim(Constants.ClaimTypes.Name, IdLdapConstants.AppConfiguration.IdLdapAdminUsername),
                        new System.Security.Claims.Claim(Constants.ClaimTypes.Role, IdLdapConstants.AppConfiguration.IdLdapAdminRole),
                        new System.Security.Claims.Claim(Constants.ClaimTypes.Subject, IdLdapConstants.AppConfiguration.AdminSubject),
                        new System.Security.Claims.Claim(Constants.ClaimTypes.Issuer, IdLdapConstants.AppConfiguration.IdentitySiteIssueruri),
                        new System.Security.Claims.Claim(IdLdapConstants.Claims.UniqueUserKey, IdLdapConstants.AppConfiguration.UniqueUserKey),
                    },
                    AllowedScopes = new List<string>
                    { 
                        IdLdapConstants.Scope.MgtAPI,
                    },
                    Flow = Flows.ClientCredentials,
                    PrefixClientClaims = false,
                    AccessTokenType = AccessTokenType.Jwt
                }

        
            };
        }
    }
}
