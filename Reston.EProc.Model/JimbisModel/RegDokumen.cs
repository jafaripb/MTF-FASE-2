using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.JimbisModel
{
    
    [Table("RegDokumen", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegDokumen
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public EDocumentType TipeDokumen { get; set; }

        [MaxLength(1000)]
        public string File { get; set; }

        public string ContentType { get; set; }

        public Nullable<bool> Active { get; set; }

        //public virtual DokumenDetail DokumenDetail { get; set; }
        //public virtual IzinUsahaDokumenDetail IzinUsahaDokumenDetail { get; set; }
        //public virtual AktaDokumenDetail AktaDokumenDetail { get; set; }
        public virtual ICollection<RegVendor> RegVendor { get; set; }
    }

    [Table("RegDokumenDetail", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegDokumenDetail : RegDokumen
    {
        //[Key]
        //public int Id { get; set; }

        [MaxLength(100)]
        public string Nomor { get; set; }

        public Nullable<DateTime> MasaBerlaku { get; set; }

    }

    [Table("RegIzinUsahaDokumenDetail", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegIzinUsahaDokumenDetail : RegDokumen
    {
        //[Key]
        //public int Id { get; set; }
        
        [MaxLength(100)]
        public string Nomor { get; set; }

        public Nullable<DateTime> MasaBerlaku { get; set; }

        [MaxLength(100)]
        public string Instansi { get; set; }

        [MaxLength(100)]
        public string Klasifikasi { get; set; }

        [MaxLength(100)]
        public string Kualifikasi { get; set; }
    }

    [Table("RegAktaDokumenDetail", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegAktaDokumenDetail : RegDokumen
    {
        //[Key]
        //public int Id { get; set; }

        [MaxLength(100)]
        public string Nomor { get; set; }

        public int order { get;set;}

        public Nullable<DateTime> Tanggal { get; set; }

        [MaxLength(100)]
        public string Notaris { get; set; }
    }
    
}
