using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public class RegistrasiRepo : IRegistrasiRepo
    {
        JimbisContext ctx;
        public RegistrasiRepo(JimbisContext j)
        {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public RegVendor GetVendor(int id){
            return ctx.RegVendors.Find(id);
        }

        public RegVendor GetVendor(string noPengajuan)
        {
            RegVendor rv = ctx.RegVendors.Where(x => x.NoPengajuan == noPengajuan).FirstOrDefault();
            if (rv != null) return rv;
            return null;
        }

        public List<RegVendor> GetVendors(ETipeVendor tipe, EStatusVendor status, int limit) {
            if (limit > 0) { 
                List<RegVendor> lv = 
                ctx.RegVendors.Where(x => (tipe == ETipeVendor.NONE || x.TipeVendor == tipe) && (status == EStatusVendor.NONE || x.StatusAkhir == status))
                    .OrderByDescending(x=>x.Id).Take(limit)
                    .ToList();
                return lv;
            }
            return new List<RegVendor>();
        }

        public List<RegVendor> GetAllVendor()
        {
            return ctx.RegVendors.ToList();
        }

        public void Save()
        {
            ctx.SaveChanges();
        }

        public int AddVendor(RegVendor v) {
            ctx.RegVendors.Add(v);
            ctx.SaveChanges();
            return v.Id;
        }

        public RegDokumen GetDokumen(Guid id)
        {
            return ctx.RegDokumens.Find(id);
        }

        public IQueryable<RegDokumen> GetAllDokumenByVendor(int vendorid)
        {
            RegVendor v = ctx.RegVendors.Find(vendorid);
            if (v != null)
                return v.RegDokumen.AsQueryable();
            return null;
        }

        public Guid AddDokumen(RegDokumen d)
        {
            ctx.RegDokumens.Add(d);
            ctx.SaveChanges();
            return d.Id;
        }

        public int RegisterVerifiedVendor(Vendor v) {
            try
            {
                ctx.Vendors.Add(v);
                ctx.SaveChanges();
                return v.Id;
            }
            catch (Exception e) {
                
            }
            return 0;
        }

        public int CheckNomor(string no)
        {
            try
            {
                RegVendor v = ctx.RegVendors.Where(x => x.NoPengajuan.StartsWith(no)).OrderByDescending(x => x.NoPengajuan).FirstOrDefault();
                if (v != null)
                {
                    return Int32.Parse(v.NoPengajuan.Substring(6, 4)) + 1;
                }
            }
            catch (Exception e)
            {
                return 1;
            }
            return 1;
        }

        public CaptchaRegistration GetCaptchaRegistration(Guid id) {
            return ctx.CaptchaRegistration.Find(id);
        }

        public void AddCaptchaRegistration(CaptchaRegistration c) {
            //delete expired
            var expired = ctx.CaptchaRegistration.Where(x => x.ExpiredDate < DateTime.Now);
            ctx.CaptchaRegistration.RemoveRange(expired);

            ctx.CaptchaRegistration.Add(c);
            ctx.SaveChanges();
        }
    }
}
