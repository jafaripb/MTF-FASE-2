using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public class ProdukRepo : IProdukRepo
    {
        public const string KATEGORI_TAMPLATE_STRING = "TEMPLATE";

        JimbisContext ctx;
        public ProdukRepo(JimbisContext j) {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public Produk GetProduk(int id){
            return ctx.Produks.Find(id);
        }

        public List<RiwayatHarga> GetHargaProdukRegion(int id, string region)
        {
            Produk p = ctx.Produks.Find(id);

            return p.RiwayatHarga.Where(x=>(region==null || x.Region == region)).ToList();
        }

        public List<Produk> GetAllProduk()
        {
            return ctx.Produks.ToList();
        }

        public List<KategoriSpesifikasi> GetAllKategori() {
            return ctx.KategoriSpesifikasis.Where(x => x.Nama != null).Distinct().ToList();
        }

        public List<Produk> GetProduks(string name = null, string region = null, string kategori = null,string klasifikasi=null) {
            //return ctx.Produks.Where(x => (name==null || x.Nama == name)).ToList();
            var Klasifikasi =(KlasifikasiPengadaan) Convert.ToInt32(klasifikasi);
            return (from a in ctx.Produks
                    where (name == null || a.Nama.ToLower().Contains(name.ToLower())) 
                        //&& (region== "" || b.Region == region)
                        && (kategori== null|| a.KategoriSpesifikasi.Nama == kategori) &&  (klasifikasi == null || a.Klasifikasi == Klasifikasi)
                    select a
                         ).ToList();

        }

        public void SaveProduk(Produk p) {           
            ctx.Produks.Add(p);
            ctx.SaveChanges();
        }

        public void DeleteProduk(int id) {
            Produk p = ctx.Produks.Find(id);
            if (p != null) {
                ctx.RiwayatHargas.RemoveRange(p.RiwayatHarga);
                ctx.AtributSpesifikasis.RemoveRange(p.KategoriSpesifikasi.AtributSpesifikasi);
                ctx.KategoriSpesifikasis.Remove(p.KategoriSpesifikasi);
                ctx.Produks.Remove(p);
                ctx.SaveChanges();
            }
        }

        public void Save() {
            ctx.SaveChanges();
        }

        public void SaveKategori(KategoriSpesifikasi k) {
            ctx.KategoriSpesifikasis.Add(k);
            ctx.SaveChanges();
        }

        public void AddRiwayatHarga(RiwayatHarga r) {
            ctx.RiwayatHargas.Add(r);
            ctx.SaveChanges();
        }

        public List<RiwayatHarga> GetRiwayatHarga(int id, string region) {
            return ctx.Produks.Find(id).RiwayatHarga.Where(x=>region==null || x.Region == region).ToList();
        }

        public KategoriSpesifikasi GetKategoriSpesifikasiByProduk(int id)
        {
            return ctx.Produks.Find(id).KategoriSpesifikasi;
        }

        public KategoriSpesifikasi GetKategoriSpesifikasi(int id) {
            return ctx.KategoriSpesifikasis.Find(id);
        }

        public List<KategoriSpesifikasi> GetTemplateKategoriSpesifikasi() {
            return ctx.KategoriSpesifikasis.Where(x=>x.Deskripsi.Equals(KATEGORI_TAMPLATE_STRING)).ToList();
        }

        public List<AtributSpesifikasi> GetDaftarAtributSpesifikasi(int id) {
            return ctx.KategoriSpesifikasis.Find(id).AtributSpesifikasi.ToList();
        }
    }
}
