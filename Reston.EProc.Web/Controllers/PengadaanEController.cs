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
    public class PengadaanEController : BaseController
    {
        string DocumentType = "eproc";
        string DocumentTypePemenang = "pengajuan_pemenang";
        private IPengadaanRepo _repository;
        private IWorkflowRepository _workflowrepo;
        private IRksRepo _reporks;
        private ISpkRepo _spkrepo;
        internal ResultMessage result = new ResultMessage();
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"];
        private string FILE_PENGADAAN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        private int WorkflowTemplateId1 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WorkflowTemplateId1"]);
        private int WorkflowTemplateId2 = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WorkflowTemplateId2"]);
        private decimal ValueBoundAprr = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["BATASAN_BIAYA"]);
        private decimal ValueBoundDireksiAprr = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["BATASAN_BIAYA_DIREKSI"]);
        private decimal BATASAN_BIAYA_DIRUT = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["BATASAN_BIAYA_DIRUT"]);
        private string BodyEmailPemenang = System.Configuration.ConfigurationManager.AppSettings["MAIL_PEMENANG_BODY"];
        private string BodyEmailkalah = System.Configuration.ConfigurationManager.AppSettings["MAIL_KALAH_BODY"];
        private string SubjeckEmailPemenang = System.Configuration.ConfigurationManager.AppSettings["MAIL_PEMENANG_SUBJECT"];
        private string SubjeckEmailKalah = System.Configuration.ConfigurationManager.AppSettings["MAIL_KALAH_SUBJECT"];
     
        public PengadaanEController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
            _workflowrepo = new WorkflowRepository(new HelperContext());
            _reporks = new RksRepo(new JimbisContext());
            _spkrepo = new SpkRepo(new JimbisContext());
        }

        public PengadaanEController(PengadaanRepo repository)
        {
            _repository = repository;
        }
        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> save(VWPengadaan vwpengadaan)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string id = "";
            try
            {

                List<JadwalPengadaan> lstMJadwalPengadaan = new List<JadwalPengadaan>();
                if (vwpengadaan.Jadwal != null)
                {
                    foreach (var item in vwpengadaan.Jadwal)
                    {
                        JadwalPengadaan MJadwalPengadaan = new JadwalPengadaan();
                        // if (!string.IsNullOrEmpty(item.Mulai)) MJadwalPengadaan.Mulai = Common.ConvertDate(item.Mulai, "dd/MM/yyyy");
                        //if (!string.IsNullOrEmpty(item.Sampai)) MJadwalPengadaan.Sampai = Common.ConvertDate(item.Sampai, "dd/MM/yyyy");
                        if (!string.IsNullOrEmpty(item.Mulai)) MJadwalPengadaan.Mulai = Common.ConvertDate(item.Mulai, "dd/MM/yyyy HH:mm");
                        if (!string.IsNullOrEmpty(item.Sampai)) MJadwalPengadaan.Sampai = Common.ConvertDate(item.Sampai, "dd/MM/yyyy HH:mm");
                        if (!string.IsNullOrEmpty(item.Sampai) && !string.IsNullOrEmpty(item.Mulai))
                        {
                            MJadwalPengadaan.Mulai = Common.ConvertDate(item.Mulai, "dd/MM/yyyy HH:mm");
                            MJadwalPengadaan.Sampai = Common.ConvertDate(item.Sampai, "dd/MM/yyyy HH:mm");
                        }
                        MJadwalPengadaan.tipe = item.tipe;
                        lstMJadwalPengadaan.Add(MJadwalPengadaan);
                    }

                    //foreach (var item in vwpengadaan.Pengadaan.DokumenPengadaans)
                    //{
                    //    var file = CopyFile(item.File);
                    //    item.File = file;
                    //}

                    vwpengadaan.Pengadaan.JadwalPengadaans = lstMJadwalPengadaan;
                }
                if (vwpengadaan.Pengadaan.Id != Guid.Empty)
                {
                    //vwpengadaan.Pengadaan.ModifiedBy = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                    //vwpengadaan.Pengadaan.ModifiedOn = DateTime.Now;
                    message = Common.UpdateSukses();
                }
                else message = Common.SaveSukses();
                if (vwpengadaan.Pengadaan.Status == EStatusPengadaan.AJUKAN)
                {
                    try
                    {
                        decimal? RKS = _repository.getRKSDetails(vwpengadaan.Pengadaan.Id, UserId()).Sum(d=>d.hps*d.jumlah);
                        var TemplateId = WorkflowTemplateId2;
                        if (RKS != null)
                        {
                            if (RKS > ValueBoundAprr)
                            {
                                TemplateId = WorkflowTemplateId1;
                            }
                        }

                        var DepHead = await listHead();
                        var DepManager = await listGuidManager();

                        #region BuatAtauUpdateTamplate

                        var getPersonil = _repository.getListPersonilPengadaan(vwpengadaan.Pengadaan.Id);

                        var WorkflowMasterTemplateDetails = new List<WorkflowMasterTemplateDetail>(){
                                new WorkflowMasterTemplateDetail()
                                    {
                                        NameValue="Gen.By.System",
                                        SegOrder=1,
                                        UserId=getPersonil.Where(d=>d.tipe=="controller").FirstOrDefault().PersonilId
                                    },
                                 new WorkflowMasterTemplateDetail()
                                    {
                                        NameValue="Gen.By.System",
                                        SegOrder=2,
                                        UserId=DepManager[0]
                                    }
                            };
                        if (RKS > ValueBoundAprr) WorkflowMasterTemplateDetails.Add(
                                     new WorkflowMasterTemplateDetail()
                                        {
                                            NameValue = "Gen.By.System",
                                            SegOrder = 3,
                                            UserId = DepHead[0]
                                        });
                        WorkflowMasterTemplate MasterTemplate = new WorkflowMasterTemplate()
                        {
                            ApprovalType = ApprovalType.BERTINGKAT,
                            CreateBy = UserId(),
                            CreateOn = DateTime.Now,
                            DescValue = "WorkFlow Pengadaan=> " + vwpengadaan.Pengadaan.Judul,
                            NameValue = "Generate By System ",
                            WorkflowMasterTemplateDetails = WorkflowMasterTemplateDetails
                        };
                        var resultTemplate = _workflowrepo.SaveWorkFlow(MasterTemplate,UserId());
                        vwpengadaan.Pengadaan.WorkflowId = Convert.ToInt32(resultTemplate.Id);
                        #endregion


                        
                    }
                    catch (Exception ex)
                    {
                        result.message = ex.Message.ToString();
                        return result;
                    }
                }
                if (vwpengadaan.Pengadaan.WorkflowId != null)
                {
                    var pengadaan = _repository.AddPengadaan(vwpengadaan.Pengadaan, UserId(), await listGuidManager());
                    respon = HttpStatusCode.OK;
                    id = pengadaan.Id.ToString();
                    var resultx = _workflowrepo.PengajuanDokumen(vwpengadaan.Pengadaan.Id, vwpengadaan.Pengadaan.WorkflowId.Value, DocumentType);
                    if (string.IsNullOrEmpty(resultx.Id))
                    {
                        result.message = resultx.message;
                        result.Id = resultx.Id;
                        return result;
                    }
                    var ajukanPnegadaanId = _repository.AjukanPengadaan(pengadaan.Id, UserId(), await listGuidManager());
                }
                else
                {
                    vwpengadaan.Pengadaan.Status = EStatusPengadaan.DRAFT;
                    var pengadaan = _repository.AddPengadaan(vwpengadaan.Pengadaan, UserId(), await listGuidManager());
                    respon = HttpStatusCode.OK;
                    message = Common.SaveSukses();
                    id = pengadaan.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = id;
            }
            //
            return result;
        }

        [HttpPost]
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

        [HttpPost]
        [Authorize]
        public HttpStatusCode cekLogin()
        {
            return HttpStatusCode.OK;
        }

        [HttpPost]
        [Authorize]
        public RKSHeader saveTotalHps(Guid Id, decimal Total)
        {
            return _repository.AddTotalHps(Id, Total, UserId());
        }

        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff,
           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_procurement_head,
           IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_end_user,
           IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.App.Roles.IdLdapSuperAdminRole,
           IdLdapConstants.App.Roles.IdLdapUserRole,IdLdapConstants.App.Roles.IdLdapApproverRole)]
        public RKSHeader getTotalHps(Guid Id)
        {
            return _repository.GetTotalHps(Id, UserId());
        }

        public class DataTable
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<VWRKSDetail> data { get; set; }
        }

        public class DataTableRksRekanan
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<VWRKSDetailRekanan> data { get; set; }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<HttpResponseMessage> getUsersStaff(int start, int limit)// ListUser(int start, int limit)
        {
            var client = new HttpClient();

            string filter = IdLdapConstants.App.Roles.IdLdapProcurementStaffRole;
           // var tokenRespones = await Reston.Identity.Client.Api.ClientTokenManagement.GetIdEPROCAPITokenAsync();
           // var toke = AksesToken();
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpTokenManagement.GetToken());

            //original base address using appmgt instead
            //client.BaseAddress = new Uri("http://localhost:53080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + limit + "&filter=" + filter));
           
            return reply;

        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<HttpResponseMessage> getUsersEnd(int start, int limit, string name)
        {
            var client = new HttpClient();
            string filter = IdLdapConstants.App.Roles.IdLdapEndUserRole;
            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + limit + "&filter=" + filter + "&name=" + name));
            return reply;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<HttpResponseMessage> getUsersComapliance(int start, int limit, string name)
        {
            var client = new HttpClient();
            string filter = IdLdapConstants.App.Roles.IdLdapComplianceRole;
            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + limit + "&filter=" + filter + "&name=" + name));
            return reply;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<HttpResponseMessage> getUsers(int start, int limit, string name)
        {
            var client = new HttpClient();
            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + limit + "&filter=" + "&name=" + name));
            return reply;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int isCreatePengadaan()
        {
            if (Roles().Contains(IdLdapConstants.App.Roles.IdLdapProcurementManagerRole) ||
                Roles().Contains(IdLdapConstants.App.Roles.IdLdapProcurementStaffRole)) return 1;
            else return 0;

        }

        [Authorize]
        public List<Menu> GetMenu()
        {
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public DataTable getRks(Guid Id)
        {
            List<VWRKSDetail> rks = _repository.getRKS(Id);
            DataTable datatable = new DataTable();
            datatable.recordsTotal = rks.Count();
            datatable.data = rks;
            return datatable;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ResultMessage saveRks(RKSHeader rks)
        {
            HttpStatusCode status = HttpStatusCode.NotFound;
            string message = "";
            string id = "";
            try
            {
                // Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);

                var viewRks = _repository.saveRks(rks, UserId());
                if (viewRks.Id != Guid.Empty)
                {
                    status = HttpStatusCode.OK;
                    id = viewRks.Id.ToString();
                    message = Common.UpdateSukses();
                }
                else
                {
                    status = HttpStatusCode.OK;
                    message = Common.SaveSukses();
                }
            }
            catch (Exception ex)
            {
                status = HttpStatusCode.NotImplemented;
                message = ex.ToString();
            }
            finally
            {
                result.status = status;
                result.message = message;
                result.Id = id;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ResultMessage saveRksFromTemplate(Guid RksId,Guid PengadaanId)
        {
            HttpStatusCode status = HttpStatusCode.NotFound;
            string message = "";
            string id = "";
            try
            {
                // Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var RKS = _reporks.MapRksFromTemplate(RksId, PengadaanId);                
                var removeOld=_repository.RemoveRks(PengadaanId, UserId());
                if (removeOld.status == HttpStatusCode.OK)
                {
                    var viewRks = _repository.saveRks(RKS, UserId());
                    if (viewRks.Id != Guid.Empty)
                    {
                        status = HttpStatusCode.OK;
                        id = viewRks.Id.ToString();
                        message = Common.UpdateSukses();
                    }
                    else
                    {
                        status = HttpStatusCode.OK;
                        message = Common.SaveSukses();
                    }
                }
                else
                {
                    status = removeOld.status;
                    message = removeOld.message;
                }
            }
            catch (Exception ex)
            {
                status = HttpStatusCode.NotImplemented;
                message = ex.ToString();
            }
            finally
            {
                result.status = status;
                result.message = message;
                result.Id = id;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public VWRKSHeaderPengadaan getHeaderRks(Guid Id)
        {
            // Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);

            var HeaderRks = _repository.GetRKSHeaderPengadaan(Id);
            return HeaderRks;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public  ViewPengadaan detailPengadaan(Guid Id)
        {
            var pengadaan = _repository.GetPengadaanByiD(Id);
            int isAprrover = 0;
            if (pengadaan.WorkflowId != null)
            {
                var ResultCurrentApprover = _workflowrepo.CurrentApproveUserSegOrder(Id, pengadaan.WorkflowId.Value);
                Guid? ApproverId = null;
                if (!string.IsNullOrEmpty(ResultCurrentApprover.Id))
                {
                    ApproverId = new Guid(ResultCurrentApprover.Id.Split('#')[1]);
                }
                 isAprrover = UserId() == ApproverId ? 1 : 0;
            }
            return _repository.GetPengadaan(Id, UserId(), isAprrover);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ViewPengadaan detailPengadaanForRekanan(Guid Id)
        {
            //Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.GetPengadaanForRekanan(Id, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ResultMessage deletePengadaan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.DeletePengadaan(Id, UserId());
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<DataPagePengadaan> getPengadaanList(int start, int length, EGroupPengadaan group, string search)
        {
            //Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            //List<string> roles = ((ClaimsIdentity)User.Identity).Claims
            //    .Where(c => c.Type == ClaimTypes.Role)
            //    .Select(c => c.Value).ToList();
            var lstPerhatianPengadaan = _repository.GetPengadaans(search, start, length, UserId(), Roles(), group, await listGuidManager(), await listHead());
            return lstPerhatianPengadaan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public List<ViewPengadaan> getPerhatianPengadaanList(int start, int length, string search)
        {
            //List<Reston.Helper.Model.ViewWorkflowModel> getDoc = _workflowrepo.ListDocumentWorkflow(UserId(), Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentType, 0, 0);
            
            //foreach(var item in getd
            var lstPerhatianPengadaan = _repository.GetPerhatianWorkflow(search, start, length, UserId()); // _repository.GetPerhatian(search, start, length, UserId(), Roles(), await isApprover(), await listGuidManager());
            foreach (var item in lstPerhatianPengadaan)
            {
                if (item.WorkflowTemplateId != null && item.WorkflowTemplateId != 0)
                {
                    List<Reston.Helper.Model.ViewWorkflowModel> getDoc = _workflowrepo.ListDocumentWorkflow(UserId(), item.WorkflowTemplateId.Value, Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentType, 0, 0);
                    if (getDoc.Where(d => d.CurrentUserId == UserId()).FirstOrDefault() != null) item.Approver = 1;
                }
            }
            return lstPerhatianPengadaan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public Reston.Helper.Util.ResultMessageWorkflowState persetujuan(Guid id)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {
                var pengadaan = _repository.GetPengadaanByiD(id);
                result=_workflowrepo.ApproveDokumen2(id,pengadaan.WorkflowId.Value, UserId(), "", Reston.Helper.Model.WorkflowStatusState.APPROVED);
                if (!string.IsNullOrEmpty(result.Id))
                {
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Dokumen Pengadaan DiSetujui";
                    nRiwayatDokumen.PengadaanId = id;
                    nRiwayatDokumen.UserId = UserId();
                    _repository.AddRiwayatDokumen(nRiwayatDokumen);
                    ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(id, pengadaan.WorkflowId.Value);
                    if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                    {
                        _repository.ChangeStatusPengadaan(id,EStatusPengadaan.DISETUJUI,UserId());
                    }
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }            
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< Reston.Helper.Util.ResultMessageWorkflowState> persetujuanWithNote(Guid id,string Note)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {
                var pengadaan = _repository.GetPengadaanByiD(id);
                result = _workflowrepo.ApproveDokumen2(id, pengadaan.WorkflowId.Value, UserId(), Note, Reston.Helper.Model.WorkflowStatusState.APPROVED);
                if (!string.IsNullOrEmpty(result.Id))
                {
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Dokumen Pengadaan DiSetujui";
                    nRiwayatDokumen.Comment = Note;
                    nRiwayatDokumen.PengadaanId = id;
                    nRiwayatDokumen.UserId = UserId();
                    _repository.AddRiwayatDokumen(nRiwayatDokumen);
                    ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(id,pengadaan.WorkflowId.Value);
                    if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                    {
                        _repository.ChangeStatusPengadaan(id, EStatusPengadaan.DISETUJUI, UserId());

                        #region BuatAtauUpdateTamplate
                        var vwpengadaan = _repository.GetPengadaanByiD(id);
                        var DepHead = await listHead();
                        var DepManager = await listGuidManager();
                        var Direksi = await listUser(IdLdapConstants.Roles.pRole_direksi);
                        var Dirut = await listUser(IdLdapConstants.Roles.pRole_dirut);
                        decimal? RKS = _repository.getRKSDetails(id, UserId()).Sum(d => d.hps * d.jumlah);
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
                        if (RKS > ValueBoundAprr) WorkflowMasterTemplateDetails.Add(
                                     new WorkflowMasterTemplateDetail()
                                     {
                                         NameValue = "Gen.By.System",
                                         SegOrder = 3,
                                         UserId = DepHead[0]
                                     });
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
                        var wokflowId = Convert.ToInt32(resultTemplate.Id);
                        #endregion

                        
                        PersetujuanPemenang ndata = new PersetujuanPemenang()
                        {
                            CreatedOn = DateTime.Now,
                            CreatedBy = UserId(),
                            PengadaanId = id,
                            WorkflowId = wokflowId
                        };
                        _repository.SavePersetujuanPemenang(ndata, UserId());
                    }
                    var nextApprover = _workflowrepo.CurrentApproveUserSegOrder(id,pengadaan.WorkflowId.Value);
                    try
                    {
                        await SendEmailToApprover(nextApprover.Id.Split('#')[1],id);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public Reston.Helper.Util.ResultMessageWorkflowState tolakPengadaan(VWPenolakan vwPenolakan)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {

                var pengadaan = _repository.GetPengadaanByiD(vwPenolakan.PenolakanId);
                result = _workflowrepo.ApproveDokumen2(vwPenolakan.PenolakanId, pengadaan.WorkflowId.Value, UserId(), vwPenolakan.AlasanPenolakan, Reston.Helper.Model.WorkflowStatusState.REJECTED);
                if (result.data != null)
                {
                    if (result.data.DocumentStatus == Reston.Helper.Model.DocumentStatus.REJECTED)
                    {
                        _repository.TolakPengadaan(vwPenolakan.PenolakanId);
                    }
                        RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                        nRiwayatDokumen.Status = "Dokumen Pengadaan DiTolak";
                        nRiwayatDokumen.PengadaanId = vwPenolakan.PenolakanId;
                        nRiwayatDokumen.Comment = vwPenolakan.AlasanPenolakan;
                        nRiwayatDokumen.UserId = UserId();
                        _repository.AddRiwayatDokumen(nRiwayatDokumen);
                        ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(vwPenolakan.PenolakanId, pengadaan.WorkflowId.Value);
                        if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                        {
                            _repository.ChangeStatusPengadaan(vwPenolakan.PenolakanId, EStatusPengadaan.DISETUJUI,UserId());
                        }
                   
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< Reston.Helper.Util.ResultMessageWorkflowState> PenolakanWithWorkflow(Guid Id,string Note)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {

                var pengadaan = _repository.GetPengadaanByiD(Id);
                result = _workflowrepo.ApproveDokumen2(Id,pengadaan.WorkflowId.Value, UserId(), Note, Reston.Helper.Model.WorkflowStatusState.REJECTED);
                if (result.data != null)
                {
                    if (result.data.DocumentStatus == Reston.Helper.Model.DocumentStatus.REJECTED)
                    {
                        _repository.TolakPengadaan(Id);
                    }
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Dokumen Pengadaan DiTolak";
                    nRiwayatDokumen.PengadaanId = Id;
                    nRiwayatDokumen.Comment = Note;
                    nRiwayatDokumen.UserId = UserId();
                    _repository.AddRiwayatDokumen(nRiwayatDokumen);
                    ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(Id, pengadaan.WorkflowId.Value);
                    if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                    {
                        _repository.ChangeStatusPengadaan(Id, EStatusPengadaan.DISETUJUI, UserId());
                    }
                    var nextApprover = _workflowrepo.CurrentApproveUserSegOrder(Id,pengadaan.WorkflowId.Value);
                    try
                    {
                        await SendEmailToApprover(nextApprover.Id.Split('#')[1],Id);
                    }
                    catch { }
                }

            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWDokumenPengadaan> getDokumens(TipeBerkas tipe, Guid Id)
        {
            return _repository.GetListDokumenPengadaan(tipe, Id, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<List<VWRiwayatDokumen>> riwayatDokumen(Guid Id)
        {
            List<VWRiwayatDokumen> lstRiwyat = new List<VWRiwayatDokumen>();
            try
            {
                var riwayat = _repository.lstRiwayatDokumen(Id).OrderBy(d => d.ActionDate);
                foreach (var item in riwayat)
                {
                    var userx=new Userx();
                    if(item.UserId!=null)
                         userx = await userDetail(item.UserId.ToString());
                    VWRiwayatDokumen nVWRiwayatDokumen = new VWRiwayatDokumen();
                    nVWRiwayatDokumen.Id = item.Id;
                    nVWRiwayatDokumen.Nama = userx.Nama;
                    nVWRiwayatDokumen.ActionDate = item.ActionDate;
                    nVWRiwayatDokumen.Status = item.Status;
                    nVWRiwayatDokumen.Comment = item.Comment;
                    lstRiwyat.Add(nVWRiwayatDokumen);
                }
            }
            catch { 
            
            }
            return lstRiwyat;
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWDokumenPengadaan> getDokumensVendor(TipeBerkas tipe, Guid Id, int VendorId)
        {
            return _repository.GetListDokumenVendor(tipe, Id, UserId(), VendorId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWPembobotanPengadaan> getKriteriaPembobotan(Guid PengadaanId)
        {
            return _repository.getKriteriaPembobotan(PengadaanId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWPembobotanPengadaan> getKriteriaPembobotan()
        {
            return _repository.getKriteriaPembobotan(new Guid());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<PembobotanPengadaan> getPembobotanPengadaan(Guid PengadaanId)
        {
            return _repository.getPembobtanPengadaan(PengadaanId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int addPembobotanPengadaan(PembobotanPengadaan newPembobotanPengadaan)
        {
            return _repository.addPembobtanPengadaan(newPembobotanPengadaan, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int addLstPembobotanPengadaan(List<PembobotanPengadaan> newPembobotanPengadaan)
        {
            return _repository.addLstPembobtanPengadaan(newPembobotanPengadaan, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int addLstNilaiKriteriaVendor(List<PembobotanPengadaanVendor> dataLstPenilaianKriteriaVendor)
        {
            return _repository.addLstPenilaianKriteriaVendor(dataLstPenilaianKriteriaVendor, UserId());
        }



        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWPembobotanPengadaanVendor> getPembobotanNilaiVendor(Guid PengadaanId, int VendorId)
        {
            return _repository.getPembobtanPengadaanVendor(PengadaanId, VendorId, UserId());
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deleteDokumen(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteDokumen(Id);
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<int> sendMail(ViewSendEmail data)
        {
            //sending email notification

            List<VWVendor> vendors = _repository.GetVendorsByPengadaanId(data.PengadaanId.Value);
            foreach (var item in vendors)
            {
                string judulSubjek = System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_TITLE"].ToString();
                string html = "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_YTH"].ToString() + "</p>";
                html = html + "<p>" + item.Nama + "</p>";
                html = html + "<br/>";
                html = html + "<p>" + data.Surat + "</p>";
                html = html + "<br/><br/>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_FOOTER1"].ToString() + "</p>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_FOOTER2"].ToString() + "</p>";
                try
                {
                    sendMail(item.Nama, item.email, html, judulSubjek);
                }
                catch { }
            }
            var pengdaan = _repository.GetPengadaanByiD(data.PengadaanId.Value);
            foreach (var item in pengdaan.PersonilPengadaans)
            {
                string judulSubjek = System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_TITLE"].ToString();
                string html = "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_YTH"].ToString() + "</p>";
                html = html + "<p>" + item.Nama + "</p>";
                html = html + "<br/>";
                html = html + "<p>" + data.Surat + "</p>";
                html = html + "<br/><br/>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_FOOTER1"].ToString() + "</p>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_FOOTER2"].ToString() + "</p>";
                var user = await userDetail(item.PersonilId.ToString());
                try
                {
                    sendMail(item.Nama, user.Email, html, judulSubjek);
                }
                catch { }
            }
            return 1;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< int> sendMailKlarifikasi(ViewSendEmail data)
        {
            //sending email notification

            List<VWVendor> vendors = _repository.GetVendorsKlarifikasiByPengadaanId2(data.PengadaanId.Value);
            foreach (var item in vendors)
            {
                string judulSubjek = System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_TITLE"].ToString();
                string html = "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_YTH"].ToString() + "</p>";
                html = html + "<p>" + item.Nama + "</p>";
                html = html + "<br/>";
                html = html + "<p>" + data.Surat + "</p>";
                html = html + "<br/><br/>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_FOOTER1"].ToString() + "</p>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_FOOTER2"].ToString() + "</p>";
                sendMail(item.Nama, item.email, html, judulSubjek);
            }
            var pengdaan = _repository.GetPengadaanByiD(data.PengadaanId.Value);
            foreach (var item in pengdaan.PersonilPengadaans)
            {
                string judulSubjek = System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_TITLE"].ToString();
                string html = "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_YTH"].ToString() + "</p>";
                html = html + "<p>" + item.Nama + "</p>";
                html = html + "<br/>";
                html = html + "<p>" + data.Surat + "</p>";
                html = html + "<br/><br/>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_FOOTER1"].ToString() + "</p>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_PENAWARAN_FOOTER2"].ToString() + "</p>";
                var user = await userDetail(item.PersonilId.ToString());
                try
                {
                    sendMail(item.Nama, user.Email, html, judulSubjek);
                }
                catch { }
            }
            return 1;
        }

        private int sendMail(string Nama, string email, string surat, string JudulSubjek)
        {
            //sending email notification

            try
            {
                Reston.Pinata.WebService.Helper.Mailer.sendText(Nama, email,
                        JudulSubjek,
                       surat);
            }
            catch (Exception e)
            {
                return 0;
            }
            return 1;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [HttpPost]
        public ResultMessage saveKandidat(KandidatPengadaan kandidat)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                KandidatPengadaan result = _repository.saveKandidatPengadaan(kandidat, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deleteKandidat(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteKandidatPengadaan(Id, UserId());
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWKandidatPengadaan> GetKandidats(Guid PId)
        {
            return _repository.getListKandidatPengadaan(PId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage SaveReadyPersonil(Guid Id, int ready)
        {
            return _repository.saveReadyPersonil(Id, ready, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<JadwalPengadaan> GetJadwals(Guid PId)
        {
            return _repository.getListJadwalPengadaan(PId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [HttpPost]
        public List<PersonilPengadaan> GetPersonil(Guid PId)
        {
            return _repository.getListPersonilPengadaan(PId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage saveJadwal(JadwalPengadaan Jadwal)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPengadaan result = _repository.saveJadwalPengadaan(Jadwal, UserId());

                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deleteJadwal(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteJadwalPengadaan(Id, UserId());
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage savePersonil(PersonilPengadaan Personil)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                PersonilPengadaan result = _repository.savePersonilPengadaan(Personil, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deletePersonil(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deletePersonilPengadaan(Id, UserId());
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> ajukan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                decimal? RKS = _repository.getRKSDetails(Id, UserId()).Sum(d => d.hps * d.jumlah);
                var TemplateId = WorkflowTemplateId2;

                var vwpengadaan = _repository.GetPengadaanByiD(Id);
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
                if (RKS > ValueBoundAprr) WorkflowMasterTemplateDetails.Add(
                             new WorkflowMasterTemplateDetail()
                             {
                                 NameValue = "Gen.By.System",
                                 SegOrder = 3,
                                 UserId = DepHead[0]
                             });
                WorkflowMasterTemplate MasterTemplate = new WorkflowMasterTemplate()
                {
                    ApprovalType = ApprovalType.BERTINGKAT,
                    CreateBy = UserId(),
                    CreateOn = DateTime.Now,
                    DescValue = "WorkFlow Pengadaan=> " + vwpengadaan.Judul,
                    NameValue = "Generate By System ",                    
                    WorkflowMasterTemplateDetails = WorkflowMasterTemplateDetails
                };
                var resultTemplate = _workflowrepo.SaveWorkFlow(MasterTemplate,UserId());
                vwpengadaan.WorkflowId = Convert.ToInt32(resultTemplate.Id);
                #endregion

                    //masukan ke workflow
                //var resultx = _workflowrepo.PengajuanDokumen(Id, TemplateId, DocumentType);
                //if (!string.IsNullOrEmpty(resultx.Id))
                //{
                //    int result = _repository.AjukanPengadaan(Id, UserId(), await listGuidManager());
                //    if (result == 1)
                //    {
                //        respon = HttpStatusCode.OK;
                //        message = "Sukses";
                //        idx = "1";
                //    }
                //}

                if (vwpengadaan.WorkflowId != null)
                {
                    vwpengadaan.Status = EStatusPengadaan.AJUKAN;
                    var pengadaan = _repository.AddPengadaan(vwpengadaan, UserId(), await listGuidManager());
                    var pengadaanId = _repository.AjukanPengadaan(Id, UserId(), await listGuidManager());
                    respon = HttpStatusCode.OK;
                    idx = pengadaanId.ToString();
                    var resultx = _workflowrepo.PengajuanDokumen(vwpengadaan.Id, vwpengadaan.WorkflowId.Value, DocumentType);
                    if (string.IsNullOrEmpty(resultx.Id))
                    {
                        result.message = resultx.message;
                        result.Id = resultx.Id;
                        return result;
                    }
                }
                else
                {
                    vwpengadaan.Status = EStatusPengadaan.DRAFT;
                    var pengadaan = _repository.AddPengadaan(vwpengadaan, UserId(), await listGuidManager());
                    respon = HttpStatusCode.OK;
                    message = Common.SaveSukses();
                    idx= pengadaan.Id.ToString();
                }
                   
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<Reston.Helper.Util.ResultMessage> ajukanWithWorkFlow(Guid Id)
        {
            Reston.Helper.Util.ResultMessage result = new Reston.Helper.Util.ResultMessage();
            try
            {
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                _repository.AjukanPengadaan(Id, UserId(), await listGuidManager());
                decimal? RKS = _repository.getRKSDetails(Id, UserId()).Sum(d => d.hps * d.jumlah);
                var TemplateId = WorkflowTemplateId2;
                if (RKS != null)
                {
                    if (RKS > ValueBoundAprr)
                    {
                        TemplateId = WorkflowTemplateId1;
                    }
                }
                result = _workflowrepo.PengajuanDokumen(Id, TemplateId, DocumentType);
                return result;
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage saveAanwjzing(VWPelaksanaanAanwijzing Aanwijzing)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan MpelaksanaanAanwijzing = new JadwalPelaksanaan();
                MpelaksanaanAanwijzing.PengadaanId = Aanwijzing.PengadaanId;
                MpelaksanaanAanwijzing.Mulai = Common.ConvertDate(Aanwijzing.Mulai, "dd/MM/yyyy HH:mm");
                JadwalPelaksanaan result = _repository.addPelaksanaanAanwijing(MpelaksanaanAanwijzing, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetPelaksanaanAanwijzings(Guid PId)
        {
            return _repository.getPelaksanaanAanwijing(PId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetPelaksanaanPendaftaran(Guid PId)
        {
            return _repository.getPelaksanaanPendaftaran(PId);
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWKehadiranKandidatAanwijzing> getKehadiranAanwjzing(Guid PengadaanId)
        {
            List<VWKehadiranKandidatAanwijzing> result = new List<VWKehadiranKandidatAanwijzing>();
            try
            {
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                result = _repository.getKehadiranAanwijzings(PengadaanId);
                return result;

            }
            catch (Exception ex)
            {
                return result;
            }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage saveKehadiranAanwjzing(Guid KandidatId)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                KehadiranKandidatAanwijzing result = _repository.addKehadiranAanwijzing(KandidatId, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deleteKehadiranAanwjzing(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.DeleteKehadiranAanwijzing(Id, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> deleteDokumenPelaksanaan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteDokumenPelaksanaan(Id, UserId(), await isApprover());
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage updateStatus(Guid Id, EStatusPengadaan status)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.UpdateStatus(Id, status);
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage updateSubmitPenawran(VWJadwalPelaksanaan PelaksanaanSubmitPenawaran)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan Mpelaksanaan = new JadwalPelaksanaan();
                Mpelaksanaan.PengadaanId = PelaksanaanSubmitPenawaran.PengadaanId;
                Mpelaksanaan.Mulai = Common.ConvertDate(PelaksanaanSubmitPenawaran.Mulai, "dd/MM/yyyy HH:mm");
                Mpelaksanaan.Sampai = Common.ConvertDate(PelaksanaanSubmitPenawaran.Sampai, "dd/MM/yyyy HH:mm");

                JadwalPelaksanaan result = _repository.AddPelaksanaanSubmitPenawaran(Mpelaksanaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetSubmitPenawran(Guid PId)
        {
            return _repository.getPelaksanaanSubmitPenawaran(PId);
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetBukaAmplop(Guid PId)
        {
            return _repository.getPelaksanaanBukaAmplop(PId);
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage updateBukaAmplop(VWJadwalPelaksanaan PelaksanaanSubmitPenawaran)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan Mpelaksanaan = new JadwalPelaksanaan();
                Mpelaksanaan.PengadaanId = PelaksanaanSubmitPenawaran.PengadaanId;
                Mpelaksanaan.Mulai = Common.ConvertDate(PelaksanaanSubmitPenawaran.Mulai, "dd/MM/yyyy HH:mm");
                Mpelaksanaan.Sampai = Common.ConvertDate(PelaksanaanSubmitPenawaran.Sampai, "dd/MM/yyyy HH:mm");

                JadwalPelaksanaan result = _repository.AddPelaksanaanBukaAmplop(Mpelaksanaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }


        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetPenilaian(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getPelaksanaanPenilaian(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [HttpPost]
        public ResultMessage updatePenilaian(VWJadwalPelaksanaan PelaksanaanPenilaian)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan Mpelaksanaan = new JadwalPelaksanaan();
                Mpelaksanaan.PengadaanId = PelaksanaanPenilaian.PengadaanId;
                Mpelaksanaan.Mulai = Common.ConvertDate(PelaksanaanPenilaian.Mulai, "dd/MM/yyyy HH:mm");
                Mpelaksanaan.Sampai = Common.ConvertDate(PelaksanaanPenilaian.Sampai, "dd/MM/yyyy HH:mm");

                JadwalPelaksanaan result = _repository.AddPelaksanaanPenilaian(Mpelaksanaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetKlarifikasi(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getPelaksanaanKlarifikasi(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage updateKlarifikasi(VWJadwalPelaksanaan PelaksanaanKlarifikasi)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan Mpelaksanaan = new JadwalPelaksanaan();
                Mpelaksanaan.PengadaanId = PelaksanaanKlarifikasi.PengadaanId;
                Mpelaksanaan.Mulai = Common.ConvertDate(PelaksanaanKlarifikasi.Mulai, "dd/MM/yyyy HH:mm");
                Mpelaksanaan.Sampai = Common.ConvertDate(PelaksanaanKlarifikasi.Sampai, "dd/MM/yyyy HH:mm");

                JadwalPelaksanaan result = _repository.AddPelaksanaanKlarifikasi(Mpelaksanaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetJadwalPelaksanaan(Guid PId,EStatusPengadaan status)
        {
            return _repository.GetJadwalPelaksanaan(PId, UserId(), status);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage UpdateJadwalPelaksanaan(VWJadwalPelaksanaan Pelaksanaan)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan Mpelaksanaan = new JadwalPelaksanaan();
                Mpelaksanaan.PengadaanId = Pelaksanaan.PengadaanId;
                if (!string.IsNullOrEmpty(Pelaksanaan.Mulai))
                    Mpelaksanaan.Mulai = Common.ConvertDate(Pelaksanaan.Mulai, "dd/MM/yyyy HH:mm");
                if (!string.IsNullOrEmpty(Pelaksanaan.Sampai))
                    Mpelaksanaan.Sampai = Common.ConvertDate(Pelaksanaan.Sampai, "dd/MM/yyyy HH:mm");
                Mpelaksanaan.statusPengadaan = Pelaksanaan.status;
                JadwalPelaksanaan result = _repository.SaveJadwalPelaksanaan(Mpelaksanaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }

            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [HttpPost]
        public JadwalPelaksanaan GetPemenang(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getPelaksanaanPemenang(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage updatePemenang(VWJadwalPelaksanaan PelaksanaanPemenang)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                JadwalPelaksanaan Mpelaksanaan = new JadwalPelaksanaan();
                Mpelaksanaan.PengadaanId = PelaksanaanPemenang.PengadaanId;
                if (!string.IsNullOrEmpty(PelaksanaanPemenang.Mulai))
                    Mpelaksanaan.Mulai = Common.ConvertDate(PelaksanaanPemenang.Mulai, "dd/MM/yyyy HH:mm");
                if (!string.IsNullOrEmpty(PelaksanaanPemenang.Sampai))
                    Mpelaksanaan.Sampai = Common.ConvertDate(PelaksanaanPemenang.Sampai, "dd/MM/yyyy HH:mm");

                JadwalPelaksanaan result = _repository.AddPelaksanaanPemenang(Mpelaksanaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage saveKualifikasiKandidat(KualifikasiKandidat kualifikasi)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                KualifikasiKandidat result = _repository.addKualifikasiKandidat(kualifikasi, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deleteKualifikasiKandidat(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteKualifikasiKandidat(Id, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addPersetujuanBukaAmplop(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                PersetujuanBukaAmplop result = _repository.AddPersetujuanBukaAmplop(Id, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWPErsetujuanBukaAmplop> getPersetujuanBukaAmplop(Guid PengadaanId)
        {
            List<VWPErsetujuanBukaAmplop> result = new List<VWPErsetujuanBukaAmplop>();
            try
            {
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                result = _repository.getPersetujuanBukaAmplop(PengadaanId, UserId());
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<ViewPengadaan> getPengadaanForRekananList(int start, int length, EGroupPengadaan group)
        {
            //Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            //List<string> roles = ((ClaimsIdentity)User.Identity).Claims
            //    .Where(c => c.Type == ClaimTypes.Role)
            //    .Select(c => c.Value).ToList();
            var lstPerhatianPengadaan = _repository.GetPengadaansForRekanan(start, length, UserId(), Roles(), group);
            return lstPerhatianPengadaan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<ViewPengadaan> getPengadaanForRekananListById(int idvendor, int start, int length)
        {
            var s = _repository.GetVendorById(idvendor);
            var lstPerhatianPengadaan = _repository.GetPengadaansForRekanan(start, length, s.Owner, new List<string>(), EGroupPengadaan.ALL);
            return lstPerhatianPengadaan;
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public string ViewFile(Guid Id)
        {
            DokumenPengadaan d = _repository.GetDokumenPengadaan(Id);
            if (d == null)
                return "";
            string bUrl = "/ViewerNotSupported.html?id=";
            if (d.ContentType.Contains("image/"))
                bUrl = "/ImageViewer2.html?id=";
            else if (d.ContentType.Contains("application/pdf"))
                bUrl = "/api/PengadaanE/OpenFile?file=";
            return bUrl + Id;
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage OpenFile(Guid Id)
        {
            DokumenPengadaan d = _repository.GetDokumenPengadaan(Id);
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_DOKUMEN_PATH + d.File;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(d.ContentType);

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = d.File
            };

            return result;
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int cekState(Guid Id, string tipe)
        {
            if (tipe == "Disetujui")
                return _repository.cekStateDiSetujui(Id);
            if (tipe == PengadaanConstants.Jadwal.Aanwijzing)
                return _repository.cekStateAanwijzing(Id);
            else if (tipe == PengadaanConstants.Jadwal.PengisianHarga)
                return _repository.cekStateSubmitPenawaran(Id);
            else if (tipe == PengadaanConstants.Jadwal.BukaAmplop)
                return _repository.cekStateBukaAmplop(Id);
            else return 0;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public DataTableRksRekanan getRksRekanan(Guid Id)
        {
            List<VWRKSDetailRekanan> rks = _repository.getRKSForRekanan(Id, UserId());
            DataTableRksRekanan datatable = new DataTableRksRekanan();
            datatable.recordsTotal = rks.Count();
            datatable.data = rks;
            return datatable;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public DataTableRksRekanan getRKSForKlarifikasiRekanan(Guid Id)
        {
            List<VWRKSDetailRekanan> rks = _repository.getRKSForKlarifikasiRekanan(Id, UserId());
            DataTableRksRekanan datatable = new DataTableRksRekanan();
            datatable.recordsTotal = rks.Count();
            datatable.data = rks;
            return datatable;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public DataTableRksRekanan getRKSForKlarifikasiLanjutanRekanan(Guid Id)
        {
            List<VWRKSDetailRekanan> rks = _repository.getRKSForKlarifikasiLanjutanRekanan(Id, UserId());
            DataTableRksRekanan datatable = new DataTableRksRekanan();
            datatable.recordsTotal = rks.Count();
            datatable.data = rks;
            return datatable;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addHargaRekanan(List<VWRKSDetailRekanan> hargaRekanan, Guid PengadaanId)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var result = _repository.addHargaRekanan(hargaRekanan, PengadaanId, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result == null ? "0" : "1";

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananSubmitHarga> GetRekananSubmit(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananSubmit(PId, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetRekananPenilaian(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananPenilaian(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetRekananPenilaian2(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananPenilaian2(PId, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetRekananPenilaianByVendor(Guid PId, int VendorId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListPenilaianByVendor(PId, UserId(), VendorId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public VWRKSVendors getRKSPenilaian(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getRKSPenilaian(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public VWRKSVendors getRKSPenilaian2(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getRKSPenilaian2(PId, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addPilihKandidat(PelaksanaanPemilihanKandidat oPelaksanaanPemilihanKandidat)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var result = _repository.addKandidatPilihan(oPelaksanaanPemilihanKandidat, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addPemenang(PemenangPengadaan oPemenangPengadaan)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var result = _repository.addPemenangPengadaan(oPemenangPengadaan, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deletePemenang(PemenangPengadaan oPemenangPengadaan)
        {
           return _repository.DeletePemenang(oPemenangPengadaan, UserId());              
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deletePilihKandidat(PelaksanaanPemilihanKandidat oPelaksanaanPemilihanKandidat)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteKandidatPilihan(oPelaksanaanPemilihanKandidat, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.ToString();

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        public DataTableRksRekanan getRksKlarifikasiRekanan(Guid Id)
        {
            List<VWRKSDetailRekanan> rks = _repository.getRKSForKlarifikasiRekanan(Id, UserId());
            DataTableRksRekanan datatable = new DataTableRksRekanan();
            datatable.recordsTotal = rks.Count();
            datatable.data = rks;
            return datatable;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        public DataTableRksRekanan getRksKlarifikasiLanjutanRekanan(Guid Id)
        {
            List<VWRKSDetailRekanan> rks = _repository.getRKSForKlarifikasiLanjutanRekanan(Id, UserId());
            DataTableRksRekanan datatable = new DataTableRksRekanan();
            datatable.recordsTotal = rks.Count();
            datatable.data = rks;
            return datatable;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addHargaKlarifikasiRekanan(List<VWRKSDetailRekanan> hargaRekanan, Guid PengadaanId)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var result = _repository.addHargaKlarifikasiRekanan(hargaRekanan, PengadaanId, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result == null ? "0" : "1";

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addHargaKlarifikasiLanjutanRekanan(List<VWRKSDetailRekanan> hargaRekanan, Guid PengadaanId)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                // Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var result = _repository.addHargaKlarifikasiLanjutanRekanan(hargaRekanan, PengadaanId, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result == null ? "0" : "1";

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRiwayatPengadaan> getRiwayatDokumenVendor()
        {
            return _repository.GetRiwayatDokumenForVendor(UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananSubmitHarga> GetRekananKlarifikasiSubmit(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananKlarifikasiSubmit(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananSubmitHarga> GetRekananKlarifikasiSubmitLanjutan(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananKlarifikasiLanjutSubmit(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetRekananKlarifikasiPenilaian(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananKlarifikasiPenilaian(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetRekananKlarifikasiPenilaianLanjutan(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananKlarifikasiPenilaianLanjutan(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetPemenangPengadaan(Guid PId)
        {

            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getPemenangPengadaan(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetAllKandidatPengadaan(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getKandidatPengadaan(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public VWRKSVendors getRKSKlarifikasi(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getRKSKlarifikasi(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public VWRKSVendors getRKSKlarifikasiLanjutan(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getRKSKlarifikasiLanjutan(PId, UserId());
        }

        [Authorize]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public VWRKSVendors getRKSKlarifikasiPenilaianVendor(Guid PId, int VendorId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getRKSKlarifikasiPenilaianVendor(PId, UserId(), VendorId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult GetAllProduk(string term, Guid pengadaanId)
        {
            List<vwProduk> lp = _repository.GetAllProduk(term);
            List<VWProdukSummary> lsm = (from a in lp

                                         select new VWProdukSummary()
                                         {
                                             Id = a.Id,
                                             Nama = a.Nama,
                                             Price = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Harga : 0,
                                             Region = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Region : "",
                                             LastUpdate = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Tanggal.ToLocalTime().ToShortDateString() : "",
                                             Source = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Sumber : "",
                                             Satuan = a.Satuan,
                                             Spesifikasi = a.Spesifikasi
                                         }).ToList();
            return Json(new { aaData = lsm });
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult GetItemByRegion(string term, string Region)
        {
            List<vwProduk> lp = _repository.GetItemByRegion(term, Region);
            List<VWProdukSummary> lsm = (from a in lp
                                         select new VWProdukSummary()
                                         {
                                             Id = a.Id,
                                             Nama = a.Nama,
                                             Price = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Harga : 0,
                                             Region = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Region : "",
                                             LastUpdate = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Tanggal.ToLocalTime().ToShortDateString() : "",
                                             Source = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Sumber : "",
                                             Satuan = a.Satuan,
                                             Spesifikasi = a.Spesifikasi
                                         }).ToList();

            //var lsm = new List<VWProdukSummary>();
            //foreach (var item in lp)
            //{               
            //    foreach (var itemHarga in item.RiwayatHarga)
            //    {
            //        var VWProdukSummary = new VWProdukSummary();
            //        VWProdukSummary.Id = item.Id;
            //        VWProdukSummary.Nama = item.Nama;
            //    }
            //}

            return Json(new { aaData = lsm });
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult GetAllSatuan(string term)
        {
            List<vwProduk> lp = _repository.GetAllSatuan(term);
            var lsm = (from a in lp

                       select new
                       {
                           Satuan = a.Satuan
                       }).ToList();
            return Json(new { aaData = lsm });
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public dataVendors GetVendors(string tipe, int start, string filter, string status, int limit)
        {
            var lv =
             _repository.GetVendors(tipe != null ? (ETipeVendor)Enum.Parse(typeof(ETipeVendor), tipe) : ETipeVendor.NONE,
                start, filter, status != null ? (EStatusVendor)Enum.Parse(typeof(EStatusVendor), status) : EStatusVendor.NONE
                 , limit);
            return lv;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public ResultMessage arsipkan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.arsipkan(Id, UserId());
                if (result == 1)
                {
                    respon = HttpStatusCode.OK;
                    message = "Sukses";
                    idx = "1";
                }
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        
        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> addBeritaAcaraSpk(VWBeritaAcara vwpengadaan)
        {
            var pemenang = _repository.getPemenangPengadaan(vwpengadaan.PengadaanId.Value, UserId());
            List<BeritaAcara> lstBeritaAcara = new List<BeritaAcara>();
            List<Spk> lstSpk = new List<Spk>();
            try
            {
                foreach (var item in pemenang)
                {
                    //load berita acara
                    BeritaAcara beritaAcara = new BeritaAcara();
                    beritaAcara.PengadaanId = vwpengadaan.PengadaanId;
                    try
                    {
                        if (!string.IsNullOrEmpty(vwpengadaan.tanggal))
                        {
                            beritaAcara.tanggal = Common.ConvertDate(vwpengadaan.tanggal, "dd/MM/yyyy");
                        }
                    }
                    catch { }
                    beritaAcara.Tipe = TipeBerkas.SuratPerintahKerja;
                    beritaAcara.VendorId = item.VendorId;
                    var SaveBeritaAcara = _repository.addBeritaAcara(beritaAcara, UserId());
                    if(SaveBeritaAcara!=null)
                        lstBeritaAcara.Add(SaveBeritaAcara);

                    //load spk
                    Spk spk = new Spk();
                    spk.CreateBy = UserId();
                    spk.CreateOn = DateTime.Now;
                    var getDokumen = _repository.GetDokumenPengadaanSpk(vwpengadaan.PengadaanId.Value, item.VendorId.Value);
                    if (getDokumen != null)
                    {
                        spk.DokumenPengadaanId = getDokumen.Id;
                    }
                    spk.PemenangPengadaanId = item.Id;
                    spk.TanggalSPK = SaveBeritaAcara.tanggal;
                    spk.StatusSpk = StatusSpk.Aktif;
                    spk.Title = "SPK Pertama Untuk Pengadaan " + SaveBeritaAcara.Pengadaan.Judul;
                    spk.NoSPk = SaveBeritaAcara.NoBeritaAcara;
                    var SaveSpk = _spkrepo.saveSpkPertam(spk, UserId());
                    if(SaveSpk!=null)
                        lstSpk.Add(SaveSpk);
                }
                return new ResultMessage()
                {
                    status = HttpStatusCode.OK,
                    message = Common.SaveSukses()
                };
            }
            catch(Exception ex)
            {
                if (lstBeritaAcara.Count() > 0)
                {
                    foreach (var item in lstBeritaAcara)
                    {
                        _repository.DeleteBeritaAcara(item.Id,UserId());
                    }
                }
                if (lstSpk.Count() > 0)
                {
                    foreach (var item in lstSpk)
                    {
                        _spkrepo.Delete(item.Id, UserId());
                    }
                }
                return new ResultMessage()
                {
                    status = HttpStatusCode.ExpectationFailed,
                    message = ex.ToString()
                };
            }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage addBeritaAcaraNota(VWBeritaAcara vwpengadaan)
        {
            var pemenang = _repository.getPemenangPengadaan(vwpengadaan.PengadaanId.Value, UserId());
            List<BeritaAcara> lstBeritaAcara = new List<BeritaAcara>();
            
            try
            {
                foreach (var item in pemenang)
                {
                    //load berita acara
                    BeritaAcara beritaAcara = new BeritaAcara();
                    beritaAcara.PengadaanId = vwpengadaan.PengadaanId;
                    try
                    {
                        if (!string.IsNullOrEmpty(vwpengadaan.tanggal))
                        {
                            beritaAcara.tanggal = Common.ConvertDate(vwpengadaan.tanggal, "dd/MM/yyyy");
                        }
                    }
                    catch { }
                    beritaAcara.Tipe = TipeBerkas.BeritaAcaraPenentuanPemenang;
                    beritaAcara.VendorId = item.VendorId;
                    var SaveBeritaAcara = _repository.addBeritaAcara(beritaAcara, UserId());
                    if (SaveBeritaAcara != null)
                        lstBeritaAcara.Add(SaveBeritaAcara); 
                }
                return new ResultMessage()
                {
                    status = HttpStatusCode.OK,
                    message = Common.SaveSukses()
                };
            }
            catch (Exception ex)
            {
                if (lstBeritaAcara.Count() > 0)
                {
                    foreach (var item in lstBeritaAcara)
                    {
                        _repository.DeleteBeritaAcara(item.Id, UserId());
                    }
                }
               
                return new ResultMessage()
                {
                    status = HttpStatusCode.ExpectationFailed,
                    message = ex.ToString()
                };
            }
        }


        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> addBeritaAcara(VWBeritaAcara vwpengadaan)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string id = "";
            try
            {
                BeritaAcara newBeritaAcara = new BeritaAcara();
                newBeritaAcara.PengadaanId = vwpengadaan.PengadaanId;
                newBeritaAcara.Tipe = vwpengadaan.Tipe;
                if (!string.IsNullOrEmpty(vwpengadaan.tanggal))
                {
                    newBeritaAcara.tanggal = Common.ConvertDate(vwpengadaan.tanggal, "dd/MM/yyyy");
                }
                var oPengadaan = _repository.GetPengadaan(vwpengadaan.PengadaanId.Value, UserId(), await isApprover());
                if (_repository.CekBukaAmplopTahapan(vwpengadaan.PengadaanId.Value) == 0 && oPengadaan.Status == EStatusPengadaan.BUKAAMPLOP)
                {
                    return new ResultMessage { Id = "0", message = "Belum Semua Pihak Buka Amplop!", status = HttpStatusCode.OK };
                }

                BeritaAcara result = _repository.addBeritaAcara(newBeritaAcara, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                id = result.Id.ToString();
            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = id;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWBeritaAcaraEnd> GetBeritaAcara(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getBeritaAcara(PId, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int nextToState(Guid Id, string tipe)
        {
            if (tipe == PengadaanConstants.Jadwal.PengisianHarga)
                return _repository.nextToState(Id, UserId(), EStatusPengadaan.SUBMITPENAWARAN);
            else if (tipe == PengadaanConstants.Jadwal.Penilaian)
            {
                if (_repository.CekBukaAmplop(Id) == 0) return 0;
                return _repository.nextToState(Id, UserId(), EStatusPengadaan.PENILAIAN);
            }

            else if (tipe == PengadaanConstants.Jadwal.Klarifikasi)
                return _repository.nextToState(Id, UserId(), EStatusPengadaan.KLARIFIKASI);
            else if (tipe == PengadaanConstants.Jadwal.PenentuanPemenang)
                return _repository.nextToState(Id, UserId(), EStatusPengadaan.PEMENANG);
            else return 0;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int nextStateAndSchelud(Guid Id,string dari,string sampai)
        {
            DateTime? dtDari = null;
            DateTime? dtSamapi=null;
            if (!string.IsNullOrEmpty(sampai))
            {
                try
                {
                    dtDari = Common.ConvertDate(dari, "dd/MM/yyyy HH:mm");
                }
                catch
                {
                    dtDari = null;
                }
            }

            if (!string.IsNullOrEmpty(sampai))
            {
                try
                {
                    dtSamapi = Common.ConvertDate(sampai, "dd/MM/yyyy HH:mm");
                }
                catch
                {
                    dtSamapi = null;
                }
            }

            ViewPengadaan oPengadaan = _repository.GetPengadaan(Id, UserId(), 0);
            int NextStatusPengadaan = (int)oPengadaan.Status;
            if (oPengadaan.Status == EStatusPengadaan.DISETUJUI && oPengadaan.AturanPengadaan=="Pengadaan Tertutup")
                NextStatusPengadaan = (int)EStatusPengadaan.AANWIJZING;
            else NextStatusPengadaan = nextStatusMaping(oPengadaan.Status.Value); //NextStatusPengadaan + 1;

            if (oPengadaan.Status == EStatusPengadaan.KLARIFIKASI)
            {
                var cekTambahTahapan = _repository.getTahapan(oPengadaan.Id);
                if (cekTambahTahapan.Where(d=>d.Status==EStatusPengadaan.KLARIFIKASILANJUTAN).Count()>0 )
                {
                    NextStatusPengadaan = (int)EStatusPengadaan.KLARIFIKASILANJUTAN;
                }
                if (cekTambahTahapan.Where(d => d.Status == EStatusPengadaan.PENILAIAN).Count() > 0 && cekTambahTahapan.Where(d => d.Status == EStatusPengadaan.KLARIFIKASILANJUTAN).Count()==0)
                {
                    NextStatusPengadaan = (int)EStatusPengadaan.PENILAIAN;
                }
                if (cekTambahTahapan.Where(d => d.Status == EStatusPengadaan.KLARIFIKASILANJUTAN).Count() == 0 && cekTambahTahapan.Where(d => d.Status == EStatusPengadaan.PENILAIAN).Count() == 0)
                {
                        NextStatusPengadaan = (int)EStatusPengadaan.PEMENANG;
                }

                //else {
                //    NextStatusPengadaan = (int)EStatusPengadaan.PEMENANG;
                //}
            }

            if (oPengadaan.Status == EStatusPengadaan.KLARIFIKASILANJUTAN)
            {
                var cekTambahTahapan = _repository.getTahapan(oPengadaan.Id);
                if (cekTambahTahapan.Where(d => d.Status == EStatusPengadaan.PENILAIAN).Count() > 0)
                {
                    NextStatusPengadaan = (int)EStatusPengadaan.PENILAIAN;
                }
                else
                {
                    NextStatusPengadaan = (int)EStatusPengadaan.PEMENANG;
                }
            }

            return _repository.nextToStateWithChangeScheduldDate(Id, UserId(), (EStatusPengadaan)NextStatusPengadaan, dtDari,dtSamapi);
        }

        private int nextStatusMaping(EStatusPengadaan status)
        {
            switch (status)
            {
                case (EStatusPengadaan.DISETUJUI): return (int)EStatusPengadaan.AANWIJZING;
                case (EStatusPengadaan.AANWIJZING): return (int)EStatusPengadaan.SUBMITPENAWARAN;
                case (EStatusPengadaan.SUBMITPENAWARAN): return (int)EStatusPengadaan.BUKAAMPLOP;
                case (EStatusPengadaan.BUKAAMPLOP): return (int)EStatusPengadaan.KLARIFIKASI;
                case (EStatusPengadaan.KLARIFIKASI): return (int)EStatusPengadaan.KLARIFIKASILANJUTAN;
                case (EStatusPengadaan.KLARIFIKASILANJUTAN): return (int)EStatusPengadaan.PENILAIAN;
                case (EStatusPengadaan.PENILAIAN): return (int)EStatusPengadaan.PEMENANG;
                default: return -1;
            }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int CurrentStatePengadaan(Guid Id)
        {
            ViewPengadaan oPengadaan = _repository.GetPengadaan(Id, UserId(), 0);

            return (int)oPengadaan.Status;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public string statusVendor(Guid Id)
        {
            return _repository.statusVendor(Id, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_manager)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int pembatalan(VWPembatalanPengadaan vwPembatalan)
        {
            return _repository.PembatalanPengadaan(vwPembatalan, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int isNotaUploaded(Guid Id)
        {
            return _repository.isNotaUploaded(Id, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int isSpkUploaded(Guid Id)
        {
            return _repository.isSpkUploaded(Id, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public string getAlasanPenolakan(Guid Id)
        {
            PenolakanPengadaan oPenolakanPengadaan = _repository.GetPenolakanMessage(Id, UserId());
            return oPenolakanPengadaan == null ? "" : oPenolakanPengadaan.Keterangan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public string getAlasanDiBatalkan(Guid Id)
        {
            PembatalanPengadaan oPembatalanPengadaan = _repository.GetPembatalanPengadaan(Id, UserId());
            return oPembatalanPengadaan == null ? "" : oPembatalanPengadaan.Keterangan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public Vendor GetPemenangVendor(Guid Id)
        {
            return _repository.GetPemenang(Id, UserId());
        }

        public int logOut()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return 1;
        }

        public void Signout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            Redirect("");
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<IHttpActionResult> UploadFile(string tipe, Guid id)
        {
            int vendorId = 0;
            try
            {
                vendorId = Convert.ToInt32(HttpContext.Current.Request["vendorId"].ToString());
            }
            catch { }
          
            var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            bool isSavedSuccessfully = true;
            string filePathSave = FILE_DOKUMEN_PATH;//+id ;
            string fileName = tipe;
            if (Directory.Exists(uploadPath + filePathSave) == false)
            {
                Directory.CreateDirectory(uploadPath + filePathSave);
            }
            TipeBerkas t = (TipeBerkas)Enum.Parse(typeof(TipeBerkas), tipe);
            if (t == TipeBerkas.SuratPerintahKerja)
            {
                if(_repository.CekPersetujuanPemenang(id,UserId()).status!=HttpStatusCode.OK){
                    return InternalServerError();
                }

            }

            var s = await Request.Content.ReadAsStreamAsync();
            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);
            string contentType = "";
            Guid newGuid = Guid.NewGuid();
            long sizeFile = 0;
            foreach (var file in provider.Contents)
            {
                string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                string extension = filename.Substring(filename.IndexOf(".") + 1, filename.Length - filename.IndexOf(".") - 1);
                byte[] buffer = await file.ReadAsByteArrayAsync();
                contentType = file.Headers.ContentType.ToString();
                sizeFile = buffer.Length;
                filePathSave += tipe + "-" + newGuid.ToString() + "." + extension;
                fileName += "-" + newGuid.ToString() + "." + extension;
                // var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //new PhysicalFileSystem(@"..\Reston.Pinata\WebService\Upload\Vendor\Dokumen\");

                try
                {
                    FileStream fs = new FileStream(uploadPath.ToString() + filePathSave, FileMode.CreateNew);
                    await fs.WriteAsync(buffer, 0, buffer.Length);

                    fs.Close();

                    isSavedSuccessfully = true;
                }
                catch (Exception e)
                {
                    return InternalServerError();
                }
            }
            Guid DokumenId = Guid.NewGuid();
            //TipeBerkas t = (TipeBerkas)Enum.Parse(typeof(TipeBerkas), tipe);
            DokumenPengadaan dokumen = new DokumenPengadaan
            {
                File = fileName,
                Tipe = t,
                ContentType = contentType,
                PengadaanId = id,
                SizeFile = sizeFile                
            };
            if (vendorId > 0)
            {
                dokumen = new DokumenPengadaan
                {
                    File = fileName,
                    Tipe = t,
                    ContentType = contentType,
                    PengadaanId = id,
                    SizeFile = sizeFile,
                    VendorId=vendorId
                };
            }

            if (isSavedSuccessfully)
            {
                try
                {
                    DokumenPengadaan dokumenUpdate = _repository.saveDokumenPengadaan(dokumen, UserId());
                    if (t == TipeBerkas.SuratPerintahKerja)
                    {
                        //  int x = _repository.UpdateStatus(id, EStatusPengadaan.SUBMITPENAWARAN);
                        
                    }
                    return Json(dokumen.Id);
                }
                catch (Exception ex)
                {
                    return Json("00000000-0000-0000-0000-000000000000");
                }
            }

            return Json(dokumen.Id);
        }

        [Authorize]
        public string CopyFile(string uidFileName)
        {
            //if (d == null) return false;
            var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string fileLoc = uploadPath + FILE_TEMP_PATH + uidFileName;
            string filePathSave = FILE_PENGADAAN_PATH + @"\";
            if (Directory.Exists(uploadPath + filePathSave) == false)
            {
                Directory.CreateDirectory(uploadPath + filePathSave);
            }
            try
            {
                FileInfo fi = new FileInfo(fileLoc);
                fi.MoveTo(uploadPath + filePathSave + uidFileName);
            }
            catch (IOException ei)
            {
                return "";
            }
            return filePathSave + uidFileName;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< IHttpActionResult> List()
        {
            string search = HttpContext.Current.Request["search[value]"].ToString();
            int length = Convert.ToInt32(HttpContext.Current.Request["length"].ToString());
            int start = Convert.ToInt32(HttpContext.Current.Request["start"].ToString());
            int more = Convert.ToInt32(HttpContext.Current.Request["more"].ToString());
            int spk = Convert.ToInt32(HttpContext.Current.Request["spk"].ToString());
            EStatusPengadaan status = (EStatusPengadaan)Convert.ToInt32(HttpContext.Current.Request["status"].ToString());
            var listUserApprover = await listUser(IdLdapConstants.Roles.pRole_approver);
            var data = _repository.List(search, start, length, status, more, spk, listUserApprover,UserId());
            
            foreach (var item in data.data)
            {
                if(item.Status==EStatusPengadaan.AJUKAN)
                    if (item.WorkflowTemplateId != null && item.WorkflowTemplateId != 0)
                    {
                        List<Reston.Helper.Model.ViewWorkflowModel> getDoc = _workflowrepo.ListDocumentWorkflow(UserId(), item.WorkflowTemplateId.Value, Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentType, 0, 0);
                        if (getDoc.Where(d => d.CurrentUserId == UserId()).FirstOrDefault() != null) item.Approver = 1;
                        item.lastApprover = _workflowrepo.isLastApprover(item.Id, item.WorkflowTemplateId.Value).Id;
                        
                    }
                if(item.StatusPersetujuanPemenang==StatusPengajuanPemenang.PENDING)
                    if (item.WorkflowPersetujuanPemenangTemplateId != null && item.WorkflowPersetujuanPemenangTemplateId != 0)
                    {
                        List<Reston.Helper.Model.ViewWorkflowModel> getDoc = _workflowrepo.ListDocumentWorkflow(UserId(), item.WorkflowPersetujuanPemenangTemplateId.Value, Reston.Helper.Model.DocumentStatus.PENGAJUAN, DocumentTypePemenang, 0, 0);
                        if (getDoc.Where(d => d.CurrentUserId == UserId()).FirstOrDefault() != null) item.ApproverPersetujuanPemenang = 1;
                        item.lastApproverPersetujuanPemenang = _workflowrepo.isLastApprover(item.IdPersetujuanPemanang.Value, item.WorkflowPersetujuanPemenangTemplateId.Value).Id;
                        if (item.WorkflowPersetujuanPemenangTemplateId != null)
                        {
                            var PrevUserId = _workflowrepo.PrevApprover(item.IdPersetujuanPemanang.Value, item.WorkflowPersetujuanPemenangTemplateId.Value).Id;
                            if (PrevUserId != "0")
                                item.PrevApproverPersetujuan = (await userDetail(PrevUserId)).Nama;
                            else item.PrevApproverPersetujuan = "";
                            var NextUserId = _workflowrepo.NextApprover(item.IdPersetujuanPemanang.Value, item.WorkflowPersetujuanPemenangTemplateId.Value).Id;
                            if (NextUserId != "0")
                                item.NextApproverPersetujuan = (await userDetail(NextUserId)).Nama;
                            else item.NextApproverPersetujuan = "";
                        }
                    }
                if (item.WorkflowTemplateId != null)
                {
                    var PrevUserId = _workflowrepo.PrevApprover(item.Id, item.WorkflowTemplateId.Value).Id;
                    if (PrevUserId != "0")
                        item.PrevApprover = (await userDetail(PrevUserId)).Nama;
                    else item.PrevApprover = "";
                    var NextUserId = _workflowrepo.NextApprover(item.Id, item.WorkflowTemplateId.Value).Id;
                    if (NextUserId != "0")
                        item.NextApprover = (await userDetail(NextUserId)).Nama;
                    else item.NextApprover = "";
                }
            }

            return Json(data);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< IHttpActionResult> ListCount()
        {
            var userApprover = await listUser(IdLdapConstants.Roles.pRole_approver);
            return Json(_repository.ListCount(UserId(), userApprover));
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<IHttpActionResult> ListUsers()
        {
            string search = HttpContext.Current.Request["search[value]"].ToString();
            int length = Convert.ToInt32(HttpContext.Current.Request["length"].ToString());
            int start = Convert.ToInt32(HttpContext.Current.Request["start"].ToString());
            var client = new HttpClient();
            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + length + "&filter=" + "&name=" + search));
            string masterDataContent = await reply.Content.ReadAsStringAsync();
            var masterData = JsonConvert.DeserializeObject<DataPageUsers>(masterDataContent);
            DataTableUsers dt = new DataTableUsers();
            dt.recordsTotal = masterData.totalRecord == null ? 0 : masterData.totalRecord.Value;
            dt.recordsFiltered = masterData.totalRecord == null ? 0 : masterData.totalRecord.Value;
            dt.data = masterData.Users;
            return Json(dt);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task< Reston.Helper.Util.ResultMessageWorkflowState> persetujuanWithNextApprover(Guid id, string Note,Guid userId)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {
                var oPengadaan = _repository.GetPengadaanByiD(id);               

                WorkflowMasterTemplateDetail oDetailTempalte = new WorkflowMasterTemplateDetail();
                oDetailTempalte.UserId = userId;
                oDetailTempalte.NameValue = "Ditambah Oleh: " + UserId();
                oDetailTempalte.WorkflowMasterTemplateId = oPengadaan.WorkflowId.Value;
                var oAddMasterTemplateDetail = _workflowrepo.AddMasterTemplateDetail(oPengadaan.WorkflowId.Value, oDetailTempalte);
                if (!string.IsNullOrEmpty(oAddMasterTemplateDetail.Id))
                {

                    var pengadaan = _repository.GetPengadaanByiD(id);
                    result = _workflowrepo.ApproveDokumen2(id, pengadaan.WorkflowId.Value, UserId(), "", Reston.Helper.Model.WorkflowStatusState.APPROVED);
                    if (!string.IsNullOrEmpty(result.Id))
                    {
                        RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                        nRiwayatDokumen.Status = "Dokumen Pengadaan DiSetujui";
                        nRiwayatDokumen.Comment = Note;
                        nRiwayatDokumen.PengadaanId = id;
                        nRiwayatDokumen.UserId = UserId();
                        _repository.AddRiwayatDokumen(nRiwayatDokumen);
                        ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(id, pengadaan.WorkflowId.Value);
                        if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                        {
                            #region BuatAtauUpdateTamplate
                            var vwpengadaan = _repository.GetPengadaanByiD(id);
                            var DepHead = await listHead();
                            var DepManager = await listGuidManager();
                            var Direksi = await listUser(IdLdapConstants.Roles.pRole_direksi);
                            var Dirut = await listUser(IdLdapConstants.Roles.pRole_dirut);
                            decimal? RKS = _repository.getRKSDetails(id, UserId()).Sum(d => d.hps * d.jumlah);
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
                            if (RKS > ValueBoundAprr) WorkflowMasterTemplateDetails.Add(
                                         new WorkflowMasterTemplateDetail()
                                         {
                                             NameValue = "Gen.By.System",
                                             SegOrder = 3,
                                             UserId = DepHead[0]
                                         });
                            if (Direksi.Count() > 0)
                                if (RKS > ValueBoundDireksiAprr)
                                {
                                    var lasOrder = WorkflowMasterTemplateDetails.LastOrDefault().SegOrder;
                                    WorkflowMasterTemplateDetails.Add(
                                        new WorkflowMasterTemplateDetail()
                                        {
                                            NameValue = "Gen.By.System",
                                            SegOrder = lasOrder + 1,
                                            UserId = Direksi[0]
                                        });
                                }
                            if (Dirut.Count() > 0)
                                if (RKS > BATASAN_BIAYA_DIRUT)
                                {
                                    var lasOrder = WorkflowMasterTemplateDetails.LastOrDefault().SegOrder;
                                    WorkflowMasterTemplateDetails.Add(
                                        new WorkflowMasterTemplateDetail()
                                        {
                                            NameValue = "Gen.By.System",
                                            SegOrder = lasOrder + 1,
                                            UserId = Dirut[0]
                                        });
                                }
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
                            var wokflowId = Convert.ToInt32(resultTemplate.Id);
                            #endregion

                            _repository.ChangeStatusPengadaan(id, EStatusPengadaan.DISETUJUI, UserId());

                            PersetujuanPemenang ndata = new PersetujuanPemenang()
                            {
                                CreatedOn = DateTime.Now,
                                CreatedBy = UserId(),
                                PengadaanId = id,
                                WorkflowId = wokflowId
                            };

                            _repository.SavePersetujuanPemenang(ndata, UserId());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int isApprovePemenang(Guid Id)
        {
            return _repository.CekPersetujuanPemenang(Id,UserId()).status==HttpStatusCode.OK?1:0;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> ajukanDokPemenangOld(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {

                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                PersetujuanPemenang nPersetujuanPemenang = new PersetujuanPemenang();
                nPersetujuanPemenang.PengadaanId = Id;
                //nPersetujuanPemenang.Note
                nPersetujuanPemenang.Status = StatusPengajuanPemenang.PENDING;
                if (_repository.StatusPersetujuanPemenang(Id) > StatusPengajuanPemenang.BELUMDIAJUKAN && _repository.StatusPersetujuanPemenang(Id) != StatusPengajuanPemenang.REJECTED)
                {
                    return new ResultMessage
                    {
                        status = HttpStatusCode.ExpectationFailed,
                        message = "Sudah Dalam Tahap Pengajuan"
                    };
                }
                var result=_repository.SavePersetujuanPemenang(nPersetujuanPemenang, UserId());
                if (result.status != HttpStatusCode.OK)
                {
                    return new ResultMessage
                    {
                        status = result.status,
                        message = result.message
                    };
                }
                
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                decimal? RKS = _repository.getRKSDetails(Id, UserId()).Sum(d => d.hps * d.jumlah);


                

                #region BuatAtauUpdateTamplate
                var vwpengadaan = _repository.GetPengadaanByiD(Id);
                var DepHead = await listHead();
                var DepManager = await listGuidManager();
                var Direksi = await listUser(IdLdapConstants.Roles.pRole_direksi);
                var Dirut = await listUser(IdLdapConstants.Roles.pRole_dirut);
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
                if (RKS > ValueBoundAprr) WorkflowMasterTemplateDetails.Add(
                             new WorkflowMasterTemplateDetail()
                             {
                                 NameValue = "Gen.By.System",
                                 SegOrder = 3,
                                 UserId = DepHead[0]
                             });
                if (Direksi.Count() > 0)
                if (RKS > ValueBoundDireksiAprr)
                {
                    var lasOrder = WorkflowMasterTemplateDetails.LastOrDefault().SegOrder;
                    WorkflowMasterTemplateDetails.Add(
                        new WorkflowMasterTemplateDetail()
                        {
                            NameValue = "Gen.By.System",
                            SegOrder = lasOrder+1,
                            UserId = Direksi[0]
                        });
                }
                if(Dirut.Count()>0)
                if (RKS > BATASAN_BIAYA_DIRUT)
                {
                    var lasOrder = WorkflowMasterTemplateDetails.LastOrDefault().SegOrder;
                    WorkflowMasterTemplateDetails.Add(
                        new WorkflowMasterTemplateDetail()
                        {
                            NameValue = "Gen.By.System",
                            SegOrder = lasOrder + 1,
                            UserId = Dirut[0]
                        });
                }
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
                nPersetujuanPemenang.WorkflowId = Convert.ToInt32(resultTemplate.Id);
                #endregion

                if (nPersetujuanPemenang.WorkflowId != null)
                {
                    nPersetujuanPemenang.Status = StatusPengajuanPemenang.PENDING;
                    var rPersetujuanPemenang = _repository.SavePersetujuanPemenang(nPersetujuanPemenang, UserId());
                    respon = HttpStatusCode.OK;
                    idx = rPersetujuanPemenang.Id;
                    var resultx = _workflowrepo.PengajuanDokumen(new Guid(rPersetujuanPemenang.Id), nPersetujuanPemenang.WorkflowId.Value, DocumentTypePemenang);
                    if (string.IsNullOrEmpty(resultx.Id))
                    {
                        result.message = resultx.message;
                        result.Id = resultx.Id;
                        return result;
                    }
                    message = Common.SaveSukses();
                }
                else
                {
                    var PersetujuanPemenang = _repository.DeletePersetujuanPemenang(nPersetujuanPemenang.Id);
                    respon = HttpStatusCode.OK;
                    message = Common.SaveSukses();
                    idx = PersetujuanPemenang.Id.ToString();
                }

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                          IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                           IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> ajukanDokPemenang(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {

                respon = HttpStatusCode.Forbidden;
                message = "Erorr";

                var pengadaan = _repository.GetPengadaanByiD(Id);

                var nPersetujuanPemenang = pengadaan.PersetujuanPemenangs.FirstOrDefault();
                if (nPersetujuanPemenang == null)
                {
                    return new ResultMessage()
                    {
                        message = "Workflow Belum di Save",
                        status = HttpStatusCode.NotImplemented
                    };
                }
                if (nPersetujuanPemenang.WorkflowId != null)
                {
                    nPersetujuanPemenang.Status = StatusPengajuanPemenang.PENDING;
                    var rPersetujuanPemenang = _repository.SavePersetujuanPemenang(nPersetujuanPemenang, UserId());
                    respon = HttpStatusCode.OK;
                    idx = rPersetujuanPemenang.Id;
                    var resultx = _workflowrepo.PengajuanDokumen(new Guid(rPersetujuanPemenang.Id), nPersetujuanPemenang.WorkflowId.Value, DocumentTypePemenang);
                    if (string.IsNullOrEmpty(resultx.Id))
                    {
                        result.message = resultx.message;
                        result.Id = resultx.Id;
                        return result;
                    }
                    message = Common.SaveSukses();
                }
                else
                {
                    var PersetujuanPemenang = _repository.DeletePersetujuanPemenang(nPersetujuanPemenang.Id);
                    respon = HttpStatusCode.OK;
                    message = Common.SaveSukses();
                    idx = PersetujuanPemenang.Id.ToString();
                }

            }
            catch (Exception ex)
            {
                respon = HttpStatusCode.NotImplemented;
                message = ex.ToString();
                idx = "0";
            }
            finally
            {
                result.status = respon;
                result.message = message;
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult StatusPemenang(Guid pengadaanId)
        {
            return Json(_repository.StatusPersetujuanPemenang(pengadaanId));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<Reston.Helper.Util.ResultMessageWorkflowState> persetujuanPemenangWithNote(Guid id, string Note)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {
                var pengadaan = _repository.GetPengadaanByiD(id);
                result = _workflowrepo.ApproveDokumen2(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value, UserId(), Note, Reston.Helper.Model.WorkflowStatusState.APPROVED);
                if (!string.IsNullOrEmpty(result.Id))
                {
                    
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Dokumen Persetujuan Pemenang DiSetujui Oleh: " + CurrentUser.UserName;
                    nRiwayatDokumen.Comment = Note;
                    nRiwayatDokumen.PengadaanId = id;
                    nRiwayatDokumen.UserId = UserId();
                    _repository.AddRiwayatDokumen(nRiwayatDokumen);
                    ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value);
                    if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                    {
                        _repository.ChangeStatusPersetujuanPemenang(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, StatusPengajuanPemenang.APPROVED, UserId());
                        SendEmailPemenang(nRiwayatDokumen.PengadaanId.Value);
                    }
                    try
                    {
                        var nextApprover = _workflowrepo.CurrentApproveUserSegOrder(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value);
                        await SendEmailToApprover(nextApprover.Id.Split('#')[1], id);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<Reston.Helper.Util.ResultMessageWorkflowState> PenolakanPemenangWithWorkflow(Guid Id, string Note)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {

                var pengadaan = _repository.GetPengadaanByiD(Id);
                result = _workflowrepo.ApproveDokumen2(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value, UserId(), Note, Reston.Helper.Model.WorkflowStatusState.REJECTED);
                if (result.data != null)
                {
                    if (result.data.DocumentStatus == Reston.Helper.Model.DocumentStatus.REJECTED)
                    {
                        _repository.ChangeStatusPersetujuanPemenang(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, StatusPengajuanPemenang.REJECTED, UserId());
                    
                    }
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Dokumen Persetujuan Ditolak Oleh: "+CurrentUser.UserName;
                    nRiwayatDokumen.PengadaanId = Id;
                    nRiwayatDokumen.Comment = Note;
                    nRiwayatDokumen.UserId = UserId();
                    _repository.AddRiwayatDokumen(nRiwayatDokumen);
                    ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value);
                    if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                    {
                        _repository.ChangeStatusPersetujuanPemenang(Id, StatusPengajuanPemenang.APPROVED, UserId());
                    }
                    try
                    {
                        var nextApprover = _workflowrepo.CurrentApproveUserSegOrder(pengadaan.PersetujuanPemenangs.FirstOrDefault().Id, pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value);
                        await SendEmailToApprover(nextApprover.Id.Split('#')[1], Id);
                    }
                    catch { }
                }

            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]      
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<Reston.Helper.Util.ResultMessageWorkflowState> persetujuanPemenangWithNextApprover(Guid id, string Note, Guid userId)
        {
            var result = new Reston.Helper.Util.ResultMessageWorkflowState();
            try
            {
                var oPersetujuanPemenang = _repository.getPersetujuanPemenangByPengadaanId(id);
                WorkflowMasterTemplateDetail oDetailTempalte = new WorkflowMasterTemplateDetail();
                oDetailTempalte.UserId = userId;
                oDetailTempalte.NameValue = "Ditambah Oleh: " + UserId();
                oDetailTempalte.WorkflowMasterTemplateId = oPersetujuanPemenang.WorkflowId.Value;
                var oAddMasterTemplateDetail = _workflowrepo.AddMasterTemplateDetail(oPersetujuanPemenang.WorkflowId.Value, oDetailTempalte);
                if (!string.IsNullOrEmpty(oAddMasterTemplateDetail.Id))
                {

                    var pengadaan = _repository.GetPengadaanByiD(id);
                    result = _workflowrepo.ApproveDokumen2(oPersetujuanPemenang.Id,pengadaan.PersetujuanPemenangs.FirstOrDefault().WorkflowId.Value, UserId(),Note, Reston.Helper.Model.WorkflowStatusState.APPROVED);
                    if (!string.IsNullOrEmpty(result.Id))
                    {
                        RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                        nRiwayatDokumen.Status = "Dokumen Pemenang DiSetujui Oleh: "+CurrentUser.UserName;
                        nRiwayatDokumen.Comment = Note;
                        nRiwayatDokumen.PengadaanId = oPersetujuanPemenang.PengadaanId;
                        nRiwayatDokumen.UserId = UserId();
                        _repository.AddRiwayatDokumen(nRiwayatDokumen);
                        ViewWorkflowState oViewWorkflowState = _workflowrepo.StatusDocument(oPersetujuanPemenang.Id, pengadaan.WorkflowId.Value);
                        if (oViewWorkflowState.DocumentStatus == DocumentStatus.APPROVED)
                        {
                            _repository.ChangeStatusPersetujuanPemenang(id, StatusPengajuanPemenang.APPROVED, UserId());
                            SendEmailPemenang(id);
                        }
                        try
                        {
                            var nextApprover = _workflowrepo.CurrentApproveUserSegOrder(id,pengadaan.WorkflowId.Value);
                            await SendEmailToApprover(nextApprover.Id.Split('#')[1], id);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
            }
            return result;
        }
       
        private void SendEmailPemenang(Guid PengadaanId)
        {
            var pengadaan= _repository.GetPengadaanByiD(PengadaanId);
            var oKandidat = _repository.getKandidatPengadaan(PengadaanId, UserId());
            var oPemenang = _repository.getPemenangPengadaan(PengadaanId, UserId());
            var oKalah = oKandidat.Except(oPemenang);
            foreach (var item in oPemenang)
            {
                string html = System.Configuration.ConfigurationManager.AppSettings["MAIL_KALAH_BODY"].ToString();
                var noMenang = _repository.GenerateNoDOKUMEN(UserId(), System.Configuration.ConfigurationManager.AppSettings["KODE_MENANG"].ToString(), TipeNoDokumen.MEANANG);
                html = html.Replace("{2}", noMenang);
                html = html.Replace("{1}", Common.ConvertDateToIndoDate(DateTime.Now));
                html = html.Replace("{3}", item.NamaVendor);
                html = html.Replace("{4}", item.Alamat);
                html = html.Replace("{0}", pengadaan.Judul);
                sendMail(item.NamaVendor, item.Email, html, SubjeckEmailPemenang);
            }
            foreach (var item in oKalah)
            {
                string html = System.Configuration.ConfigurationManager.AppSettings["MAIL_KALAH_BODY"].ToString();
                var noKalah = _repository.GenerateNoDOKUMEN(UserId(), System.Configuration.ConfigurationManager.AppSettings["KODE_KALAH"].ToString(), TipeNoDokumen.KALAH);
                html=html.Replace("{2}",noKalah);
                html = html.Replace("{1}", Common.ConvertDateToIndoDate(DateTime.Now));
                html = html.Replace("{3}", item.NamaVendor);
                html = html.Replace("{4}", item.Alamat);
                html = html.Replace("{0}", pengadaan.Judul);
                sendMail(item.NamaVendor, item.Email, html, SubjeckEmailKalah);
            }
        }

        private async Task<int> SendEmailToApprover(string UserId,Guid PengadaanId)
        {
            try
            {
                var userApprover = await userDetail(UserId);
                var oPengadaan = _repository.GetPengadaanByiD(PengadaanId);
                string html = "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_YTH"].ToString() + "</p>";
                html = html + "<p>" + userApprover.Nama + "</p>";
                html = html + "<br/>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_BODY_APPROVER"].ToString() + "</p>";
                if(oPengadaan.AturanPengadaan=="terbuka")
                    html = html + "<p><a href='" + IdLdapConstants.Proc.Url + "pengadaan-terbuka-detail.html#" + oPengadaan .Id+ "' target='_blank'>" + oPengadaan.Judul + "</a></p>";
                else html = html + "<p><a href='" + IdLdapConstants.Proc.Url + "pengadaan-detail.html#" + oPengadaan.Id + "' target='_blank'>" + oPengadaan.Judul + "</a></p>";
                html = html + "<br/><br/>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_FOOTER1"].ToString() + "</p>";
                html = html + "<p>" + System.Configuration.ConfigurationManager.AppSettings["MAIL_KLARIFIKASI_FOOTER2"].ToString() + "</p>";
                sendMail(userApprover.Nama, userApprover.Email, html, System.Configuration.ConfigurationManager.AppSettings["MAIL_SUBJECT_APPROVER"].ToString());
                return 1;
            }catch{
                return 0;
            }
        }


        #region persetujuan tiap tahapan
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SavePersetujuanTahapan(Guid PengadaanId, string status)
        {
             EStatusPengadaan s = (EStatusPengadaan)Enum.Parse(typeof(EStatusPengadaan), status);
             var data = new PersetujuanTahapan()
             {
                 PengadaanId = PengadaanId,
                 StatusPengadaan = s,                 
                 Status = StatusTahapan.Approved
             };
             
            var rData = _repository.SavePersetujuanTahapan(data,UserId());
            if (rData.Id == null) return Json(new ResultMessage()
            {
                message=Common.Forbiden(),status=HttpStatusCode.Forbidden            
            });
            if (s == EStatusPengadaan.BUKAAMPLOP)
            {
                _repository.CekBukaAmplopTahapan(PengadaanId);
            }
            return Json(new ResultMessage()
            {
                Id = rData.Id.ToString(),
                message = Common.SaveSukses(),
                status = HttpStatusCode.OK
            });

        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<IHttpActionResult> GetPersetujuanTahapan(Guid PengadaanId, string status)
        {
            EStatusPengadaan s = (EStatusPengadaan)Enum.Parse(typeof(EStatusPengadaan), status);
            var data = _repository.GetPersetujuanTahapan(PengadaanId, s);
            foreach (var item in data)
            {
                item.UserName = (await userDetail(item.UserId.ToString())).Nama;
            }
            return Json(data);
        }

        #endregion


        #region tambah tahapan
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult saveTahapan(LewatTahapan data){
            var result=_repository.SaveTahapan(data, UserId());
            return Json(new VWLewatTahapan(){
                Id= result.Id,
                PengadaanId = result.PengadaanId,
                Tambah = result.Tambah
            });
        }
        #endregion


        #region persetujuan terkait
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]    
        public async Task<IHttpActionResult> SavePersetujuanTerkait(Guid PengadanId,Guid UserId)
        {
            PersetujuanTerkait data = new PersetujuanTerkait()
            {
                PengadaanId = PengadanId,
                UserId = UserId
            };
            var result = _repository.savePersetujuanTerkait(data);

            if (!string.IsNullOrEmpty(result.Id.ToString()))
            {
                await SendEmailToApprover(UserId.ToString(),PengadanId);
            }            
            VWPersetujuanTerkait datax = new VWPersetujuanTerkait()
            {
                Id = result.Id,
            };


            return Json(datax);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult DeletePersetujuanTerkait(Guid Id)
        {
            var result = _repository.deletePersetujuanTerkait(Id,UserId());
            return Json(result);
        }
        

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      
        public IHttpActionResult TerkaitSetuju(Guid PengadanId)
        {
            PersetujuanTerkait data = new PersetujuanTerkait()
            {
                PengadaanId = PengadanId,
                UserId = UserId()
            };
            var result=_repository.TerkaitSetuju(data);
            VWPersetujuanTerkait datax = new VWPersetujuanTerkait()
            {
                Id = result.Id
            };
            return Json(datax);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_approver,
                                           IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                            IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
      
        public async Task< IHttpActionResult> UserTerkait(Guid PengadanId)
        {
            var result = _repository.GetUserTerkait(PengadanId);
            List<VWPersetujuanTerkait> data = new List<VWPersetujuanTerkait>();
            foreach (var item in result)
            {
                VWPersetujuanTerkait ndata = new VWPersetujuanTerkait();
                ndata.Id = item.Id;
                var user = await userDetail(item.UserId.ToString());
                ndata.UserId = item.UserId;
                ndata.Nama = user.Nama;
                ndata.setuju = item.setuju == true ? 1 : 0;
                ndata.isthismine = item.UserId == UserId() ? 1 : 0;
                data.Add(ndata);
            }

            return Json(data);
        } 
        #endregion
    }
    
}
