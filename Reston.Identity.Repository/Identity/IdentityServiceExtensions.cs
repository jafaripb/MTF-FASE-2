using IdentityManager.AspNetIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Identity.Repository.Identity
{
    
    public class IdentityManagerService : AspNetIdentityManagerService<User, string, Role, string>
    {
        public IdentityManagerService(UserManager userMgr, RoleManager roleMgr)
            : base(userMgr, roleMgr)
        {
        }
    }
}
