using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Eproc.Model.Monitoring.Repository;
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
        ResultMessage deleteTahap(Guid Id, Guid UserId);
        ViewProyekPerencanaan GetDataProyek(Guid PengadaanId);
        DataTableViewTahapanPekerjaan GetDataPekerjaan(Guid PengadaanId);
        DataTableViewTahapanPembayaran GetDataPembayaran(Guid PengadaanId);
        ResultMessage savePICProyek(ViewUntukProyekAddPersonil Personil, Guid UserId);
        ResultMessage SimpanRencanaProyekRepo(Guid xPengadaanId,string xStatus, Guid UserId, DateTime? xStartDate, DateTime? xEndDate);
        ResultMessage SimpanTahapanPekerjaanRepo(Guid xPengadaanId, string xNamaTahapanPekerjaan, string xJenisPekerjaan, Guid UserId, DateTime? xTanggalMulaiPekerjaan, DateTime? xTanggalSelsaiPekerjaan);
        ResultMessage SimpanTahapanPekerjaanDokumenRepo(Guid xId_Tahapan, string xNamaDokumen, string xJenisDokumen, Guid UserId);
        DataTableViewDokumenTahapanPekerjaan GetDataDokumenPekerjaan(Guid TahapanId);
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
            string xNamaTahapanPekerjaan, string xJenisPekerjaan, Guid UserId, DateTime? xTanggalMulaiPekerjaan, DateTime? xTanggalSelesaiPekerjaan)
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

                   TanggalMulai = xTanggalMulaiPekerjaan,
                   TanggalSelesai = xTanggalSelesaiPekerjaan,
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

        public DataTableViewDokumenTahapanPekerjaan GetDataDokumenPekerjaan(Guid TahapanId)
        {
            DataTableViewDokumenTahapanPekerjaan dtd = new DataTableViewDokumenTahapanPekerjaan();

            var caritahapandokumenpekerjaan = ctx.DokumenProyeks.Where(d =>d.TahapanId == TahapanId).ToList();

            List<ViewListTahapanDokumenPekerjaan> vlistViewListTahapanDokumenPekerjaan = new List<ViewListTahapanDokumenPekerjaan>();

            foreach (var item in caritahapandokumenpekerjaan)
            {
                ViewListTahapanDokumenPekerjaan nViewListTahapanDokumenPekerjaan = new ViewListTahapanDokumenPekerjaan();

                nViewListTahapanDokumenPekerjaan.Id = item.Id;
                nViewListTahapanDokumenPekerjaan.NamaDokumen = item.NamaDokumen;
                vlistViewListTahapanDokumenPekerjaan.Add(nViewListTahapanDokumenPekerjaan);
            }
            dtd.data = vlistViewListTahapanDokumenPekerjaan;
            return dtd;
        }

        public DataTableViewTahapanPekerjaan GetDataPekerjaan(Guid PengadaanId)
        {
            DataTableViewTahapanPekerjaan tp = new DataTableViewTahapanPekerjaan();

            var ProyekId2 = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).Count();

            if(ProyekId2 != 0)
            {
                var ProyekId = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault().Id;

                // record total yang tampil 
                tp.recordsTotal = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == ProyekId).Count();

                // filter berdasarkan Id
                tp.recordsFiltered = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == ProyekId).Count();

                var caritahapanpekerjaan = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == ProyekId).ToList();

                List<ViewListTahapan> vListTahapanPekerjaan = new List<ViewListTahapan>();
                foreach (var item in caritahapanpekerjaan)
                {
                    ViewListTahapan nViewListTahapanPekerjaan = new ViewListTahapan();

                    nViewListTahapanPekerjaan.Id = item.Id;
                    nViewListTahapanPekerjaan.NamaTahapan = item.NamaTahapan;
                    nViewListTahapanPekerjaan.TanggalMulai = item.TanggalMulai;
                    nViewListTahapanPekerjaan.TanggalSelesai = item.TanggalSelesai;
                    nViewListTahapanPekerjaan.JenisTahapan = item.JenisTahapan;
                    vListTahapanPekerjaan.Add(nViewListTahapanPekerjaan);
                }
                tp.data = vListTahapanPekerjaan;
            }
            else
            {

            }
            
            return tp;
        }

        public DataTableViewTahapanPembayaran GetDataPembayaran(Guid PengadaanId)
        {
            DataTableViewTahapanPembayaran tp = new DataTableViewTahapanPembayaran();


            var ProyekId2 = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).Count();
            
            if(ProyekId2 != 0)
            {
                var ProyekId = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault().Id;

                // record total yang tampil 
                tp.recordsTotal = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == ProyekId).Count();

                // filter berdasarkan Id
                tp.recordsFiltered = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == ProyekId).Count();

                var caritahapanpembayaran = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == ProyekId).ToList();

                List<ViewListTahapan> vListTahapanPembayaran = new List<ViewListTahapan>();
                foreach (var item in caritahapanpembayaran)
                {
                    ViewListTahapan nViewListTahapanPembayaran = new ViewListTahapan();

                    //nViewListTahapanPembayaran.Id = item.Id;
                    //nViewListTahapanPembayaran.NamaTahapan = item.NamaTahapan;
                    //nViewListTahapanPembayaran.Tanggal = item.Tanggal.Value;
                    //nViewListTahapanPembayaran.JenisTahapan = item.JenisTahapan;
                    //vListTahapanPembayaran.Add(nViewListTahapanPembayaran);
                }
                tp.data = vListTahapanPembayaran;
            }
            else
            {

            }
            
            return tp;
        }

        public ResultMessage SimpanTahapanPekerjaanDokumenRepo(Guid xId_Tahapan, string xNamaDokumen, string xJenisDokumen, Guid UserId)
        {
            ResultMessage rm2 = new ResultMessage();
            try
            {
                DokumenProyek dkm = new DokumenProyek
                {
                    TahapanId = xId_Tahapan,
                    NamaDokumen = xNamaDokumen,
                    JenisDokumen = xJenisDokumen,
                    CreatedOn = DateTime.Now,
                    CreatedBy = UserId,

                };

                ctx.DokumenProyeks.Add(dkm);
                ctx.SaveChanges(UserId.ToString());
                rm2.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                rm2.status = HttpStatusCode.ExpectationFailed;
                rm2.message = ex.ToString();
            }
            return rm2;

            
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

        

        public ResultMessage deleteTahap(Guid Id, Guid UserId)
        {
            ResultMessage msg = new ResultMessage();
            TahapanProyek oData = ctx.TahapanProyeks.Find(Id);
            ctx.TahapanProyeks.Remove(oData);
            try
            {
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
            }
            catch (Exception ex)
            {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }

        public ResultMessage savePICProyek(ViewUntukProyekAddPersonil Personil, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.PengadaanId == Personil.PengadaanId).FirstOrDefault();
                var idproyek = odata.Id;
                var idata = ctx.PICProyeks.Where(d => d.ProyekId == idproyek).FirstOrDefault();
                if (idata != null)
                {}
                else
                {
                    PICProyek p1 = new PICProyek
                    {
                        ProyekId = idproyek,
                        UserId = Personil.UserId,
                        Nama = Personil.Nama,
                        Jabatan = Personil.Jabatan,
                        tipe = Personil.tipe,
                        CreatedOn = DateTime.Now,
                        CreatedBy = UserId
                    };

                    ctx.PICProyeks.Add(p1);
                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.Id = p1.Id.ToString();
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
