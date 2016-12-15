using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model.JimbisModel;

namespace Reston.Pinata.Model.PengadaanRepository.View
{
    public class VWPengadaan
    {
        public Pengadaan Pengadaan { get; set; }
        public List<Jadwal> Jadwal { get; set; }
    }
    public class Userx
    {
        public string PersonilId { get; set; }
        public string Nama { get; set; }
        public string jabatan { get; set; }
        public string tlp { get; set; }
    }

    public class DataPageUsers
    {
        public int? totalRecord { get; set; }
        public List<Userx> Users { get; set; }
    }

    public class Jadwal
    {
        public string Mulai { get; set; }
        public string Sampai { get; set; }
        public string tipe { get; set; }
    }

    public class StateJadwalPengadaan
    {
        public Nullable<Guid> PengadaanId { get; set; }
        public string tipe { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
        public EStatusPengadaan status { get; set; }
    }

    public class ViewPengadaan
    {
        public Guid Id { get; set; }
        public Guid? IdPersetujuanPemanang { get; set; }
        public string Judul { get; set; }
        public string Keterangan { get; set; }
        public string AturanPengadaan { get; set; }
        public string AturanBerkas { get; set; }
        public string AturanPenawaran { get; set; }
        public Nullable<int> StatusBintang { get; set; }
        public string MataUang { get; set; }

        public decimal? Pagu { get; set; }
        public string PeriodeAnggaran { get; set; }
        public string JenisPembelanjaan { get; set; }
        public Nullable<Guid> HpsId { get; set; }
        public string UnitKerjaPemohon { get; set; }
        public string Region { get; set; }
        public string Provinsi { get; set; }
        public string KualifikasiRekan { get; set; }
        public string JenisPekerjaan { get; set; }
        public string TitleDokumenNotaInternal { get; set; }
        public string TitleDokumenLain { get; set; }
        public string TitleBerkasRujukanLain { get; set; }
        public int nextStatus { get; set; }
        public string NoPengadaan { get; set; }
        public Nullable<int> Approver { get; set; }
        public Nullable<int> ApproverPersetujuanPemenang { get; set; }
        public Nullable<int> isCreated { get; set; }
        public Nullable<int> isPIC { get; set; }
        public Nullable<int> isTEAM { get; set; }
        public Nullable<int> isPersonil { get; set; }
        public Nullable<int> isController { get; set; }
        public Nullable<int> isCompliance { get; set; }
        public Nullable<int> isUser { get; set; }

        public Nullable<EStatusPengadaan> Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<StatusPengajuanPemenang> StatusPersetujuanPemenang { get; set; }
        public string StatusPersetujuanPemenangName { get; set; }        
        public Nullable<EGroupPengadaan> GroupPengadaan { get; set; }
        public Nullable<Guid> IdBerkasRujukanLain { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<int> isMasukKlarifikasi { get; set; }
        public List<RKSHeader> RKSHeaders { get; set; }
        public Nullable<int> WorkflowTemplateId { get; set; }
        public Nullable<int> WorkflowPersetujuanPemenangTemplateId { get; set; }
        public Nullable<decimal> HPS { get; set; }
        public string lastApprover { get; set; }
        public string lastApproverPersetujuanPemenang { get; set; }        
        public List<VWDokumenPengadaan> DokumenPengadaans { get; set; }
        public List<VWKandidatPengadaan> KandidatPengadaans { get; set; }
        public List<VWJadwalPengadaan> JadwalPengadaans { get; set; }
        public List<VWPersonilPengadaan> PersonilPengadaans { get; set; }
        public List<VWKualifikasiKandidat> KualifikasiKandidats { get; set; }
        public List<VWJadwalPelaksanaan2> JadwalPelaksanaans { get; set; }
    }

    public class DataPagePengadaan
    {
        public Nullable<int> TotalRecord { get; set; }
        public List<ViewPengadaan> data { get; set; }
    }

    public class VWVendor
    {
        public string Nama { get; set; }
        public string email { get; set; }
    }

    public class vwProduk
    {
        public Nullable<int> Id { get; set; }
        public string Nama { get; set; }
        public string Spesifikasi { get; set; }
        public string Deskripsi { get; set; }
        public string Satuan { get; set; }
        public List<RiwayatHarga> RiwayatHarga { get; set; }
    }

    public class VWProdukSummary
    {
        public Nullable<int> Id { get; set; }
        public string Nama { get; set; }
        public string Region { get; set; }
        public decimal Price { get; set; }
        public string LastUpdate { get; set; }
        public string Source { get; set; }
        public string Satuan { get; set; }
        public string Spesifikasi { get; set; }
    }

    public class VWDokumenPengadaan
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string File { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public Nullable<TipeBerkas> Tipe { get; set; }
        public Nullable<long> SizeFile { get; set; }
    }

    public class VWKandidatPengadaan
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string Nama { get; set; }
        public string Telepon { get; set; }
    }

