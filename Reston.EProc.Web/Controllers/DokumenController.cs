using Microsoft.Owin.FileSystems;
using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.Repository;
using Reston.Pinata.WebService.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net;

namespace Reston.Pinata.WebService
{
    public class DokumenController : ApiController
    {
        private IDokumenRepo _repository;
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"];
        private string FILE_VENDOR_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_VENDOR"];
        public DokumenController()
        {
            _repository = new DokumenRepo(new JimbisContext());
        }

        public DokumenController(DokumenRepo repository)
        {
            _repository = repository;
        }

        public IEnumerable<string> Get()
        {
            return new[] { "Vendor is so", "more of it", "more" };
        }

        public IHttpActionResult GetDokumenDetail(string id)
        {
            //do something, prevent access etc
            Dokumen d = _repository.GetDokumen(new Guid(id));
            var dict = new Dictionary<string, object>();
            dict["id"] = d.Id.ToString();
            dict["file"] = d.File;
            if (d is DokumenDetail)
            {
                DokumenDetail _vd = (DokumenDetail)d;
                dict["nomor"] = _vd.Nomor;
            }
            else if (d is AktaDokumenDetail)
            {
                AktaDokumenDetail _vd = (AktaDokumenDetail)d;
                dict["nomor"] = _vd.Nomor;
                dict["tanggal"] = _vd.Tanggal;
                dict["notaris"] = _vd.Notaris;
            }
            else if (d is IzinUsahaDokumenDetail)
            {
                IzinUsahaDokumenDetail _vd = (IzinUsahaDokumenDetail)d;
                dict["nomor"] = _vd.Nomor;
                dict["masaberlaku"] = _vd.MasaBerlaku;
                dict["instansi"] = _vd.Instansi;
                dict["klasifikasi"] = _vd.Klasifikasi;
                dict["kualifikasi"] = _vd.Kualifikasi;
            }
            return Json(dict);
        }

        [System.Web.Http.HttpGet]
        public string ViewFile(string id)
        {
            Dokumen d = _repository.GetDokumen(new Guid(id));
            if (d == null)
                return "";
            string bUrl = "/ViewerNotSupported.html?id=";
            if (d.ContentType.Contains("image/"))
                bUrl = "/ImageViewer.html?id=";
            else if (d.ContentType.Contains("application/pdf"))
                bUrl = "/api/Dokumen/OpenFile/";
            return bUrl + id;
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult ViewFile2(string id)
        {
            Dokumen d = _repository.GetDokumen(new Guid(id));
            if (d != null) return Json(new { id = id, contentType = d.ContentType });
            return Json(new { id = "", content = "" });
        }

        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage OpenFile(string id)
        {
            Dokumen d = _repository.GetDokumen(new Guid(id));
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + d.File;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(d.ContentType);

            return result;
        }

        [HttpPost]
        public async Task<IHttpActionResult> UploadFile(string tipe)
        {
            //bool isSavedSuccessfully = true;
            string filePathSave = FILE_TEMP_PATH;
            var s = await Request.Content.ReadAsStreamAsync();
            var provider = new MultipartMemoryStreamProvider();
            Guid tempFileName = Guid.NewGuid();
            string contentType = "";
            string fileExtension = "";

            await Request.Content.ReadAsMultipartAsync(provider);
            foreach (var file in provider.Contents)
            {
                string filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                contentType = file.Headers.ContentType.ToString();
                fileExtension = filename.Substring(filename.IndexOf(".") + 1, filename.Length - filename.IndexOf(".") - 1);
                byte[] buffer = await file.ReadAsByteArrayAsync();
                filePathSave += tempFileName.ToString() + "." + fileExtension;
                var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //new PhysicalFileSystem(@"..\Reston.Pinata\WebService\Upload\Vendor\Dokumen\");

                try
                {
                    FileStream fs = new FileStream(uploadPath.ToString() + filePathSave, FileMode.CreateNew);

                    await fs.WriteAsync(buffer, 0, buffer.Length);
                    fs.Close();
                    //isSavedSuccessfully = true;
                }
                catch (Exception e)
                {
                    return InternalServerError();
                }
            }
            return Json(new { File = tempFileName.ToString() + '.' + fileExtension, ContentType = contentType });
        }

    }
}
