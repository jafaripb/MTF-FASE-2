using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;


namespace Reston.Pinata.WebService.Auth
{
    public class AuthorizeRoleAttribute : ResourceAuthorizationManager
    {
        private string[] roles;
        public AuthorizeRoleAttribute(params string[] roles) { this.roles = roles; }
        private Nullable<bool> _isAuthorized;




    }
}
