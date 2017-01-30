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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Reporting.WebForms;
using System.Reflection;

namespace Reston.EProc.Web.Controllers
{
    public class MonitoringSelectionController : BaseController
    {

        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOCPRO"];
        private string FILE_REPORT_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_REPORT_PATH"];

        private IMoritoringRepo _repository;

        internal ResultMessage result = new ResultMessage();

        public MonitoringSelectionController()
        {
            _repository = new MonitoringRepo(new JimbisContext());
        }

        public IHttpActionResult ListProyek()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            string klasifikasi = HttpContext.Current.Request["klasifikasi"].ToString();
            if (klasifikasi == "")
            {
                return Json(_repository.GetDataListProyekMonitoring(search, start, length, null));
            }
            Klasifikasi dklasifikasi = (Klasifikasi)Convert.ToInt32(klasifikasi);
            return Json(_repository.GetDataListProyekMonitoring(search, start, length, dklasifikasi));
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deletedokumen(Guid Id)
        {
            try
            {
                result = _repository.DeleteDokumenProyek(Id, UserId());
            }
            catch (Exception ex)
            {
                result.message = ex.ToString();
                result.status = HttpStatusCode.ExpectationFailed;
            }
            return result;
        }

        public IHttpActionResult ListProyekDetailMonitoringPembayaran()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            Guid Id = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataListProyekDetailMonitoringPembayaran(search, start, length, Id));
        }

        public IHttpActionResult ListProyekRekanan()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            string klasifikasi = HttpContext.Current.Request["klasifikasi"].ToString();

            if (klasifikasi == "")
            {
                return Json(_repository.GetDataListProyekMonitoringRekanan(search, start, length, null, UserId()));
            }

            Klasifikasi dklasifikasi = (Klasifikasi)Convert.ToInt32(klasifikasi);
            return Json(_repository.GetDataListProyekMonitoringRekanan(search, start, length, dklasifikasi, UserId()));
        }

        public IHttpActionResult ListProyekDetailMonitoring()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            Guid Id = Guid.Parse(HttpContext.Current.Request["Id"].ToString());
            string klasifikasi = HttpContext.Current.Request["klasifikasi"].ToString();

            if (klasifikasi == "")
            {
                return Json(_repository.GetDataListProyekDetailMonitoring(search, start, length, Id, null));
            }

            Klasifikasi dklasifikasi = (Klasifikasi)Convert.ToInt32(klasifikasi);
            return Json(_repository.GetDataListProyekDetailMonitoring(search, start, length, Id, dklasifikasi));
        }

        public IHttpActionResult TampilJudulDetail()
        {
            Guid ProyekId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDetailProyek(ProyekId, UserId()));
        }

        public IHttpActionResult TampilJudul()
        {
            return Json(_repository.GetResumeProyek());
        }

        
        public IHttpActionResult List()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32( HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32( HttpContext.Current.Request["length"]);
            string status = HttpContext.Current.Request["status"].ToString();

            if (status == "")
            {
                return Json(_repository.GetDataMonitoringSelection(search, start, length, null));
            }

            StatusSeleksi dStatusSeleksi = (StatusSeleksi)Convert.ToInt32(status);
            return Json(_repository.GetDataMonitoringSelection(search, start, length, dStatusSeleksi));
        }

        public IHttpActionResult ListSedangBerjalan()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            string status = HttpContext.Current.Request["status"].ToString();

            if (status == "")
            {
                return Json(_repository.GetDataMonitoringSelectionSedangBerjalan(search, start, length, null));
            }

            StatusSeleksi dStatusSeleksi = (StatusSeleksi)Convert.ToInt32(status);
            return Json(_repository.GetDataMonitoringSelectionSedangBerjalan(search, start, length, dStatusSeleksi));
        }

        public IHttpActionResult ListSelesai()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            string status = HttpContext.Current.Request["status"].ToString();

            if (status == "")
            {
                return Json(_repository.GetDataMonitoringSelectionSelesai(search, start, length, null));
            }

            StatusSeleksi dStatusSeleksi = (StatusSeleksi)Convert.ToInt32(status);
            return Json(_repository.GetDataMonitoringSelectionSelesai(search, start, length, dStatusSeleksi));
        }

        public IHttpActionResult ListDraf()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            string status = HttpContext.Current.Request["status"].ToString();

            if (status == "")
            {
                return Json(_repository.GetDataMonitoringSelectionDraf(search, start, length, null));
            }

            StatusSeleksi dStatusSeleksi = (StatusSeleksi)Convert.ToInt32(status);
            return Json(_repository.GetDataMonitoringSelectionDraf(search, start, length, dStatusSeleksi));
        }

        public ResultMessage toFinish()
        {
            Guid xProyekId = Guid.Parse(HttpContext.Current.Request["aIdProyek"].ToString());
            string xStatus = HttpContext.Current.Request["aStatus"].ToString();

            return _repository.toFinishRepo(xProyekId, xStatus, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage TidakMonitor(Guid Id)
        {
            Guid spkid = Id;
            string status = "Tidak Dimonitor";

            return _repository.toTidakDimonitor(spkid, status, UserId());
        }

        public ResultMessage toDisable()
        {
            Guid xProyekId = Guid.Parse(HttpContext.Current.Request["aIdProyek"].ToString());
            string xStatus = HttpContext.Current.Request["aStatus"].ToString();

            return _repository.toDisableRepo(xProyekId, xStatus, UserId());
        }

        public ResultMessage toEnable()
        {
            Guid xProyekId = Guid.Parse(HttpContext.Current.Request["aIdProyek"].ToString());
            string xStatus = HttpContext.Current.Request["aStatus"].ToString();

            return _repository.toEnableRepo(xProyekId, xStatus, UserId());
        }

        public ResultMessage Add()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["aPengadaanId"].ToString());
            string StatusSeleksi = HttpContext.Current.Request["aStatusSeleksi"].ToString();
            string StatusMonitoring = HttpContext.Current.Request["aStatusMonitoring"].ToString();

            StatusSeleksi dStatusSeleksi = (StatusSeleksi)Convert.ToInt32(StatusSeleksi);
            StatusMonitored dStatusMonitoring = (StatusMonitored)Convert.ToInt32(StatusMonitoring);

            return _repository.Save(PengadaanId, dStatusMonitoring, dStatusSeleksi,UserId());
        }

        //-----------------------------------
        // Update Progres Monitoring Selection
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]

        public ResultMessage UpdateProgressPekerjaan(List<TahapanProyek> Tahapan)
        {
            try
            {
                result = _repository.SimpanProgresPekerjaan(Tahapan, UserId());
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

        public ResultMessage UpdateProgressPembayaran(List<TahapanProyek> Tahapan)
        {
            try
            {
                result = _repository.SimpanProgresPembayaran(Tahapan, UserId());
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
        public HttpResponseMessage OpenFile(Guid Id)
        {
            DokumenProyek d = _repository.GetDokumenProyek(Id);
            var path = FILE_DOKUMEN_PATH + d.URL;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(d.ContentType);

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = d.URL
            };

            return result;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_head,
                                            IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_end_user,
                                             IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance,IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public async Task<IHttpActionResult> UploadFile()
        {
            HttpRequestMessage request = this.Request;

            Guid DokumenId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());
            string NamaDokumen = HttpContext.Current.Request["NamaDokumen"].ToString();
            bool isSavedSuccessfully = true;
            string NamaFileSave = "";
            string extension = "";

            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/UploadDokumenMonitor");
            //var provider = new MultipartFormDataStreamProvider(root);
            var s = await Request.Content.ReadAsStreamAsync();
            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);
            string contentType = "";
            Guid newGuid = Guid.NewGuid();
            long sizeFile = 0;
            foreach (var file in provider.Contents)
            {
                string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                extension = filename.Substring(filename.IndexOf(".") + 1, filename.Length - filename.IndexOf(".") - 1);
                byte[] buffer = await file.ReadAsByteArrayAsync();
                contentType = file.Headers.ContentType.ToString();
                sizeFile = buffer.Length;
                string a = DateTime.Now.ToString("yyyyMMddHHmmss");

                NamaFileSave = "Dokumen" + DokumenId.ToString() + "-" + NamaDokumen + "-" + a + "." + extension;

                try
                {
                    FileStream fs = new FileStream(root + "//" + NamaFileSave, FileMode.CreateNew);
                    await fs.WriteAsync(buffer, 0, buffer.Length);

                    fs.Close();

                    isSavedSuccessfully = true;
                }
                catch (Exception e)
                {
                    return InternalServerError();
                }
            }
            if (isSavedSuccessfully)
            {
                try
                {
                    return Json(_repository.saveDokumenProyeks(DokumenId, NamaFileSave, contentType, UserId()));

                }
                catch (Exception ex)
                {
                    return Json(0);
                }
            }
            return null;
        }

        // Report Monitoring
        public HttpResponseMessage ReportMonitoring(string dari, string sampai)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_REPORT_PATH;

                path = Path.Combine(path, "ReportMonitoring.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }

                else
                {
                    //return View("Index");
                }
                var oDari = Common.ConvertDate(dari, "dd/MM/yyyy");
                var oSampai = Common.ConvertDate(sampai, "dd/MM/yyyy");

                var Monitoring = _repository.GetReportMonitoring(oDari, oSampai, UserId());

                ReportDataSource rd = new ReportDataSource("Monitoring", Monitoring);
                lr.DataSources.Add(rd);
                string param1 = "";
                string filename = "";
                string param2 = "";
                string paramSemester = "";
                string paramTahunAjaran = "";


                string reportType = "doc";
                string mimeType;
                string encoding;
                string fileNameExtension;


                string[] streamids = null;
                String extension = null;
                Byte[] bytes = null;
                Warning[] warnings;

                bytes = lr.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(bytes);

                result.Content = new StreamContent(stream);

                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Report-Monitoring" + UserId() + DateTime.Now.ToString("dd-MM-yy") + ".xls"
                };

                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                result.Content = new StringContent(sb.ToString());

                return result;
                //Display or log the error based on your application.
            }
        }

        // Report Pekerjaan
        public HttpResponseMessage ReportPekerjaan(Guid Id)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_REPORT_PATH;

                path = Path.Combine(path, "ReportPekerjaan.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }

                else
                {
                    //return View("Index");
                }
                var id = Id;

                var Pekerjaan = _repository.GetReportPekerjaan(id, UserId());
                int i = 0;
                foreach (var item in Pekerjaan)
                {
                    if (i != 0)
                    {
                        item.Pengadaan = "";
                    }
                    i = 1;
                }

                ReportDataSource rd = new ReportDataSource("Pekerjaan", Pekerjaan);
                lr.DataSources.Add(rd);
                string param1 = "";
                string filename = "";
                string param2 = "";
                string paramSemester = "";
                string paramTahunAjaran = "";


                string reportType = "doc";
                string mimeType;
                string encoding;
                string fileNameExtension;


                string[] streamids = null;
                String extension = null;
                Byte[] bytes = null;
                Warning[] warnings;

                bytes = lr.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(bytes);

                result.Content = new StreamContent(stream);

                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Report-Progress-Pelaksanaan-Project" + UserId() + DateTime.Now.ToString("dd-MM-yy") + ".xls"
                };

                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                result.Content = new StringContent(sb.ToString());

                return result;
                //Display or log the error based on your application.
            }
        }

        // Report Pembayaran
        public HttpResponseMessage ReportPembayaran(Guid Id)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_REPORT_PATH;

                path = Path.Combine(path, "ReportPembayaran.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }

                else
                {
                    //return View("Index");
                }
                var id = Id;

                var Pembayaran = _repository.GetReportPembayaran(id, UserId());
                int i = 0;
                foreach (var item in Pembayaran)
                {
                    if (i != 0)
                    {
                        item.Pengadaan = "";
                    }
                    i = 1;
                }

                ReportDataSource rd = new ReportDataSource("Pembayaran", Pembayaran);
                lr.DataSources.Add(rd);
                string param1 = "";
                string filename = "";
                string param2 = "";
                string paramSemester = "";
                string paramTahunAjaran = "";


                string reportType = "doc";
                string mimeType;
                string encoding;
                string fileNameExtension;


                string[] streamids = null;
                String extension = null;
                Byte[] bytes = null;
                Warning[] warnings;

                bytes = lr.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(bytes);

                result.Content = new StreamContent(stream);

                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Report-Pembayaran-Project" + UserId() + DateTime.Now.ToString("dd-MM-yy") + ".xls"
                };

                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                result.Content = new StringContent(sb.ToString());

                return result;
                //Display or log the error based on your application.
            }
        }

        // Report Penilaian Vendor
        public HttpResponseMessage ReportPenilaian(Guid Id)
        {
            try
            {
                LocalReport lr = new LocalReport();
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_REPORT_PATH;

                path = Path.Combine(path, "ReportPenilaianVendor.rdlc");
                if (System.IO.File.Exists(path))
                {
                    lr.ReportPath = path;
                }

                else
                {
                    //return View("Index");
                }
                var id = Id;

                var PenilaianVendor = _repository.GetReportPenilaianVendor(id, UserId());

                int i = 0;
                foreach (var item in PenilaianVendor)
                {
                    if (i != 0)
                    {
                        item.Judul = "";
                        item.Vendor = "";
                    }
                    i = 1;
                }

                ReportDataSource rd = new ReportDataSource("PenilaianVendor", PenilaianVendor);
                lr.DataSources.Add(rd);
                string param1 = "";
                string filename = "";
                string param2 = "";
                string paramSemester = "";
                string paramTahunAjaran = "";


                string reportType = "doc";
                string mimeType;
                string encoding;
                string fileNameExtension;


                string[] streamids = null;
                String extension = null;
                Byte[] bytes = null;
                Warning[] warnings;

                bytes = lr.Render("Excel", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                Stream stream = new MemoryStream(bytes);

                result.Content = new StreamContent(stream);

                //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Report-Nilai-Kinerja-Vendor" + UserId() + DateTime.Now.ToString("dd-MM-yy") + ".xls"
                };

                return result;
            }
            catch (ReflectionTypeLoadException ex)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                result.Content = new StringContent(sb.ToString());

                return result;
                //Display or log the error based on your application.
            }
        }
    }
}
