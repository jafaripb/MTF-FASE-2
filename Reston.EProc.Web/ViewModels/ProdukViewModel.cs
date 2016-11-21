using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model.PengadaanRepository;

namespace Reston.Pinata.WebService.ViewModels
{
    public class ProdukViewModel
    {
        public int id { get;set;}
        public string Nama { get; set; }

        public string Deskripsi { get; set; }

        public string Satuan { get; set; }

        public decimal HargaTerakhir { get; set; }

        public string Currency { get; set; }
        public KlasifikasiPengadaan Klasifikasi { get; set; }

        public string DefaultRegion { get; set; }
        public KategoriSpesifikasiViewModel Spesifikasi { get; set; }
    }

    public class ProdukSummaryViewModel {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Region { get; set; }
        public decimal Price { get; set; }
        public string LastUpdate { get; set; }
        public string Source { get; set; }
        public string Satuan { get; set; }
        public string Klasifikasi { get; set; }
    }

   

    public class KategoriSpesifikasiViewModel {
        public int id { get; set; }
        public string NamaKategori { get; set; }

        public string DeskripsiKategori { get; set; }

        public List<AtributSpesifikasiViewModel> DaftarAtribut { get; set; }

    }

    public class RiwayatHargaTabelViewModel
    {
        public List<RiwayatHargaViewModel> aaData { get; set; }

    }

    public class AtributSpesifikasiViewModel
    {
        public int id { get; set; }
        public string NamaAtribut { get; set; }

        public string Nilai { get; set; }

        public string Grup { get; set; }

    }

    public class RiwayatHargaViewModel
    {
        public int id { get; set; }
        public int IdProduk { get; set; }
        public string Tanggal { get; set; }

        public decimal Harga { get; set; }

        public string Currency { get; set; }

        public string Region { get; set; }

        public string Sumber { get; set; }

        public string User { get; set; }

    }
}
