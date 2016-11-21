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
using Novacode;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Drawing;
using Microsoft.Reporting.WebForms;
using System.Reflection;

namespace Reston.Pinata.WebService.Controllers
{
    public class DashboardController : BaseController
    {
        private IPengadaanRepo _repository;
        internal ResultMessage result = new ResultMessage();
        public DashboardController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
        }

        public DashboardController(PengadaanRepo repository)
        {
            _repository = repository;
        }

        public IHttpActionResult GetTblReport(DateTime? from, DateTime? to)
        {
            DateTime f = from ?? DateTime.Now.AddDays(-30);
            DateTime t = to ?? DateTime.Now;
            List<VWReportPengadaan> lv = _repository.GetRepotPengadan(f, t, UserId());
            return Json(new{aaData = lv});
        }

        public IHttpActionResult GetProgress(DateTime? from, DateTime? to) {
            DateTime f = from ?? DateTime.Now.AddDays(-30);
            DateTime t = to ?? DateTime.Now;
            return Json(_repository.GetProgressReport(f,t));
        }

        public IHttpActionResult GetPIC(DateTime? from, DateTime? to) {
            DateTime f = from ?? DateTime.Now.AddDays(-30);
            DateTime t = to ?? DateTime.Now;

            //todo
            return Json(_repository.GetStaffCharges(PengadaanConstants.StaffPeranan.PIC, f,t));
        }

        public IHttpActionResult GetSummaryTotal(DateTime? from, DateTime? to)
        {
            DateTime f = from ?? DateTime.Now.AddDays(-30);
            DateTime t = to ?? DateTime.Now;

            return Json(_repository.GetSummaryTotal(f, t));
        }
    }
}