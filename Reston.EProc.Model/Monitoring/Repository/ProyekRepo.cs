using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Repository
{
    public interface IProyekRepo
    {
        ViewProyekPerencanaan GetDataProyek(Guid PengadaanId);
        ResultMessage SimpanRencanaProyekRepo(Guid xPengadaanId,string xStatus, Guid UserId, DateTime? xStartDate, DateTime? xEndDate);
        ResultMessage SimpanTahapanPekerjaanRepo(Guid xPengadaanId, string xNamaTahapanPekerjaan, string xJenisPekerjaan, Guid UserId, DateTime? xTanggalPekerjaan);
       
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
                TanggalMulai = proyek != null? proyek.StartDate: null,
                TanggalSelesai = proyek != null? proyek.EndDate : null,
            };
        }
        
        public  ResultMessage SimpanTahapanPekerjaanRepo(Guid xPengadaanId,
            string xNamaTahapanPekerjaan, string xJenisPekerjaan, Guid UserId, DateTime? xTanggalPekerjaan)
        {
            ResultMessage rkk = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d =>d.PengadaanId == xPengadaanId).FirstOrDefault();
                var IdProyek = odata.Id;

                TahapanProyek th = new TahapanProyek
                {
                   ProyekId = IdProyek,
                   NamaTahapan = xNamaTahapanPekerjaan,
                   Tanggal = xTanggalPekerjaan,
                   CreatedOn = DateTime.Now,
                   CreatedBy = UserId,
                   JenisTahapan = xJenisPekerjaan

                };

                ctx.TahapanProyeks.Add(th);
                ctx.SaveChanges(UserId.ToString());
                rkk.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                rkk.status = HttpStatusCode.ExpectationFailed;
                rkk.message = ex.ToString();
                
            }
            return rkk; ;
        }

        public ResultMessage SimpanRencanaProyekRepo(Guid xPengadaanId,string xStatus, Guid UserId, DateTime? xStartDate, DateTime? xEndDate)
        {
            ResultMessage rm = new ResultMessage();

            try
            {
                var odata = ctx.RencanaProyeks.Where(d=>d.PengadaanId==xPengadaanId).FirstOrDefault();

                if(odata != null)
                {

                }
                else
                {
                    RencanaProyek m2 = new RencanaProyek
                    {
                        PengadaanId = xPengadaanId,
                        StartDate = xStartDate,
                        EndDate = xEndDate,
                        Status = xStatus,
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    };

                    ctx.RencanaProyeks.Add(m2);
                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }
    }

}
