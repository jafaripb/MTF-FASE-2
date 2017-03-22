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
using Model.Helper;
using Reston.Pinata.WebService.Helper;
using System.Security.Claims;
using System.Threading;

namespace Reston.Pinata.WebService
{
    public class RegistrasiController : BaseController
    {
        private IRegistrasiRepo _repository;
        private string FILE_TEMP_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_TEMP"]; //config soon
        private string FILE_VENDOR_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_REG"]; //--^

        public RegistrasiController()
        {
            _repository = new RegistrasiRepo(new JimbisContext());
        }

        public RegistrasiController(RegistrasiRepo repository)
        {
            _repository = repository;
        }

        public IEnumerable<string> Get()
        {
            return new[] { "Vendor is so", "more of it", "more" };
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        public RegVendor GetVendor(int id)
        {
            return _repository.GetVendor(id);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        public List<VendorViewModel> GetVendors(string tipe, string status, int limit, string search)
        {
            var lv =
            _repository.GetVendors(tipe != null ? (ETipeVendor)Enum.Parse(typeof(ETipeVendor), tipe) : ETipeVendor.NONE,
                status != null ? (EStatusVendor)Enum.Parse(typeof(EStatusVendor), status) : EStatusVendor.NONE
                , limit,search);
            if (lv != null)
                return lv.Where(x => search == null || x.Nama.ToLower().Contains(search.ToLower())).Select(x => new VendorViewModel() { id = x.Id, Nama = x.Nama, Alamat = x.Alamat, Telepon = x.Telepon, Email = x.Email, Website = x.Website, NoPengajuan = x.NoPengajuan }).ToList();
            return new List<VendorViewModel>();
        }

        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        public int validasiCaptcha(Guid guid, string answer)
        {
            if (!(new CaptchaHelper()).Verify(guid, answer))
            {
                return 0;//captcha failed
            }
            else return 1;
        }

        [HttpPost]
        public string AddVendor([FromBody]VendorViewModel model)
        {
            //bool useGoogleCaptcha = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["USE_GOOGLE_RECAPTCHA"]);
            //if (useGoogleCaptcha) { 
            //    if (!ValidateRecaptcha(model.gCaptchaResponse))
            //    {
            //        return "Captcha failed";
            //    }
            //}
            if (!(new CaptchaHelper()).Verify(Guid.Parse(model.gCaptchaK), model.gCaptchaResponse))
            {
                return "0";//captcha failed
            }
            //access?
            var ss = Request.GetQueryNameValuePairs();
            RegVendor v = new RegVendor
            {
                //NoPengajuan = GenerateNoPengajuan(),
                NoPengajuan = GenerateNomorVendor((int)model.TipeVendor),
                TipeVendor = (ETipeVendor)model.TipeVendor,
                Nama = model.Nama,
                Alamat = model.Alamat,
                Provinsi = model.Provinsi,
                Kota = model.Kota,
                KodePos = model.KodePos,
                Email = model.Email,
                Website = model.Website,
                Telepon = model.Telepon,
                StatusAkhir = EStatusVendor.NEW
            };

            if (model.VendorPerson != null)
            {
                List<RegVendorPerson> lvp = new List<RegVendorPerson>();
                foreach (VendorPersonViewModel vpvm in model.VendorPerson)
                {
                    if (vpvm.Nama != null && vpvm.Nama != "") //persons without name are ignored
                        lvp.Add(new RegVendorPerson
                        {
                            Nama = vpvm.Nama,
                            Jabatan = vpvm.Jabatan,
                            Email = vpvm.Email,
                            Telepon = vpvm.Telepon,
                            Active = true
                        });
                }
                v.RegVendorPerson = lvp;
            }
            if (model.BankInfo != null)
            {
                RegBankInfo bi = new RegBankInfo
                {
                    NamaBank = model.BankInfo.Nama,
                    Cabang = model.BankInfo.Cabang,
                    NomorRekening = model.BankInfo.NomorRekening,
                    NamaRekening = model.BankInfo.NamaRekening,
                    Active = true
                };

                if (bi.NamaBank != null && bi.NamaBank != "") //banks without name are ignored
                    v.RegBankInfo = new List<RegBankInfo>() { bi };
            }

            RegRiwayatPengajuanVendor rp = new RegRiwayatPengajuanVendor() { Komentar = "Pengajuan rekanan BARU.", Status = EStatusVendor.NEW, Urutan = 0, Metode = EMetodeVerifikasiVendor.NONE, Waktu = DateTime.Now };
            v.RegRiwayatPengajuanVendor = new List<RegRiwayatPengajuanVendor>() { rp };

            RegDokumenDetail NPWP = null;
            RegDokumenDetail PKP = null;
            RegDokumenDetail KTP = null;
            RegDokumenDetail NPWPPemilik = null;
            RegDokumenDetail KTPPemilik = null;
            RegDokumenDetail DOMISILI = null;
            RegAktaDokumenDetail Akta = null;
            RegAktaDokumenDetail AktaTerakhir = null;
            RegIzinUsahaDokumenDetail TDP = null;
            RegIzinUsahaDokumenDetail SIUP = null;
            RegIzinUsahaDokumenDetail SIUJK = null;

            _repository.AddVendor(v);

            if (model.NPWP.File != null)
                NPWP = new RegDokumenDetail { File = CopyFileVendor(model.NPWP.File, v.Id), ContentType = model.NPWP.ContentType, Nomor = model.NPWP.Nomor, TipeDokumen = EDocumentType.NPWP, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.PKP.File != null)
                PKP = new RegDokumenDetail { File = CopyFileVendor(model.PKP.File, v.Id), ContentType = model.PKP.ContentType, Nomor = model.PKP.Nomor, TipeDokumen = EDocumentType.PKP, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.KTP.File != null)
                KTP = new RegDokumenDetail { File = CopyFileVendor(model.KTP.File, v.Id), ContentType = model.KTP.ContentType, Nomor = model.KTP.Nomor, TipeDokumen = EDocumentType.KTP, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.NPWPPemilik.File != null)
                NPWPPemilik = new RegDokumenDetail { File = CopyFileVendor(model.NPWPPemilik.File, v.Id), ContentType = model.NPWPPemilik.ContentType, Nomor = model.NPWPPemilik.Nomor, TipeDokumen = EDocumentType.NPWPPemilik, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.KTPPemilik.File != null)
                KTPPemilik = new RegDokumenDetail { File = CopyFileVendor(model.KTPPemilik.File, v.Id), ContentType = model.KTPPemilik.ContentType, Nomor = model.KTPPemilik.Nomor, TipeDokumen = EDocumentType.KTPPemilik, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.DOMISILI.File != null)
                DOMISILI = new RegDokumenDetail { File = CopyFileVendor(model.DOMISILI.File, v.Id), ContentType = model.DOMISILI.ContentType, Nomor = model.DOMISILI.Nomor, TipeDokumen = EDocumentType.DOMISILI, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.Akta.File != null)
                Akta = new RegAktaDokumenDetail { File = CopyFileVendor(model.Akta.File, v.Id), ContentType = model.Akta.ContentType, Nomor = model.Akta.Nomor, TipeDokumen = EDocumentType.AKTA, Notaris = model.Akta.Notaris, Tanggal = model.Akta.Tanggal, RegVendor = new List<RegVendor>() { v }, order = 1, Active = true };
            if (model.AktaTerakhir.File != null)
                AktaTerakhir = new RegAktaDokumenDetail { File = CopyFileVendor(model.AktaTerakhir.File, v.Id), ContentType = model.AktaTerakhir.ContentType, Nomor = model.AktaTerakhir.Nomor, TipeDokumen = EDocumentType.AKTA, Notaris = model.AktaTerakhir.Notaris, Tanggal = model.AktaTerakhir.Tanggal, RegVendor = new List<RegVendor>() { v }, order = 2, Active = true };
            if (model.TDP.File != null)
                TDP = new RegIzinUsahaDokumenDetail { File = CopyFileVendor(model.TDP.File, v.Id), ContentType = model.TDP.ContentType, Nomor = model.TDP.Nomor, TipeDokumen = EDocumentType.TDP, Instansi = model.TDP.Instansi, Klasifikasi = model.TDP.Klasifikasi, Kualifikasi = model.TDP.Kualifikasi, MasaBerlaku = model.TDP.MasaBerlaku, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.SIUP.File != null)
                SIUP = new RegIzinUsahaDokumenDetail { File = CopyFileVendor(model.SIUP.File, v.Id), ContentType = model.SIUP.ContentType, Nomor = model.SIUP.Nomor, TipeDokumen = EDocumentType.SIUP, Instansi = model.SIUP.Instansi, Klasifikasi = model.SIUP.Klasifikasi, Kualifikasi = model.SIUP.Kualifikasi, MasaBerlaku = model.SIUP.MasaBerlaku, RegVendor = new List<RegVendor>() { v }, Active = true };
            if (model.SIUJK.File != null)
                SIUJK = new RegIzinUsahaDokumenDetail { File = CopyFileVendor(model.SIUJK.File, v.Id), ContentType = model.SIUJK.ContentType, Nomor = model.SIUJK.Nomor, TipeDokumen = EDocumentType.SIUJK, Instansi = model.SIUJK.Instansi, Klasifikasi = model.SIUJK.Klasifikasi, Kualifikasi = model.SIUJK.Kualifikasi, MasaBerlaku = model.SIUJK.MasaBerlaku, RegVendor = new List<RegVendor>() { v }, Active = true };

            v.RegDokumen = new List<RegDokumen>() { NPWP, PKP, Akta, AktaTerakhir, TDP, SIUJK, SIUP, KTP, NPWPPemilik, KTPPemilik, DOMISILI };

            _repository.Save();

            //sending email notification
            try
            {
                Reston.Pinata.WebService.Helper.Mailer.sendText(v.Nama, model.Email,
                        System.Configuration.ConfigurationManager.AppSettings["MAIL_VENDOR_REGISTRATION_TITLE"],
                        String.Format(System.Configuration.ConfigurationManager.AppSettings["MAIL_VENDOR_REGISTRATION_BODY"].ToString(), IdLdapConstants.Proc.Url, v.NoPengajuan));
            }
            catch (Exception e)
            {

            }

            return v.NoPengajuan;
        }

        [HttpGet]
        public string TestSendMail()
        {
            try
            {
                string sendermail = System.Configuration.ConfigurationManager.AppSettings["MAIL_VENDOR_REGISTRATION_TITLE"];
                string body = String.Format(System.Configuration.ConfigurationManager.AppSettings["MAIL_VENDOR_REGISTRATION_BODY"], IdLdapConstants.Proc.Url, "#nopengajuan");
                Reston.Pinata.WebService.Helper.Mailer.sendText("Email Test", "jafar.ipb@gmail.com",
                    sendermail,
                    body
                    );
            }
            catch (Exception e)
            {
                return "failed";
            }
            return "should be sent successfully";
        }

        public bool ValidateRecaptcha(string cResponse)
        {
            string secret = System.Configuration.ConfigurationManager.AppSettings["g-recaptcha-secret"].ToString();
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, cResponse));
            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (!captchaResponse.Success)
            {
                return false;
            }
            return true;
        }

