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


namespace Reston.Pinata.WebService.Controllers
{
    public class RksController : BaseController
    {
        private IRksRepo _repository;
        internal ResultMessage result = new ResultMessage();


        public RksController()
        {
            _repository = new RksRepo(new JimbisContext());
        }

        public RksController(RksRepo repository)
        {
            _repository = repository;
        }
        
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage save(RKSHeaderTemplate rks)
        {
            try
            {
                result=_repository.saveRks(rks, UserId());                
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
                result.status = HttpStatusCode.ExpectationFailed;
            }
            return result;
        }

        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public DataTableRksTemplate List()
        {
            try
            {
                int start =Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
                string search = System.Web.HttpContext.Current.Request["search"].ToString();
                int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
                string klasifikasi = System.Web.HttpContext.Current.Request["klasifikasi"].ToString();
                var data=_repository.GetRks(search, start, length, klasifikasi);
                return data;
            }
            catch (Exception ex)
            {
                return new DataTableRksTemplate();
            }            
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage delete(Guid Id)
        {
            try
            {
                result = _repository.deleteRks(Id, UserId());
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
                result.status = HttpStatusCode.ExpectationFailed;
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public DataTableRksDetailTemplate getRks(Guid? Id)
        {
            var odata = new DataTableRksDetailTemplate();
            odata.data = new List<VWRKSDetailTemplate>();
            if (Id == null) return odata;
            return _repository.GetRksDetail(Id.Value);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public VWRKSTemplate getHeaderRks(Guid Id)
        {
            return _repository.getRksHeader(Id);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult GetRks(string term, string klarifikasi)
        {
            List<VWRKSTemplate> lp = _repository.GetRks(term, 0, 10, klarifikasi).data;
            return Json(new { aaData = lp });
        }

    }
}
