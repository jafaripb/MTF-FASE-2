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

namespace Reston.Pinata.WebService.Controllers
{
    public class VendorActionController : BaseController
    {
        private IPengadaanRepo _repository;
        internal ResultMessage result = new ResultMessage();
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"];
        private string FILE_PENGADAAN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOC"];
        //const string[] arrRoleExRekanan =  { IdLdapConstants.Roles.pRole_procurement_head, 
        //                                    IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_end_user,
        //                                     IdLdapConstants.Roles.pRole_procurement_manager};


        public VendorActionController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
        }

        public VendorActionController(PengadaanRepo repository)
        {
            _repository = repository;
        }
       


        public class DataTableRksRekanan
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<VWRKSDetailRekanan> data { get; set; }
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        public ViewPengadaan detailPengadaanForRekanan(Guid Id)
        {
            //Guid UserID = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
            return _repository.GetPengadaanForRekanan(Id, UserId());
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
        public async Task<IHttpActionResult> UploadFile(string tipe, Guid id)
        {
            TipeBerkas t = (TipeBerkas)Enum.Parse(typeof(TipeBerkas), tipe);
            if (t == TipeBerkas.BerkasRekanan)
            {
                var getpengadaan = _repository.GetPengadaan(id, UserId(), await isApprover());
                if (getpengadaan == null) return Json(0);
                if (getpengadaan.Status != EStatusPengadaan.SUBMITPENAWARAN) return Json(0);
            }
            if (t == TipeBerkas.BerkasRekananKlarifikasi)
            {
                var getpengadaan = _repository.GetPengadaan(id, UserId(), await isApprover());
                if (getpengadaan == null) return Json(0);
                if (getpengadaan.Status != EStatusPengadaan.KLARIFIKASI) return Json(0);
            }
            if (t != TipeBerkas.BerkasRekananKlarifikasi && t != TipeBerkas.BerkasRekanan)
            {
                return Json(0);
            }
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public List<VWDokumenPengadaan> getDokumens(TipeBerkas tipe, Guid Id)
        {
            return _repository.GetListDokumenPengadaan(tipe, Id, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public ResultMessage deleteDokumenPelaksanaan(Guid Id)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                int result = _repository.deleteDokumenRekanan(Id, UserId());
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


        [ApiAuthorize(IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public string statusVendor(Guid Id)
        {
            return _repository.statusVendor(Id, UserId());
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetSubmitPenawran(Guid PId)
        {
            return _repository.getPelaksanaanSubmitPenawaran(PId);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_vendor)]
        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public JadwalPelaksanaan GetKlarifikasi(Guid PId)
        {
            return _repository.getPelaksanaanKlarifikasi(PId, UserId());
        }

    }
}
