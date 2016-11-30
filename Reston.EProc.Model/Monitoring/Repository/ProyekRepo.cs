using Reston.Eproc.Model.Monitoring.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Repository
{
    public interface IProyekRepo
    {
        ViewProyekPerencanaan GetDataProyek(Guid PengadaanId);
    }

    public class ProyekRepo : IProyekRepo
    {
        JimbisContext ctx;

        public ProyekRepo(JimbisContext j)
        {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public ViewProyekPerencanaan GetDataProyek(Guid PengadaanId)
        {
            var oProyekPerencanaan = ctx.Pengadaans.Where(d =>d.Id == PengadaanId).FirstOrDefault();

            var vendorId= ctx.PemenangPengadaans.Where(d =>d.PengadaanId == PengadaanId).FirstOrDefault() != null ? ctx.PemenangPengadaans.Where(d =>d.PengadaanId == PengadaanId).FirstOrDefault().VendorId : null;
            var proyek = ctx.RencanaProyeks.Where(d =>d.PengadaanId == PengadaanId).FirstOrDefault();

            return new ViewProyekPerencanaan { Id = oProyekPerencanaan.Id, 
                Judul = oProyekPerencanaan.Judul,
                NoPengadaan = oProyekPerencanaan.NoPengadaan,
                Pelaksana=ctx.Vendors.Where(d =>d.Id == vendorId).FirstOrDefault() != null ? ctx.Vendors.Where(d =>d.Id == vendorId).FirstOrDefault().Nama : null,
                TanggalMulai = proyek != null? proyek.Startdate: null,
                TanggalSelesai = proyek != null? proyek.Enddate : null,
            };
        }
    }

}
