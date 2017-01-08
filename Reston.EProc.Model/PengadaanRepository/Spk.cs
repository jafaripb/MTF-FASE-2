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
    [Table("Spk", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class Spk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pks")]
        public Nullable<Guid> PksId { get; set; }
        [ForeignKey("PemenangPengadaan")]
        public Nullable<Guid> PemenangPengadaanId { get; set; }
        //[ForeignKey("DokumenPengadaan")]
        public Nullable<Guid> DokumenPengadaanId { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<int> WorkflowId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        [MaxLength(25)]
        public string NoSPk { get; set; }
        public DateTime? TanggalSPK { get; set; }
        public DateTime? Dari { get; set; }
        public DateTime? Sampai { get; set; }
        public decimal? NilaiSPK { get; set; }
        public StatusSpk StatusSpk { get; set; }
        public virtual PemenangPengadaan PemenangPengadaan { get; set; }
        public virtual Pks Pks { get; set; }
       // public virtual DokumenPengadaan DokumenPengadaan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
        public virtual ICollection<DokumenSpk> DokumenSpk { get; set; }
        public virtual ICollection<RiwayatDokumenSpk> RiwayatDokumenSpk { get; set; }
        

    }

    [Table("DokumenSpk", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class DokumenSpk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Spk")]
        public Nullable<Guid> SpkId { get; set; }
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
        public Nullable<long> SizeFile { get; set; }
        public virtual Spk Spk { get; set; }
    }

    [Table("RiwayatDokumenSpk", Schema = JimbisContext.MONITORING_SCHEMA_NAME)]
    public class RiwayatDokumenSpk
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        [ForeignKey("Spk")]
        public Nullable<Guid> SpkId { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }
        [MaxLength(500)]
        public String Comment { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
        public virtual Spk Spk { get; set; }
    }

    public class DataTableSpkTemplate
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWSpk> data { get; set; }
    }
 
    public class VWSpk
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PksId { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<int> WorkflowId { get; set; }
        public string Note { get; set; }
        public string NoSpk { get; set; }
        public string NoPks { get; set; }
        public string Judul { get; set; }
        public string JenisPekerjaan { get; set; }
        public string Vendor { get; set; }
        public string AturanPengadaan { get; set; }
        public decimal? HPS { get; set; }
        public string NoPengadaan { get; set; }
        public Guid? PengadaanId { get; set; }
        public int? VendorId { get; set; }
        public string Keterangan { get; set; }
        public int? isOwner { get; set; }
        public DateTime? TanggalSPK { get; set; }
        public String TanggalSPKStr { get; set; }
        public decimal? NilaiSPK { get; set; }
        public StatusSpk StatusSpk { get; set; }
        public string StatusSpkName { get; set; }
        public Nullable<Guid> PemenangPengadaanId { get; set; }
    }

    public class VWDokumenSPK
    {
        public Guid Id { get; set; }
        public Nullable<Guid> SpkId { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public Nullable<long> SizeFile { get; set; }
    }


    public enum StatusSpk
    {
        Draft,Aktif,Batal
    }


}
