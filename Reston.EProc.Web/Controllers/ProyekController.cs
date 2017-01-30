using Model.Helper;
using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Eproc.Model.Monitoring.Repository;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using Reston.Pinata.WebService;
using Reston.Pinata.WebService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Reston.EProc.Web.Controllers
{
    public class ProyekController : BaseController
    {
        private IProyekRepo _repository;
        internal ResultMessage result = new ResultMessage();

        public ProyekController()
        {
            _repository = new ProyekRepo(new JimbisContext());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilJudul()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json( _repository.GetDataProyek(PengadaanId));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilTahapanPekerjaan()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataPekerjaan(PengadaanId));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilTahapanPembayaran()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataPembayaran(PengadaanId));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilDokumenPekerjaan()
        {
            Guid TahapanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataDokumenPekerjaan(TahapanId));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilDokumenPembayaran()
        {
            Guid TahapanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataDokumenPembayaran(TahapanId));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilPenilaian()
        {
            Guid IdProyek = Guid.Parse(HttpContext.Current.Request["Id"].ToString());
            return Json(_repository.GetDataPenilaian(IdProyek));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilPenilaianRekanan()
        {
            Guid IdProyek = Guid.Parse(HttpContext.Current.Request["Id"].ToString());
            return Json(_repository.GetDataPenilaianRekanan(IdProyek));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult TampilNilai()
        {
            Guid IdProyek = Guid.Parse(HttpContext.Current.Request["Id"].ToString());
            return Json(_repository.GetNilai(IdProyek));
        }

        [HttpPost]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage delete(Guid Id)
        {
            try
            {
                result = _repository.deleteTahap(Id, UserId());
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
        public ResultMessage deleteDok(Guid Id)
        {
            try
            {
                result = _repository.deleteDokTahap(Id, UserId());
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
        public ResultMessage deletePIC(Guid Id)
        {
            try
            {
                result = _repository.deletePICProyek(Id, UserId());
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
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SimpanRencanaProyek()
        {
            Guid xPengadaanId = Guid.Parse(HttpContext.Current.Request["aPengadaanId"].ToString());
            DateTime? xStartDate = Common.ConvertDate(HttpContext.Current.Request["aStartDate"].ToString(), "dd/MM/yyyy HH:mm");
            DateTime? xEndDate = Common.ConvertDate(HttpContext.Current.Request["aEndDate"].ToString(), "dd/MM/yyyy HH:mm");
            string xStatus = HttpContext.Current.Request["aStatus"].ToString();

            return Json(_repository.SimpanRencanaProyekRepo(xPengadaanId, xStatus, UserId(), xStartDate, xEndDate));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult UbahStatusRencanaProyek()
        {
            Guid Id = Guid.Parse(HttpContext.Current.Request["Id"].ToString());
            string NoKontrak = HttpContext.Current.Request["NoKontrak"].ToString();
            string Status = HttpContext.Current.Request["Status"].ToString();
            return Json(_repository.SimpanProyekRepo(Id, NoKontrak, Status, UserId()));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SimpanTahapanPekerjaan()
        {
            Guid xPengadaanId = Guid.Parse(HttpContext.Current.Request["aPengdaanId"].ToString());
            string xNamaTahapanPekerjaan = HttpContext.Current.Request["aNamaTahapanPekerjaan"].ToString();
            DateTime? xTanggalMulai = Common.ConvertDate(HttpContext.Current.Request["aTanggalMulai"].ToString(), "dd/MM/yyyy HH:mm");
            DateTime? xTanggalSelesai = Common.ConvertDate(HttpContext.Current.Request["aTanggalSelesai"].ToString(), "dd/MM/yyyy HH:mm");
            string xJenisPekerjaan = HttpContext.Current.Request["aJenisTahapan"].ToString();
            decimal xBobotPekerjaan = Convert.ToDecimal(HttpContext.Current.Request["aBobotPekerjaan"].ToString());

            return Json(_repository.SimpanTahapanPekerjaanRepo(xPengadaanId, xNamaTahapanPekerjaan, xJenisPekerjaan, xBobotPekerjaan, UserId(), xTanggalMulai, xTanggalSelesai));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SimpanTahapanPembayaran()
        {
            Guid xPengadaanId = Guid.Parse(HttpContext.Current.Request["aPengdaanId"].ToString());
            string xNamaTahapanPekerjaan = HttpContext.Current.Request["aNamaTahapanPekerjaan"].ToString();
            DateTime? xTanggalMulai = Common.ConvertDate(HttpContext.Current.Request["aTanggalMulai"].ToString(), "dd/MM/yyyy HH:mm");
            DateTime? xTanggalSelesai = Common.ConvertDate(HttpContext.Current.Request["aTanggalSelesai"].ToString(), "dd/MM/yyyy HH:mm");
            string xJenisPekerjaan = HttpContext.Current.Request["aJenisTahapan"].ToString();

            return Json(_repository.SimpanTahapanPembayaranRepo(xPengadaanId, xNamaTahapanPekerjaan, xJenisPekerjaan, UserId(), xTanggalMulai, xTanggalSelesai));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SimpanTahapanPekerjaanRekanan()
        {
            Guid xProyekId = Guid.Parse(HttpContext.Current.Request["aProyekId"].ToString());
            string xNamaTahapanPekerjaan = HttpContext.Current.Request["aNamaTahapanPekerjaan"].ToString();
            DateTime? xTanggalMulai = Common.ConvertDate(HttpContext.Current.Request["aTanggalMulai"].ToString(), "dd/MM/yyyy HH:mm");
            DateTime? xTanggalSelesai = Common.ConvertDate(HttpContext.Current.Request["aTanggalSelesai"].ToString(), "dd/MM/yyyy HH:mm");
            string xJenisPekerjaan = HttpContext.Current.Request["aJenisTahapan"].ToString();
            decimal xBobotPekerjaan = Convert.ToDecimal(HttpContext.Current.Request["aBobotPekerjaan"].ToString());

            return Json(_repository.SimpanTahapanPekerjaanRekananRepo(xProyekId, xNamaTahapanPekerjaan, xJenisPekerjaan, xBobotPekerjaan, UserId(), xTanggalMulai, xTanggalSelesai));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SimpanTahapanPekerjaanDokumen()
        {
            Guid xId_Tahapan = Guid.Parse(HttpContext.Current.Request["aId_Tahapan"].ToString());
            string xNamaDokumen = HttpContext.Current.Request["aNama_Dokumen"].ToString();
            string xJenisDokumen = HttpContext.Current.Request["aJenis_Tahapan"].ToString();

            return Json(_repository.SimpanTahapanPekerjaanDokumenRepo(xId_Tahapan, xNamaDokumen, xJenisDokumen, UserId()));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public IHttpActionResult SimpanTahapanPembayaranDokumen()
        {
            Guid xId_Tahapan = Guid.Parse(HttpContext.Current.Request["aId_Tahapan"].ToString());
            string xNamaDokumen = HttpContext.Current.Request["aNama_Dokumen"].ToString();
            string xJenisDokumen = HttpContext.Current.Request["aJenis_Tahapan"].ToString();

            return Json(_repository.SimpanTahapanPembayaranDokumenRepo(xId_Tahapan, xNamaDokumen, xJenisDokumen, UserId()));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage savePersonil(ViewUntukProyekAddPersonil Personil)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                var result = _repository.savePICProyek(Personil, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
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
            }
            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage SimpanNilai(PenilaianVendorHeader PenilaianHeader)
        {
            try
            {
                result = _repository.SimpanPenilaian(PenilaianHeader, UserId());
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
                result.status = HttpStatusCode.ExpectationFailed;
            }
            return result;
        }
    }
}
