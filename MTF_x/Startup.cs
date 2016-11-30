using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using MTF_x.Helper;
using Newtonsoft.Json.Linq;
using Owin;

[assembly: OwinStartupAttribute(typeof(MTF_x.Startup))]
namespace MTF_x
{
    
        public partial class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                //idldp
                //JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>
                //{
                //    {"role", System.Security.Claims.ClaimTypes.Role}
                //};
                JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string> { };
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
                    ClientId = Constants.Portal.ClientId,
                    Authority = Constants.Id.Url,
                    RedirectUri = Constants.Portal.Url,
                    PostLogoutRedirectUri = Constants.Portal.Url,
                    SignInAsAuthenticationType = Constants.SignInAsAuthenticationType.Cookies,
                    UseTokenLifetime = false,

                    ResponseType = string.Format("{0} {1} {2}", Constants.ResponseType.Code,
                                                Constants.ResponseType.IdToken,
                                                Constants.ResponseType.Token),

                    Scope = string.Format("{0} {1} {2} {3}",
                                           Constants.Scope.Openid, Constants.Scope.Profile,
                                           Constants.Scope.Roles, Constants.Scope.OfflineAccess),

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
                                Constants.Id.TokenEndpoint,
                                Constants.Portal.ClientId,
                                Constants.Portal.FirstSecret);

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


                            id.AddClaim(new Claim(Constants.Claims.UniqueUserKey,
                                issuerClaim.Value + "_" + subjectClaim.Value));

                            id.AddClaim(new Claim(Constants.Claims.Subject, subjectClaim.Value));
                            id.AddClaim(new Claim(Constants.Claims.AccessToken, response.AccessToken));
                            id.AddClaim(new Claim(Constants.Claims.ExpiresAt, response.ExpiresIn.ToString()));//DateTime.Now.AddSeconds(response.ExpiresIn).Ticks.ToString())
                            id.AddClaim(new Claim(Constants.Claims.RefreshToken, response.RefreshToken));
                            id.AddClaim(new Claim(Constants.Claims.IdToken, n.ProtocolMessage.IdToken));
                            id.AddClaim(new Claim(Constants.Claims.LogoutUri, Constants.Portal.Url));
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
                                var idTokenHint = n.OwinContext.Authentication.User.FindFirst(Constants.Claims.IdToken);
                                var logouturi = n.OwinContext.Authentication.User.FindFirst(Constants.Claims.LogoutUri);

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
            }

        }
    
}
