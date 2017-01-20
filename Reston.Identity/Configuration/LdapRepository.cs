using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Configuration
{
    public class LdapRepository : ILdapRepository, IDisposable
    {
        PrincipalContext _AuthLdapConnect;

        public LdapRepository(PrincipalContext AuthLdapConnect)
        {
            _AuthLdapConnect = AuthLdapConnect;
        }


        public UserPrincipal FindUser(string searchterm)
        {
            //return UserPrincipal.FindByIdentity(_AuthLdapConnect,IdentityType.SamAccountName , searchterm);
            var appBAse = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = appBAse + @"\log\login.txt";
            System.IO.File.AppendAllText(path, "finduser " + _AuthLdapConnect.UserName + Environment.NewLine);
            try
            {
                var userprincipl = UserPrincipal.FindByIdentity(_AuthLdapConnect, IdentityType.SamAccountName, searchterm);

                System.IO.File.AppendAllText(path, "userprincipl " + userprincipl.SamAccountName.ToString() + Environment.NewLine);
                return userprincipl;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(path, "errr " + ex.ToString() + Environment.NewLine);
                return new UserPrincipal(_AuthLdapConnect);
            }
        }

     
        public UserPrincipal FindUser2(string searchterm)
        {
            var appBAse = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = appBAse + @"\log\login.txt";
            System.IO.File.AppendAllText(path, "finduser " + _AuthLdapConnect.UserName + Environment.NewLine);
            try
            {
                var userprincipl = UserPrincipal.FindByIdentity(_AuthLdapConnect, IdentityType.UserPrincipalName, searchterm);

                System.IO.File.AppendAllText(path, "userprincipl " + userprincipl.UserPrincipalName.ToString() + Environment.NewLine);
                return userprincipl;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(path, "errr " + ex.ToString() + Environment.NewLine);
                return new UserPrincipal(_AuthLdapConnect);
            }
        }
        
        public GroupPrincipal FindGroup(string searchterm)
        {
            return GroupPrincipal.FindByIdentity(_AuthLdapConnect, searchterm);
        }

        public bool ValidateCredentials(string username, string password)
        {
           
                //_AuthLdapConnect.ValidateCredentials()
            var authenticated = false;
            var message = "";
            try
            {
                authenticated = _AuthLdapConnect.ValidateCredentials(username, password, ContextOptions.SimpleBind);
                if (!authenticated)
                {
                    authenticated = _AuthLdapConnect.ValidateCredentials(username, password, ContextOptions.Negotiate);

                    if (!authenticated)

                        throw new AuthenticationException();
                }
            }
            catch (Exception ex)
            {
                message = ex.ToString();
            }
                var appBAse = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var path = appBAse + @"\log\login.txt";
                System.IO.File.AppendAllText(path, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);
                System.IO.File.AppendAllText(path, "return " + authenticated.ToString() + Environment.NewLine);
                System.IO.File.AppendAllText(path, "messege " + message + Environment.NewLine);
                System.IO.File.AppendAllText(path, "user " + username + Environment.NewLine);
                System.IO.File.AppendAllText(path, "password " + password + Environment.NewLine);
                return authenticated;
        }

        public IdLdap.Models.GridUserItem GetUsersByUsername(string searchterm, int page = 0, int limit = int.MaxValue )
        {
            UserPrincipal userSearch =
                new UserPrincipal(_AuthLdapConnect);

            PrincipalSearcher searcher = new PrincipalSearcher();
            
            
            //userSearch.SamAccountName = searchterm;
            //userSearch.Name = searchterm;
            userSearch.UserPrincipalName = searchterm;
            //userSearch.GivenName = searchterm;
            //userSearch.SamAccountName = searchterm;
            searcher.QueryFilter = userSearch;
            int ct = searcher.FindAll().Count();
            
            using (searcher)
            {
                ((DirectorySearcher)searcher.GetUnderlyingSearcher()).SizeLimit = limit;
                ((DirectorySearcher)searcher.GetUnderlyingSearcher()).PageSize = page;

                PrincipalSearchResult<Principal> results =
                    searcher.FindAll();

                IdLdap.Models.GridUserItem gr = new IdLdap.Models.GridUserItem()
                {
                    Length = ct,
                    Users = results.Skip(page * limit).Take(limit).Select(principal => principal as UserPrincipal)
                };
                return gr;
            }
        }

        public PrincipalSearchResult<Principal> GetGroups(string searchterm)
        {
            GroupPrincipal userSearch =
                new GroupPrincipal(_AuthLdapConnect);
            userSearch.SamAccountName = searchterm;


            PrincipalSearcher searcher = new PrincipalSearcher();
            searcher.QueryFilter = userSearch;

            using (searcher)
            {
                PrincipalSearchResult<Principal> results =
                    searcher.FindAll();

                return results;
            } 
        }


        public UserPrincipal GetUserByGuid(String guid)
        {
            var userPrincipal = UserPrincipal.FindByIdentity(_AuthLdapConnect, IdentityType.Guid, guid);
            return userPrincipal;
        }


        public void Dispose()
        {
            _AuthLdapConnect.Dispose();
        }




        
    }
}
