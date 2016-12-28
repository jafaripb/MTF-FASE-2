using Reston.Eproc.Model.Monitoring.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.PengadaanRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Entities
{
    // Query membuat table di  database 

    [Table("MonitoringPekerjaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class MonitoringPekerjaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //------------------------------------------------------
        [ForeignKey("Pengadaan")]   //-- yang atas punya yang bawah
        public Guid PengadaanId { get; set; }  
        //-----------------------------------------------------
        
        public StatusMonitored? StatusMonitoring { get; set; }
        // ----------------------------------------------------
        
        public StatusSeleksi? StatusSeleksi { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        //-----------------------------------------------------
        public virtual Pengadaan Pengadaan { get; set; }  //-- ditambahakan untuk foreign key
    }

    [Table("JadwalProyek", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class JadwalProyek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Pengadaan")]
        public Guid PengadaanId { get; set; }

        public string StartDate { get; set; }
        public string EndDate {get;set;}
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("DetailPekerjaan", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class DetailPekerjaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Pengadaan")]
        public Guid PengadaanId { get; set; }
        public string NamaPekerjaan {get; set;}
        public int BobotPekerjaan{get; set;}
        public int ProgressPekerjaan {get; set;}
        public string StartDate {get; set;}
        public string EndDate {get; set;}
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }

    }
   
    /////////////////////////////------------------------------------------------------------------------------
    // Monitoring Proyek

    [Table("RencanaProyek", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class RencanaProyek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Spk")]
        public Nullable<Guid> SpkId { get; set; }
        public string NoKontrak { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
        public string Status { get; set; }
        public string StatusLockTahapan { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual Spk Spk { get; set; }
        public virtual ICollection<PICProyek> PICProyeks { get; set; }
        public virtual ICollection<TahapanProyek> TahapanProyeks { get; set; }
    }

    [Table("TahapanProyek", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class TahapanProyek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("RencanaProyek")]
        public Guid ProyekId { get; set; }
        public string NamaTahapan { get; set; }
        public Nullable<DateTime> TanggalMulai { get; set; }
        public Nullable<DateTime> TanggalSelesai { get; set; }
        public string JenisTahapan { get; set; }
        public decimal Progress { get; set; }
        public decimal BobotPekerjaan { get; set; }
        public decimal PersenPembayaran { get; set; }
        public string StatusPembayaran { get; set; }
        public string KonfirmasiPengecekanDokumen { get; set; }
        public Nullable<DateTime> TanggalPembayaran { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual RencanaProyek RencanaProyek { get; set; }
    }

    [Table("PICProyek", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class PICProyek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("RencanaProyek")]
        public Guid ProyekId { get; set; }
        public Guid UserId { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string tipe { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual RencanaProyek RencanaProyek { get; set; }
    }

    [Table("DokumenProyek", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class DokumenProyek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("TahapanProyek")]
        public Guid TahapanId { get; set; }
        public string NamaDokumen { get; set; }
        public string JenisDokumen { get; set; }
        public string URL { get; set; }
        public string ContentType { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual TahapanProyek TahapanProyek { get; set; }
    }
}
