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
