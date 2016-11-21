using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdLdap.Controllers
{
    [AllowAnonymous]
    public class HandleExceptionController : Controller
    {
        // GET: HandleException
        public ActionResult Index()
        {
            ViewBag.RedirectReason = "SOMETHING WENT WRONG.";
            if (TempData["RedirectReason"] != null)
            {
                ViewBag.RedirectReason = TempData["RedirectReason"].ToString();
            }



            return View();
        }

    }
}