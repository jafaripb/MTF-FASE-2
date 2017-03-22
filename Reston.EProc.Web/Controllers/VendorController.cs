using Microsoft.Owin.FileSystems;
using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.Repository;
using Reston.Pinata.WebService.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net;
using Reston.Pinata.WebService.Helper;
using Model.Helper;

namespace Reston.Pinata.WebService
{
    public class VendorController : BaseController
    {
        private IVendorRepo _repository;
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"];
        private string FILE_VENDOR_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_VENDOR"];
        public VendorController()
        {
            _repository = new VendorRepo(new JimbisContext());
        }

        public VendorController(VendorRepo repository)
        {
            _repository = repository;
        }

        public IEnumerable<string> Get()
        {
            return new[] { "Vendor is so", "more of it", "more" };
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_superadmin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_end_user, IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance })]
        public Vendor GetVendor(int id)
        {
            return _repository.GetVendor(id);
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_superadmin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_end_user, IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance })]
        public List<VendorViewModel> GetVendors(string tipe, string status, int limit, string search)
        {
            var lv =
            _repository.GetVendors(tipe != null ? (ETipeVendor)Enum.Parse(typeof(ETipeVendor), tipe) : ETipeVendor.NONE,
                status != null ? (EStatusVendor)Enum.Parse(typeof(EStatusVendor), status) : EStatusVendor.NONE
                , limit,search);
            if (lv != null)
                return lv.Where(x => search == null || x.Nama.ToLower().Contains(search.ToLower())).Select(x => new VendorViewModel() { id = x.Id, Nama = x.Nama, Alamat = x.Alamat, Provinsi = x.Provinsi, Telepon = x.Telepon, Email = x.Email, Website = x.Website, NoPengajuan = x.NomorVendor }).ToList();
            return new List<VendorViewModel>();
        }

        private string GenerateNomorVendor(int tipe)
        {
            string no = "";
            DateTime d = DateTime.Now;
            no = ("" + d.Year).Substring(2, 2) + (d.Month < 10 ? "0" + d.Month : "" + d.Month) + "1" + tipe;//1 means by staff
            string ctrn = "000" + _repository.CheckNomor(no);
            no += ctrn.Substring(ctrn.Length - 4, 4);
            return no;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpPost]
        public async Task<int> AddVendor([FromBody]VendorViewModel model)
        {
            //access?
            Vendor v = new Vendor
            {
                TipeVendor = (ETipeVendor)model.TipeVendor,
                NomorVendor = GenerateNomorVendor((int)model.TipeVendor),
                Nama = model.Nama,
                Alamat = model.Alamat,
                Provinsi = model.Provinsi,
                Kota = model.Kota,
                KodePos = model.KodePos,
                Email = model.Email,
                Website = model.Website,
                Telepon = model.Telepon,
                StatusAkhir = EStatusVendor.VERIFIED
            };

            if (model.VendorPerson != null)
            {
                List<VendorPerson> lvp = new List<VendorPerson>();
                foreach (VendorPersonViewModel vpvm in model.VendorPerson)
                {
                    if (vpvm.Nama != null && vpvm.Nama != "") //persons without name are ignored
                        lvp.Add(new VendorPerson
                        {
                            Nama = vpvm.Nama,
                            Jabatan = vpvm.Jabatan,
                            Email = vpvm.Email,
                            Telepon = vpvm.Telepon,
                            Active = true
                        });
                }
                v.VendorPerson = lvp;
            }
            if (model.BankInfo != null)
            {
                BankInfo bi = new BankInfo
                {
                    NamaBank = model.BankInfo.Nama,
                    Cabang = model.BankInfo.Cabang,
                    NomorRekening = model.BankInfo.NomorRekening,
                    NamaRekening = model.BankInfo.NamaRekening,
                    Active = true
                };

                if (bi.NamaBank != null && bi.NamaBank != "") //banks without name are ignored
                    v.BankInfo = new List<BankInfo>() { bi };
            }

            RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor() { Komentar = "Rekanan baru TERVERIFIKASI, oleh " + CurrentUser.UserName, Status = EStatusVendor.NEW, Urutan = 0, Metode = EMetodeVerifikasiVendor.NONE, Waktu = DateTime.Now };
            v.RiwayatPengajuanVendor = new List<RiwayatPengajuanVendor>() { rp };

            DokumenDetail NPWP = null;
            DokumenDetail KTP = null;
            DokumenDetail PKP = null;
            DokumenDetail NPWPPemilik = null;
            DokumenDetail KTPPemilik = null;
            DokumenDetail DOMISILI = null;
            AktaDokumenDetail Akta = null;
            AktaDokumenDetail AktaTerakhir = null;
            IzinUsahaDokumenDetail TDP = null;
            IzinUsahaDokumenDetail SIUP = null;
            IzinUsahaDokumenDetail SIUJK = null;

            _repository.AddVendor(v);

            if (model.NPWP.File != null)
                NPWP = new DokumenDetail { File = CopyFileVendor(model.NPWP.File, v.Id), ContentType = model.NPWP.ContentType, Nomor = model.NPWP.Nomor, TipeDokumen = EDocumentType.NPWP, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.KTP.File != null)
                KTP = new DokumenDetail { File = CopyFileVendor(model.KTP.File, v.Id), ContentType = model.KTP.ContentType, Nomor = model.KTP.Nomor, TipeDokumen = EDocumentType.KTP, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.PKP.File != null)
                PKP = new DokumenDetail { File = CopyFileVendor(model.PKP.File, v.Id), ContentType = model.PKP.ContentType, Nomor = model.PKP.Nomor, TipeDokumen = EDocumentType.PKP, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.NPWPPemilik.File != null)
                NPWPPemilik = new DokumenDetail { File = CopyFileVendor(model.NPWPPemilik.File, v.Id), ContentType = model.NPWPPemilik.ContentType, Nomor = model.NPWPPemilik.Nomor, TipeDokumen = EDocumentType.NPWPPemilik, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.KTPPemilik.File != null)
                KTPPemilik = new DokumenDetail { File = CopyFileVendor(model.KTPPemilik.File, v.Id), ContentType = model.KTPPemilik.ContentType, Nomor = model.KTPPemilik.Nomor, TipeDokumen = EDocumentType.KTPPemilik, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.DOMISILI.File != null)
                DOMISILI = new DokumenDetail { File = CopyFileVendor(model.DOMISILI.File, v.Id), ContentType = model.DOMISILI.ContentType, Nomor = model.DOMISILI.Nomor, TipeDokumen = EDocumentType.DOMISILI, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.Akta.File != null)
                Akta = new AktaDokumenDetail { File = CopyFileVendor(model.Akta.File, v.Id), ContentType = model.Akta.ContentType, Nomor = model.Akta.Nomor, TipeDokumen = EDocumentType.AKTA, Notaris = model.Akta.Notaris, Tanggal = model.Akta.Tanggal, Vendor = new List<Vendor>() { v }, order = 1, Active = true };
            if (model.AktaTerakhir.File != null)
                AktaTerakhir = new AktaDokumenDetail { File = CopyFileVendor(model.AktaTerakhir.File, v.Id), ContentType = model.AktaTerakhir.ContentType, Nomor = model.AktaTerakhir.Nomor, TipeDokumen = EDocumentType.AKTA, Notaris = model.AktaTerakhir.Notaris, Tanggal = model.AktaTerakhir.Tanggal, Vendor = new List<Vendor>() { v }, order = 2, Active = true };
            if (model.TDP.File != null)
                TDP = new IzinUsahaDokumenDetail { File = CopyFileVendor(model.TDP.File, v.Id), ContentType = model.TDP.ContentType, Nomor = model.TDP.Nomor, TipeDokumen = EDocumentType.TDP, Instansi = model.TDP.Instansi, Klasifikasi = model.TDP.Klasifikasi, Kualifikasi = model.TDP.Kualifikasi, MasaBerlaku = model.TDP.MasaBerlaku, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.SIUP.File != null)
                SIUP = new IzinUsahaDokumenDetail { File = CopyFileVendor(model.SIUP.File, v.Id), ContentType = model.SIUP.ContentType, Nomor = model.SIUP.Nomor, TipeDokumen = EDocumentType.SIUP, Instansi = model.SIUP.Instansi, Klasifikasi = model.SIUP.Klasifikasi, Kualifikasi = model.SIUP.Kualifikasi, MasaBerlaku = model.SIUP.MasaBerlaku, Vendor = new List<Vendor>() { v }, Active = true };
            if (model.SIUJK.File != null)
                SIUJK = new IzinUsahaDokumenDetail { File = CopyFileVendor(model.SIUJK.File, v.Id), ContentType = model.SIUJK.ContentType, Nomor = model.SIUJK.Nomor, TipeDokumen = EDocumentType.SIUJK, Instansi = model.SIUJK.Instansi, Klasifikasi = model.SIUJK.Klasifikasi, Kualifikasi = model.SIUJK.Kualifikasi, MasaBerlaku = model.SIUJK.MasaBerlaku, Vendor = new List<Vendor>() { v }, Active = true };

            v.Dokumen = new List<Dokumen>() { NPWP, KTP, PKP, Akta, AktaTerakhir, TDP, SIUJK, SIUP, NPWPPemilik, KTPPemilik, DOMISILI };

            v.Owner = Guid.NewGuid();

            _repository.Save();
            string rand = CreatePassword(8);

            await TestCreateUser(v.NomorVendor, rand, v.Nama, v.Owner);

            return v.Id;
        }

        private string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string number = "1234567890";
            System.Text.StringBuilder res = new System.Text.StringBuilder();
            Random rnd = new Random();
            int i = length / 4;
            while (0 < i--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
                res.Append(lower[rnd.Next(lower.Length)]);
                res.Append(upper[rnd.Next(upper.Length)]);
                res.Append(number[rnd.Next(number.Length)]);
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString().Substring(0, length);
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.Roles.pRole_procurement_vendor })]
        [HttpPost]
        public int EditVendor([FromBody]VendorViewModel model)
        {
            //access?
            Vendor v = _repository.GetVendor(model.id);
            if (v == null) return 0;
            v.TipeVendor = (ETipeVendor)model.TipeVendor;
            v.Nama = model.Nama ?? v.Nama;
            v.Alamat = model.Alamat ?? v.Alamat;
            v.Provinsi = model.Provinsi ?? v.Provinsi;
            v.Kota = model.Kota ?? v.Kota;
            v.KodePos = model.KodePos ?? v.KodePos;
            v.Email = model.Email ?? v.Email;
            v.Website = model.Website ?? v.Website;
            v.Telepon = model.Telepon ?? v.Telepon;
            v.StatusAkhir = EStatusVendor.UPDATED;

            if (model.VendorPerson != null)
            {
                foreach (VendorPerson vp in v.VendorPerson)
                {
                    vp.Active = false;
                }
                List<VendorPerson> lvp = new List<VendorPerson>();
                foreach (VendorPersonViewModel vpvm in model.VendorPerson)
                {
                    if (vpvm.Nama != null && vpvm.Nama != "") //persons without name are ignored
                        lvp.Add(new VendorPerson
                        {
                            Nama = vpvm.Nama,
                            Jabatan = vpvm.Jabatan,
                            Email = vpvm.Email,
                            Telepon = vpvm.Telepon,
                            Active = true
                        });
                }
                v.VendorPerson = lvp;
            }
            if (model.BankInfo != null)
            {
                foreach (BankInfo bif in v.BankInfo)
                {
                    bif.Active = false;
                }

                BankInfo bi = new BankInfo
                {
                    NamaBank = model.BankInfo.Nama,
                    Cabang = model.BankInfo.Cabang,
                    NomorRekening = model.BankInfo.NomorRekening,
                    NamaRekening = model.BankInfo.NamaRekening,
                    Active = true
                };

                if (bi.NamaBank != null && bi.NamaBank != "") //banks without name are ignored
                    v.BankInfo = new List<BankInfo>() { bi };
            }

            RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor() { Komentar = "Pengajuan PERUBAHAN, oleh " + CurrentUser.UserName, Status = EStatusVendor.UPDATED, Urutan = 0, Metode = EMetodeVerifikasiVendor.NONE, Waktu = DateTime.Now };
            v.RiwayatPengajuanVendor = new List<RiwayatPengajuanVendor>() { rp };

            DokumenDetail NPWP = null;
            if (model.NPWP.File != null)
            {
                NPWP = new DokumenDetail { File = CopyFileVendor(model.NPWP.File, v.Id), ContentType = model.NPWP.ContentType, Nomor = model.NPWP.Nomor, TipeDokumen = EDocumentType.NPWP, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.NPWP.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.NPWP && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(NPWP);
            }
            DokumenDetail KTP = null;
            if (model.KTP.File != null)
            {
                KTP = new DokumenDetail { File = CopyFileVendor(model.KTP.File, v.Id), ContentType = model.KTP.ContentType, Nomor = model.KTP.Nomor, TipeDokumen = EDocumentType.KTP, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.KTP.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.KTP && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(KTP);
            }
            DokumenDetail PKP = null;
            if (model.PKP.File != null)
            {
                PKP = new DokumenDetail { File = CopyFileVendor(model.PKP.File, v.Id), ContentType = model.PKP.ContentType, Nomor = model.PKP.Nomor, TipeDokumen = EDocumentType.PKP, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.PKP.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.PKP && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(PKP);
            }
            DokumenDetail KTPPemilik = null;
            if (model.KTPPemilik.File != null)
            {
                KTPPemilik = new DokumenDetail { File = CopyFileVendor(model.KTPPemilik.File, v.Id), ContentType = model.KTPPemilik.ContentType, Nomor = model.KTPPemilik.Nomor, TipeDokumen = EDocumentType.KTPPemilik, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.KTPPemilik.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.KTPPemilik && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(KTPPemilik);
            }
            DokumenDetail NPWPPemilik = null;
            if (model.NPWPPemilik.File != null)
            {
                NPWPPemilik = new DokumenDetail { File = CopyFileVendor(model.NPWPPemilik.File, v.Id), ContentType = model.NPWPPemilik.ContentType, Nomor = model.NPWPPemilik.Nomor, TipeDokumen = EDocumentType.NPWPPemilik, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.NPWPPemilik.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.NPWPPemilik && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(NPWPPemilik);
            }
            DokumenDetail DOMISILI = null;
            if (model.DOMISILI.File != null)
            {
                DOMISILI = new DokumenDetail { File = CopyFileVendor(model.DOMISILI.File, v.Id), ContentType = model.DOMISILI.ContentType, Nomor = model.DOMISILI.Nomor, TipeDokumen = EDocumentType.DOMISILI, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.DOMISILI.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.DOMISILI && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(DOMISILI);
            }
            List<Dokumen> add = v.Dokumen.Where(x => x.Active == true && x.TipeDokumen == EDocumentType.AKTA).ToList();
            List<AktaDokumenDetail> lad = new List<AktaDokumenDetail>();
            foreach (Dokumen d in add)
            {
                AktaDokumenDetail a = (AktaDokumenDetail)d;
                lad.Add(a);
            }
            AktaDokumenDetail Akta = null;
            if (model.Akta.File != null)
            {
                Akta = new AktaDokumenDetail { File = CopyFileVendor(model.Akta.File, v.Id), ContentType = model.Akta.ContentType, Nomor = model.Akta.Nomor, TipeDokumen = EDocumentType.AKTA, Notaris = model.Akta.Notaris, Tanggal = model.Akta.Tanggal, Vendor = new List<Vendor>() { v }, order = 1, Active = true };
                if (model.Akta.id != null)
                {
                    AktaDokumenDetail d = (AktaDokumenDetail)v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.AKTA && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;

                }
                v.Dokumen.Add(Akta);
            }
            AktaDokumenDetail AktaTerakhir = null;
            if (model.AktaTerakhir.File != null)
            {
                AktaTerakhir = new AktaDokumenDetail { File = CopyFileVendor(model.AktaTerakhir.File, v.Id), ContentType = model.AktaTerakhir.ContentType, Nomor = model.AktaTerakhir.Nomor, TipeDokumen = EDocumentType.AKTA, Notaris = model.AktaTerakhir.Notaris, Tanggal = model.AktaTerakhir.Tanggal, Vendor = new List<Vendor>() { v }, order = 1, Active = true };
                if (model.AktaTerakhir.id != null)
                {
                    AktaDokumenDetail d = (AktaDokumenDetail)v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.AKTA && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(AktaTerakhir);
            }
            IzinUsahaDokumenDetail TDP = null;
            if (model.TDP.File != null)
            {
                TDP = new IzinUsahaDokumenDetail { File = CopyFileVendor(model.TDP.File, v.Id), ContentType = model.TDP.ContentType, Nomor = model.TDP.Nomor, TipeDokumen = EDocumentType.TDP, Instansi = model.TDP.Instansi, Klasifikasi = model.TDP.Klasifikasi, Kualifikasi = model.TDP.Kualifikasi, MasaBerlaku = model.TDP.MasaBerlaku, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.TDP.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.TDP && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(TDP);
            }
            IzinUsahaDokumenDetail SIUP = null;
            if (model.SIUP.File != null)
            {
                SIUP = new IzinUsahaDokumenDetail { File = CopyFileVendor(model.SIUP.File, v.Id), ContentType = model.SIUP.ContentType, Nomor = model.SIUP.Nomor, TipeDokumen = EDocumentType.SIUP, Instansi = model.SIUP.Instansi, Klasifikasi = model.SIUP.Klasifikasi, Kualifikasi = model.SIUP.Kualifikasi, MasaBerlaku = model.SIUP.MasaBerlaku, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.SIUP.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.SIUP && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(SIUP);
            }
            IzinUsahaDokumenDetail SIUJK = null;
            if (model.SIUJK.File != null)
            {
                SIUJK = new IzinUsahaDokumenDetail { File = CopyFileVendor(model.SIUJK.File, v.Id), ContentType = model.SIUJK.ContentType, Nomor = model.SIUJK.Nomor, TipeDokumen = EDocumentType.SIUJK, Instansi = model.SIUJK.Instansi, Klasifikasi = model.SIUJK.Klasifikasi, Kualifikasi = model.SIUJK.Kualifikasi, MasaBerlaku = model.SIUJK.MasaBerlaku, Vendor = new List<Vendor>() { v }, Active = true };
                if (model.SIUJK.id != null)
                {
                    Dokumen d = v.Dokumen.Where(x => x.TipeDokumen == EDocumentType.SIUJK && x.Active == true).FirstOrDefault() ?? null;
                    if (d != null) d.Active = false;
                }
                v.Dokumen.Add(SIUJK);
            }

            _repository.Save();

            return v.Id;
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_superadmin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_end_user, IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance })]
        public VendorViewModel GetVendorDetail(int id)
        {
            //do something, prevent access etc
            VendorViewModel vm = new VendorViewModel();
            Vendor v = _repository.GetVendor(id);
            if (v == null)
            {
                return null;
            }
            vm.TipeVendor = (int)v.TipeVendor;
            vm.id = v.Id;
            vm.Nama = v.Nama;
            vm.Alamat = v.Alamat;
            vm.NoPengajuan = v.NomorVendor;
            vm.Provinsi = v.Provinsi;
            vm.Kota = v.Kota;
            vm.KodePos = v.KodePos;
            vm.Email = v.Email;
            vm.Website = v.Website;
            vm.Telepon = v.Telepon;
            vm.StatusAkhir = v.StatusAkhir.ToString();

            if (v.BankInfo != null)
            {
                vm.BankInfo.Nama = v.BankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.BankInfo.Where(x => x.Active == true).LastOrDefault().NamaBank : null;// 1 bank
                vm.BankInfo.NamaRekening = v.BankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.BankInfo.Where(x => x.Active == true).LastOrDefault().NamaRekening : null;// 1 bank
                vm.BankInfo.Cabang = v.BankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.BankInfo.Where(x => x.Active == true).LastOrDefault().Cabang : null;// 1 bank
                vm.BankInfo.NomorRekening = v.BankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.BankInfo.Where(x => x.Active == true).LastOrDefault().NomorRekening : null;// 1 bank
            }
            else { vm.BankInfo = null; }

            List<VendorPersonViewModel> lvp = new List<VendorPersonViewModel>();
            if (v.VendorPerson != null)
            {
                foreach (VendorPerson vp in v.VendorPerson.Where(x => x.Active == true))
                {
                    lvp.Add(new VendorPersonViewModel() { Nama = vp.Nama, Jabatan = vp.Jabatan, Email = vp.Email, Telepon = vp.Telepon });
                }
                vm.VendorPerson = lvp.ToArray();
            }

            //var dokumens = _repository2.GetAllDokumenByVendor(id);
            var dokumens = v.Dokumen.Where(x => x.Active == true);
            List<Dokumen> la = dokumens.Where(x => x.TipeDokumen == EDocumentType.AKTA).ToList();
            List<AktaDokumenDetail> lad = new List<AktaDokumenDetail>();
            foreach (Dokumen d in la)
            {
                AktaDokumenDetail a = (AktaDokumenDetail)d;
                lad.Add(a);
            }
            //vm.NPWP.id = dokumens.Where(x => x.TipeDokumen == EDocumentType.NPWP).Select(a=>a.Id).FirstOrDefault().ToString() ?? "";
            //vm.PKP.id = dokumens.Where(x => x.TipeDokumen == EDocumentType.PKP).Select(a => a.Id).FirstOrDefault().ToString() ?? "";
            //vm.Akta.id = lad.OrderBy(x => x.order).Select(a => a.Id).FirstOrDefault().ToString() ?? ""; //suppose only 2 aktas
            //vm.AktaTerakhir.id = lad.Where(x=>x.order!=1).OrderByDescending(x => x.order).Select(a => a.Id).FirstOrDefault().ToString() ?? ""; //latest
            //vm.TDP.id = dokumens.Where(x => x.TipeDokumen == EDocumentType.TDP).Select(a => a.Id).FirstOrDefault().ToString() ?? "";
            //vm.SIUP.id = dokumens.Where(x => x.TipeDokumen == EDocumentType.SIUP).Select(a => a.Id).FirstOrDefault().ToString() ?? "";
            //vm.SIUJK.id = dokumens.Where(x => x.TipeDokumen == EDocumentType.SIUJK).Select(a => a.Id).FirstOrDefault().ToString() ?? "";
            DokumenDetail npwp = (DokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.NPWP).FirstOrDefault();
            if (npwp != null) vm.NPWP = new DokumenDetailViewModel() { id = npwp.Id.ToString(), Nomor = npwp.Nomor, File = npwp.File, ContentType = npwp.ContentType };
            DokumenDetail ktp = (DokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.KTP).FirstOrDefault();
            if (ktp != null) vm.KTP = new DokumenDetailViewModel() { id = ktp.Id.ToString(), Nomor = ktp.Nomor, File = ktp.File, ContentType = ktp.ContentType };
            DokumenDetail KTPPemilik = (DokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.KTPPemilik).FirstOrDefault();
            if (KTPPemilik != null) vm.KTPPemilik = new DokumenDetailViewModel() { id = KTPPemilik.Id.ToString(), Nomor = KTPPemilik.Nomor, File = KTPPemilik.File, ContentType = KTPPemilik.ContentType };
            DokumenDetail NPWPPemilik = (DokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.NPWPPemilik).FirstOrDefault();
            if (NPWPPemilik != null) vm.NPWPPemilik = new DokumenDetailViewModel() { id = NPWPPemilik.Id.ToString(), Nomor = NPWPPemilik.Nomor, File = NPWPPemilik.File, ContentType = NPWPPemilik.ContentType };
            DokumenDetail DOMISILI = (DokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.DOMISILI).FirstOrDefault();
            if (DOMISILI != null) vm.DOMISILI = new DokumenDetailViewModel() { id = DOMISILI.Id.ToString(), Nomor = DOMISILI.Nomor, File = DOMISILI.File, ContentType = DOMISILI.ContentType };
            DokumenDetail pkp = (DokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.PKP).FirstOrDefault();
            if (pkp != null) vm.PKP = new DokumenDetailViewModel() { id = pkp.Id.ToString(), Nomor = pkp.Nomor, File = pkp.File, ContentType = pkp.ContentType };
            AktaDokumenDetail akta = (AktaDokumenDetail)lad.OrderBy(x => x.order).FirstOrDefault();
            if (akta != null) vm.Akta = new AktaDokumenDetailViewModel() { id = akta.Id.ToString(), Nomor = akta.Nomor, File = akta.File, Notaris = akta.Notaris, Tanggal = akta.Tanggal, ContentType = akta.ContentType };
            AktaDokumenDetail aktaTerakhir = (AktaDokumenDetail)lad.Where(x => x.order != 1).OrderByDescending(x => x.order).FirstOrDefault();
            if (aktaTerakhir != null) vm.AktaTerakhir = new AktaDokumenDetailViewModel() { id = aktaTerakhir.Id.ToString(), Nomor = aktaTerakhir.Nomor, File = aktaTerakhir.File, Notaris = aktaTerakhir.Notaris, Tanggal = aktaTerakhir.Tanggal, ContentType = aktaTerakhir.ContentType };
            IzinUsahaDokumenDetail tdp = (IzinUsahaDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.TDP).FirstOrDefault();
            if (tdp != null) vm.TDP = new IzinUsahaDokumenDetailViewModel() { id = tdp.Id.ToString(), Nomor = tdp.Nomor, File = tdp.File, Instansi = tdp.Instansi, MasaBerlaku = tdp.MasaBerlaku, Klasifikasi = tdp.Klasifikasi, Kualifikasi = tdp.Kualifikasi, ContentType = tdp.ContentType };
            IzinUsahaDokumenDetail siup = (IzinUsahaDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.SIUP).FirstOrDefault();
            if (siup != null) vm.SIUP = new IzinUsahaDokumenDetailViewModel() { id = siup.Id.ToString(), Nomor = siup.Nomor, File = siup.File, Instansi = siup.Instansi, MasaBerlaku = siup.MasaBerlaku, Klasifikasi = siup.Klasifikasi, Kualifikasi = siup.Kualifikasi, ContentType = siup.ContentType };
            IzinUsahaDokumenDetail siujk = (IzinUsahaDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.SIUJK).FirstOrDefault();
            if (siujk != null) vm.SIUJK = new IzinUsahaDokumenDetailViewModel() { id = siujk.Id.ToString(), Nomor = siujk.Nomor, File = siujk.File, Instansi = siujk.Instansi, MasaBerlaku = siujk.MasaBerlaku, Klasifikasi = siujk.Klasifikasi, Kualifikasi = siujk.Kualifikasi, ContentType = siujk.ContentType };

            return vm;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_vendor)]
        public VendorViewModel GetVendorDetailRekanan()
        {
            //string uid = UserId();
            return GetVendorDetail(_repository.GetVendorByUser(UserId()).Id);
        }

        public string CopyFileVendor(string uidFileName, int id)
        {
            //if (d == null) return false;
            var uploadPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string fileLoc = uploadPath + FILE_TEMP_PATH + uidFileName;
            string fileVendorPathSave = FILE_VENDOR_PATH + id + @"\";
            if (Directory.Exists(uploadPath + fileVendorPathSave) == false)
            {
                Directory.CreateDirectory(uploadPath + fileVendorPathSave);
            }
            try
            {
                FileInfo fi = new FileInfo(fileLoc);
                fi.MoveTo(uploadPath + fileVendorPathSave + uidFileName);
            }
            catch (IOException ei)
            {
                return "";
            }
            return fileVendorPathSave + uidFileName;
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_superadmin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_end_user, IdLdapConstants.Roles.pRole_procurement_vendor, IdLdapConstants.Roles.pRole_procurement_manager, IdLdapConstants.Roles.pRole_compliance })]
        public List<StatusVerifikasiViewModel> GetStatusVerifikasi(int id)
        {
            Vendor v = _repository.GetVendor(id);
            List<StatusVerifikasiViewModel> sv = new List<StatusVerifikasiViewModel>();
            if (v != null)
            {
                RiwayatPengajuanVendor desk = v.RiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.DESK).OrderByDescending(x => x.Urutan).FirstOrDefault();
                if (desk != null)
                {
                    StatusVerifikasiViewModel s = new StatusVerifikasiViewModel() { Comment = desk.Komentar, Waktu = desk.Waktu, Metode = EMetodeVerifikasiVendor.DESK.ToString() };
                    sv.Add(s);
                }
                desk = v.RiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.PHONE).OrderByDescending(x => x.Urutan).FirstOrDefault();
                if (desk != null)
                {
                    StatusVerifikasiViewModel s = new StatusVerifikasiViewModel() { Comment = desk.Komentar, Waktu = desk.Waktu, Metode = EMetodeVerifikasiVendor.PHONE.ToString() };
                    sv.Add(s);
                }
                desk = v.RiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.VISIT).OrderByDescending(x => x.Urutan).FirstOrDefault();
                if (desk != null)
                {
                    StatusVerifikasiViewModel s = new StatusVerifikasiViewModel() { Comment = desk.Komentar, Waktu = desk.Waktu, Metode = EMetodeVerifikasiVendor.VISIT.ToString() };
                    sv.Add(s);
                }
            }
            return sv;
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string DeskVerification(int id, string comment)
        {
            return VerificationSubmit(id, comment, EMetodeVerifikasiVendor.DESK);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string PhoneVerification(int id, string comment)
        {
            return VerificationSubmit(id, comment, EMetodeVerifikasiVendor.PHONE);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string VisitVerification(int id, string comment)
        {
            return VerificationSubmit(id, comment, EMetodeVerifikasiVendor.VISIT);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        public string VerificationSubmit(int id, string comment, EMetodeVerifikasiVendor metode)
        {
            Vendor v = _repository.GetVendor(id);
            if (v != null)
            {
                int vd = v.RiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.DESK).Count() > 0 ? (metode != EMetodeVerifikasiVendor.DESK) ? 1 : 0 : 0;
                int vp = v.RiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.PHONE).Count() > 0 ? (metode != EMetodeVerifikasiVendor.PHONE) ? 1 : 0 : 0;
                int vv = v.RiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.VISIT).Count() > 0 ? (metode != EMetodeVerifikasiVendor.VISIT) ? 1 : 0 : 0;

                EStatusVendor status = vd + vp + vv == 0 ? EStatusVendor.PASS_1 :
                    vd + vp + vv == 1 ? EStatusVendor.PASS_2 :
                    vd + vp + vv == 2 ? EStatusVendor.PASS_3 :
                    vd + vp + vv == 3 ? EStatusVendor.VERIFIED :
                    EStatusVendor.VERIFIED;
                int urutan = v.RiwayatPengajuanVendor.OrderByDescending(x => x.Urutan).FirstOrDefault() != null ?
                    v.RiwayatPengajuanVendor.OrderByDescending(x => x.Urutan).Select(x => x.Urutan).FirstOrDefault() : 0;
                RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor()
                {
                    Komentar = "Verifikasi " + status.ToString() + ", pesan: " + comment + ", oleh " + CurrentUser.UserName,
                    Urutan = urutan + 1,
                    Status = status,
                    Metode = metode,
                    Waktu = DateTime.Now
                };
                v.RiwayatPengajuanVendor.Add(rp);
                v.StatusAkhir = status;

                _repository.Save();
                return "Success!";
            }
            return "Vendor not found!";
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string TolakPerubahan(int id, string msg)
        {
            Vendor v = _repository.GetVendor(id);
            if (v != null)
            {
                RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor()
                {
                    Komentar = "Pengajuan perubahan DITOLAK dengan alasan: " + msg,
                    Metode = EMetodeVerifikasiVendor.NONE,
                    Status = EStatusVendor.UPDATED,
                    Waktu = DateTime.Now
                };
                v.RiwayatPengajuanVendor.Add(rp);
                _repository.Save();
                return "Sukses.";
            }
            return "Gagal.";
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string DaftarHitam(int id, string msg)
        {
            Vendor v = _repository.GetVendor(id);
            if (v != null)
            {
                RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor()
                {
                    Komentar = "DAFTAR HITAM. alasan: " + msg,
                    Metode = EMetodeVerifikasiVendor.NONE,
                    Status = EStatusVendor.BLACKLIST,
                    Waktu = DateTime.Now
                };
                v.RiwayatPengajuanVendor.Add(rp);
                v.StatusAkhir = EStatusVendor.BLACKLIST;
                _repository.Save();
                return "Sukses.";
            }
            return "Gagal.";
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string DaftarPutih(int id, string msg)
        {
            Vendor v = _repository.GetVendor(id);
            if (v != null && v.StatusAkhir == EStatusVendor.BLACKLIST)
            {
                RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor()
                {
                    Komentar = "KELUAR dari DAFTAR HITAM. alasan: " + msg,
                    Metode = EMetodeVerifikasiVendor.NONE,
                    Status = EStatusVendor.VERIFIED,
                    Waktu = DateTime.Now
                };
                v.RiwayatPengajuanVendor.Add(rp);
                v.StatusAkhir = EStatusVendor.VERIFIED;
                _repository.Save();
                return "Sukses.";
            }
            return "Gagal.";
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string SetujuiPerubahan(int id)
        {
            Vendor v = _repository.GetVendor(id);
            if (v != null && v.StatusAkhir == EStatusVendor.UPDATED)
            {
                RiwayatPengajuanVendor rp = new RiwayatPengajuanVendor()
                {
                    Komentar = "Pengajuan perubahan DISETUJUI, oleh " + CurrentUser.UserName,
                    Metode = EMetodeVerifikasiVendor.NONE,
                    Status = EStatusVendor.VERIFIED,
                    Waktu = DateTime.Now
                };
                v.StatusAkhir = EStatusVendor.VERIFIED;
                v.RiwayatPengajuanVendor.Add(rp);
                _repository.Save();
                return "Sukses.";
            }
            return "Gagal.";
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_vendor })]
        public List<RiwayatPengajuanVendorViewModel> GetRiwayatVendor(int id, int skip = 0, int limit = 5)
        {
            Vendor v = _repository.GetVendor(id);
            if (v != null)
            {
                List<RiwayatPengajuanVendorViewModel> rv = v.RiwayatPengajuanVendor.Select(
                    x => new RiwayatPengajuanVendorViewModel()
                    {
                        Tanggal = ((DateTime)x.Waktu).ToString("yyyy-MM-dd HH:mm"),
                        Pesan = x.Komentar
                    }
                    ).OrderByDescending(x => x.Tanggal).Skip(skip).Take(limit).ToList();
                return rv;
            }
            return new List<RiwayatPengajuanVendorViewModel>();
        }


        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_vendor })]
        public List<RiwayatPengajuanVendorViewModel> GetRiwayatVendorDetail()
        {
            //string uid = UserId();
            return GetRiwayatVendor(_repository.GetVendorByUser(UserId()).Id);
        }


        [Authorize]
        private async Task<string> TestCreateUser(string username, string password, string displayname, Guid guid)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var user = new { UserName = username, DisplayName = displayname, NewPassword = password, IsLdapUser = false, guid = guid };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var stringContent = new StringContent(user.ToString(), System.Text.Encoding.UTF8, "application/json");
            //stringContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            HttpResponseMessage reply = await client.PostAsync(
                    string.Format("{0}/{1}", IdLdapConstants.IDM.Url, "admin/NewVendorUser"), content);
            if (reply.IsSuccessStatusCode)
            {
                return reply.Content.ReadAsStringAsync().Result;
            };
            return "";
        }
    }
}
