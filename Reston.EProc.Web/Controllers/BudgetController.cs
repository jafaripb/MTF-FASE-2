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
//using Reston.Pinata.Model.Helper;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using Reston.Pinata.Model.PengadaanRepository.View;
using Reston.Pinata.WebService.Helper;
using Reston.Pinata.WebService.ViewModels;
using Webservice.Helper.Util;
using Reston.Helper.Repository;
using Reston.Helper;
using Reston.Helper.Util;
using Reston.Helper.Model;
using System.Web;
using SpreadsheetLight;
using Reston.Eproc.Model.Monitoring.Entities;


namespace Reston.Pinata.WebService.Controllers
{
    public class BudgetController : BaseController
    {
        private IPengadaanRepo _repoPengadaan;
        private IBudgetRepo _repository;
       
        public BudgetController()
        {
            _repoPengadaan = new PengadaanRepo(new JimbisContext());
            _repository = new BudgetRepo(new JimbisContext());
        }

        public BudgetController(BudgetRepo repository)
        {
            _repository = repository;
        }
       
      
      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      public IHttpActionResult List()
      {
          try
          {
              int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
              string cari = System.Web.HttpContext.Current.Request["cari"].ToString();
              int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString()); 
              var data = _repository.List( start, length,cari);
              return Json(data);
          }
          catch (Exception ex)
          {
              return Json(new DataTablePksTemplate());
          }
      }

      [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                               IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                                IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
      [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]      
      public IHttpActionResult importXls(HttpPostedFileBase file)
      {
           List<COA> lstCoa = new List<COA>();
          using (var xls = new SLDocument(file.InputStream))
          {
             
              for (var i = 2; i <= xls.GetWorksheetStatistics().EndRowIndex; i++)
              {
                  COA nCoa = new COA();
                  nCoa.NoCoa = xls.GetCellValueAsString(i, 1);
                  nCoa.Region = xls.GetCellValueAsString(i, 2);
                  nCoa.Divisi = xls.GetCellValueAsString(i, 3);
                  nCoa.Periode = xls.GetCellValueAsString(i, 4);
                  nCoa.GroupAset = xls.GetCellValueAsString(i, 5);
                  nCoa.JenisAset = xls.GetCellValueAsString(i, 6);
                  nCoa.NilaiAset = xls.GetCellValueAsString(i, 7);
                  lstCoa.Add(nCoa);
              }  
          }
          return Json(_repository.add(lstCoa, UserId()));          
      }

      
    }
    
}
