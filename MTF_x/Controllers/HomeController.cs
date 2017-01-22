using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MTF_x.Models;
using WebGrease.Css.Ast.Selectors;
using Reston.Pinata.Model.PengadaanRepository;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Repository;
using Model.Helper;
using MTF_x.Helper;

namespace MTF_x.Controllers
{

    public class HomeController : BaseController
    {
        PengadaanContext context = new PengadaanContext();
        private IPengadaanRepo _repository;
        private IVendorRepo _repositoryVendor;
        public HomeController()
        {
            _repository = new PengadaanRepo(new JimbisContext());
            _repositoryVendor = new VendorRepo(new JimbisContext());
        }
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Login()
        {
            return RedirectToAction("Announcement");
        }
        public void Signout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            Redirect(IdLdapConstants.IDM.Url);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FindData()
        {
            var sql = @"select * from  pengadaan.pengadaan x 
                        where aturanpengadaan = 'Pengadaan Terbuka' and  x.Id in (select distinct pengadaanid from pengadaan.JadwalPengadaan 
                        where tipe='pendaftaran' and mulai <= getdate() and Sampai >= GETDATE())";

            var dt = context.Pengadaan.SqlQuery(sql).ToList();
            return View(dt);
        }

        public ActionResult Announcement()
        {
            var dt = _repository.GetPengadaanAnnouncment();
            return View(dt);
        }

        public ActionResult DetailData(Guid idGuid)
        {
            ViewBag.ProcUrl = IdLdapConstants.Proc.Url;
            
            ViewBag.IsDaftar=0;
            if (User.Identity.IsAuthenticated==true)
            {
                var userid = UserId();
                var userVendor = _repositoryVendor.GetVendorByUser(UserId());
                if (userVendor != null)
                {
                    var pengadaan = _repository.GetPengadaanByiD(idGuid);
                    if (pengadaan != null)
                    {
                        var findVendor = pengadaan.KandidatPengadaans.Where(d => d.VendorId == userVendor.Id).FirstOrDefault();
                        if (findVendor != null) ViewBag.IsDaftar = 1;
                    }
                }
            }
            var dt = _repository.GetPengadaanByiD(idGuid);
            return View(dt);
        }

        public ActionResult CriteriaQuality(Guid idGuid)
        {
            var dt = context.Pengadaan.Find(idGuid);
            var oo = new KriteriaKualifikasi(dt);
            return View(oo);
        }
        
        [Authorize]
        public ActionResult Daftar(Guid id)
        {
            string status="";
            var pengadaan = _repository.GetPengadaan(id, UserId(), 0);
            var vendor = _repositoryVendor.GetVendorByUser(UserId());
            if (vendor == null)
            {
                status = "Vendor Tidak Terdaftar";
                ViewBag.status = status;
                return View();
            }
            KandidatPengadaan ndata = new KandidatPengadaan();
            ndata.PengadaanId=id;
            ndata.VendorId=vendor.Id;
            ndata.addKandidatType = addKandidatType.VENDORSELFADDED;
            var result = _repository.addKandidatPilihanVendor(ndata, UserId());
            if (result.Id == null) status = "Vendor Tidak Berhasil Mendaftar";
            status = "Anda Berhasil Mendaftar";
            ViewBag.status = status;
            return View();
        }
    }
}