        private string GenerateNomorVendor(int tipe)
        {
            string no = "";
            DateTime d = DateTime.Now;
            no = ("" + d.Year).Substring(2, 2) + (d.Month < 10 ? "0" + d.Month : "" + d.Month) + "0" + tipe;
            string ctrn = "000" + _repository.CheckNomor(no);
            no += ctrn.Substring(ctrn.Length - 4, 4);
            return no;
        }

        private string GenerateNoPengajuan()
        {
            string no = DateTime.Now.Year.ToString().Substring(2, 2) + (DateTime.Now.Month < 10 ? "0" : "") + DateTime.Now.Month.ToString();
            string unique = (Guid.NewGuid()).ToString().Substring(0, 6);
            while (_repository.GetVendor(no + unique) != null)
            {
                unique = (new Guid()).ToString().Substring(0, 6);
            }
            return no + unique;
        }

        public IHttpActionResult GetStatusRegistrasi(string no)
        {
            RegVendor rv = _repository.GetVendor(no);
            if (rv != null)
            {
                DateTime? d = rv.RegRiwayatPengajuanVendor.Where(x => x.Status == EStatusVendor.NEW).Select(x => x.Waktu).FirstOrDefault();
                return Json(new { status = rv.StatusAkhir.ToString(), tanggal = d });
            }
            return Json(new { });
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_staff, IdLdapConstants.Roles.pRole_procurement_vendor })]
        public VendorViewModel GetVendorDetail(string no)
        {
            //do something, prevent access etc
            VendorViewModel vm = new VendorViewModel();
            RegVendor v = _repository.GetVendor(no);
            if (v == null)
            {
                return null;
            }
            //vm.TipeVendor = (ETipeVendor)v.TipeVendor.;
            vm.NoPengajuan = v.NoPengajuan;
            vm.id = v.Id;
            vm.Nama = v.Nama;
            vm.Alamat = v.Alamat;
            vm.Provinsi = v.Provinsi;
            vm.Kota = v.Kota;
            vm.KodePos = v.KodePos;
            vm.Email = v.Email;
            vm.Website = v.Website;
            vm.Telepon = v.Telepon;
            vm.StatusAkhir = v.StatusAkhir.ToString();

            if (v.RegBankInfo != null)
            {
                vm.BankInfo.Nama = v.RegBankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.RegBankInfo.Where(x => x.Active == true).LastOrDefault().NamaBank : null;// 1 bank
                vm.BankInfo.NamaRekening = v.RegBankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.RegBankInfo.Where(x => x.Active == true).LastOrDefault().NamaRekening : null;// 1 bank
                vm.BankInfo.Cabang = v.RegBankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.RegBankInfo.Where(x => x.Active == true).LastOrDefault().Cabang : null;// 1 bank
                vm.BankInfo.NomorRekening = v.RegBankInfo.Where(x => x.Active == true).LastOrDefault() != null ? v.RegBankInfo.Where(x => x.Active == true).LastOrDefault().NomorRekening : null;// 1 bank
            }
            else { vm.BankInfo = null; }

            List<VendorPersonViewModel> lvp = new List<VendorPersonViewModel>();
            if (v.RegVendorPerson != null)
            {
                foreach (RegVendorPerson vp in v.RegVendorPerson.Where(x => x.Active == true))
                {
                    lvp.Add(new VendorPersonViewModel() { Nama = vp.Nama, Jabatan = vp.Jabatan, Email = vp.Email, Telepon = vp.Telepon });
                }
                vm.VendorPerson = lvp.ToArray();
            }

            //var dokumens = _repository2.GetAllDokumenByVendor(id);
            var dokumens = v.RegDokumen.Where(x => x.Active == true);
            List<RegDokumen> la = dokumens.Where(x => x.TipeDokumen == EDocumentType.AKTA).ToList();
            List<RegAktaDokumenDetail> lad = new List<RegAktaDokumenDetail>();
            foreach (RegDokumen d in la)
            {
                RegAktaDokumenDetail a = (RegAktaDokumenDetail)d;
                lad.Add(a);
            }
            RegDokumenDetail npwp = (RegDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.NPWP).FirstOrDefault();
            if (npwp != null) vm.NPWP = new DokumenDetailViewModel() { id = npwp.Id.ToString(), Nomor = npwp.Nomor, File = npwp.File, ContentType = npwp.ContentType };
            RegDokumenDetail pkp = (RegDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.PKP).FirstOrDefault();
            if (pkp != null) vm.PKP = new DokumenDetailViewModel() { id = pkp.Id.ToString(), Nomor = pkp.Nomor, File = pkp.File, ContentType = pkp.ContentType };
            RegDokumenDetail ktp = (RegDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.KTP).FirstOrDefault();
            if (ktp != null) vm.KTP = new DokumenDetailViewModel() { id = ktp.Id.ToString(), Nomor = ktp.Nomor, File = ktp.File, ContentType = ktp.ContentType };
            RegDokumenDetail NPWPPemilik = (RegDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.NPWPPemilik).FirstOrDefault();
            if (NPWPPemilik != null) vm.NPWPPemilik = new DokumenDetailViewModel() { id = NPWPPemilik.Id.ToString(), Nomor = NPWPPemilik.Nomor, File = NPWPPemilik.File, ContentType = NPWPPemilik.ContentType };
            RegDokumenDetail KTPPemilik = (RegDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.KTPPemilik).FirstOrDefault();
            if (KTPPemilik != null) vm.KTPPemilik = new DokumenDetailViewModel() { id = KTPPemilik.Id.ToString(), Nomor = KTPPemilik.Nomor, File = KTPPemilik.File, ContentType = KTPPemilik.ContentType };
            RegDokumenDetail DOMISILI = (RegDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.DOMISILI).FirstOrDefault();
            if (DOMISILI != null) vm.DOMISILI = new DokumenDetailViewModel() { id = DOMISILI.Id.ToString(), Nomor = DOMISILI.Nomor, File = DOMISILI.File, ContentType = DOMISILI.ContentType };
            RegAktaDokumenDetail akta = (RegAktaDokumenDetail)lad.OrderBy(x => x.order).FirstOrDefault();
            if (akta != null) vm.Akta = new AktaDokumenDetailViewModel() { id = akta.Id.ToString(), Nomor = akta.Nomor, File = akta.File, Notaris = akta.Notaris, Tanggal = akta.Tanggal, ContentType = akta.ContentType };
            RegAktaDokumenDetail aktaTerakhir = (RegAktaDokumenDetail)lad.Where(x => x.order != 1).OrderByDescending(x => x.order).FirstOrDefault();
            if (aktaTerakhir != null) vm.AktaTerakhir = new AktaDokumenDetailViewModel() { id = aktaTerakhir.Id.ToString(), Nomor = aktaTerakhir.Nomor, File = aktaTerakhir.File, Notaris = aktaTerakhir.Notaris, Tanggal = aktaTerakhir.Tanggal, ContentType = aktaTerakhir.ContentType };
            RegIzinUsahaDokumenDetail tdp = (RegIzinUsahaDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.TDP).FirstOrDefault();
            if (tdp != null) vm.TDP = new IzinUsahaDokumenDetailViewModel() { id = tdp.Id.ToString(), Nomor = tdp.Nomor, File = tdp.File, Instansi = tdp.Instansi, MasaBerlaku = tdp.MasaBerlaku, Klasifikasi = tdp.Klasifikasi, Kualifikasi = tdp.Kualifikasi, ContentType = tdp.ContentType };
            RegIzinUsahaDokumenDetail siup = (RegIzinUsahaDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.SIUP).FirstOrDefault();
            if (siup != null) vm.SIUP = new IzinUsahaDokumenDetailViewModel() { id = siup.Id.ToString(), Nomor = siup.Nomor, File = siup.File, Instansi = siup.Instansi, MasaBerlaku = siup.MasaBerlaku, Klasifikasi = siup.Klasifikasi, Kualifikasi = siup.Kualifikasi, ContentType = siup.ContentType };
            RegIzinUsahaDokumenDetail siujk = (RegIzinUsahaDokumenDetail)dokumens.Where(x => x.TipeDokumen == EDocumentType.SIUJK).FirstOrDefault();
            if (siujk != null) vm.SIUJK = new IzinUsahaDokumenDetailViewModel() { id = siujk.Id.ToString(), Nomor = siujk.Nomor, File = siujk.File, Instansi = siujk.Instansi, MasaBerlaku = siujk.MasaBerlaku, Klasifikasi = siujk.Klasifikasi, Kualifikasi = siujk.Kualifikasi, ContentType = siujk.ContentType };

            return vm;
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

        public List<StatusVerifikasiViewModel> GetStatusVerifikasi(string no)
        {
            RegVendor v = _repository.GetVendor(no);
            List<StatusVerifikasiViewModel> sv = new List<StatusVerifikasiViewModel>();
            if (v != null)
            {
                RegRiwayatPengajuanVendor desk = v.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.DESK).OrderByDescending(x => x.Urutan).FirstOrDefault();
                if (desk != null)
                {
                    StatusVerifikasiViewModel s = new StatusVerifikasiViewModel() { Comment = desk.Komentar, Waktu = desk.Waktu, Metode = EMetodeVerifikasiVendor.DESK.ToString() };
                    sv.Add(s);
                }
                desk = v.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.PHONE).OrderByDescending(x => x.Urutan).FirstOrDefault();
                if (desk != null)
                {
                    StatusVerifikasiViewModel s = new StatusVerifikasiViewModel() { Comment = desk.Komentar, Waktu = desk.Waktu, Metode = EMetodeVerifikasiVendor.PHONE.ToString() };
                    sv.Add(s);
                }
                desk = v.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.VISIT).OrderByDescending(x => x.Urutan).FirstOrDefault();
                if (desk != null)
                {
                    StatusVerifikasiViewModel s = new StatusVerifikasiViewModel() { Comment = desk.Komentar, Waktu = desk.Waktu, Metode = EMetodeVerifikasiVendor.VISIT.ToString() };
                    sv.Add(s);
                }
            }
            return sv;
        }

        //[Authorize(Roles=IdLdapConstants.Roles.pRole_user)]
        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string DeskVerification(string no, string comment)
        {
            if (!UserInRole(IdLdapConstants.Roles.pRole_procurement_staff))
            {
                return "";
            }
            return VerificationSubmit(no, comment, EMetodeVerifikasiVendor.DESK);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string PhoneVerification(string no, string comment)
        {
            return VerificationSubmit(no, comment, EMetodeVerifikasiVendor.PHONE);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string VisitVerification(string no, string comment)
        {
            return VerificationSubmit(no, comment, EMetodeVerifikasiVendor.VISIT);
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public async Task<int> VerifyVendor(string no)
        {
            RegVendor rv = _repository.GetVendor(no);
            RegRiwayatPengajuanVendor rpp = rv.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.DESK).FirstOrDefault();
            if (rv != null && rpp != null)
            {
                //if (rv.StatusAkhir != EStatusVendor.PASS_3) {
                //if (rpp!=null) { //by desk checking only is also eligible
                //    return 0;
                //    //return "status pendor salah";
                //}
                rv.StatusAkhir = EStatusVendor.VERIFIED;
                RegRiwayatPengajuanVendor rp = new RegRiwayatPengajuanVendor()
                {
                    Metode = EMetodeVerifikasiVendor.NONE,
                    Status = EStatusVendor.VERIFIED,
                    Komentar = "Pengajuan rekanan telah TERVERIFIKASI, oleh " + CurrentUser.UserName,
                    Waktu = DateTime.Now,
                };
                rv.RegRiwayatPengajuanVendor.Add(rp);
                _repository.Save();

                // Creating User Account?
                Guid iduser = Guid.NewGuid();
                string rand = CreatePassword(8);
                string uname = await TestCreateUser(rv.NoPengajuan, rand, rv.Nama, iduser);
                //sending email notification
                try
                {
                    Reston.Pinata.WebService.Helper.Mailer.sendText(rv.Nama, rv.Email,
                        System.Configuration.ConfigurationManager.AppSettings["MAIL_VENDOR_VERIFICATION_TITLE"],
                        String.Format(System.Configuration.ConfigurationManager.AppSettings["MAIL_VENDOR_VERIFICATION_BODY"].ToString(), rv.Nama, uname, rand, IdLdapConstants.IDM.Url));
                }
                catch (Exception e)
                {

                }

                // Creating Vendor
                return RegisterVerifiedVendor(rv, iduser.ToString());
            }
            return -1;//not found
            //return "tak ada";
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

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [HttpGet]
        public string RejectVendor(string no)
        {
            RegVendor rv = _repository.GetVendor(no);
            if (rv != null)
            {
                if (rv.StatusAkhir == EStatusVendor.VERIFIED || rv.StatusAkhir == EStatusVendor.REJECTED || rv.StatusAkhir == EStatusVendor.UPDATED)
                {
                    return "Vendor current status not allowing rejection.";
                }
                rv.StatusAkhir = EStatusVendor.REJECTED;
                RegRiwayatPengajuanVendor rp = new RegRiwayatPengajuanVendor()
                {
                    Metode = EMetodeVerifikasiVendor.NONE,
                    Status = EStatusVendor.REJECTED,
                    Komentar = "Pengajuan rekanan DITOLAK, oleh " + CurrentUser.UserName,
                    Waktu = DateTime.Now,
                };
                rv.RegRiwayatPengajuanVendor.Add(rp);
                _repository.Save();
                return "Success.";
            }
            return "Vendor not found.";
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [Authorize]
        private string VerificationSubmit(string no, string comment, EMetodeVerifikasiVendor metode)
        {
            RegVendor v = _repository.GetVendor(no);
            if (v != null && v.StatusAkhir != EStatusVendor.REJECTED)
            {
                int vd = v.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.DESK).Count() > 0 ? (metode != EMetodeVerifikasiVendor.DESK) ? 1 : 0 : 0;
                int vp = v.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.PHONE).Count() > 0 ? (metode != EMetodeVerifikasiVendor.PHONE) ? 1 : 0 : 0;
                int vv = v.RegRiwayatPengajuanVendor.Where(x => x.Metode == EMetodeVerifikasiVendor.VISIT).Count() > 0 ? (metode != EMetodeVerifikasiVendor.VISIT) ? 1 : 0 : 0;

                EStatusVendor status = vd + vp + vv == 0 ? EStatusVendor.PASS_1 :
                    vd + vp + vv == 1 ? EStatusVendor.PASS_2 :
                    vd + vp + vv == 2 ? EStatusVendor.PASS_3 :
                    vd + vp + vv == 3 ? EStatusVendor.VERIFIED :
                    EStatusVendor.VERIFIED;
                int urutan = v.RegRiwayatPengajuanVendor.OrderByDescending(x => x.Urutan).FirstOrDefault() != null ?
                    v.RegRiwayatPengajuanVendor.OrderByDescending(x => x.Urutan).Select(x => x.Urutan).FirstOrDefault() : 0;
                RegRiwayatPengajuanVendor rp = new RegRiwayatPengajuanVendor()
                {
                    Komentar = comment,
                    Urutan = urutan + 1,
                    Status = status,
                    Metode = metode,
                    Waktu = DateTime.Now
                };
                v.RegRiwayatPengajuanVendor.Add(rp);
                v.StatusAkhir = status;

                _repository.Save();
                return "Success!";
            }
            return "Vendor not found!";
        }

        [ApiAuthorize(IdLdapConstants.Roles.pRole_procurement_staff)]
        [Authorize]
        private int RegisterVerifiedVendor(RegVendor rv, string owner)
        {
            //RegVendor rv = _repository.GetVendor(no);
            try
            {
                if (rv != null && rv.StatusAkhir == EStatusVendor.VERIFIED)
                {
                    Vendor v = new Vendor()
                    {
                        NomorVendor = rv.NoPengajuan,
                        Nama = rv.Nama,
                        Alamat = rv.Alamat,
                        Provinsi = rv.Provinsi,
                        Kota = rv.Kota,
                        Email = rv.Email,
                        KodePos = rv.KodePos,
                        Website = rv.Website,
                        Telepon = rv.Telepon,
                        StatusAkhir = rv.StatusAkhir,
                        TipeVendor = rv.TipeVendor
                    };
                    if (rv.RegVendorPerson != null)
                    {
                        List<VendorPerson> lvp = new List<VendorPerson>();
                        foreach (RegVendorPerson rvp in rv.RegVendorPerson)
                        {
                            VendorPerson vp = new VendorPerson()
                            {
                                Nama = rvp.Nama,
                                Active = rvp.Active,
                                Email = rvp.Email,
                                Jabatan = rvp.Jabatan,
                                Telepon = rvp.Telepon
                            };
                            lvp.Add(vp);
                        }
                        v.VendorPerson = lvp;
                    }
                    if (rv.RegBankInfo != null)
                    {
                        List<BankInfo> lvp = new List<BankInfo>();
                        foreach (RegBankInfo rvp in rv.RegBankInfo)
                        {
                            BankInfo vp = new BankInfo()
                            {
                                NamaBank = rvp.NamaBank,
                                Active = rvp.Active,
                                Cabang = rvp.Cabang,
                                NamaRekening = rvp.NamaRekening,
                                NomorRekening = rvp.NomorRekening
                            };
                            lvp.Add(vp);
                        }
                        v.BankInfo = lvp;
                    }
                    List<Dokumen> ld = new List<Dokumen>();
                    RegDokumenDetail npwp = (RegDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.NPWP).FirstOrDefault();
                    if (npwp != null)
                    {
                        DokumenDetail dd = new DokumenDetail()
                        {
                            Nomor = npwp.Nomor,
                            Active = npwp.Active,
                            ContentType = npwp.ContentType,
                            File = npwp.File,
                            TipeDokumen = npwp.TipeDokumen,
                            MasaBerlaku = npwp.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    RegDokumenDetail pkp = (RegDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.PKP).FirstOrDefault();
                    if (pkp != null)
                    {
                        DokumenDetail dd = new DokumenDetail()
                        {
                            Nomor = pkp.Nomor,
                            Active = pkp.Active,
                            ContentType = pkp.ContentType,
                            File = pkp.File,
                            TipeDokumen = pkp.TipeDokumen,
                            MasaBerlaku = pkp.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    RegDokumenDetail ktp = (RegDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.KTP).FirstOrDefault();
                    if (ktp != null)
                    {
                        DokumenDetail dd = new DokumenDetail()
                        {
                            Nomor = ktp.Nomor,
                            Active = ktp.Active,
                            ContentType = ktp.ContentType,
                            File = ktp.File,
                            TipeDokumen = ktp.TipeDokumen,
                            MasaBerlaku = ktp.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    RegDokumenDetail NPWPPemilik = (RegDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.NPWPPemilik).FirstOrDefault();
                    if (NPWPPemilik != null)
                    {
                        DokumenDetail dd = new DokumenDetail()
                        {
                            Nomor = NPWPPemilik.Nomor,
                            Active = NPWPPemilik.Active,
                            ContentType = NPWPPemilik.ContentType,
                            File = NPWPPemilik.File,
                            TipeDokumen = NPWPPemilik.TipeDokumen,
                            MasaBerlaku = NPWPPemilik.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    RegDokumenDetail KTPPemilik = (RegDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.KTPPemilik).FirstOrDefault();
                    if (KTPPemilik != null)
                    {
                        DokumenDetail dd = new DokumenDetail()
                        {
                            Nomor = KTPPemilik.Nomor,
                            Active = KTPPemilik.Active,
                            ContentType = KTPPemilik.ContentType,
                            File = KTPPemilik.File,
                            TipeDokumen = KTPPemilik.TipeDokumen,
                            MasaBerlaku = KTPPemilik.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    RegDokumenDetail DOMISILI = (RegDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.DOMISILI).FirstOrDefault();
                    if (DOMISILI != null)
                    {
                        DokumenDetail dd = new DokumenDetail()
                        {
                            Nomor = DOMISILI.Nomor,
                            Active = DOMISILI.Active,
                            ContentType = DOMISILI.ContentType,
                            File = DOMISILI.File,
                            TipeDokumen = DOMISILI.TipeDokumen,
                            MasaBerlaku = DOMISILI.MasaBerlaku
                        };
                        ld.Add(dd);
                    }

                    List<RegDokumen> ladd = rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.AKTA).ToList();
                    List<RegAktaDokumenDetail> radd = new List<RegAktaDokumenDetail>();
                    if (ladd != null)
                    {
                        foreach (RegDokumen r in ladd)
                        {
                            RegAktaDokumenDetail rad = (RegAktaDokumenDetail)r;
                            radd.Add(rad);
                        }
                    }
                    RegAktaDokumenDetail akta = radd.Where(x => x.TipeDokumen == EDocumentType.AKTA && x.order == 1).FirstOrDefault();
                    if (akta != null)
                    {
                        AktaDokumenDetail dd = new AktaDokumenDetail()
                        {
                            Nomor = akta.Nomor,
                            Active = akta.Active,
                            ContentType = akta.ContentType,
                            File = akta.File,
                            TipeDokumen = akta.TipeDokumen,
                            Tanggal = akta.Tanggal,
                            Notaris = akta.Notaris,
                            order = akta.order
                        };
                        ld.Add(dd);
                    }
                    akta = radd.Where(x => x.TipeDokumen == EDocumentType.AKTA && x.order != 1).FirstOrDefault();
                    if (akta != null)
                    {
                        AktaDokumenDetail dd = new AktaDokumenDetail()
                        {
                            Nomor = akta.Nomor,
                            Active = akta.Active,
                            ContentType = akta.ContentType,
                            File = akta.File,
                            TipeDokumen = akta.TipeDokumen,
                            Tanggal = akta.Tanggal,
                            Notaris = akta.Notaris,
                            order = akta.order
                        };
                        ld.Add(dd);
                    }
                    RegIzinUsahaDokumenDetail tdp = (RegIzinUsahaDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.TDP).FirstOrDefault();
                    if (tdp != null)
                    {
                        IzinUsahaDokumenDetail dd = new IzinUsahaDokumenDetail()
                        {
                            Nomor = tdp.Nomor,
                            Active = tdp.Active,
                            ContentType = tdp.ContentType,
                            File = tdp.File,
                            TipeDokumen = tdp.TipeDokumen,
                            Instansi = tdp.Instansi,
                            Klasifikasi = tdp.Klasifikasi,
                            Kualifikasi = tdp.Kualifikasi,
                            MasaBerlaku = tdp.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    tdp = (RegIzinUsahaDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.SIUP).FirstOrDefault();
                    if (tdp != null)
                    {
                        IzinUsahaDokumenDetail dd = new IzinUsahaDokumenDetail()
                        {
                            Nomor = tdp.Nomor,
                            Active = tdp.Active,
                            ContentType = tdp.ContentType,
                            File = tdp.File,
                            TipeDokumen = tdp.TipeDokumen,
                            Instansi = tdp.Instansi,
                            Klasifikasi = tdp.Klasifikasi,
                            Kualifikasi = tdp.Kualifikasi,
                            MasaBerlaku = tdp.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    tdp = (RegIzinUsahaDokumenDetail)rv.RegDokumen.Where(x => x.TipeDokumen == EDocumentType.SIUJK).FirstOrDefault();
                    if (tdp != null)
                    {
                        IzinUsahaDokumenDetail dd = new IzinUsahaDokumenDetail()
                        {
                            Nomor = tdp.Nomor,
                            Active = tdp.Active,
                            ContentType = tdp.ContentType,
                            File = tdp.File,
                            TipeDokumen = tdp.TipeDokumen,
                            Instansi = tdp.Instansi,
                            Klasifikasi = tdp.Klasifikasi,
                            Kualifikasi = tdp.Kualifikasi,
                            MasaBerlaku = tdp.MasaBerlaku
                        };
                        ld.Add(dd);
                    }
                    v.Dokumen = ld;
                    v.Owner = Guid.Parse(owner);
                    int ias = _repository.RegisterVerifiedVendor(v);
                    return ias;
                    //return "harusnya sih berhasil, nih id nya:"+ias;
                }
            }
            catch (Exception e)
            {
                //return e.ToString();
            }
            return 0; //"vendor null or wrong status lol";
        }

        //reg doc
        //[Authorize(Roles = IdLdapConstants.Roles.pRole_user)]
        [System.Web.Http.HttpGet]
        public IHttpActionResult ViewFile(string id)
        {
            RegDokumen d = _repository.GetDokumen(new Guid(id));
            if (d != null) return Json(new { id = id, contentType = d.ContentType });
            return Json(new { id = "", content = "" });
        }

        [System.Web.Http.AcceptVerbs("GET", "POST", "HEAD")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage OpenFile(string id)
        {
            RegDokumen d = _repository.GetDokumen(new Guid(id));
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + d.File;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(d.ContentType);

            return result;
        }

        public string GetCaptcha()
        {
            CaptchaHelper c = new CaptchaHelper();
            CaptchaRegistration cr = c.GetCaptcha();
            return cr.Id.ToString();
        }

        public HttpResponseMessage GetCaptchaImage(string k)
        {
            CaptchaHelper c = new CaptchaHelper();
            Guid g;

            if (Guid.TryParse(k, out g))
            {
                var ms = new MemoryStream();
                (c.GenerateImage(g)).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                HttpResponseMessage r = new HttpResponseMessage(HttpStatusCode.OK);
                r.Content = new StreamContent(ms);

                ms.Position = 0;
                r.Content.Headers.ContentLength = ms.Length;
                r.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return r;
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
