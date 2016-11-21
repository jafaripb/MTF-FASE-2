using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.JimbisModel
{
    [Table("RegVendor", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegVendor
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(12)]
        public string NoPengajuan { get; set; }

        public ETipeVendor TipeVendor{ get; set;}

        [MaxLength(255)]
        public string Nama { get; set; }

        [MaxLength(1000)]
        public string Alamat { get; set; }

        [MaxLength(100)]
        public string Provinsi { get; set; }

        [MaxLength(100)]
        public string Kota { get; set; }

        [MaxLength(6)]
        public string KodePos { get; set; }

        [MaxLength(100)]
        public string Website { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Telepon { get; set; }

        public EStatusVendor StatusAkhir { get; set; }

        public virtual ICollection<RegBankInfo> RegBankInfo { get; set; }
        public virtual ICollection<RegVendorPerson> RegVendorPerson { get; set; }
        public virtual ICollection<RegRiwayatPengajuanVendor> RegRiwayatPengajuanVendor { get; set; }
        public virtual ICollection<RegDokumen> RegDokumen { get; set; }
    }

    [Table("RegBankInfo", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegBankInfo {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string NamaBank { get; set; }

        [MaxLength(100)]
        public string Cabang { get; set; }

        [MaxLength(50)]
        public string NomorRekening { get; set; }

        [MaxLength(255)]
        public string NamaRekening { get; set; }

        public Nullable<bool> Active { get; set; }
    }

    [Table("RegVendorPerson", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegVendorPerson
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nama { get; set; }

        [MaxLength(100)]
        public string Jabatan { get; set; }

        [MaxLength(20)]
        public string Telepon { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        public Nullable<bool> Active { get; set; }
    }

    [Table("RegRiwayatPengajuanVendor", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class RegRiwayatPengajuanVendor
    {
        [Key]
        public int Id { get; set; }

        public Nullable<DateTime> Waktu { get; set; }

        public EStatusVendor Status { get; set; }

        public EMetodeVerifikasiVendor Metode { get; set; }

        [MaxLength(1000)]
        public string Komentar { get; set; }

        public int Urutan { get; set; }
    }

    [Table("CaptchaRegistration", Schema = JimbisContext.VENDORREG_SCHEMA_NAME)]
    public class CaptchaRegistration {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(10)]
        public string Text { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
