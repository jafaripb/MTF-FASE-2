using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.PengadaanRepository.View
{
    public class VWRKS
    {
        public string Judul { get; set; }
        public Nullable<Guid> pengadaanId { get; set; }
        public List<VWRKSDetail> VWRKSDetails { get; set; }
    }

    public class VWRKSDetail
    {
        public Nullable<Guid> Id { get; set; }
        public Nullable<Guid> RKSHeaderId { get; set; }
        public Nullable<Guid> ItemId { get; set; }
        public string judul { get; set; }
        public Nullable<int> level { get; set; }
        public int? grup { get; set; }
        public string item { get; set; }
        public string satuan { get; set; }
        public Nullable<decimal> jumlah { get; set; }
        public Nullable<decimal> hps { get; set; }
        public Nullable<decimal> total { get; set; }
        public string keterangan { get; set; }
    }

    public class VWRKSDetailRekanan
    {
        public Nullable<Guid> Id { get; set; }
        public Nullable<Guid> RKSHeaderId { get; set; }
        public Nullable<Guid> ItemId { get; set; }
        public Nullable<Guid> HargaRekananId { get; set; }
        public string item { get; set; }
        public string judul { get; set; }
        public string keteranganItem { get; set; }
        public string satuan { get; set; }
        public Nullable<int> level { get; set; }
        public Nullable<int> grup { get; set; }
        public Nullable<decimal> harga { get; set; }
        public string hargaEncript { get; set; }
        public string keterangan { get; set; }
        public Nullable<decimal> jumlah { get; set; }
    }

    public class VWRekananSubmitHarga
    {
        public Nullable<int> VendorId { get; set; }
        public string NamaVendor { get; set; }
        public Nullable<int> status { get; set; }
    }

    public class VWRekananPenilaian
    {
        public Nullable<int> VendorId { get; set; }
        public string NamaVendor { get; set; }
        public Nullable<int> NilaiKriteria { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<int> terpilih { get; set; }
    }

    public class VWVendorsHarga
    {
        public Nullable<int> VendorId { get; set; }
        public string nama { get; set; }
        public string Keterangan { get; set; }
        public Nullable<int> NIlaiKriteria { get; set; }
        public List<item> items { get; set; }

    }

    public class VWRKSVendors
    {
        public List<VWRKSPenilaian> hps { get; set; }
        public List<VWVendorsHarga> vendors{get;set;}
    }

    public class VWRKSPenilaian
    {
        public Nullable<Guid> Id { get; set; }
        public string item { get; set; }
        public string keteranganItem { get; set; }
        public string satuan { get; set; }
        public string judul { get; set; }
        public int? level { get; set; }
        public int? grup { get; set; }
        public Nullable<decimal> harga { get; set; }
        public Nullable<decimal> jumlah { get; set; }
    }

    public class item
    {
        public Nullable<Guid> Id { get; set; }
        public Nullable<decimal> jumlah { get; set; }
        public Nullable<decimal> harga { get; set; }
        public string Keterangan { get; set; }
    }

    public class VWPembobotanPengadaan
    {
        public Guid Id { get; set; }
        public string NamaKreteria { get; set; }
        public Nullable<int> Bobot { get; set; }
        public Nullable<int> Nilai { get; set; }
    }

    public class VWPembobotanPengadaanVendor
    {
        public Nullable<Guid> Id { get; set; }
        public string NamaKreteria { get; set; }
        public Nullable<Guid> KreteriaPembobotanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<int> Bobot { get; set; }
        public Nullable<int> Nilai { get; set; }
    }

    public class VWRKSTemplate
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public String  Deskripsi { get; set; }
        public KlasifikasiPengadaan Klasifikasi { get; set; }
        public String Region { get; set; }
        public List<VWRKSDetailTemplate> VWRKSDetails { get; set; }
    }

    public class DataTable
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWRKSDetail> data { get; set; }
    }

    public class DataTableRksTemplate
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWRKSTemplate> data { get; set; }
    }

    public class DataTableRksDetailTemplate
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<VWRKSDetailTemplate> data { get; set; }
    }

    public class VWRKSDetailTemplate
    {
        public Guid Id { get; set; }
        public Nullable<Guid> RKSHeaderTemplateId { get; set; }
        public Nullable<Guid> ItemId { get; set; }
        public string item { get; set; }
        public string judul { get; set; }
        public Nullable<int> level{ get; set; }
        public Nullable<int> group { get; set; }
        public string satuan { get; set; }
        public Nullable<decimal> jumlah { get; set; }
        public Nullable<decimal> hps { get; set; }
        public Nullable<decimal> total { get; set; }
        public string keterangan { get; set; }
        public Nullable<DateTime> CreateOn { get; set; }
        public Nullable<Guid> CreateBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }
    }
}
