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

    public class RencanaPekerjaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Pengadaan")]
        public Guid PengadaanId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    public class TahapanProyek
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("Pengadaan")]
        public Guid PengadaanId { get; set; }
        public string NamaTahapan { get; set; }
        public string Tanggal { get; set; }
    }

    public class PICProyek
    {

    }
}
