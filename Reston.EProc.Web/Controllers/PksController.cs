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


namespace Reston.Pinata.WebService.Controllers
{
    public class PksController : BaseController
    {
        private IPksRepo _repository;
        private IWorkflowRepository _workflowrepo;
        private IPengadaanRepo _repoPengadaan;
        private string DocumentType = "PKS";
        public PksController()
        {
            _repository = new PksRepo(new JimbisContext());
            _repoPengadaan = new PengadaanRepo(new JimbisContext());
            _workflowrepo = new WorkflowRepository(new HelperContext());
        }

        public PksController(PksRepo repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult List()
        {
            try
            {
                int start = Convert.ToInt32(System.Web.HttpContext.Current.Request["start"].ToString());
                string search = System.Web.HttpContext.Current.Request["search"].ToString();
                int length = Convert.ToInt32(System.Web.HttpContext.Current.Request["length"].ToString());
                string klasifikasi = System.Web.HttpContext.Current.Request["klasifikasi"].ToString();
                var data = _repository.List(search, start, length, klasifikasi);
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new DataTablePksTemplate());
            }
        }

        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult Detail(Guid PengadaanId,int VendorId)
        {
            try
            {

                return Json( _repository.detail(PengadaanId, VendorId));
            }
            catch (Exception ex)
            {
                return Json(new VWPks());
            }
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<IHttpActionResult> ajukan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                var vwpengadaan = _repoPengadaan.GetPengadaanByiD(Id);
                var DepHead = await listHead();
                var DepManager = await listGuidManager();

                #region BuatAtauUpdateTamplate

                var WorkflowMasterTemplateDetails = new List<WorkflowMasterTemplateDetail>(){
                                new WorkflowMasterTemplateDetail()
                                    {
                                        NameValue="Gen.By.System",
                                        SegOrder=1,
                                        UserId=vwpengadaan.PersonilPengadaans.Where(d=>d.tipe=="controller").FirstOrDefault().PersonilId
                                    },
                                 new WorkflowMasterTemplateDetail()
                                    {
                                        NameValue="Gen.By.System",
                                        SegOrder=2,
                                        UserId=DepManager[0]
                                    }
                            };
                WorkflowMasterTemplate MasterTemplate = new WorkflowMasterTemplate()
                {
                    ApprovalType = ApprovalType.BERTINGKAT,
                    CreateBy = UserId(),
                    CreateOn = DateTime.Now,
                    DescValue = "WorkFlow Pengadaan=> " + vwpengadaan.Judul,
                    NameValue = "Generate By System ",
                    WorkflowMasterTemplateDetails = WorkflowMasterTemplateDetails
                };
                var resultTemplate = _workflowrepo.SaveWorkFlow(MasterTemplate, UserId());

                #endregion

                if (vwpengadaan.WorkflowId != null)
                {

                    var pks = _repository.get(Id);
                    

                    var savePks = _repository.save(pks, UserId());
                    var resultx = _workflowrepo.PengajuanDokumen(new Guid(savePks.Id), Convert.ToInt32(resultTemplate.Id), DocumentType);
                    if (string.IsNullOrEmpty(resultx.Id))
                    {
                        return Json(resultx);
                    }
                    return Json(new ResultMessage()
                    {
                        Id = resultx.Id,
                        message="Berhasil",
                        status=HttpStatusCode.OK
                    });
                }
                else
                {
                    return Json(new ResultMessage() { 
                        message="GAGAL BUAT WORKFLOW APPROVAL",
                        status=HttpStatusCode.NoContent
                    });
                }

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            return Json(new ResultMessage());
        }

    }
    
}
