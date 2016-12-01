using Reston.Eproc.Model.Monitoring.Model;
using Reston.Eproc.Model.Monitoring.Repository;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using Reston.Pinata.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
