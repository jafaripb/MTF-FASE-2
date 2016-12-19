using Reston.Eproc.Model.Monitoring.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
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

    public class PenilaianVendorHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //------------------------------------------------------
        [ForeignKey("RencanaProyek")]   
        public Guid ProyekId { get; set; }
        //------------------------------------------------------
        [ForeignKey("Vendor")]
        public Nullable<int> VendorId { get; set; }
        [MaxLength(256)]
        public string Catatan { get; set; }
        public int Total_nilai { get; set; }
        public int Jumlah_penilaian { get; set; }
        public virtual RencanaProyek RencanaProyek { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<PenilaianVendorDetail> PenilaianVendorDetails { get; set; }
    }

    public class PenilaianVendorDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //------------------------------------------------------
        [ForeignKey("PenilaianVendorHeader")]
        public Guid PenilaianVendorHeaderId { get; set; }
        //------------------------------------------------------
        [ForeignKey("ReferenceData")]
        public Nullable<int> ReferenceDataId { get; set; }
        //------------------------------------------------------
        public int Nilai { get; set; }
        [MaxLength(256)]
        public string Catatan_item { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public virtual PenilaianVendorHeader PenilaianVendorHeader { get; set; }
        public virtual ReferenceData ReferenceData { get; set; }
    }
}
