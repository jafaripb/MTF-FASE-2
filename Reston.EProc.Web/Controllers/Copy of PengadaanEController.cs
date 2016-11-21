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
using Webservice.Helper.Util;

namespace Reston.Pinata.WebService.Controllers
{
    public class PengadaanEController : BaseController
    {
        private IPengadaanRepo _repository;
        internal ResultMessage result = new ResultMessage();
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"];
        private string FILE_PENGADAAN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        //const string[] arrRoleExRekanan =  { IdLdapConstants.Roles.pRole_procurement_head, 
        //                                    IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_end_user,
        //                                     IdLdapConstants.Roles.pRole_procurement_manager};


        public PengadaanEController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
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
                //Guid userId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                //vwpengadaan.Pengadaan.CreatedBy = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                //vwpengadaan.Pengadaan.CreatedOn = DateTime.Now;

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

                var pengadaan = _repository.AddPengadaan(vwpengadaan.Pengadaan, UserId(), await listGuidManager());
                respon = HttpStatusCode.OK;
                id = pengadaan.Id.ToString();
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
        [Authorize]
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


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<HttpResponseMessage> getUsers(int start, int limit, string name)
        {
            var client = new HttpClient();
            HttpResponseMessage reply = await client.GetAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/ListUser?start=" + start + "&limit=" + limit + "&filter=" + "&name=" + name));
            return reply;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
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
        public VWRKSHeaderPengadaan getHeaderRks(Guid Id)
        {
            // Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);

            var HeaderRks = _repository.GetRKSHeaderPengadaan(Id);
            return HeaderRks;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<ViewPengadaan> detailPengadaan(Guid Id)
        {
            // Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);

            var ss = MyConverter.Terbilang("1254500");
            var xx = MyConverter.MoneyRoundUp(121234324432);
            return _repository.GetPengadaan(Id, UserId(), await isApprover());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<DataPagePengadaan> getPengadaanList(int start, int length, EGroupPengadaan group, string search)
        {
            //Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            //List<string> roles = ((ClaimsIdentity)User.Identity).Claims
            //    .Where(c => c.Type == ClaimTypes.Role)
            //    .Select(c => c.Value).ToList();
            var lstPerhatianPengadaan = _repository.GetPengadaans(search, start, length, UserId(), Roles(), group, await listGuidManager(), await listHead());
            return lstPerhatianPengadaan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        public async Task<List<ViewPengadaan>> getPerhatianPengadaanList(int start, int length, string search)
        {
            //Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            //List<string> roles = ((ClaimsIdentity)User.Identity).Claims
            //    .Where(c => c.Type == ClaimTypes.Role)
            //    .Select(c => c.Value).ToList();
            var lstPerhatianPengadaan = _repository.GetPerhatian(search, start, length, UserId(), Roles(), await isApprover(), await listGuidManager());
            return lstPerhatianPengadaan;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> persetujuan(Guid id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "";
            try
            {
                //Guid UserId= new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);

                Pengadaan pengadaan = _repository.Persetujuan(UserId(), id, await isApprover());
                idx = pengadaan.Id.ToString();
                respon = HttpStatusCode.OK;
                message = Common.SaveSukses();
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
                result.Id = idx;
            }
            //
            return result;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage persetujuanWithWorkflow(Guid id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "";
            try
            {
               // Pengadaan pengadaan = _repository.PersetujuanWorkflow(id,UserId());
                //idx = pengadaan.Id.ToString();
                respon = HttpStatusCode.OK;
                message = Common.SaveSukses();
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
                result.Id = idx;
            }
            //
            return result;
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<ResultMessage> tolakPengadaan(VWPenolakan vwPenolakan)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "";
            try
            {
                Pengadaan pengadaan = _repository.Penolakan(UserId(), vwPenolakan, await isApprover());
                idx = pengadaan.Id.ToString();
                respon = HttpStatusCode.OK;
                message = Common.SaveSukses();
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
                result.Id = idx;
            }
            //
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWDokumenPengadaan> getDokumens(TipeBerkas tipe, Guid Id)
        {
            return _repository.GetListDokumenPengadaan(tipe, Id, UserId());
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
        public int sendMail(ViewSendEmail data)
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
                sendMail(item.Nama, item.email, html, judulSubjek);
            }
            return 1;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int sendMailKlarifikasi(ViewSendEmail data)
        {
            //sending email notification

            List<VWVendor> vendors = _repository.GetVendorsKlarifikasiByPengadaanId(data.PengadaanId.Value);
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
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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
                int result = _repository.AjukanPengadaan(Id, UserId(), await listGuidManager());
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
        public ResultMessage ajukanWithWorkFlow(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = 0;// _repository.AjukanWorkflow(Id, UserId(), new Guid("B6EA58A3-345D-E611-A7D2-38EAA7E56C6E"));
                if (result==1)
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananSubmitHarga> GetRekananKlarifikasiSubmit(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananKlarifikasiSubmit(PId, UserId());
        }


        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetRekananKlarifikasiPenilaian(Guid PId)
        {
            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getListRekananKlarifikasiPenilaian(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                             IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                              IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWRekananPenilaian> GetPemenangPengadaan(Guid PId)
        {

            //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.getPemenangPengadaan(PId, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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
                if (_repository.CekBukaAmplop(vwpengadaan.PengadaanId.Value) == 0 && oPengadaan.Status == EStatusPengadaan.BUKAAMPLOP)
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<BeritaAcara> GetBeritaAcara(Guid PId)
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
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
            var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            bool isSavedSuccessfully = true;
            string filePathSave = FILE_DOKUMEN_PATH;//+id ;
            string fileName = tipe;
            if (Directory.Exists(uploadPath + filePathSave) == false)
            {
                Directory.CreateDirectory(uploadPath + filePathSave);
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
            TipeBerkas t = (TipeBerkas)Enum.Parse(typeof(TipeBerkas), tipe);
            DokumenPengadaan dokumen = new DokumenPengadaan
            {
                File = fileName,
                Tipe = t,
                ContentType = contentType,
                PengadaanId = id,
                SizeFile = sizeFile
            };

            if (isSavedSuccessfully)
            {
                try
                {
                    DokumenPengadaan dokumenUpdate = _repository.saveDokumenPengadaan(dokumen, UserId());
                    if (t == TipeBerkas.BeritaAcaraAanwijzing)
                    {
                        //  int x = _repository.UpdateStatus(id, EStatusPengadaan.SUBMITPENAWARAN);
                    }
                    return Json(dokumen.Id);
                }
                catch (Exception ex)
                {
                    return Json(0);
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
    }
}