    public class VWJadwalPengadaan
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string tipe { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
    }
    public class VWJadwalPelaksanaan2
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public EStatusPengadaan? statusPengadaan { get; set; }
        public Nullable<DateTime> Mulai { get; set; }
        public Nullable<DateTime> Sampai { get; set; }
    }

    public class VWJadwalPelaksanaan
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string tipe { get; set; }
        public string Mulai { get; set; }
        public string Sampai { get; set; }
    }

    public class VWPersonilPengadaan
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<Guid> PersonilId { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string tipe { get; set; }
        public int? isReady{ get; set; }
        public int? isMine { get; set; }
    }

    public class VWRKSHeaderPengadaan
    {
        public string Judul { get; set; }
        public string Keterangan { get; set; }
        public Nullable<EStatusPengadaan> Status { get; set; }
        public Nullable<Guid> RKSHeaderId { get; set; }
    }

    public class VWPelaksanaanAanwijzing
    {
        public Nullable<Guid> Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string Mulai { get; set; }
        public string IsiUndangan { get; set; }
    }

    public class VWKehadiranKandidatAanwijzing
    {
        public Nullable<Guid> Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<int> VendorId { get; set; }
        public string NamaVendor { get; set; }
        public string Telp { get; set; }
        public string Kontak { get; set; }
        public int hadir { get; set; }
    }

    public class VWKualifikasiKandidat
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string kualifikasi { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }
    public class VWPErsetujuanBukaAmplop
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string tipe { get; set; }
        public virtual Pengadaan Pengadaan { get; set; }
    }

    public class dataVendors
    {
        public List<ViewVendors> Vendors { get; set; }
        public int totalRecord { get; set; }
    }

    public class ViewVendors
    {
        public int id { get; set; }
        public string gCaptchaResponse { get; set; }
        public string NoPengajuan { get; set; }
        public int TipeVendor { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Provinsi { get; set; }
        public string Kota { get; set; }
        public string KodePos { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telepon { get; set; }
        public string StatusAkhir { get; set; }
        public Guid Owner { get; set; }
    }

    public class ViewSendEmail
    {
        public Nullable<Guid> PengadaanId { get; set; }
        public string Surat { get; set; }
    }

    public class VWReportVendor
    {
        public string Nama { get; set; }
    }

    public class VWBeritaAcara
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public string tanggal { get; set; }
        public Nullable<TipeBerkas> Tipe { get; set; }
        public string NoBeritaAcara { get; set; }
    }
    public class VWReportPengadaan
    {
        public Nullable<Guid> PengadaanId { get; set; }
        public string Judul { get; set; }
        public string User { get; set; }
        public Nullable<decimal> hps { get; set; }
        public Nullable<decimal> realitas { get; set; }
        public Nullable<decimal> efisiensi { get; set; }
        public string Pemenang { get; set; }
        public Nullable<DateTime> Aanwjzing { get; set; }
        public Nullable<DateTime> PembukaanAmplop { get; set; }
        public Nullable<DateTime> Klasrifikasi { get; set; }
        public Nullable<DateTime> Scoring { get; set; }
        public Nullable<DateTime> NotaPemenang { get; set; }
        public Nullable<DateTime> SPK { get; set; }
    }

    public class VWPembatalanPengadaan
    {
        public Nullable<Guid> PengadaanId { get; set; }
        public string Keterangan { get; set; }
    }


    public class VWProgressReport
    {
        public string Judul { get; set; }
        public int Progress { get; set; }
    }

    public class VWStaffCharges
    {
        public string Nama { get; set; }
        public int Jumlah { get; set; }
    }

    public class VWPenolakan
    {
        public Guid PenolakanId { get; set; }
        public string AlasanPenolakan { get; set; }
    }

    public class VWRiwayatDokumen
    {
        public Guid Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string Nama { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }
        public String Comment { get; set; }
        public string Status { get; set; }
        public string JudulPengadaan { get; set; }
    }
    public class VWRiwayatPengadaan
    {
        public Guid Id { get; set; }
        public Nullable<Guid> PengadaanId { get; set; }
        public Nullable<DateTime> Waktu { get; set; }
        public string Status { get; set; }
        public string Komentar { get; set; }
        public int Urutan { get; set; }
        public string JudulPengadaan { get; set; }
    }

    public class DataTablePengadaan
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewPengadaan> data { get; set; }
    }

    public class DataTableUsers
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<Userx> data { get; set; }
    }

    public class VWCountListDokumen
    {
        public int PengadaanButuhPerSetujuan { get; set; }
        public int PengadaanDiSetujui { get; set; }
        public int PengadaanDiTolak { get; set; }
        public int PemenangButuhPerSetujuan { get; set; }
        public int PemenangDiSetujui { get; set; }
        public int PemenangDiTolak { get; set; }
        public int MonitorSelection { get; set; }
    }
}
