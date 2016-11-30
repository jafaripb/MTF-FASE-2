using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using MTF_x.Models;
using WebGrease.Css.Ast.Selectors;

namespace MTF_x.Controllers
{

    public class HomeController : Controller
    {
        PengadaanContext context = new PengadaanContext();
        public ActionResult Index()
        {
            return View();

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
            var sql = @"select * from  pengadaan.pengadaan x 
                        where aturanpengadaan = 'Pengadaan Terbuka' and x.Id in 
                        (
                            select distinct pengadaanid from pengadaan.JadwalPengadaan 
                            where tipe='pendaftaran' and Sampai < GETDATE()
                        )";

            //sql = @"select * from  pengadaan.pengadaan x";

            var dt = context.Pengadaan.SqlQuery(sql).ToList();

            var list = new List<AnnouncementPengadaan>();
            foreach (var pengadaan in dt)
            {
                var an = new AnnouncementPengadaan(pengadaan);
                list.Add(an);
                
            }
            return View(list);

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
    }
}