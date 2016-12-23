using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;

namespace Reston.Eproc.Model.Monitoring.Entities
{
    [Table("PO", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class PO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Judul { get; set; }
        [ForeignKey("Vendor")]
        public int VendorId { get; set; }
        public string NoPO { get; set; } //generate
        public string TanggalPO { get; set; }
        public decimal NilaiPO { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<DokumenPO> DokumenPO { get; set; }
    }

    [Table("DokumenPO", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class DokumenPO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("PO")]
        public Nullable<Guid> POId { get; set; }
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
        public virtual PO PO { get; set; }
    }
}
