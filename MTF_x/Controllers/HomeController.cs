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

           // sql = @"select * from  pengadaan.pengadaan x";
            var dt = context.Pengadaan.SqlQuery(sql).ToList();
            return View(dt);
        }

        public ActionResult Announcement()
        {
            /* var sql = @"select * from  pengadaan.pengadaan x 
                         where aturanpengadaan = 'Pengadaan Terbuka' and x.Id in 
                         (
                             select distinct pengadaanid from pengadaan.JadwalPengadaan 
                             where tipe='pendaftaran' and Sampai < GETDATE()
                         )";*/
           // var sql = @"select * from  pengadaan.pengadaan x where aturanpengadaan = 'Pengadaan Terbuka' and GroupPengadaan='1'";
           // var dt = context.Pengadaan.Where(d => d.AturanPengadaan== "Pengadaan Terbuka" & d.GroupPengadaan==MTF_x.Models.EGroupPengadaan.DALAMPELAKSANAAN).ToList(); //context.Pengadaan.SqlQuery(sql).ToList();
           var dt = _repository.GetPengadaanAnnouncment();
          /*  var list = new List<AnnouncementPengadaan>();
            foreach (var pengadaan in dt)
            {
                var an = new AnnouncementPengadaan(pengadaan);
                list.Add(an);    
            }*/
            return View(dt);
        }

        public ActionResult DetailData(Guid idGuid)
        {
            var dt = context.Pengadaan.Find(idGuid);
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
            if(vendor==null)status="Vendor Tidak Terdaftar";
            PelaksanaanPemilihanKandidat ndata=new PelaksanaanPemilihanKandidat();
            ndata.PengadaanId=id;
            ndata.VendorId=vendor.Id;
            var result = _repository.addKandidatPilihanVendor(ndata, UserId());
           if (result.Id == null) status = "Vendor Tidak Berhasil Mendaftar";
           status = "Anda Berhasil Mendaftar";
           ViewBag.status = status;
            return View();
        }
    }
}