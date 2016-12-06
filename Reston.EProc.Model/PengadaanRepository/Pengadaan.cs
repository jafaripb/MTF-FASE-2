using Reston.Eproc.Model.Monitoring.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model.JimbisModel;

namespace Reston.Pinata.Model.PengadaanRepository
{
    [Table("Pengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
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
        public Nullable<Guid> HpsId { get; set; }
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
        public Nullable<EStatusPengadaan> Status { get; set; }
        public Nullable<EGroupPengadaan> GroupPengadaan { get; set; }
        public string TitleBerkasRujukanLain { get; set; }
        public string NoPengadaan { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<DateTime> TanggalMenyetujui { get; set; }
        public Nullable<Decimal> Pagu { get; set; }
        public Nullable<int> WorkflowId { get; set; }
        public virtual ICollection<DokumenPengadaan> DokumenPengadaans { get; set; }
        public virtual ICollection<KandidatPengadaan> KandidatPengadaans { get; set; }
        public virtual ICollection<JadwalPengadaan> JadwalPengadaans { get; set; }
        public virtual ICollection<PersonilPengadaan> PersonilPengadaans { get; set; }
        public virtual ICollection<RKSHeader> RKSHeaders { get; set; }
        public virtual ICollection<BintangPengadaan> BintangPengadaans { get; set; }
        public virtual ICollection<MonitoringPekerjaan> MonitoringPekerjaans { get; set; }
        public virtual ICollection<JadwalPelaksanaan> JadwalPelaksanaans { get; set; }
        public virtual ICollection<PersetujuanPemenang> PersetujuanPemenangs { get; set; }
        
    }

    [Table("DokumenPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class DokumenPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        [MaxLength(1000)]
        public string File { get; set; }
        [MaxLength(255)]
        public string ContentType { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<TipeBerkas> Tipe { get; set; }
        public Nullable<long> SizeFile { get; set; }
        public Nullable<int> VendorId { get; set; }

        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("KandidatPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class KandidatPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        [ForeignKey("Vendor")]
        public Nullable<int> VendorId { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
        public virtual Vendor Vendor { get; set; }
    }

    [Table("KualifikasiKandidat", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class KualifikasiKandidat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public string kualifikasi { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("JadwalPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class JadwalPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public string tipe { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PersonilPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PersonilPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<Guid> PersonilId { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string tipe { get; set; }
        public Nullable<int> isReady { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("RKSHeader", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class RKSHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual ICollection<RKSDetail> RKSDetails { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("RKSDetail", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class RKSDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("RKSHeader")]
        public Nullable<Guid> RKSHeaderId { get; set; }
        public string judul { get; set; }
        public Nullable<int> level { get; set; }
        public int? grup { get; set; }
        public Nullable<Guid> ItemId { get; set; }
        public string item { get; set; }
        public string satuan { get; set; }
        public Nullable<decimal> jumlah { get; set; }
        public Nullable<decimal> hps { get; set; }
        public string keterangan { get; set; }
        public virtual RKSHeader RKSHeader { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
    }

    [Table("RKSHeaderTemplate", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class RKSHeaderTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public KlasifikasiPengadaan Klasifikasi { get; set; }
        public String Region { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public virtual ICollection<RKSDetailTemplate> RKSDetailTemplate { get; set; }
    }

    [Table("RKSDetailTemplate", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class RKSDetailTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("RKSHeaderTemplate")]
        public Nullable<Guid> RKSHeaderTemplateId { get; set; }
        public string judul { get; set; }
        public Nullable<int> level { get; set; }
        public Nullable<int> group { get; set; }
        public Nullable<Guid> ItemId { get; set; }
        public string item { get; set; }
        public string satuan { get; set; }
        public Nullable<decimal> jumlah { get; set; }
        public Nullable<decimal> hps { get; set; }
        public string keterangan { get; set; }
        public virtual RKSHeaderTemplate RKSHeaderTemplate { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
    }

    [Table("RiwayatPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class RiwayatPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Waktu { get; set; }
        public EStatusPengadaan Status { get; set; }
        [MaxLength(1000)]
        public string Komentar { get; set; }
        public int Urutan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("RiwayatDokumen", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class RiwayatDokumen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }
        [MaxLength(500)]
        public String Comment { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
    }

    [Table("MessagePengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class MessagePengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Waktu { get; set; }
        public EStatusPengadaan Status { get; set; }
        [MaxLength(1000)]
        public string Message { get; set; }
        public int Urutan { get; set; }
        public Nullable<Guid> UserTo { get; set; }
        public Nullable<Guid> FromTo { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("BintangPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class BintangPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Guid UserId { get; set; }
        public Nullable<int> StatusBintang { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PelaksanaanAanwijzing", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PelaksanaanAanwijzing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public string IsiUndangan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("KehadiranKandidatAanwijzing", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class KehadiranKandidatAanwijzing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PelaksanaanSubmitPenawaran", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PelaksanaanSubmitPenawaran
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
        [ForeignKey("DokumenPengadaan")]
        public Nullable<Guid> DokomenPengadaanId { get; set; }
        public DokumenPengadaan DokumenPengadaan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PelaksanaanBukaAmplop", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PelaksanaanBukaAmplop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
        [ForeignKey("DokumenPengadaan")]
        public Nullable<Guid> DokomenPengadaanId { get; set; }
        public DokumenPengadaan DokumenPengadaan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PersetujuanBukaAmplop", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PersetujuanBukaAmplop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PelaksanaanPenilaianKandidat", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PelaksanaanPenilaianKandidat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
        [ForeignKey("DokumenPengadaan")]
        public Nullable<Guid> DokomenPengadaanId { get; set; }
        public DokumenPengadaan DokumenPengadaan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PemenangPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PemenangPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("PelaksanaanKlarifikasi", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PelaksanaanKlarifikasi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
        [ForeignKey("DokumenPengadaan")]
        public Nullable<Guid> DokomenPengadaanId { get; set; }
        public DokumenPengadaan DokumenPengadaan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
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

    [Table("HargaRekanan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class HargaRekanan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> RKSDetailId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<decimal> harga { get; set; }
        public string hargaEncrypt { get; set; }
        public string keterangan { get; set; }
    }

    [Table("PelaksanaanPemilihanKandidat", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PelaksanaanPemilihanKandidat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    [Table("HargaKlarifikasiRekanan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class HargaKlarifikasiRekanan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> RKSDetailId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<decimal> harga { get; set; }
        public string keterangan { get; set; }
    }

    [Table("CatatanPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class CatatanPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Komentar { get; set; }
        public TipeCatatan tipeCatatan { get; set; }
    }

    [Table("KreteriaPembobotan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class KreteriaPembobotan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string NamaKreteria { get; set; }
        public Nullable<int> Bobot { get; set; }
    }

    [Table("PembobotanPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PembobotanPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<Guid> KreteriaPembobotanId { get; set; }
        public Nullable<int> Bobot { get; set; }
    }

    [Table("PembobotanPengadaanVendor", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PembobotanPengadaanVendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<Guid> KreteriaPembobotanId { get; set; }
        public Nullable<int> Nilai { get; set; }
    }

    [Table("NoDokumenGenerator", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class NoDokumenGenerator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string No { get; set; }
        public Nullable<TipeNoDokumen> tipe { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
    }

    [Table("BeritaAcara", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class BeritaAcara
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> tanggal { get; set; }
        public Nullable<TipeBerkas> Tipe { get; set; }
        public string NoBeritaAcara { get; set; }
    }

    [Table("ReportPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class ReportPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string Judul { get; set; }
        public string User { get; set; }
        public Nullable<decimal> hps { get; set; }
        public Nullable<decimal> realitas { get; set; }
        public Nullable<decimal> efisiensi { get; set; }
        public string Pemenang { get; set; }
        public Nullable<DateTime> Aanwjzing { get; set; }
        public Nullable<DateTime> PembukaanAmplop { get; set; }
        public Nullable<DateTime> Klasrifikasi { get; set; }
        public Nullable<DateTime> Scoring { get; set; }
        public Nullable<DateTime> NotaPemenang { get; set; }
        public Nullable<DateTime> SPK { get; set; }
    }

    [Table("PembatalanPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PembatalanPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string Keterangan { get; set; }
        public DateTime CreateOn { get; set; }
        public Guid CreateBy { get; set; }
    }

    [Table("PenolakanPengadaan", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PenolakanPengadaan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string Keterangan { get; set; }
        public Nullable<int> status { get; set; }
        public DateTime CreateOn { get; set; }
        public Guid CreateBy { get; set; }
    }


    [Table("PersetujuanPemenang", Schema = JimbisContext.PENGADAAN_SCHEMA_NAME)]
    public class PersetujuanPemenang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public StatusPengajuanPemenang Status { get; set; }
        public string Note { get; set; }
        public Nullable<int> WorkflowId { get; set; }
        public  Nullable<DateTime> CreatedOn { get; set; }
        public  Nullable<Guid> CreatedBy { get; set; }
        public  Nullable<DateTime> ModifiedOn { get; set; }
        public  Nullable<Guid> ModifiedBy { get; set; }
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

    public enum TipeBerkas
    {
        NOTA, DOKUMENLAIN, BerkasRujukanLain, BeritaAcaraAanwijzing, BeritaAcaraSubmitPenawaran, BeritaAcaraBukaAmplop, BeritaAcaraPenilaian, BeritaAcaraKlarifikasi, BeritaAcaraPenentuanPemenang, BerkasRekanan, BerkasRekananKlarifikasi, LembarDisposisi, SuratPerintahKerja, BeritaAcaraPendaftaran
    }

    public enum TipeCatatan
    {
        Penilaian, Klarifikasi
    }

    public enum TipeNoDokumen
    {
        PENGADAAN, BERITAACARA, NOTA, SPK
    }

    public enum KlasifikasiPengadaan
    {
        SIPIL,NONSIPIL
    }

    public enum StatusPengajuanPemenang
    {
        BELUMDIAJUKAN,PENDING,APPROVED,REJECTED
    }
}
