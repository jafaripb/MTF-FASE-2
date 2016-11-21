using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Configuration
{
    public interface ILdapRepository : IDisposable
    {
        UserPrincipal FindUser(string searchterm);
        UserPrincipal FindUser2(string searchterm);
        IdLdap.Models.GridUserItem GetUsersByUsername(string searchterm, int page, int limit);
        PrincipalSearchResult<Principal> GetGroups(string searchterm);
        GroupPrincipal FindGroup(string searchterm);
        bool ValidateCredentials(string username, string password);

        UserPrincipal GetUserByGuid(String Guid);
    }
}
