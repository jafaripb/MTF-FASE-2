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
    [Table("Pks", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class Pks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("PksParent")]
        public Nullable<Guid> PksId { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<int> WorkflowId { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        [MaxLength(25)]
        public string NoDokumen { get; set; }
        public StatusPks StatusPks { get; set; }
        [ForeignKey("PemenangPengadaan")]
        public Nullable<Guid> PemenangPengadaanId { get; set; }
        public virtual PemenangPengadaan PemenangPengadaan { get; set; }
        public virtual Pks PksParent { get; set; }
        public virtual ICollection<DokumenPks> DokumenPks { get; set; }
        public virtual ICollection<CatatanPks> CatatanPks { get; set; }
    }

    [Table("DokumenPks", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class DokumenPks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pks")]
        public Nullable<Guid> PksId { get; set; }
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
        public virtual Pks Pks { get; set; }
    }

    [Table("CatatanPks", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class CatatanPks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("Pks")]
        public Nullable<Guid> PksId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Catatan { get; set; }
        public virtual Pks Pks { get; set; }
    }

    public class VWCatatanPks
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PksId { get; set; }
        public Guid CreatedBy { get; set; }
        public string Nama { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime Date { get; set; }
        public string Catatan { get; set; }
        public string Status { get; set; }
    }

    public class DataTablePksTemplate
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWPks> data { get; set; }
    }
    public class VWPks
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
        public string NoPks{ get; set; }
        public string Judul { get; set; }
        public string JenisPekerjaan { get; set; }
        public string Vendor { get; set; }
        public string AturanPengadaan { get; set; }
        public decimal? HPS { get; set; }
            public decimal? NilaiSPK { get; set; }
        public string NoPengadaan { get; set; }
        public Guid? PengadaanId { get; set; }
        public int? VendorId { get; set; }
        public string Title { get; set; }  
        public string Keterangan { get; set; }
        public Nullable<Guid> PemenangPengadaanId { get; set; }
        public StatusPks? StatusPks { get; set; }
        public string StatusPksName { get; set; }
        public int isOwner { get; set; }
        public int Approver { get; set; }

        public string CreatedName { get; set; }
    }


    [Table("RiwayatDokumenPks", Schema = JimbisContext.MONITORING_SCHEMA_NAME)]
    public class RiwayatDokumenPks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public Nullable<Guid> PksId { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }
        [MaxLength(500)]
        public String Comment { get; set; }
        [MaxLength(100)]
        public string Status { get; set; }
    }

    public class VWDokumenPks
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PksId { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public Nullable<TipeBerkas> Tipe { get; set; }
        public Nullable<long> SizeFile { get; set; }
    }

    public enum StatusPks
    {
        Draft, Ajukan,Pending, Approve, Reject
    }
}
