using IdLdap.Configuration;
using Reston.Identity.Helper;
using IdLdap.Models;
using Reston.Identity.Repository.Identity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdLdap.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILdapRepository _LdapRepository;
        private readonly IdentityContext _IdentityContext;
        private readonly UserManager _UserManager;
        private readonly RoleManager _RoleManager;
        private readonly IdentityManagerService _IdentityManagerService;



        public AdminController()
        {
            bool UseAppDir = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["LDAP_APPDIR"]);
            _LdapRepository = new LdapRepository(new PrincipalContext(UseAppDir ? ContextType.ApplicationDirectory : ContextType.Domain,
                 IdLdapConstants.LdapConfiguration.Host,
                 IdLdapConstants.LdapConfiguration.ContextNaming,
                 IdLdapConstants.LdapConfiguration.Username,
                 IdLdapConstants.LdapConfiguration.Password));
            _IdentityContext = new IdentityContext();
            _UserManager = new UserManager(new UserStore(_IdentityContext));
            _RoleManager = new RoleManager(new RoleStore(_IdentityContext));

            _IdentityManagerService = new IdentityManagerService(_UserManager, _RoleManager);
            
        }
        // GET: Admin
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public ActionResult UserLdap()
        {
            return View();
        }

        [HttpPost]
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<JsonResult> LinkedAccount(string username)
        {
            if (String.IsNullOrEmpty(username))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "Username tidak boleh kosong" }, JsonRequestBehavior.AllowGet);
            }
            
            var userIdentity = await _UserManager.FindByNameAsync(username);
            if (userIdentity != null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { message = "User sudah di link" }, JsonRequestBehavior.AllowGet);
            }
            bool UseAppDir = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["LDAP_APPDIR"]);
            var userLdap = UseAppDir ? _LdapRepository.FindUser2(username) : _LdapRepository.FindUser(username);
           // var userLdap = _LdapRepository.FindUser2(username);
            //bool b = IsMember(userLdap, "doski");
            
            //await CreateUser(username, "123456", userLdap.Guid.GetValueOrDefault());//password gakepake, ttp pake password ldap masing2
            await CreateUserLinkedLdap(username, "P@ssw0rd!", userLdap.DisplayName, userLdap.EmailAddress, userLdap.Guid.GetValueOrDefault());//password gakepake, ttp pake password ldap masing2

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { message = "Sukses linked account ldap. " }, JsonRequestBehavior.AllowGet);
        }

        private static bool IsMember(UserPrincipal user, string GroupName)
        {
            try
            {
                foreach (Principal result in user.GetAuthorizationGroups())
                {
                    if (string.Compare(result.Name, GroupName, true) == 0)
                        return true;
                }
                return false;
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        private async Task<User> CreateUser(string username, string password, Guid guid, params string[] roles)
        {
            var newUser = new User()
            {
                IsLdapUser = true,
                UserName = username,
                //Email = username + ".com",
                Id = guid.ToString()
                
            };
            var result = await _UserManager.CreateAsync(newUser, password);

            //add role user eoffice
            await _IdentityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, IdLdapConstants.AppConfiguration.IdLdapUserRole);

            //add role lain-lain
            foreach (var role in roles)
            {
                await _IdentityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, role);
            }

            return newUser;
        }

        private async Task<User> CreateUserLinkedLdap(string username, string password, string displayname, string Email, Guid guid, params string[] roles)
        {
            var newUser = new User()
            {
                IsLdapUser = true,
                UserName = username,
                //Email = username + ".com",
                Email=Email,
                Id = guid.ToString(),
                DisplayName = displayname
            };
            var result = await _UserManager.CreateAsync(newUser, password);

            //add role user eoffice
            await _IdentityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, IdLdapConstants.AppConfiguration.IdLdapUserRole);

            //add role lain-lain
            foreach (var role in roles)
            {
                await _IdentityManagerService.AddUserClaimAsync(newUser.Id, IdentityServer3.Core.Constants.ClaimTypes.Role, role);
            }

            return newUser;
        }

        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public ActionResult UserId()
        {
            return View();
        }

        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<ActionResult> DetailUser(String id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ApplicationException("empty id");

            var userIdentity = await _UserManager.FindByNameAsync(id);
            if (userIdentity == null)
                throw new ApplicationException("username tidak terdaftar");

            List<RoleUser> Roles = _RoleManager.Roles.Select(x => new RoleUser() {
                Id = x.Id,
                Name = x.Name,
                RoleDescription = x.RoleDescription
            }).ToList();

            ViewModelUserDetail vm = new ViewModelUserDetail();
            vm.UserDetail = new UserDetail()
            {
                Email = userIdentity.Email,
                Id = userIdentity.Id,
                DisplayName = userIdentity.DisplayName,
                Position = userIdentity.Position,
                IsLdapUser = userIdentity.IsLdapUser,
                LockoutEnabled = userIdentity.LockoutEnabled,
                PhoneNumber = userIdentity.PhoneNumber,
                UserName = userIdentity.UserName,
                UserClaims = userIdentity.Claims.Select(x => new UserClaims() {
                    ClaimType = x.ClaimType,
                    ClaimValue = x.ClaimValue,
                    Id = x.Id,
                    UserId = x.UserId
                }).ToList()
            };

            List<String> claimsRoleUser = userIdentity.Claims.Where(x => x.ClaimType == IdLdapConstants.Claims.Role).Select(x => x.ClaimValue).ToList();
            Roles.Where(x => claimsRoleUser.Contains(x.Name)).ToList().ForEach(x => { x.Selected = true; });


            vm.RoleUser = Roles;

            return View(vm);
        }


        //[LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.pRole_procurement_user,IdLdapConstants.App.Roles.pRole_procurement_manager)]
        //[Authorize]
        public async Task<JsonResult> ListUser(int start, int limit,string filter,string name)
        {
            var s = User;
            DataPageUsers dataPageUsers = new DataPageUsers();

            var dbContext = new IdentityContext();
            var user = dbContext.Users.Where(d => d.IsLdapUser == true);

            if (!string.IsNullOrEmpty(filter))
                user = user.Where(d => d.Claims.Select(x => x.ClaimValue).Contains(filter));
            if (!string.IsNullOrEmpty(name))
                user = user.Where(d=>d.UserName.Contains(name));
            dataPageUsers.totalRecord = user.Count();
            dataPageUsers.Users = user.Select(d => new Userx
            {
                PersonilId=d.Id,
                Nama = d.DisplayName,
                tlp = d.PhoneNumber,
                jabatan=d.Position
            }).OrderBy(d => d.Nama).Skip(start).Take(limit).ToList();
            
            return Json(dataPageUsers, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetManager()
        {

            var dbContext = new IdentityContext();
            var oData=dbContext.Users.Where(d => d.Claims.Select(x => x.ClaimValue).Contains(IdLdapConstants.App.Roles.pRole_procurement_manager))
                    .Select(d=>new Userx{
                        Nama=d.UserName,
                        PersonilId=d.Id                        
                    }).ToList();

            return Json(oData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUser(string Id)
        {
            var dbContext = new IdentityContext();
            var oData = dbContext.Users.Where(d => d.Id == Id)
                    .Select(d => new Userx
                    {
                        Nama = d.UserName,
                        PersonilId = d.Id,
                        Email=d.Email
                    }).FirstOrDefault();

            return Json(oData, JsonRequestBehavior.AllowGet);

        }


        public JsonResult capca()
        {
            return Json("H4522");
        }

        [HttpPost]
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<ActionResult> UpdateUserProperty(UserDetail UserDetail)
        {
            if (UserDetail == null)
                throw new ApplicationException("Detail User is Null");

            var userIdentity = _UserManager.Users.FirstOrDefault(x => x.UserName == UserDetail.UserName);
            if (userIdentity == null)
                throw new ApplicationException("User not exist");

            //userIdentity.PhoneNumber = UserDetail.PhoneNumber;
            userIdentity.Email = UserDetail.Email;
            userIdentity.Position = UserDetail.Position;
            userIdentity.LockoutEnabled = UserDetail.LockoutEnabled;

            await _UserManager.UpdateAsync(userIdentity);

            return RedirectToAction("UserId", new { id = UserDetail.UserName });
        }

        [HttpPost]
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<ActionResult> UpdateUserRole(List<RoleUser> RoleUser, UserDetail UserDetail)
        {
            if (RoleUser == null)
                throw new ApplicationException("Input null");

            if(UserDetail == null)
                throw new ApplicationException("Tidak ada Detail User");

            var userIdentity = await _UserManager.FindByNameAsync(UserDetail.UserName);

            if(userIdentity == null)
                throw new ApplicationException("User tidak terregister");

            foreach (var item in RoleUser)
            {
                await _UserManager.RemoveClaimAsync(userIdentity.Id, new Claim(IdLdapConstants.Claims.Role, item.Name));
            }

            

            foreach(var item in RoleUser.Where(x=>x.Selected == true))
            {
                await _UserManager.AddClaimAsync(userIdentity.Id, new Claim(IdLdapConstants.Claims.Role, item.Name));  
            }

            return RedirectToAction("UserId", new { id = UserDetail.UserName });
        }

        [HttpPost]
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<ActionResult> UpdateUserPassword(UserDetail UserDetail)
        {
            if(UserDetail == null)
                throw new ApplicationException("Null Input");

            if( String.IsNullOrEmpty(UserDetail.NewPassword))
                throw new ApplicationException("Password Null !");

            var userIndentity = await _UserManager.FindByNameAsync(UserDetail.UserName);
            if(userIndentity == null)
                throw new ApplicationException("User not exist");

            _UserManager.RemovePassword(userIndentity.Id);

            IdentityResult result = await _UserManager.AddPasswordAsync(userIndentity.Id, UserDetail.NewPassword);
            if (!result.Succeeded)
            {
                string error = String.Join(", ", result.Errors.ToArray());
                //throw new ApplicationException(error);
                //return Json(new { msg = error });
                return RedirectToAction("ErrorMessage", new { msg = error, redir = this.Url.Action("DetailUser", "Admin", new { id = UserDetail.UserName }, this.Request.Url.Scheme) });
            }

            return RedirectToAction("UserId", new { id = UserDetail.UserName });
            //return Json(new { msg = "cukces." });
        }


        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public ActionResult CreateUser()
        {
            ViewModelNewUser vm = new ViewModelNewUser();
            vm.UserDetail = new UserDetail();

            return View(vm);
        }


        [HttpPost]
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<ActionResult> NewUser(UserDetail UserDetail)
        {
            if (UserDetail == null)
                throw new ApplicationException("Null Input");

            if (String.IsNullOrEmpty(UserDetail.UserName) && String.IsNullOrEmpty(UserDetail.NewPassword))
                throw new ApplicationException("Important Input Null !");

            var newUser = new User()
            {
                IsLdapUser = false,
                UserName = UserDetail.UserName,
                Email = UserDetail.Email,

            };

            IdentityResult result = await _UserManager.CreateAsync(newUser, UserDetail.NewPassword);
            if (!result.Succeeded)
            {
                string error = String.Join(", ", result.Errors.ToArray());
                throw new ApplicationException(error);
            }
            
            return RedirectToAction("DetailUser", new { id = UserDetail.UserName });
        }

        public async Task<ActionResult> ErrorMessage(string msg, string redir) {
            ViewBag.message = msg;
            ViewBag.redir = redir;
            return View();
        }

        [HttpPost]
        //[LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.pRole_staff)]
        public async Task<string> NewVendorUser(UserDetail UserDetail) {
            
            if (UserDetail == null)
                throw new ApplicationException("Null Input");

            if (String.IsNullOrEmpty(UserDetail.UserName) && String.IsNullOrEmpty(UserDetail.NewPassword))
                throw new ApplicationException("Important Input Null !");

            User b = _UserManager.FindByName(UserDetail.UserName);
            int i = 1;
            if (b!=null) {
                b = null;
                UserDetail.UserName = UserDetail.UserName + i++.ToString();
                b = _UserManager.FindByName(UserDetail.UserName);
            }

            var newUser = new User()
            {
                Id = UserDetail.guid.ToString(),
                IsLdapUser = false,
                UserName = UserDetail.UserName,
                DisplayName = UserDetail.DisplayName,
                Email = UserDetail.Email,
            };
            IdentityResult result = await _UserManager.CreateAsync(newUser, UserDetail.NewPassword);
            if (!result.Succeeded)
            {
                string error = String.Join(", ", result.Errors.ToArray());
                throw new ApplicationException(error);
            }

            //add vendor role
            await _UserManager.AddClaimAsync(newUser.Id, new Claim(IdLdapConstants.Claims.Role, IdLdapConstants.App.Roles.pRole_procurement_vendor));
            await _UserManager.AddClaimAsync(newUser.Id, new Claim(IdLdapConstants.Claims.Role, IdLdapConstants.App.Roles.IdLdapUserRole));  
            
            return UserDetail.UserName;
        }


        [HttpPost]
        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public async Task<ActionResult> DeleteUser(string username)
        {
            if (String.IsNullOrEmpty(username))
                throw new ApplicationException("input null");

            var userIdentity = await _UserManager.FindByNameAsync(username);
            if (userIdentity == null)
                throw new ApplicationException("User not exist");
            var result = await _UserManager.DeleteAsync(userIdentity);

            if (!result.Succeeded)
            {
                string error = String.Join(", ", result.Errors.ToArray());
                throw new ApplicationException(error);
            }

            return RedirectToAction("UserId");
        }

        [LdapMvcAuthorizeRole(IdLdapConstants.App.Roles.IdLdapAdminRole)]
        public ActionResult ListRole()
        {
            return View();
        }


        public ActionResult CreateRole()
        {
            ViewModelNewRole vm = new ViewModelNewRole();
            vm.RoleUser = new RoleUser();
            return View(vm);
        }

        public async Task<ActionResult> CreateNewRole(RoleUser RoleUser)
        {
            if(RoleUser == null)
                throw new  ApplicationException("input null");
            
            if(await _RoleManager.RoleExistsAsync(RoleUser.Name))
                throw new  ApplicationException("role exist");

              await _RoleManager.CreateAsync(new Role() { Name = RoleUser.Name });

              return RedirectToAction("ListRole");
        }


        public async Task<ActionResult> DeleteRole(string rolename)
        {
            if (String.IsNullOrEmpty(rolename))
                throw new ApplicationException("input invalid");

            var datarole = await _RoleManager.FindByNameAsync(rolename);
            if(datarole == null)
                throw new  ApplicationException("role never exist");

            await _RoleManager.DeleteAsync(datarole);
            var userHaveClaim =  _UserManager.Users.Where(x=>x.Claims.Where(z=>z.ClaimType==IdLdapConstants.Claims.Role && z.ClaimValue == rolename).Any()).ToList();
            

            foreach (var item in userHaveClaim)
            {
                await _UserManager.RemoveClaimAsync(item.Id, new Claim(IdLdapConstants.Claims.Role, rolename));
            }

            return RedirectToAction("ListRole");
        }
    }
}