using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using Reston.Pinata.Model.PengadaanRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// ---------      query ada di sini

namespace Reston.Eproc.Model.Monitoring.Repository
{
    public interface IMoritoringRepo
    {
        DataTableViewMonitoring GetDataMonitoringSelection(string search, int start, int length, StatusSeleksi? status);

        DataTableViewProyekSistemMonitoring GetDataListProyekMonitoring(string search, int start, int length, Klasifikasi? dklasifikasi);
        DataTableViewProyekDetailMonitoring GetDataListProyekDetailMonitoring(string search, int start, int length,Guid Id, Klasifikasi? dklasifikasi);
        ResultMessage Save(Guid PengadaanId, StatusMonitored nStatusMonitoring, StatusSeleksi nStatusSeleksi,Guid UserId);
        ViewResumeProyek GetResumeProyek();
        ViewDetailMonitoring GetDetailProyek(Guid ProyekId);
    }

    public class MonitoringRepo : IMoritoringRepo
    {
        JimbisContext ctx;

        // koneksi ke database
        public MonitoringRepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public DataTableViewProyekDetailMonitoring GetDataListProyekDetailMonitoring(string search, int start, int length,Guid Id, Klasifikasi? dklasifikasi)
        {
            DataTableViewProyekDetailMonitoring dt = new DataTableViewProyekDetailMonitoring();
            // total
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();
            // record
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();
           

            var tampildetailtahapan = ctx.TahapanProyeks.Where(d => d.ProyekId == Id && d.JenisTahapan == "Pekerjaan") .ToList();

            List<ViewTableDetailPekerjaan> LstnViewTableDetailPekerjaan = new List<ViewTableDetailPekerjaan>();

            foreach (var item in tampildetailtahapan)
            {
                ViewTableDetailPekerjaan vt = new ViewTableDetailPekerjaan();

                vt.Id = Id;
                vt.NamaPekerjaan = item.NamaTahapan;
                vt.StartDate = item.TanggalMulai;
                vt.EndDate = item.TanggalSelesai;

                LstnViewTableDetailPekerjaan.Add(vt);
            }
            dt.data = LstnViewTableDetailPekerjaan;
            return dt;
        }
        public DataTableViewProyekSistemMonitoring GetDataListProyekMonitoring(string search, int start, int length, Klasifikasi? dklasifikasi)
        {
            DataTableViewProyekSistemMonitoring dt = new DataTableViewProyekSistemMonitoring();

            // ambil dari status rencana proyek yang "dijalankan"

            // record total
            dt.recordsTotal = ctx.RencanaProyeks.Where(d => d.Status == "dijalankan").Count();

            //filter berdasarkan pencarian
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Status == "dijalankan").Count();

            var tampilproyek = ctx.RencanaProyeks.Where(d => d.Status == "dijalankan").ToList();

            List<ViewProyekSistemMonitoring> LstnViewProyekSistemMonitoring = new List<ViewProyekSistemMonitoring>();

            foreach (var item in tampilproyek)
            {
                ViewProyekSistemMonitoring vp = new ViewProyekSistemMonitoring();

                var aNamaProyek = ctx.Pengadaans.Where(d => d.Id == item.PengadaanId).FirstOrDefault().Judul;
                var vendorId = ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.PengadaanId).FirstOrDefault() == null ? 0 :
                     ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.PengadaanId).FirstOrDefault().VendorId;
                var vendor = ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault() == null ? "" :
                     ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault().Nama;
                var aklasifikasi = ctx.Pengadaans.Where(d => d.Id == item.PengadaanId).FirstOrDefault().JenisPekerjaan;

                vp.id = item.Id;
                vp.NoKontrak = item.Status;
                vp.NamaProyek = aNamaProyek;
                vp.NamaPelaksana = vendor;
                vp.Klasifikasi = aklasifikasi;

