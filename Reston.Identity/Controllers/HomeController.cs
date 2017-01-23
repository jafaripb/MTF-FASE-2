using IdLdap.Configuration;
using Reston.Identity.Helper;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentityServer3.Core.ViewModels;
using System.Threading.Tasks;
using Reston.Identity.Repository.Identity;
using System.Net.Http;
using Reston.Helper;
using System.IO;
using System.Net;
using Reston.Helper.Model;

namespace IdLdap.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {

        }
        public ActionResult Index()
        {
            return RedirectToAction("UserId", "Admin");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return Redirect(returnUrl);

        }

        public JsonResult capca()
        {
            return Json("wtf", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetCaptchaImage(string k)
        {
            try
            {
                CaptchaHelper c = new CaptchaHelper();
                Guid g;

                if (Guid.TryParse(k, out g))
                {
                    var ms = new MemoryStream();
                    (c.GenerateImage(g)).Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    return File(ms.ToArray(), "image/png");
                }
            }
            catch(Exception ex)
            {
                return Json(ex.ToString(),JsonRequestBehavior.AllowGet);
            }
            return  Json("sdsds",JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult createCaptca()
        {
            CaptchaHelper captcha = new CaptchaHelper();
            try
            {
                CaptchaViewModel Generate = captcha.Generate();
                return Json(Generate.Id.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return  Json("sdsds",JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult insertCapcha()
        {
            HelperContext db = new HelperContext();
            try
            {
                var data=new Captcha() { 
                    Text="sdsdsd"
                };
                db.Captchas.Add(data);
                db.SaveChanges();

                return Json(data.Id.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult getCapcha()
        {
            HelperContext db = new HelperContext();
            IdentityContext cx = new IdentityContext();
            try
            {
                var data = db.Captchas.FirstOrDefault();
                //db.SaveChanges();

                return Json(data.Id.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }

        }
        
    }


}