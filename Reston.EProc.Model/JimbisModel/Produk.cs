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
    [Table("Produk", Schema = JimbisContext.CATALOG_SCHEMA_NAME)]
    public class Produk
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        [Index]
        public string Nama { get; set; }

        [MaxLength(255)]
        public string Deskripsi { get; set; }

        [MaxLength(20)]
        public string Satuan { get; set; }

        public KlasifikasiPengadaan Klasifikasi { get; set; }

        public virtual KategoriSpesifikasi KategoriSpesifikasi { get; set; }
        public virtual ICollection<RiwayatHarga> RiwayatHarga { get; set; }
    }

    [Table("RiwayatHarga", Schema = JimbisContext.CATALOG_SCHEMA_NAME)]
    public class RiwayatHarga
    {
        [Key]
        public int Id { get; set; }

        public DateTime Tanggal { get; set; }

        public decimal Harga { get; set; }

        [MaxLength(4)]
        public string Currency { get; set; }

        public string Region { get; set; }

        [MaxLength(500)]
        public string Sumber { get; set; }

        [MaxLength(50)]
        public string User { get; set; }
    }

    [Table("KategoriSpesifikasi", Schema = JimbisContext.CATALOG_SCHEMA_NAME)]
    public class KategoriSpesifikasi
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public string Nama { get; set; }

        [MaxLength(255)]
        public string Deskripsi { get; set; }

        public KategoriSpesifikasi ParentKategori { get; set; }
        public virtual ICollection<AtributSpesifikasi> AtributSpesifikasi { get; set; }
    }

    [Table("AtributSpesifikasi", Schema = JimbisContext.CATALOG_SCHEMA_NAME)]
    public class AtributSpesifikasi
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nama { get; set; }

        [MaxLength(255)]
        public string Nilai { get; set; }

        public string Grup { get; set; }
    }


    
}
