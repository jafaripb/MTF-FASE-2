using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public interface IProdukRepo
    {
        Produk GetProduk(int id);
        List<RiwayatHarga> GetHargaProdukRegion(int id, string region);
        List<Produk> GetAllProduk();
        List<KategoriSpesifikasi> GetAllKategori();
        List<Produk> GetProduks(string name = null, string region = null, string kategori = null,string klasifikasi=null);

        void SaveProduk(Produk p);

        void DeleteProduk(int id);
        void SaveKategori(KategoriSpesifikasi k);

        void AddRiwayatHarga(RiwayatHarga r);

        void Save();

        List<RiwayatHarga> GetRiwayatHarga(int id, string region);

        KategoriSpesifikasi GetKategoriSpesifikasi(int id);

        KategoriSpesifikasi GetKategoriSpesifikasiByProduk(int id);

        List<KategoriSpesifikasi> GetTemplateKategoriSpesifikasi();

        List<AtributSpesifikasi> GetDaftarAtributSpesifikasi(int id);
    }
}
