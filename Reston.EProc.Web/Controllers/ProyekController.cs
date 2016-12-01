using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Eproc.Model.Monitoring.Repository;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using Reston.Pinata.WebService;
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

        public ProyekController()
        {
            _repository = new ProyekRepo(new JimbisContext());
        }

        public IHttpActionResult TampilJudul()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json( _repository.GetDataProyek(PengadaanId));
        }

        public IHttpActionResult TampilTahapanPekerjaan()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return Json(_repository.GetDataPekerjaan(PengadaanId));
        }

        public IHttpActionResult SimpanRencanaProyek()
        {
            Guid xPengadaanId = Guid.Parse(HttpContext.Current.Request["aPengadaanId"].ToString());
            DateTime? xStartDate = Common.ConvertDate(HttpContext.Current.Request["aStartDate"].ToString(), "dd/MM/yyyy HH:mm");
            DateTime? xEndDate = Common.ConvertDate(HttpContext.Current.Request["aEndDate"].ToString(), "dd/MM/yyyy HH:mm");
            string xStatus = HttpContext.Current.Request["aStatus"].ToString();

            return Json(_repository.SimpanRencanaProyekRepo(xPengadaanId, xStatus, UserId(), xStartDate, xEndDate));
        }

        public IHttpActionResult SimpanTahapanPekerjaan()
        {
            Guid xPengadaanId = Guid.Parse(HttpContext.Current.Request["aPengdaanId"].ToString());
            string xNamaTahapanPekerjaan = HttpContext.Current.Request["aNamaTahapanPekerjaan"].ToString();
            DateTime? xTanggalPekerjaan = Common.ConvertDate(HttpContext.Current.Request["aTanggalPekerjaan"].ToString(), "dd/MM/yyyy HH:mm");
            string xJenisPekerjaan = HttpContext.Current.Request["aJenisTahapan"].ToString();

            return Json(_repository.SimpanTahapanPekerjaanRepo(xPengadaanId, xNamaTahapanPekerjaan, xJenisPekerjaan, UserId(), xTanggalPekerjaan));
        }

        public ResultMessage savePersonil(PICProyek Personil)
        {
            HttpStatusCode respon = HttpStatusCode.NotFound;
            string message = "";
            string idx = "0";
            try
            {
                respon = HttpStatusCode.Forbidden;
                message = "Erorr";
                //Guid UserId = new Guid(((ClaimsIdentity)User.Identity).Claims.First().Value);
                PICProyek result = _repository.savePICProyek(Personil, UserId());
                respon = HttpStatusCode.OK;
                message = "Sukses";
                idx = result.Id.ToString();
            }
            catch(Exception ex)
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
    }
}
