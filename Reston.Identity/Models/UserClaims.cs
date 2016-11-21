using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Models
{
    public class UserClaims
    {

        public  string ClaimType { get; set; }

        public  string ClaimValue { get; set; }

        public  int Id { get; set; }

        public String UserId { get; set; }
    }
}
