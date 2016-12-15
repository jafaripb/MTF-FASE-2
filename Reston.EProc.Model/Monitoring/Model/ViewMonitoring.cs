using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Model
{
    // membuat viewnya
    public class ViewMonitoringSelection
    {
        public Guid Id { get; set; }
        public string NoPengadaan{ get; set; }
        public string NOSPK { get; set; }
        public string Judul { get; set; }
        public string Pemenang { get; set; }
        public string Klasifikasi { get; set; }
        public DateTime? TanggalPenentuanPemenang { get; set; }
        public Decimal NilaiKontrak { get; set; }
        public String PIC{ get; set; }
        public StatusMonitored Monitored { get; set; }
        public StatusSeleksi Status { get; set; }
    }

    // membuat data table untuk merima data dari AJAX
    public class DataTableViewMonitoring
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public  List<ViewMonitoringSelection> data { get; set; }
    }
    // ----------------------------------------------------------------------------------------------------------------------------
    public class ViewProyekSistemMonitoring
    {
        public Guid id { get; set; }
        public string NOSPK { get; set; }
        public string NOPKS { get; set; }
        public string NamaProyek { get; set; }
        public string NamaPelaksana { get; set; }
        public string Klasifikasi { get; set; }      
		public decimal PersenPekerjaan { get; set; }
        public decimal PersenPembayaran { get; set; }
        public Nullable<DateTime> TanggalMulai { get; set; }
        public Nullable<DateTime> TanggalSelesai { get; set; }  
    }
    public class ViewProyekSistemMonitoringPembayaran
    {
        public Guid ID { get; set; }
        public string NamaPembayaran { get; set; }
        public decimal PersenPembayaran { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> TanggalPembayaran { get; set; }

    }
    public class DataTableViewProyekSistemMonitoring
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewProyekSistemMonitoring> data { get; set; }
    }

    public class DataTableViewProyekSistemMonitoringPembayaran
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewProyekSistemMonitoringPembayaran> data { get; set; }
    }

    // ----------------------------------------------------------------------------------------------------------------------

    public class ViewMonitoringReal
    {
        public Guid id { get; set; }
        public string NamaProyek { get; set; }
        public string NamaVendor { get; set; }
        public string Klasifikasi { get; set; }

    }
    // --------------------------------------------------------------------------------------------------------------------------

    public class ViewResumeProyek
    {
        public int ProyekDalamPelaksanaan { get; set; }
        public int ProyekLewatWaktuPelaksanaan { get; set; }
        public int ProyekMendekatiWaktuPelaksanaan { get; set; }
    }

    public class ViewDetailMonitoring
    {
        public Guid Id { get; set; }
        public string NamaProyek { get; set; }
        public Nullable<DateTime> TanggalMulai { get; set; }
        public Nullable<DateTime> TanggalSelesai { get; set; }
        public decimal NilaiKontrak { get; set; }
        public decimal NilaiKontrak { get; set; }

    }

    // -----------------------------------------------------------------------------------------------------------------------------

    public class ViewTableDetailPekerjaan
    {
        public Guid Id { get; set; }
        public string NamaPekerjaan { get; set; }
        public decimal BobotPekerjaan { get; set; }
        public decimal Progress { get; set; }
        public decimal Penyelesaian { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }

    public class DataTableViewProyekDetailMonitoring
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewTableDetailPekerjaan> data { get; set; }
    }
    
}
