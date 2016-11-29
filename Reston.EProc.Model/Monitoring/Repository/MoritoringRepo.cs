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

        ResultMessage Save(Guid PengadaanId, StatusMonitored nStatusMonitoring, StatusSeleksi nStatusSeleksi,Guid UserId);
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

        
        public DataTableViewMonitoring GetDataMonitoringSelection(string search, int start, int length,StatusSeleksi? status)
        {
            DataTableViewMonitoring dt = new DataTableViewMonitoring();

            // record total yang tampil 
            dt.recordsTotal = ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.PEMENANG && 
                d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja ).FirstOrDefault() != null).Count();
            
            // filter berdasarkan pencarian
            dt.recordsFiltered=ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.PEMENANG && d.Judul.Contains(search)  && 
                d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null
                 && d.MonitoringPekerjaans.Where(x=>x.StatusMonitoring==StatusMonitored.TIDAK).Count()>0 &&   (d.MonitoringPekerjaans.Where(x=>x.StatusSeleksi==status).Count()>0|| status==null)).Count();     
            
            var carimonitoring = ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.PEMENANG && d.Judul.Contains(search)&&
                d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null &&
                 d.MonitoringPekerjaans.Where(x=>x.StatusMonitoring!=StatusMonitored.TIDAK).Count()>0 && (d.MonitoringPekerjaans.Where(x => x.StatusSeleksi == status).Count() > 0 || status == null)).OrderByDescending(d => d.CreatedOn).Skip(start).Take(length).ToList();
           
            List<ViewMonitoringSelection> LstnViewMonitoringSelection = new List<ViewMonitoringSelection>();
            
            foreach(var item in carimonitoring)
            {
                ViewMonitoringSelection nViewMonitoringSelection = new ViewMonitoringSelection();

                nViewMonitoringSelection.Id = item.Id;
                nViewMonitoringSelection.NamaPekejaan = item.Judul;
                nViewMonitoringSelection.Klasifikasi = item.JenisPekerjaan;
                var RksHeader=ctx.RKSHeaders.Where(d=>d.PengadaanId==item.Id).FirstOrDefault();
                var TotalHps = RksHeader!=null? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah == null ? 0 : d.jumlah * d.hps == null ? 0 : d.hps):0;
                nViewMonitoringSelection.Nilai = TotalHps.Value;
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
