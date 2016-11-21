using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using Reston.Identity.Helper;
using Reston.Identity.Repository.Identity;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer3.Core.Extensions;
using IdentityModel;
using IdentityServer3.Core;
using System.Net.Http;
using Newtonsoft.Json;
using Reston.Helper;


namespace IdLdap.Configuration
{
    public class ADUserService : UserServiceBase
    {
        private readonly ILdapRepository _LdapRepository;
        private readonly UserManager _UserManager;
        public bool EnableSecurityStamp { get; set; }

        public ADUserService(ILdapRepository LdapRepository)
        {

            _LdapRepository = LdapRepository;

            _UserManager = new UserManager(new UserStore(new IdentityContext()));

            EnableSecurityStamp = true;
        }

        public int validasiCaptcha(Guid guid,string answer)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //HttpResponseMessage reply = await client.GetAsync(
            //       string.Format("{0}/{1}", IdLdapConstants.Proc.Url, "api/Registrasi/validasiCaptcha?guid="+guid+"&answer="+answer));

            //if (reply.IsSuccessStatusCode)
            //{
            //    string captca = await reply.Content.ReadAsStringAsync();
            //    var result = JsonConvert.DeserializeObject<int>(captca);

            //    return result;
            //}
            //else
            //{
            //    return 0;
            //}
            CaptchaHelper captcha = new CaptchaHelper();
            var validasi = captcha.Verify(guid, answer);
            if (validasi) return 1;
            else return 0;

        }

        public override async Task AuthenticateLocalAsync(IdentityServer3.Core.Models.LocalAuthenticationContext context)
        {
            try
            {
                var splitData = context.UserName.Split('#');
                if (splitData.Count() < 3) return;
                var username = splitData[0];
                var answerCaptcha = splitData[1];
                Guid guid =new Guid(splitData[2]);
                int valid = validasiCaptcha(guid, answerCaptcha);
                if (valid == 0) return;

               // var username = context.UserName;
                var password = context.Password;
                var message = context.SignInMessage;

                context.AuthenticateResult = null;
                var appBAse = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var path = appBAse + @"\log\login.txt";
                System.IO.File.AppendAllText(path, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine);

                var userLdap = _LdapRepository.FindUser2(username);
                System.IO.File.AppendAllText(path, "cekuserldap " + userLdap.DisplayName + Environment.NewLine);
                var userIdentity = await FindUserAsync(username);
                System.IO.File.AppendAllText(path, "cekuserldap " + userIdentity.DisplayName + Environment.NewLine);
                if (userIdentity != null)
                {
                    System.IO.File.AppendAllText(path, "cekuser-idenity " + Environment.NewLine);
                    if (userLdap != null)
                    {
                        if (userLdap.IsAccountLockedOut())
                        {
                            return;
                        }
                    }

                    if (userIdentity.LockoutEnabled)
                    {
                        return;
                    }

                    if (_UserManager.SupportsUserLockout &&
                            await _UserManager.IsLockedOutAsync(userIdentity.Id))
                    {
                        return;
                    }

                    if (userIdentity.IsLdapUser)
                    {
                        System.IO.File.AppendAllText(path, "user-ldap" + Environment.NewLine);
                        if (_LdapRepository.ValidateCredentials(username, password))
                        {

                            var claims = await GetClaimsForAuthenticateResult(userIdentity);
                            //var result = new AuthenticateResult(userLdap.Guid.ToString(), userLdap.SamAccountName, claims);
                            var result = new AuthenticateResult(userLdap.Guid.ToString(), userLdap.UserPrincipalName, claims);

                            context.AuthenticateResult = result;
                        }
                    }
                    else
                    {
                        System.IO.File.AppendAllText(path, "bukan-user-ldap" + Environment.NewLine);
                        if (await _UserManager.CheckPasswordAsync(userIdentity, password))
                        {
                            if (_UserManager.SupportsUserLockout)
                            {
                                await _UserManager.ResetAccessFailedCountAsync(userIdentity.Id);
                            }

                            var claims = await GetClaimsForAuthenticateResult(userIdentity);
                            var result = new AuthenticateResult(userIdentity.Id.ToString(), userIdentity.UserName, claims);


                            context.AuthenticateResult = result;
                        }
                        else if (_UserManager.SupportsUserLockout)
                        {
                            await _UserManager.AccessFailedAsync(userIdentity.Id);
                        }

                    }

                }
               
                System.IO.File.AppendAllText(path, "messege " + message + Environment.NewLine);
                System.IO.File.AppendAllText(path, "user " + username + Environment.NewLine);
                System.IO.File.AppendAllText(path, "password " + password + Environment.NewLine);
            }
            catch (Exception ex)
            {
                context.AuthenticateResult = null;
            }
        }

