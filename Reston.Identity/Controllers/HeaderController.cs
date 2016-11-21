using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdLdap.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Header
        public ActionResult Signout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect(Reston.Identity.Helper.IdLdapConstants.IDM.Url);
        }
    }
}