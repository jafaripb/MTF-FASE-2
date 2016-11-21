using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Webservice.Helper
{
    public class LdapMvcAuthorizeRoleAttribute : AuthorizeAttribute
    {
        private string[] roles;
        public LdapMvcAuthorizeRoleAttribute(params string[] roles) { this.roles = roles; }
        private Nullable<bool> _isAuthorized;

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            _isAuthorized = null;

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var user = ((ClaimsPrincipal)httpContext.User);
            foreach (var r in roles)
            {
                if (user.HasClaim("role", r))
                {
                    _isAuthorized = true;
                    return true;
                }

            }

            _isAuthorized = false;
            return false;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (_isAuthorized != null)
            {
                if (_isAuthorized == false)
                {
                    filterContext.Controller.TempData.Add("RedirectReason", "Aku tidak memiliki akses pada aplikasi ini, silahkan contact administrator anda.");
                    filterContext.Result = new RedirectResult("~/HandleException");
                }
            }

        }
    }
}
