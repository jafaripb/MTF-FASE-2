using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Reston.Pinata.WebService.Helper
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string UniqueUserKey
        {
            get
            {
                return this.FindFirst("unique_user_key").Value;
            }
        }
        public string AccessToken
        {
            get
            {
                return this.FindFirst("access_token").Value;
            }
        }

        public string FirstName
        {
            get
            {
                return this.FindFirst("given_name").Value;
            }
        }

        public string LastName
        {
            get
            {
                return this.FindFirst("family_name").Value;
            }
        }

        public string UserName
        {
            get
            {
                return this.FindFirst("preferred_username").Value;
            }
        }

        public string Jimbis
        {
            get
            {
                return this.FindFirst("jimbis").Value;
            }
        }

        public string UserId
        {
            get
            {
                return this.FindFirst("userid").Value;
            }
        }

        public string Subject
        {
            get
            {
                return this.FindFirst("unique_user_key").Value.Split('_')[1];
            }
        }
        public string Issuer
        {
            get
            {
                return this.FindFirst("unique_user_key").Value.Split('_')[0];
            }
        }

        public List<string> Roles
        {
            get { return this.FindAll("role").Select(x => x.Value).ToList(); }
        }


    }
}