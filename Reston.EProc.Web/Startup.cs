using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.Logging;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;
//using System.Web.Helpers;
using Microsoft.Owin.Security.Cookies;
using System.Data.Entity;
using Reston.Pinata.Model.Repository;
//using Reston.Pinata.Model.IdentityRepository;
using Reston.Pinata.Model;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OpenIdConnect;
using IdentityModel.Client;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Protocols;
using Model.Helper;
using Webservice.Helper.Util;

//using Reston.Pinata.Model.PengadaanRepository;

//[assembly: OwinStartup(typeof(StartUp), "Configuration")]
namespace Reston.Pinata.WebService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //idldp
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>
            {
                {"role", System.Security.Claims.ClaimTypes.Role}
            };
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = IdLdapConstants.Claims.UniqueUserKey;

            //app.Map("/api", api =>
            //{


            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                SlidingExpiration = true,
                ExpireTimeSpan = System.TimeSpan.FromMinutes(150)

            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = IdLdapConstants.Proc.ClientId,
                Authority = IdLdapConstants.Id.Url,
                RedirectUri = IdLdapConstants.Proc.Url,
                PostLogoutRedirectUri = IdLdapConstants.Proc.Url,
                SignInAsAuthenticationType = IdLdapConstants.SignInAsAuthenticationType.Cookies,
                UseTokenLifetime = false,

                ResponseType = string.Format("{0} {1} {2}", IdLdapConstants.ResponseType.Code,
                                            IdLdapConstants.ResponseType.IdToken,
                                            IdLdapConstants.ResponseType.Token),

                Scope = string.Format("{0} {1} {2} {3}",
                                       IdLdapConstants.Scope.Openid, IdLdapConstants.Scope.Profile,
                                       IdLdapConstants.Scope.Roles, IdLdapConstants.Scope.OfflineAccess),

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    MessageReceived = async n =>
                    {
                        EndpointAndTokenHelper.DecodeAndWrite(n.ProtocolMessage.IdToken);
                        EndpointAndTokenHelper.DecodeAndWrite(n.ProtocolMessage.AccessToken);
                    },

                    AuthorizationCodeReceived = async n =>
                    {
                        // use the code to get the access and refresh token
                        var tokenClient = new TokenClient(
                            IdLdapConstants.Id.TokenEndpoint,
                            "proc",
                            "secret");

                        var response = await tokenClient.RequestAuthorizationCodeAsync(n.Code, n.RedirectUri);
                        var id = new ClaimsIdentity(n.AuthenticationTicket.Identity.AuthenticationType);

                        var userInfo = await EndpointAndTokenHelper.CallUserInfoEndpoint(response.AccessToken);


                        JToken roles;
                        try
                        {
                            roles = userInfo.Value<JValue>(Thinktecture.IdentityModel.Client.JwtClaimTypes.Role).ToObject<JToken>();
                        }
                        catch
                        {
                            roles = userInfo.Value<JArray>(Thinktecture.IdentityModel.Client.JwtClaimTypes.Role).ToObject<JToken>();
                        }

                        foreach (var role in roles)
                        {
                            id.AddClaim(new Claim(
                            Thinktecture.IdentityModel.Client.JwtClaimTypes.Role,
                            role.ToString()));
                        }


                        var issuerClaim = n.AuthenticationTicket.Identity
                            .FindFirst(Thinktecture.IdentityModel.Client.JwtClaimTypes.Issuer);
                        var subjectClaim = n.AuthenticationTicket.Identity
                            .FindFirst(Thinktecture.IdentityModel.Client.JwtClaimTypes.Subject);


                        id.AddClaim(new Claim(IdLdapConstants.Claims.UniqueUserKey,
                            issuerClaim.Value + "_" + subjectClaim.Value));

                        id.AddClaim(new Claim(IdLdapConstants.Claims.Subject, subjectClaim.Value));
                        id.AddClaim(new Claim(IdLdapConstants.Claims.AccessToken, response.AccessToken));
                        id.AddClaim(new Claim(IdLdapConstants.Claims.ExpiresAt, DateTime.Now.AddSeconds(response.ExpiresIn).ToLocalTime().ToString()));
                        id.AddClaim(new Claim(IdLdapConstants.Claims.RefreshToken, response.RefreshToken));
                        id.AddClaim(new Claim(IdLdapConstants.Claims.IdToken, n.ProtocolMessage.IdToken));
                        id.AddClaim(new Claim(IdLdapConstants.Claims.LogoutUri, IdLdapConstants.Proc.Url));
                        //id.AddClaim(new Claim("jimbis", "Jimbis S"));


                        n.AuthenticationTicket = new AuthenticationTicket(
                            new ClaimsIdentity(id.Claims, n.AuthenticationTicket.Identity.AuthenticationType),
                            n.AuthenticationTicket.Properties);
                    },

                    RedirectToIdentityProvider = n =>
                    {
                        // if signing out, add the id_token_hint
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst(IdLdapConstants.Claims.IdToken);
                            var logouturi = n.OwinContext.Authentication.User.FindFirst(IdLdapConstants.Claims.LogoutUri);

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                                n.ProtocolMessage.PostLogoutRedirectUri = logouturi.Value;
                            }

                        }

                        return Task.FromResult(0);
                    },

                }

            });
            //});

            var log = app.CreateLogger(typeof(Startup));

            log.WriteInformation("Startup Configuration is executing...");
            //ConfigureAuth(app); //commented by jimbis

            //var contentDir = ConfigurationManager.AppSettings["www.rootDir"];
            //var physicalFileSystem = new PhysicalFileSystem(@"D:\project mandiri\mockup html");

            // var physicalFileSystem = new PhysicalFileSystem(@"..\..\..\WebService\mockup");
            // var physicalFileSystem = new PhysicalFileSystem(@"..\Reston.Pinata\WebService\mockup");

            //var physicalFileSystem = new PhysicalFileSystem(@"..\..\..\WebService\mockup");
            string viewPath = System.Configuration.ConfigurationManager.AppSettings["VIEW_BASE_PATH"];
            var physicalFileSystem = new PhysicalFileSystem(viewPath);//
            //var physicalFileSystem = new PhysicalFileSystem(@"..\PROC\mockup");//mtf serper

            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem

            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[]
			{
				"index.html"
			};

            app.UseFileServer(options);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Reston.Pinata.Model.IdentityMigrations.IdentityConfiguration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<JimbisContext, Reston.Pinata.Model.Migrations.Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PengadaanContext, Reston.Pinata.Model.PengadaanMigrations.PengadaanConfiguration>()); 

            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            app.UseWebApi(config);

            if (PengadaanConstants.RunSeeder == true)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<JimbisContext, Reston.Pinata.Model.Migrations.Configuration>());
            }
            /*
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                SlidingExpiration = true,
                ExpireTimeSpan = System.TimeSpan.FromMinutes(15),
                LoginPath = new PathString("/login.html")
            });
		    */
        }
    }
}
