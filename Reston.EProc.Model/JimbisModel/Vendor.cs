using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model.PengadaanRepository;

namespace Reston.Pinata.Model.JimbisModel
{
    [Table("Vendor", Schema = JimbisContext.VENDOR_SCHEMA_NAME)]
    public class Vendor
    {
        [Key]
        public int Id { get; set; }

        public ETipeVendor TipeVendor{ get; set;}

        [MaxLength(10)]
        public string NomorVendor { get; set; }

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

        public Guid Owner { get; set; }

        public virtual ICollection<BankInfo> BankInfo { get; set; }
        public virtual ICollection<VendorPerson> VendorPerson { get; set; }
        public virtual ICollection<RiwayatPengajuanVendor> RiwayatPengajuanVendor { get; set; }
        public virtual ICollection<Dokumen> Dokumen { get; set; }
        
    }

    [Table("BankInfo", Schema = JimbisContext.VENDOR_SCHEMA_NAME)]
    public class BankInfo {
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

    [Table("VendorPerson", Schema = JimbisContext.VENDOR_SCHEMA_NAME)]
    public class VendorPerson
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

    [Table("RiwayatPengajuanVendor", Schema = JimbisContext.VENDOR_SCHEMA_NAME)]
    public class RiwayatPengajuanVendor
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
    
    public enum ETipeVendor{
        NONE = 0,
        PERORANGAN = 1, UKM = 2, BUKAN_UKM = 3
    }

    public enum EStatusVendor
    {
        NEW, PASS_1, PASS_2, PASS_3, VERIFIED, REJECTED, UPDATED, NONE, BLACKLIST
    }

    public enum EMetodeVerifikasiVendor { 
        DESK, PHONE, VISIT, NONE
    }
}
