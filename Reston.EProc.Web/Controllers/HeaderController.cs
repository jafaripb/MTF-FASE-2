﻿using System;
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

namespace Reston.Pinata.WebService.Controllers
{
    public class HeaderController : BaseController
    {
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
                 return lstMenu;
                 
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementStaffRole) && roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementAdminRole))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu-staff-admin.json"));
             }
             else if (roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementHeadRole) || roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementManagerRole) || roles.Contains(IdLdapConstants.App.Roles.IdLdapProcurementStaffRole))
             {
                 return JsonConvert.DeserializeObject<List<Menu>>(File.ReadAllText(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\data\menu.json"));
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


    }
}