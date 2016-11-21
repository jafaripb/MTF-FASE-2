using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Models
{
    public class UserDetail 
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }

        public Guid guid { get; set; }
        public bool IsLdapUser { get; set; }
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public ICollection<UserClaims> UserClaims { get; set; }
    }
}
