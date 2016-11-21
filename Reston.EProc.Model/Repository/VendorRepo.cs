using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public class VendorRepo : IVendorRepo
    {
        JimbisContext ctx;
        public VendorRepo(JimbisContext j) {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public Vendor GetVendor(int id){
            Vendor v = ctx.Vendors.Find(id);
            if (v != null) {
                return v;
            }
            return null;
        }

        public Vendor GetVendorByUser(Guid id) {
            Vendor v = ctx.Vendors.Where(x => x.Owner == id).FirstOrDefault();
            if (v != null) return v;
            return null;
        }

        public List<Vendor> GetVendors(ETipeVendor tipe, EStatusVendor status, int limit) {
            if (limit > 0) { 
                List<Vendor> lv = 
                ctx.Vendors.Where(x => (tipe == ETipeVendor.NONE || x.TipeVendor == tipe) && (status == EStatusVendor.NONE || x.StatusAkhir == status))
                    .OrderByDescending(x=>x.Id).Take(limit)
                    .ToList();
                return lv;
            }
            return new List<Vendor>();
        }

        public List<Vendor> GetAllVendor()
        {
            return ctx.Vendors.ToList();
        }

        public void Save()
        {
            ctx.SaveChanges();
        }

        public int AddVendor(Vendor v) {
            ctx.Vendors.Add(v);
            ctx.SaveChanges();
            return v.Id;
        }

        public int CheckNomor(string no) {
            try
            {
                Vendor v = ctx.Vendors.Where(x => x.NomorVendor.StartsWith(no)).OrderByDescending(x => x.NomorVendor).FirstOrDefault();
                if (v != null)
                {
                    return Int32.Parse(v.NomorVendor.Substring(6, 4)) + 1;
                }
            }
            catch (Exception e) {
                return 1;
            }
            return 1;
        }
    }
}
