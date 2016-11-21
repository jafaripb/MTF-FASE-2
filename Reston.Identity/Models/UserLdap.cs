using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Models
{
    public class UserLdap
    {
        public string Guid { get; set; }
        public string SamAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string UserGroup { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public string StreetAddress { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public bool IsAccountLockedOut { get; set; }
        public Nullable<bool> IsLinked { get; set; }
    }

    public class GridUserItem{
        public int Length {get;set;}

        public IEnumerable<System.DirectoryServices.AccountManagement.UserPrincipal> Users{get;set;}
    }
}
