
using Reston.EProc.Client;
using Reston.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using Thinktecture.IdentityModel.Client;
using System.Threading.Tasks;
using System.Globalization;

namespace Reston.Eproc.Client.Helper
{
    public class ClientTokenManagement
    {
        public static string GetToken()
        {
           // CheckAndPossiblyRefreshToken((HttpContext.Current.User.Identity as ClaimsIdentity));
            var token = (HttpContext.Current.User.Identity as ClaimsIdentity).FindFirst(IdLdapConstants.Claims.AccessToken);
            if (token != null)
            {
                return token.Value;
            }
            return null;
        }
        public static DateTime ConvertDate(string date, string formatDate)
        {
            DateTime Date = DateTime.ParseExact(date, formatDate, CultureInfo.InvariantCulture);
            return Date;
        }
        private static void CheckAndPossiblyRefreshToken(ClaimsIdentity id)
        {
            // check if the access token hasn't expired.
            var exp = id.FindFirst(IdLdapConstants.Claims.ExpiresAt).Value;
            string pattern = "d/MM/yyyy h:mm:ss tt";

            var expDate = ConvertDate(exp, pattern);

           // var expDate = new DateTime(long.Parse(exp));
            if (DateTime.Now.ToLocalTime() >= expDate)
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
        public static async Task<Thinktecture.IdentityModel.Client.TokenResponse> GetProcAPITokenAsync()
        {
            try
            {
                var client = new OAuth2ClientProxy(
                    new Uri(IdLdapConstants.Id.TokenEndpoint),
                    IdLdapConstants.Proc.ClientId,
                    IdLdapConstants.Proc.FirstSecret
                    );

                return await client.RequestClientCredentialsAsync(IdLdapConstants.Scope.ProcAPI);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        class OAuth2ClientProxy : OAuth2Client
        {
            public OAuth2ClientProxy(Uri address, string clientId, string clientSecret)
                : base(address, clientId, clientSecret)
            {
                _clientId = clientId;
                _clientSecret = clientSecret;
            }
        }
    }
}
