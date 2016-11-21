using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Helper.Model;

namespace Reston.Helper.Repository
{

     public interface IHelperRepo
    {
         Captcha GetCaptcha(Guid id);
         void AddCaptchaRegistration(Captcha c);
    }
     public class HelperRepo : IHelperRepo
     {
         HelperContext ctx;
         public HelperRepo(HelperContext j)
        {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }
         public Captcha GetCaptcha(Guid id)
         {
             return ctx.Captchas.Find(id);
         }
         public void AddCaptchaRegistration(Captcha c)
         {
             //delete expired
             var expired = ctx.Captchas.Where(x => x.ExpiredDate < DateTime.Now);
             ctx.Captchas.RemoveRange(expired);

             ctx.Captchas.Add(c);
             ctx.SaveChanges();
         }
     }
}