                LstnViewProyekSistemMonitoring.Add(vp);
            }
            dt.data = LstnViewProyekSistemMonitoring;
            return dt;
        }
        
        public DataTableViewMonitoring GetDataMonitoringSelection(string search, int start, int length,StatusSeleksi? status)
        {
            DataTableViewMonitoring dt = new DataTableViewMonitoring();

            // record total yang tampil 
            dt.recordsTotal = ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.PEMENANG && 
            d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja ).FirstOrDefault() != null).Count();
            
            // filter berdasarkan pencarian
            dt.recordsFiltered=ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.PEMENANG && d.Judul.Contains(search)  && 
            d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null
            && d.MonitoringPekerjaans.Where(x=>x.StatusMonitoring==StatusMonitored.TIDAK).Count()==0 &&   (d.MonitoringPekerjaans.Where(x=>x.StatusSeleksi==status).Count()>0|| status==null)).Count();     
            
            var carimonitoring = ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.PEMENANG && d.Judul.Contains(search)&&
            d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null &&
            d.MonitoringPekerjaans.Where(x=>x.StatusMonitoring!=StatusMonitored.TIDAK).Count()==0 && (d.MonitoringPekerjaans.Where(x => x.StatusSeleksi == status).Count() > 0 || status == null)).OrderByDescending(d => d.CreatedOn).Skip(start).Take(length).ToList();
           
            List<ViewMonitoringSelection> LstnViewMonitoringSelection = new List<ViewMonitoringSelection>();
            foreach (var item in carimonitoring)
            {
                ViewMonitoringSelection nViewMonitoringSelection = new ViewMonitoringSelection();

                var vendorId = ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() == null ? 0 : 
                    ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().VendorId;
                var vendor = ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault() == null ? "" :
                    ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault().Nama;
                var tanggalpenentuanpemenang = ctx.DokumenPengadaans.Where(d => d.PengadaanId == item.Id && d.Tipe==TipeBerkas.SuratPerintahKerja).FirstOrDefault();
                var pic = ctx.PersonilPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() == null ? "" :
                    ctx.PersonilPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().Nama;

                nViewMonitoringSelection.Id = item.Id;
                nViewMonitoringSelection.NoPengadaan = item.NoPengadaan;
                nViewMonitoringSelection.Judul = item.Judul;
                nViewMonitoringSelection.Pemenang = vendor;
                nViewMonitoringSelection.Klasifikasi = item.JenisPekerjaan;
                //nViewMonitoringSelection.TanggalPenentuanPemenang = tanggalpenentuanpemenang.;
                var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == item.Id).FirstOrDefault();
                var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah * d.hps == null ? 0 : d.jumlah * d.hps) : 0;
                nViewMonitoringSelection.NilaiKontrak = TotalHps.Value;
                nViewMonitoringSelection.PIC = pic;

                if (ctx.MonitoringPekerjaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() != null)
                {
                    nViewMonitoringSelection.Monitored = ctx.MonitoringPekerjaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().StatusMonitoring.Value;
                    nViewMonitoringSelection.Status = ctx.MonitoringPekerjaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().StatusSeleksi.Value;
                }
                LstnViewMonitoringSelection.Add(nViewMonitoringSelection);
            }
            dt.data = LstnViewMonitoringSelection;
            return dt;
        }

        public ViewResumeProyek GetResumeProyek()
        {
            var TotalProyekDalamPelaksanaan = ctx.RencanaProyeks.Where(d => d.Status == "dijalankan").Count();
            var TotalProyekLewatWaktuPelaksanaan = 0;
            var TotalProyekMendekatiWaktuPelaksanaan = 0;

            return new ViewResumeProyek
            {
                ProyekDalamPelaksanaan = TotalProyekDalamPelaksanaan,
                ProyekLewatWaktuPelaksanaan = TotalProyekLewatWaktuPelaksanaan,
                ProyekMendekatiWaktuPelaksanaan = TotalProyekMendekatiWaktuPelaksanaan,
            };
        }

        public ViewDetailMonitoring GetDetailProyek(Guid ProyekId)
        {
            var odata = ctx.RencanaProyeks.Where(d => d.Id == ProyekId).FirstOrDefault();
            var NamaProyek = ctx.Pengadaans.Where(d => d.Id == odata.PengadaanId).FirstOrDefault().Judul;

            return new ViewDetailMonitoring
            {
                Id = odata.Id,
                NamaProyek = NamaProyek,
                StarDate = odata.StartDate,
                EndDate = odata.EndDate
            };
        }

        public ResultMessage Save(Guid nPengadaanId, StatusMonitored nStatusMonitoring, StatusSeleksi nStatusSeleksi,Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.MonitoringPekerjaans.Where(d=>d.PengadaanId==nPengadaanId).FirstOrDefault();

                if(odata!=null)
                {
                    odata.StatusMonitoring = nStatusMonitoring;
                    odata.StatusSeleksi = nStatusSeleksi;
                    odata.ModifiedBy = UserId;
                    odata.ModifiedOn = DateTime.Now;
                }
                else
                {

                    MonitoringPekerjaan m2 = new MonitoringPekerjaan
                    {
                        PengadaanId = nPengadaanId,
                        StatusMonitoring = nStatusMonitoring,
                        StatusSeleksi = nStatusSeleksi,
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    };

                    ctx.MonitoringPekerjaans.Add(m2);
                }

            
                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.message = "Sukses";
            }
            catch(Exception ex){
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }


     
    }
}
