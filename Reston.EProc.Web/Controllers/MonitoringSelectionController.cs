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

namespace Reston.EProc.Web.Controllers
{
    public class MonitoringSelectionController : BaseController
    {

        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadMonitoringDokumen"];

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

        public IHttpActionResult ListProyekDetailMonitoringPembayaran()
        {
            string search = HttpContext.Current.Request["search"].ToString();
            int start = Convert.ToInt32(HttpContext.Current.Request["start"]);
            int length = Convert.ToInt32(HttpContext.Current.Request["length"]);
            Guid Id = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataListProyekDetailMonitoringPembayaran(search, start, length, Id));
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

            return Json(_repository.GetDetailProyek(ProyekId));
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
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FILE_DOKUMEN_PATH + d.URL;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(d.ContentType);

            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = d.URL
            };

            return result;

            //return null;
        }

        public  async Task<IHttpActionResult> UploadFile()
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
               //// filePathSave += tipe + "-" + newGuid.ToString() + "." + extension;
                //string fileName = ." + extension;
                // var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //new PhysicalFileSystem(@"..\Reston.Pinata\WebService\Upload\Vendor\Dokumen\");
                NamaFileSave = "Dokumen" + DokumenId.ToString()+ "-" + NamaDokumen + "." + extension;

                try
                {
                    FileStream fs = new FileStream(root +"//"+ NamaFileSave, FileMode.CreateNew);
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
                    return Json(_repository.saveDokumenProyeks(DokumenId, NamaFileSave, extension, UserId()));
                    
                }
                catch (Exception ex)
                {
                    return Json(0);
                }
            }

            return null;

        }



    }
}
