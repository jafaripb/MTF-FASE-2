using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.WebService.ViewModels
{
    public class VendorViewModel
    {
        public VendorViewModel() {
            //this.VendorPerson;
            this.NPWP = new DokumenDetailViewModel();
            this.PKP = new DokumenDetailViewModel();
            this.KTP = new DokumenDetailViewModel();
            this.NPWPPemilik = new DokumenDetailViewModel();
            this.KTPPemilik = new DokumenDetailViewModel();
            this.DOMISILI = new DokumenDetailViewModel();
            this.Akta = new AktaDokumenDetailViewModel();
            this.AktaTerakhir = new AktaDokumenDetailViewModel();
            this.TDP = new IzinUsahaDokumenDetailViewModel();
            this.SIUP = new IzinUsahaDokumenDetailViewModel();
            this.SIUJK = new IzinUsahaDokumenDetailViewModel();
            this.BankInfo = new BankInfoViewModel();//1
        }
        public int id { get;set;}
        public string gCaptchaResponse { get; set; }
        public string gCaptchaK { get; set; }
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
        public VendorPersonViewModel[] VendorPerson { get; set; }
        public BankInfoViewModel BankInfo { get; set; }
        public DokumenDetailViewModel NPWP { get; set; }
        public DokumenDetailViewModel PKP { get; set; }
        public DokumenDetailViewModel KTP { get; set; }
        public DokumenDetailViewModel DOMISILI { get; set; }
        public DokumenDetailViewModel NPWPPemilik { get; set; }
        public DokumenDetailViewModel KTPPemilik { get; set; }
        public SertifikatDokumenDetailViewModel[] Sertifikat { get; set; }
        public AktaDokumenDetailViewModel Akta { get; set; }
        public AktaDokumenDetailViewModel AktaTerakhir { get; set; }
        public IzinUsahaDokumenDetailViewModel TDP { get; set; }
        public IzinUsahaDokumenDetailViewModel SIUP { get; set; }
        public IzinUsahaDokumenDetailViewModel SIUJK { get; set; }
    }

    public class DataVendor
    {
        public Nullable<int> TotalRecord { get; set; }
        public List<VWVendor2> data { get; set; }
    }

    public class VWVendor2
    {
        public int Id { get; set; }
        public int TipeVendor { get; set; }
        public string NomorVendor { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Provinsi { get; set; }
        public string Kota { get; set; }
        public string KodePos { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telepon { get; set; }
        public string StatusAkhir { get; set; }
        public string Owner { get; set; }
    }

    public class VendorPersonViewModel
    {
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Email { get; set; }
        public string Telepon { get; set; }
    }

    public class BankInfoViewModel
    {
        public string Nama { get; set; }
        public string Cabang { get; set; }
        public string NomorRekening { get; set; }
        public string NamaRekening { get; set; }
    }

    public class DokumenDetailViewModel {
        public string id { get;set;} //guid
        public string ContentType { get; set; }
        public string Nomor { get; set; }
        public string File { get; set; }
    }

    public class AktaDokumenDetailViewModel
    {
        public string id { get; set; } //guid
        public string ContentType { get; set; }
        public string Nomor { get; set; }
        public Nullable<DateTime> Tanggal { get; set; }
        public string Notaris { get; set; }
        public string File { get; set; }
    }

    public class SertifikatDokumenDetailViewModel
    {
        public string id { get; set; } //guid
        public string ContentType { get; set; }
        public string Nomor { get; set; }
        public Nullable<DateTime> Tanggal { get; set; }
        public string Lembaga { get; set; }
        public string File { get; set; }
    }

    public class IzinUsahaDokumenDetailViewModel
    {
        public string id { get; set; } //guid
        public string ContentType { get; set; }
        public string Nomor { get; set; }
        public Nullable<DateTime> MasaBerlaku { get; set; }
        public string Instansi { get; set; }
        public string Klasifikasi { get; set; }
        public string Kualifikasi { get; set; }
        public string File { get; set; }
    }

    public class StatusVerifikasiViewModel {
        public string Metode { get; set; }

        public Nullable<DateTime> Waktu { get; set; }

        public string Comment { get; set; }
    }

    public class RiwayatPengajuanVendorViewModel{
        public string Tanggal{get;set;}
        public string Pesan{get;set;}
        public string User{get;set;}
    }

    public class CaptchaResponse
    {
        [Newtonsoft.Json.JsonProperty("success")]
        public bool Success { get; set; }

        [Newtonsoft.Json.JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }

    public class CaptchaRegistrationViewModel {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime ExpiredDate { get; set; }
        public System.Drawing.Bitmap Image { get; set; }
    }
}
