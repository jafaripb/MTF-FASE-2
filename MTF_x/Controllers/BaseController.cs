using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using MTF_x.Models;

namespace MTF_x.Controllers
{
    public class BaseController : Controller
    {
        public AppUser CurrentUser
        {
            get
            {
                return new AppUser(this.User as ClaimsPrincipal);
            }
        }

        public bool UserInRole(string role)
        {
            if (!this.User.Identity.IsAuthenticated) return false;
            return CurrentUser.Roles.Contains(role);
        }
        public Guid UserId()
        {
            return new Guid(CurrentUser.Subject);
        }

    }
}
