using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MTF_x.Models
{
     [Table("Pengadaan", Schema="Pengadaan")]
    public class Pengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(255)]
        public string Judul { get; set; }
        [MaxLength(500)]
        public string Keterangan { get; set; }
        [MaxLength(50)]
        public string AturanPengadaan { get; set; }
        [MaxLength(50)]
        public string AturanBerkas { get; set; }
        [MaxLength(50)]
        public string AturanPenawaran { get; set; }
        [MaxLength(50)]
        public string MataUang { get; set; }
        [MaxLength(50)]
        public string PeriodeAnggaran { get; set; }
        [MaxLength(50)]
        public string JenisPembelanjaan { get; set; }
        public Guid? HpsId { get; set; }
        public string TitleDokumenNotaInternal { get; set; }
        [MaxLength(50)]
        public string UnitKerjaPemohon { get; set; }
        public string TitleDokumenLain { get; set; }
        [MaxLength(50)]
        public string Region { get; set; }
        [MaxLength(50)]
        public string Provinsi { get; set; }
        [MaxLength(50)]
        public string KualifikasiRekan { get; set; }
        [MaxLength(50)]
        public string JenisPekerjaan { get; set; }
        public EStatusPengadaan? Status { get; set; }
        public EGroupPengadaan? GroupPengadaan { get; set; }
        public string TitleBerkasRujukanLain { get; set; }
        public string NoPengadaan { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? TanggalMenyetujui { get; set; }
        public Decimal? Pagu { get; set; }
        public virtual ICollection<JadwalPengadaan> JadwalPengadaans { get; set; }
        public virtual ICollection<KualifikasiKandidat> KualifikasiKandidats { get; set; }
        public virtual ICollection<JadwalPelaksanaan> JadwalPelaksanaans { get; set; }
    }

     [Table("JadwalPengadaan", Schema = "Pengadaan")]
     public class JadwalPengadaan
     {
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public Guid Id { get; set; }
         [Column("tipe")]
         public string Tipe { get; set; }
         public DateTime? Mulai { get; set; }
         public DateTime? Sampai { get; set; }
         [ForeignKey("PengadaanId")]
         public virtual Pengadaan Pengadaan { get; set; } 
         public Guid? PengadaanId { get; set; }
     }


     [Table("JadwalPelaksanaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
     public class JadwalPelaksanaan
     {
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public Guid Id { get; set; }
         [ForeignKey("Pengadaan")]
         public Nullable<Guid> PengadaanId { get; set; }
         public Nullable<EStatusPengadaan> statusPengadaan { get; set; }
         public Nullable<DateTime> Mulai { get; set; }
         public Nullable<DateTime> Sampai { get; set; }
         public virtual Pengadaan Pengadaan { get; set; }
     }

     [Table("KualifikasiKandidat", Schema = "Pengadaan")]
     public class KualifikasiKandidat
     {
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public Guid Id { get; set; }
        
         public Guid? PengadaanId { get; set; }
         public string Kualifikasi { get; set; }
         [ForeignKey("PengadaanId")]
         public virtual Pengadaan Pengadaan { get; set; }
     }

     public enum EStatusPengadaan
     {
         DRAFT, AJUKAN, DISETUJUI, AANWIJZING, SUBMITPENAWARAN, BUKAAMPLOP, PENILAIAN, KLARIFIKASI, PEMENANG, ARSIP, DITOLAK, DIBATALKAN
     }

     public enum EStatusPengadaanVendor
     {
         SUBMITPENAWARAN, KLARIFIKASI, DITOLAK
     }

     public enum EGroupPengadaan
     {
         PERLUPERHATIAN, DALAMPELAKSANAAN, BELUMTERJADWAL, ARSIP, ALL
     }

   

}