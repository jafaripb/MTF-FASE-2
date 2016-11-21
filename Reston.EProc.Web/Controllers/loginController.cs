using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using Model.Helper;

namespace Reston.Pinata.WebService.Controllers
{
    public class loginController:BaseController
    {
        //private readonly UserManager _userManager;
        public string Get(string username,string password)
        {

            /*
            if (username == "alice" && password == "supersecret")
            {
                System.Web.Http.Owin.ge
                HttpContext.GetOwinContext().Authentication
                  .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                Claim claim1 = new Claim(ClaimTypes.Name, username);
                Claim[] claims = new Claim[] { claim1 };
                ClaimsIdentity claimsIdentity =
                  new ClaimsIdentity(claims,
                    DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication
                 .SignIn(new AuthenticationProperties() { IsPersistent = false }, claimsIdentity);

                //return Redirect("/Home");
            }
           
             * */
            return "ok";
        }

        [System.Web.Http.HttpGet]
        public async void Signout() {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage reply =  await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "Header/Signout"));
        }

    }
}
