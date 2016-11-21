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

namespace Reston.Pinata.WebService
{
    public class ProdukController : BaseController
    {
        private IProdukRepo _repository;
        public ProdukController()
        {
            _repository = new ProdukRepo(new JimbisContext());
        }

        public ProdukController(ProdukRepo repository)
        {
            _repository = repository;
        }

        public IEnumerable<string> Get()
        {
            return new[] { "Produk is so", "more of it", "more" };
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public Produk GetProduk(int id)
        {
            return _repository.GetProduk(id) ?? null;
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public IHttpActionResult GetAllProduk(int draw, int length, int start, string name, string region, string kategori,string klasifikasi)
        {
            //System.Web.HttpContext.Current.Request["a"].ToString();
            int total = _repository.GetAllProduk().Count();
            List<Produk> lp = _repository.GetProduks(name, region, kategori,klasifikasi);
            lp = lp.Where(x => (region == null || (x.RiwayatHarga.LastOrDefault() != null && x.RiwayatHarga.LastOrDefault().Region == region))).ToList();
            List<ProdukSummaryViewModel> lsm = (from a in lp
                                                where (name == null || a.Nama.ToLower().Contains(name.ToLower()))
                                                select new ProdukSummaryViewModel()
                                                {
                                                    Id = a.Id,
                                                    Nama = a.Nama,
                                                    Klasifikasi=a.Klasifikasi.ToString(),
                                                    Price = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Harga : 0,
                                                    Region = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Region : "",
                                                    LastUpdate = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Tanggal.ToLocalTime().ToShortDateString() : "",
                                                    Source = a.RiwayatHarga.LastOrDefault() != null ? a.RiwayatHarga.LastOrDefault().Sumber : "",
                                                    Satuan = a.Satuan
                                                }).Skip(start).Take(length).ToList();
            return Json(new { aaData = lsm, recordsTotal = total, recordsFiltered = lp.Count });
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public List<string> GetAllKategori()
        {
            return _repository.GetAllKategori().Select(x => x.Nama).Distinct().ToList();
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        [HttpGet]
        public void DeleteProduk(int? id)
        {
            if (id != null)
            {
                _repository.DeleteProduk((int)id);
            }
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public int SaveProduk(ProdukViewModel model)
        {
            if (model == null)
                return 0;
            Produk p = new Produk()
            {
                Nama = model.Nama,
                Deskripsi = model.Deskripsi,
                Satuan = model.Satuan,
                Klasifikasi=model.Klasifikasi
            };

            KategoriSpesifikasi ks = new KategoriSpesifikasi()
            {
                Nama = model.Spesifikasi.NamaKategori
            };

            List<AtributSpesifikasi> la = new List<AtributSpesifikasi>();
            if (model.Spesifikasi.DaftarAtribut != null)
            {
                foreach (AtributSpesifikasiViewModel sp in model.Spesifikasi.DaftarAtribut)
                {
                    AtributSpesifikasi a = new AtributSpesifikasi()
                    {
                        Nama = sp.NamaAtribut,
                        Nilai = sp.Nilai
                    };
                    la.Add(a);
                }
                ks.AtributSpesifikasi = la;
            }
            p.KategoriSpesifikasi = ks;

            //save produk
            _repository.SaveProduk(p);

            //processing new template category
            if (model.Spesifikasi.NamaKategori != null && model.Spesifikasi.DaftarAtribut != null)
            { //create new template
                KategoriSpesifikasi nk = new KategoriSpesifikasi()
                {
                    Nama = model.Spesifikasi.NamaKategori,
                    Deskripsi = ProdukRepo.KATEGORI_TAMPLATE_STRING
                };
                List<AtributSpesifikasi> nla = new List<AtributSpesifikasi>();
                foreach (AtributSpesifikasiViewModel sp in model.Spesifikasi.DaftarAtribut)
                {
                    AtributSpesifikasi na = new AtributSpesifikasi()
                    {
                        Nama = sp.NamaAtribut
                    };
                    nla.Add(na);
                }
                nk.AtributSpesifikasi = nla;
                //save kategori
                _repository.SaveKategori(nk);
            }
            return p.Id;
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public List<KategoriSpesifikasi> GetTemplateKategoriSpesifikasi()
        {
            return _repository.GetTemplateKategoriSpesifikasi() ?? null;
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public ProdukViewModel GetDetailProduk(int id)
        {
            Produk p = _repository.GetProduk(id);
            //KategoriSpesifikasi s = _repository.GetKategoriSpesifikasiByProduk(id);
            ProdukViewModel pvm = new ProdukViewModel()
            {
                Nama = p.Nama,
                id = p.Id,
                Deskripsi = p.Deskripsi,
                Satuan = p.Satuan,
                HargaTerakhir = p.RiwayatHarga.LastOrDefault() != null ? p.RiwayatHarga.LastOrDefault().Harga : 0,
                Currency = p.RiwayatHarga.LastOrDefault() != null ? p.RiwayatHarga.LastOrDefault().Currency : "",
                DefaultRegion = p.RiwayatHarga.LastOrDefault() != null ? p.RiwayatHarga.LastOrDefault().Region : ""
                //Spesifikasi = p.KategoriSpesifikasi
            };
            if (p.KategoriSpesifikasi != null)
            {
                KategoriSpesifikasiViewModel ks = new KategoriSpesifikasiViewModel()
                {
                    NamaKategori = p.KategoriSpesifikasi.Nama,
                    id = p.KategoriSpesifikasi.Id
                };
                List<AtributSpesifikasiViewModel> lam = new List<AtributSpesifikasiViewModel>();
                foreach (AtributSpesifikasi a in p.KategoriSpesifikasi.AtributSpesifikasi)
                {
                    AtributSpesifikasiViewModel na = new AtributSpesifikasiViewModel()
                    {
                        NamaAtribut = a.Nama,
                        Nilai = a.Nilai
                    };
                    lam.Add(na);
                }
                ks.DaftarAtribut = lam;
                pvm.Spesifikasi = ks;
            }
            return pvm;
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public KategoriSpesifikasi GetKategoriSpesifikasiByProduk(int id)
        {
            return _repository.GetKategoriSpesifikasiByProduk(id);
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public IHttpActionResult GetRiwayatHarga(int id)
        {
            var rh = _repository.GetRiwayatHarga(id, null);
            List<RiwayatHargaViewModel> rvm = (from a in rh
                                               select new RiwayatHargaViewModel()
                                               {
                                                   Tanggal = a.Tanggal.ToLocalTime().ToShortDateString().Substring(0, 10),
                                                   Harga = a.Harga,
                                                   Currency = a.Currency,
                                                   Sumber = a.Sumber,
                                                   User = a.User
                                               }).ToList();
            return Json(new { aaData = rh });
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public RiwayatHargaTabelViewModel GetRiwayatHarga2(int id, string region, bool desc = false)
        {
            List<RiwayatHarga> rh = _repository.GetRiwayatHarga(id, region);
            if (desc)
                rh = rh.OrderByDescending(x => x.Tanggal).ToList();
            List<RiwayatHargaViewModel> rvm = (from a in rh
                                               select new RiwayatHargaViewModel()
                                               {
                                                   Tanggal = a.Tanggal.ToString("yyyy-MM-dd"),
                                                   Harga = a.Harga,
                                                   Currency = a.Currency,
                                                   Sumber = a.Sumber,
                                                   User = a.User
                                               }).ToList();
            return new RiwayatHargaTabelViewModel() { aaData = rvm };
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public void TambahHarga(RiwayatHargaViewModel model)
        {
            Produk p = _repository.GetProduk(model.IdProduk);
            RiwayatHarga rh = new RiwayatHarga()
            {
                Harga = model.Harga,
                Currency = model.Currency,
                Region = model.Region,
                Sumber = model.Sumber,
                Tanggal = DateTime.Now,
                User = CurrentUser.UserName
            };
            //different region creating new instance of produk 
            if (p.RiwayatHarga.LastOrDefault() != null && p.RiwayatHarga.LastOrDefault().Region != model.Region)
            {

            }
            p.RiwayatHarga.Add(rh);
            _repository.Save();
            //return Json(true);
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public KategoriSpesifikasi GetKategoriSpesifikasi(int id)
        {
            return _repository.GetKategoriSpesifikasi(id);
        }

        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager })]
        public List<AtributSpesifikasi> GetDaftarAtributSpesifikasi(int id)
        {//id kategori
            return _repository.GetDaftarAtributSpesifikasi(id);
        }

        public List<string> GetAllRegionDetail(int id)
        {
            Produk p = _repository.GetProduk(id);
            if (p != null)
            {
                return p.RiwayatHarga.Select(x => x.Region).Distinct().ToList();
            }
            return new List<string>();
        }
    }
}