using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model;
using Reston.Pinata.Model.PengadaanRepository;

namespace Reston.Eproc.Model.Monitoring.Entities
{
    [Table("COA", Schema = JimbisContext.PROYEK_SCHEMA_NAME)]
    public class COA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //------------------------------------------------------
        [ForeignKey("Pengadaan")]
        public Guid PengadaanId { get; set; }
        public string NoCoa { get; set; }
        public string Region { get; set; }
        public string Divisi { get; set; }
        public string Periode { get; set; }
        public string GroupAset { get; set; }
        public string JenisAset { get; set; }
        public string NilaiAset { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        public Guid ModifiedUploadBy { get; set; }
        public DateTime ModifiedUploadOn { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    public class DataTableBudget
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VVWCOA> data { get; set; }
    }

    public class VVWCOA
    {
        public Guid Id { get; set; }
        public Guid PengadaanId { get; set; }
        public string NoCoa { get; set; }
        public string Region { get; set; }
        public string Divisi { get; set; }
        public string Periode { get; set; }
        public string GroupAset { get; set; }
        public string JenisAset { get; set; }
        public string NilaiAset { get; set; }
        public string HargaVendor { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        public Guid ModifiedUploadBy { get; set; }
        public DateTime ModifiedUploadOn { get; set; }
    }
}
