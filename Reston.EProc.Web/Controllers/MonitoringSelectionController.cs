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
    public class MonitoringSelectionController : BaseController
    {
        private IMoritoringRepo _repository;

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

            if(klasifikasi == "")
            {
                return Json(_repository.GetDataListProyekMonitoring(search, start, length, null));
            }

            Klasifikasi dklasifikasi = (Klasifikasi)Convert.ToInt32(klasifikasi);
            return Json(_repository.GetDataListProyekMonitoring(search, start, length, dklasifikasi));
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
    }
}
