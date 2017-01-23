using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Model.Helper;
using Newtonsoft.Json;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using Reston.Pinata.Model.PengadaanRepository.View;
using Reston.Pinata.WebService.Helper;
using Reston.Pinata.WebService.ViewModels;
using Reston.Eproc.Model.Monitoring.Repository;

namespace Reston.Pinata.WebService.Controllers
{
    public class HeaderController : BaseController
    {
        private IPengadaanRepo _repository;
        internal ResultMessage result = new ResultMessage();

        public HeaderController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
        }

         [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
         [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff,
             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_procurement_head,
             IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_end_user,
             IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.App.Roles.IdLdapSuperAdminRole, IdLdapConstants.App.Roles.IdLdapUserRole)]
        public ResultMessage cekRole()
        {
            ResultMessage oResul = new ResultMessage();
            oResul.status = HttpStatusCode.OK;
            oResul.message = String.Join(", ", Roles().ToArray());
            return oResul;
        }

         [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
         [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff,
             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_procurement_head,
             IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_end_user,
             IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.App.Roles.IdLdapSuperAdminRole, IdLdapConstants.App.Roles.IdLdapUserRole)]
        public HttpStatusCode cekLogin()
        {
            var user = UserId();
            return HttpStatusCode.OK;
        }

        private List<Menu> cekdasboard(List<Menu> menu)
        {
            var dasbord = menu.Where(d => d.menu == "Dashboard").FirstOrDefault();

            var total = _repository.ListCount();

            if (dasbord != null)
            {
                dasbord.menu = dasbord.menu + " (" +total.TotalSeluruhPersetujuan + ")";
            }
            return menu;
        }

         //[Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
         public async Task<List<Menu>> GetMenu()
         {


             var userr = CurrentUser;
             // read file into a string and deserialize JSON to a type
             
             var roles = Roles();
             if (roles.Contains(IdLdapConstants.App.Roles.IdLdapSuperAdminRole))
             {
                 Menu newMenu = new Menu { id = 2, css = "fa fa-user", url = IdLdapConstants.IDM.Url + "admin/userid", menu = "User Management" };
                 var lstMenu = JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-admin.json"));
                 lstMenu.Insert(1, newMenu);
                lstMenu=cekdasboard(lstMenu);
                return lstMenu;
                 
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementStaffRole) && roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementAdminRole))
             {
                var menu = JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-staff-admin.json"));
               /* var dasbord = menu.Where(d => d.menu == "Dasboard").FirstOrDefault();
                if (dasbord != null)
                {
                    dasbord.menu = dasbord.menu + "(10)";
                }*/
                return cekdasboard( menu);//JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-staff-admin.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementHeadRole) || roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementManagerRole) || roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementStaffRole))
             {
                var lstMenu = JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu.json"));
                lstMenu = cekdasboard(lstMenu);
                return lstMenu;//JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapEndUserRole))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-user.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapComplianceRole))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-compliance.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapRekananTerdaftarRole))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-vendor.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdaplegal_direksi))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-direksi.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdaplegal_dirut))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-direksi.json"));
             }
             else
             {
                 return new List<Menu>();
             }

         }

     [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public void Signout()
       {
           Request.GetOwinContext().Authentication.SignOut();
           Redirect(IdLdapConstants.IDM.Url);
       }

     [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
     public async Task<IHttpActionResult> GetUrl()
     {
         try
         {
             

             return Json(new { proc = IdLdapConstants.Proc.Url, idsrv = IdLdapConstants.IDM.Url });
         }
         catch {
             return Json("");
         }
     }

     [Authorize]
     [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
     public async Task<IHttpActionResult> User()
     {
         try
         {
             Userx user = await userDetail(UserId().ToString());
             return Json(user);
         }
         catch
         {
             return Json("");
         }
     }



    }
}