        protected async virtual Task<User> FindUserAsync(string username)
        {
            return await _UserManager.FindByNameAsync(username);
        }

        public virtual async Task<IEnumerable<Claim>> GetClaimsForAuthenticateResult(User userIdentity)
        {
            List<Claim> claims = new List<Claim>();
            if (EnableSecurityStamp && _UserManager.SupportsUserSecurityStamp)
            {
                var stamp = await _UserManager.GetSecurityStampAsync(userIdentity.Id);
                if (!String.IsNullOrWhiteSpace(stamp))
                {
                    claims.Add(new Claim("security_stamp", stamp));
                }
            }

            return claims;
        }

        public override async Task GetProfileDataAsync(IdentityServer3.Core.Models.ProfileDataRequestContext context)
        {
            var subject = context.Subject;
            var requestedClaimTypes = context.RequestedClaimTypes;

            if (subject == null) throw new ArgumentNullException("subject");

            String key = subject.GetSubjectId();

            var userIdentity = await _UserManager.FindByIdAsync(key);
            var userLdap = _LdapRepository.GetUserByGuid(key);

            if (userIdentity == null)
            {
                throw new ArgumentException("Invalid subject identifier");
            }


            IEnumerable<Claim> claims;
            if (userIdentity.IsLdapUser)
            {
                claims = await GetClaimsFromAccount(userLdap, userIdentity);
            }
            else
            {
                claims = await GetClaimsFromAccount(userIdentity);
            }
            //if (requestedClaimTypes != null && requestedClaimTypes.Any())
            //{
            //    claims = claims.Where(x => requestedClaimTypes.Contains(x.Type));
            //}

            context.IssuedClaims = claims;

        }

        public virtual async System.Threading.Tasks.Task<IEnumerable<Claim>> GetClaimsFromAccount(User userIdentity)
        {
            var claims = new List<Claim>{
                new Claim(Constants.ClaimTypes.Subject, userIdentity.Id.ToString()),
                new Claim(Constants.ClaimTypes.PreferredUserName, userIdentity.UserName),
            };

            if (_UserManager.SupportsUserClaim)
            {
                claims.AddRange(await _UserManager.GetClaimsAsync(userIdentity.Id.ToString()));
            }

            if (_UserManager.SupportsUserRole)
            {
                var roleClaims =
                    from role in await _UserManager.GetRolesAsync(userIdentity.Id.ToString())
                    select new Claim(Constants.ClaimTypes.Role, role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }

        public virtual async System.Threading.Tasks.Task<IEnumerable<Claim>> GetClaimsFromAccount(UserPrincipal userLdap, User userIdentity)
        {

            var claims = new List<Claim>{
                new Claim(Constants.ClaimTypes.Subject, userLdap.Guid.ToString()),
                //new Claim(Constants.ClaimTypes.PreferredUserName, userLdap.SamAccountName),
                new Claim(Constants.ClaimTypes.PreferredUserName, userLdap.UserPrincipalName),
            };


            if (_UserManager.SupportsUserClaim)
            {
                claims.AddRange(await _UserManager.GetClaimsAsync(userLdap.Guid.ToString()));
            }

            if (_UserManager.SupportsUserRole)
            {
                var roleClaims =
                    from role in await _UserManager.GetRolesAsync(userLdap.Guid.ToString())
                    select new Claim(Constants.ClaimTypes.Role, role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }




        public override async System.Threading.Tasks.Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject;

            if (subject == null) throw new ArgumentNullException("subject");

            String key = subject.GetSubjectId();
            var acct = await _UserManager.FindByIdAsync(key);

            context.IsActive = false;

            if (acct != null)
            {
                if (EnableSecurityStamp && _UserManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(x => x.Type == "security_stamp").Select(x => x.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp = await _UserManager.GetSecurityStampAsync(key);
                        if (db_security_stamp != security_stamp)
                        {
                            return;
                        }
                    }
                }

                context.IsActive = true;
            }
        }

        
    }
}
