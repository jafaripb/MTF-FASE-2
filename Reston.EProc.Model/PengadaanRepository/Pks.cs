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
        [ForeignKey("Pengadaan")]
        public Nullable<Guid> PengadaanId { get; set; }
        [ForeignKey("Vendor")]
        public Nullable<int> VendorId { get; set; }
        [ForeignKey("DokumenPengadaan")]
        public Nullable<Guid> DokumenPengadaanId { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<int> WorkflowId { get; set; }
        public DokumenPengadaan DokumenPengadaan { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
        public virtual Vendor Vendor { get; set; }
    }

    [Table("DokumenPks", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class DokumenPengadaan
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
        [MaxLength(25)]
        public string NoDokumen { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<TipeBerkas> Tipe { get; set; }
        public Nullable<long> SizeFile { get; set; }
        public virtual Pks Pks { get; set; }
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
        public Nullable<Guid> PengadaanId { get; set; }
        public string Judul { get; set; }
        public string Keterangan { get; set; }
        public string AturanPengadaan { get; set; }
        public string AturanBerkas { get; set; }
        public string JenisPekerjaan { get; set; }
        public string NoPengadaan { get; set; }
        public decimal? HPS { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string Vendor { get; set; }
        public Nullable<Guid> DokumenPengadaanId { get; set; }
        public string NoSpk { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
        public Nullable<int> WorkflowId { get; set; }
    }
}
