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

namespace Reston.EProc.Web.Controllers
{
    public class ProyekController : BaseController
    {
        private IProyekRepo _repository;

        public ProyekController()
        {
            _repository = new ProyekRepo(new JimbisContext());
        }

        public ViewProyekPerencanaan TampilJudul()
        {
            Guid PengadaanId = Guid.Parse(HttpContext.Current.Request["Id"].ToString());

            return _repository.GetDataProyek(PengadaanId);
        }
    }
}
