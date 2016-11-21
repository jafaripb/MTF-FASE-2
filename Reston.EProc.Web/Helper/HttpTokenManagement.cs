using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using Thinktecture.IdentityModel.Client;
using Model.Helper;

namespace Webservice.Helper.Util
{
    public class HttpTokenManagement
    {
        public static string GetToken()
        {
            var xxx = HttpContext.Current.User.Identity as ClaimsIdentity;

            CheckAndPossiblyRefreshToken((HttpContext.Current.User.Identity as ClaimsIdentity));
            var token = (HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(IdLdapConstants.Claims.AccessToken);
            if (token != null)
            {
                return token.Value;
            }
            return null;
        }

        private static void CheckAndPossiblyRefreshToken(ClaimsIdentity id)
        {


            // check if the access token hasn't expired.
            if (DateTime.Now.ToLocalTime() >=
                 (DateTime.Parse(id.FindFirst(IdLdapConstants.Claims.ExpiresAt).Value)))
            {
                // expired.  Get a new one.
                var tokenEndpointClient = new OAuth2Client(
                    new Uri(IdLdapConstants.Id.TokenEndpoint),
                    IdLdapConstants.Proc.ClientId,
                    IdLdapConstants.Proc.FirstSecret);


                var tokenEndpointResponse =
                     tokenEndpointClient
                    .RequestRefreshTokenAsync(id.FindFirst("refresh_token").Value);

                tokenEndpointResponse.Wait();
                if (tokenEndpointResponse.IsCompleted && !tokenEndpointResponse.Result.IsError)
                {
                    // replace the claims with the new values - this means creating a 
                    // new identity!                              
                    var result = from claim in id.Claims
                                 where claim.Type != "access_token" && claim.Type != "refresh_token" &&
                                       claim.Type != "expires_at"
                                 select claim;

                    var claims = result.ToList();

                    claims.Add(new Claim("access_token", tokenEndpointResponse.Result.AccessToken));
                    claims.Add(new Claim("expires_at",
                                 DateTime.Now.AddSeconds(tokenEndpointResponse.Result.ExpiresIn)
                                 .ToLocalTime().ToString()));
                    claims.Add(new Claim("refresh_token", tokenEndpointResponse.Result.RefreshToken));

                    var newIdentity = new ClaimsIdentity(claims, "Cookies");
                    var wrapper = new HttpRequestWrapper(HttpContext.Current.Request);
                    wrapper.GetOwinContext().Authentication.SignIn(newIdentity);
                }
                else
                {
                    // log, ...
                    throw new Exception("An error has occurred :(");
                }
            }
        }
    }
}