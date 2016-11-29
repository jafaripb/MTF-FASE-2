using Reston.Eproc.Model.Approval.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.PengadaanRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Approval.Repository
{
    public interface IApprovalRepo
    {
            
    }

    public class ApprovalRepo : IApprovalRepo
    {
        JimbisContext ctx;

        public ApprovalRepo(JimbisContext j)
        {
            ctx = j;

            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public DataTableViewApprovalh GetDataTableViewApprovalh(string search, int start, int length, string klasifikasi)
        {
            DataTableViewApprovalh dt = new DataTableViewApprovalh();

            dt.recordsTotal = ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.DISETUJUI).Count();

            dt.recordFiltered = ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.DISETUJUI).Count();

            var tampilApproval =ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.DISETUJUI && d.Judul.Contains(search) ) ;

            List<ViewApprovalh> lstnViewApprovalh = new List<ViewApprovalh>();

            foreach(var item in tampilApproval)
            {
                ViewApprovalh nViewApprovalh = new ViewApprovalh();

                nViewApprovalh.NamaPekerjaan = item.Judul;

                var RKSHeader = ctx.RKSHeaders.Where(d => d.PengadaanId==item.Id).FirstOrDefault();
                
                var TotalHPS = RKSHeader != null? ctx.RKSDetails.Where(d => d.RKSHeaderId == RKSHeader.Id).Sum(d => d.jumlah == null ? 0: d.jumlah * d.hps == null ? 0 : d.hps):0;

                //------
                nViewApprovalh.Total = TotalHPS.Value;
                //nViewApprovalh.Approved = ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.DISETUJUI).FirstOrDefault() == null ? EStatusPengadaan.AJUKAN : ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.DISETUJUI).FirstOrDefault();

            }

            return dt;
        }
    }
}
