using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Reston.Pinata.WebService.Helper
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        //readonly CreditPointModelContext _ctx = new CreditPointModelContext();
        private string[] roles;
        private Nullable<bool> _isAuthorized;
        public ApiAuthorizeAttribute(params string[] proles) { this.roles = proles; }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (_isAuthorized != null) {
                if (_isAuthorized == false) { 
                    //do send to index? 403
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                }
            }
            else { 
                //do redirection 401
                var challengeMessage = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                challengeMessage.Headers.Add("WWW-Authenticate", "Basic");
                actionContext.Response = challengeMessage;
            }
        }

        private bool Authorize(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                var principal = actionContext.RequestContext.Principal;
                var cp = (ClaimsPrincipal)principal;
                if (!IsAuthorized(actionContext))
                {
                    _isAuthorized = null;
                    return false;
                }
                foreach (var r in roles) { 
                    if(cp.HasClaim("role",r)){
                        _isAuthorized = true;
                        return true;
                    }
                }
                if (roles.Count() < 1) {
                    _isAuthorized = true;
                    return true;
                }
                _isAuthorized = false;
            }   
            catch (Exception)
            {
                _isAuthorized = false;
                return false;
            }
            _isAuthorized = false;
            return false;
        }
    }
}
