﻿using System;
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
    [Table("PO", Schema = JimbisContext.PO_SCHEMA_NAME)]
    public class PO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Prihal { get; set; }
        public string Vendor { get; set; }
        public string UP { get; set; }
        public string NoPO { get; set; }
        public DateTime? TanggalPO { get; set; }
        public decimal? NilaiPO { get; set; }
        public string Keterangan { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual ICollection<DokumenPO> DokumenPO { get; set; }
        public virtual ICollection<PODetail> PODetail { get; set; }
    }

    [Table("PODetail", Schema = JimbisContext.PO_SCHEMA_NAME)]
    public class PODetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("PO")]
        public Guid POId { get; set; }
        public string NamaBarang { get; set; }
        public string Kode { get; set; }
        public decimal? Banyak { get; set; }
        public string Satuan { get; set; }
        public decimal? Harga { get; set; }
        public decimal? Deskripsi { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual PO PO { get; set; }
        public virtual ICollection<PODetail> PODetails { get; set; }
    }

    public class DataTablePO
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWPO> data { get; set; }
    }

    public class VWPO
    {
        public Guid Id { get; set; }
        public string Prihal { get; set; }
        public string Vendor { get; set; }
        public string UP { get; set; }
        public string NoPO { get; set; } 
        public DateTime? TanggalPO { get; set; }
        public string TanggalPOstr { get; set; }
        public decimal? NilaiPO { get; set; }
        public string Keterangan { get; set; }
        public string Created { get; set; }
        public Guid? CreatedId { get; set; }
    }

    public class DataTablePODetail
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWPODetail> data { get; set; }
    }

    public class VWPODetail
    {
        public Guid Id { get; set; }
        public Guid POId { get; set; }
        public string NamaBarang { get; set; }
        public string Kode { get; set; }
        public decimal? Banyak { get; set; }
        public string Satuan { get; set; }
        public decimal? Harga { get; set; }
        public decimal? Jumlah { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PPH { get; set; }
        public decimal? DPP { get; set; }
        public decimal? PPN { get; set; }
        public decimal? Deskripsi { get; set; }
    }


    [Table("DokumenPO", Schema = JimbisContext.PO_SCHEMA_NAME)]
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


    public class VWDokumenPO
    {
        public Guid Id { get; set; }
        public Nullable<Guid> POId { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public Nullable<long> SizeFile { get; set; }
    }

}
