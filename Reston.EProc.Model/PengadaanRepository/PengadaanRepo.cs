﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Model.Helper;
using Reston.Pinata.Model.PengadaanRepository.View;
using System.Configuration;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.Helper;

namespace Reston.Pinata.Model.PengadaanRepository
{
    public interface IPengadaanRepo
    {
        ViewPengadaan GetPengadaan(Guid id, Guid UserID, int approver);

        DataPagePengadaan GetPengadaans(string search, int start, int limit, Guid? UserId, List<string> Roles, EGroupPengadaan groupstatus, List<Guid> lstMenejer, List<Guid> listHead);
        List<ViewPengadaan> GetPerhatianWorkflow(string search, int start, int limit, Guid? UserId, List<Reston.Helper.Model.ViewWorkflowModel> lstDocument);
        List<Pengadaan> GetAllPengadaan();

        Pengadaan AddPengadaan(Pengadaan pengadaan, Guid UserId, List<Guid> manager);
        //
        List<VWRKSDetail> getRKS(Guid id);
        List<VWRKSDetailRekanan> getRKSForRekanan(Guid id, Guid UserId);
        RKSHeader saveRks(RKSHeader rks, Guid UserId);
        VWRKSHeaderPengadaan GetRKSHeaderPengadaan(Guid id);
        //
        void Save();
        Pengadaan Persetujuan(Guid? UserId, Guid id, int approver);
        Pengadaan Penolakan(Guid? UserId, VWPenolakan vwPenolakan, int approver);
        List<ViewPengadaan> GetPerhatian(string search, int start, int limit, Guid? UserId, List<string> Roles, int approver, List<Guid> lstManager);

        DokumenPengadaan GetDokumenPengadaan(Guid Id);
        DokumenPengadaan saveDokumenPengadaan(DokumenPengadaan dokumenPengadaan, Guid UserId);
        List<VWDokumenPengadaan> GetListDokumenPengadaan(TipeBerkas tipe, Guid Id, Guid UserId);
        int deleteDokumen(Guid Id);
        KandidatPengadaan saveKandidatPengadaan(KandidatPengadaan kandidat, Guid UserId);
        int deleteKandidatPengadaan(Guid Id, Guid UserId);
        int deleteJadwalPengadaan(Guid Id, Guid UserId);
        JadwalPengadaan saveJadwalPengadaan(JadwalPengadaan Jadwal, Guid UserId);
        PersonilPengadaan savePersonilPengadaan(PersonilPengadaan Personil, Guid UserId);
        int deletePersonilPengadaan(Guid Id, Guid UserId);
        List<VWKandidatPengadaan> getListKandidatPengadaan(Guid PengadaanId);
        List<JadwalPengadaan> getListJadwalPengadaan(Guid PengadaanId);
        List<PersonilPengadaan> getListPersonilPengadaan(Guid PengadaanId);
        int DeletePengadaan(Guid Id, Guid UserId);
        int arsipkan(Guid Id, Guid UserId);

        JadwalPelaksanaan getPelaksanaanPendaftaran(Guid PengadaanId);

        JadwalPelaksanaan getPelaksanaanAanwijing(Guid PengadaanId);
        JadwalPelaksanaan addPelaksanaanAanwijing(JadwalPelaksanaan pelaksanaanAanwijzing, Guid UserId);
        List<VWKehadiranKandidatAanwijzing> getKehadiranAanwijzings(Guid PengadaanId);
        KehadiranKandidatAanwijzing addKehadiranAanwijzing(Guid Id, Guid UserId);
        int DeleteKehadiranAanwijzing(Guid Id, Guid UserId);
        JadwalPelaksanaan AddPelaksanaanSubmitPenawaran(JadwalPelaksanaan pelaksanaanSubmitPenawaran, Guid UserId);
        JadwalPelaksanaan getPelaksanaanSubmitPenawaran(Guid PengadaanId);
        JadwalPelaksanaan AddPelaksanaanBukaAmplop(JadwalPelaksanaan pelaksanaanBukaAmplop, Guid UserId);
        JadwalPelaksanaan getPelaksanaanBukaAmplop(Guid PengadaanId);
        PersetujuanBukaAmplop AddPersetujuanBukaAmplop(Guid PengadaanId, Guid UserId);
        List<VWPErsetujuanBukaAmplop> getPersetujuanBukaAmplop(Guid PengadaanId, Guid UserId);
        int deleteDokumenPelaksanaan(Guid Id, Guid UserId, int isApprovel);
        int deleteDokumenRekanan(Guid Id, Guid UserId);
        int UpdateStatus(Guid Id, EStatusPengadaan status);
        int cekStateDiSetujui(Guid PengadaanId);
        int AjukanPengadaan(Guid Id, Guid UserId, List<Guid> manager);
        int cekStateAanwijzing(Guid PengadaanId);
        int cekStateSubmitPenawaran(Guid PengadaanId);
        int cekStateBukaAmplop(Guid PengadaanId);

        int nextToState(Guid PengadaanId, Guid UserId, EStatusPengadaan state);

        KualifikasiKandidat addKualifikasiKandidat(KualifikasiKandidat dKualifikasiKandidat, Guid UserId);
        int deleteKualifikasiKandidat(Guid Id, Guid UserId);
        List<ViewPengadaan> GetPengadaansForRekanan(int start, int limit, Guid? UserId, List<string> Roles, EGroupPengadaan groupstatus);
        ViewPengadaan GetPengadaanForRekanan(Guid id, Guid UserId);
        List<VWRKSDetailRekanan> addHargaRekanan(List<VWRKSDetailRekanan> dlstHargaRekanan, Guid PengadaanId, Guid UserId);
        List<VWRekananSubmitHarga> getListRekananSubmit(Guid PengadaanId, Guid UserId);
        List<VWRekananPenilaian> getListRekananPenilaian(Guid PengadaanId, Guid UserId);
        JadwalPelaksanaan AddPelaksanaanPenilaian(JadwalPelaksanaan pelaksanaanPenilaian, Guid UserId);
        JadwalPelaksanaan getPelaksanaanPenilaian(Guid PengadaanId, Guid UserId);
        VWRKSVendors getRKSPenilaian(Guid PengadaanId, Guid UserId);
        PelaksanaanPemilihanKandidat addKandidatPilihan(PelaksanaanPemilihanKandidat oPelaksanaanPemilihanKandidat, Guid UserId);
        int deleteKandidatPilihan(PelaksanaanPemilihanKandidat oPelaksanaanPemilihanKandidat, Guid UserId);
        JadwalPelaksanaan AddPelaksanaanKlarifikasi(JadwalPelaksanaan pelaksanaanKlarifikasi, Guid UserId);
        JadwalPelaksanaan getPelaksanaanKlarifikasi(Guid PengadaanId, Guid UserId);
        JadwalPelaksanaan AddPelaksanaanPemenang(JadwalPelaksanaan pelaksanaanPemenang, Guid UserId);
        JadwalPelaksanaan getPelaksanaanPemenang(Guid PengadaanId, Guid UserId);

        List<VWRKSDetailRekanan> addHargaKlarifikasiRekanan(List<VWRKSDetailRekanan> dlstHargaKlarifikasiRekanan, Guid PengadaanId, Guid UserId);
        List<VWRKSDetailRekanan> getRKSForKlarifikasiRekanan(Guid id, Guid UserId);
        VWRKSVendors getRKSKlarifikasi(Guid PengadaanId, Guid UserId);
        List<VWRekananPenilaian> getListRekananKlarifikasiPenilaian(Guid PengadaanId, Guid UserId);
        List<VWRekananSubmitHarga> getListRekananKlarifikasiSubmit(Guid PengadaanId, Guid UserId);
        VWRKSVendors getRKSKlarifikasiPenilaianVendor(Guid PengadaanId, Guid UserId, int VendorId);
        VWRKSVendors getRKSKlarifikasiPenilaianVendor2(Guid PengadaanId, Guid UserId, int VendorId);

        List<vwProduk> GetAllProduk(string term);
        List<vwProduk> GetItemByRegion(string term,string region);
        List<vwProduk> GetAllSatuan(string term);
        dataVendors GetVendors(ETipeVendor tipe, int start, string filter, EStatusVendor status, int limit);
        RKSHeader AddTotalHps(Guid PengadaanId, decimal Total, Guid UserId);
        RKSHeader GetTotalHps(Guid PengadaanId, Guid UserId);
        List<VWVendor> GetVendorsByPengadaanId(Guid PengadaanId);
        VWRKSVendors getRKSKlarifikasiPenilaian(Guid PengadaanId, Guid UserId);
        List<VWRKSDetail> getRKSDetails(Guid PengadaanId, Guid UserId);
        List<VWDokumenPengadaan> GetListDokumenVendor(TipeBerkas tipe, Guid Id, Guid UserId, int VendorId);
        List<VWPembobotanPengadaan> getKriteriaPembobotan(Guid PengadaanId);
        int addPembobtanPengadaan(PembobotanPengadaan dataPembobotanPengadaan, Guid UserId);
        List<PembobotanPengadaan> getPembobtanPengadaan(Guid PengadaanId, Guid UserId);
        List<VWPembobotanPengadaanVendor> getPembobtanPengadaanVendor(Guid PengadaanId, int VendorId, Guid UserId);
        int addLstPembobtanPengadaan(List<PembobotanPengadaan> dataLstPembobotanPengadaan, Guid UserId);
        int addLstPenilaianKriteriaVendor(List<PembobotanPengadaanVendor> dataLstPenilaianKriteriaVendor, Guid UserId);
        string statusVendor(Guid PengadaanId, Guid UserId);

        List<VWRekananPenilaian> getListPenilaianByVendor(Guid PengadaanId, Guid UserId, int VendorId);
        VWRKSVendors getRKSPenilaian2Report(Guid PengadaanId, Guid UserId);
        int addPemenangPengadaan(PemenangPengadaan dtPemenangPengadaan, Guid UserId);
        List<VWRekananPenilaian> getPemenangPengadaan(Guid PengadaanId, Guid UserId);
        List<VWRekananPenilaian> getKandidatPengadaan(Guid PengadaanId, Guid UserId);
        List<VWVendor> GetVendorsKlarifikasiByPengadaanId(Guid PengadaanId);
        BeritaAcara addBeritaAcara(BeritaAcara newBeritaAcara, Guid UserId);
        List<BeritaAcara> getBeritaAcara(Guid PengadaanId, Guid UserId);
        BeritaAcara getBeritaAcaraByTipe(Guid PengadaanId, TipeBerkas tipe, Guid UserId);
        int CekBukaAmplop(Guid PengadaanId);
        ViewVendors GetVendorById(int VendorId);
        List<VWReportPengadaan> GetRepotPengadan(DateTime? dari, DateTime? sampai, Guid UserId);
        int PembatalanPengadaan(VWPembatalanPengadaan vwPembatalan, Guid UserId);
        List<VWStaffCharges> GetSummaryTotal(DateTime dari, DateTime sampai, int limit = Int32.MaxValue, int skip = 0);
        List<VWStaffCharges> GetStaffCharges(string charge, DateTime dari, DateTime sampai, int limit = Int32.MaxValue, int skip = 0);
        List<VWProgressReport> GetProgressReport(DateTime dari, DateTime sampai, int limit = Int32.MaxValue, int skip = 0);
        int isNotaUploaded(Guid PengadaanId, Guid UserId);
        int isSpkUploaded(Guid PengadaanId, Guid UserId);

        PenolakanPengadaan GetPenolakanMessage(Guid Id, Guid userId);
        PembatalanPengadaan GetPembatalanPengadaan(Guid Id, Guid userId);
        Vendor GetPemenang(Guid Id, Guid userId);


        ResultMessage TolakPengadaan(Guid Id);
        RiwayatDokumen AddRiwayatDokumen(RiwayatDokumen nRiwayatDokumen);
        List<RiwayatDokumen> lstRiwayatDokumen(Guid Id);
        Pengadaan ChangeStatusPengadaan(Guid Id, EStatusPengadaan status,Guid UserId);
        int nextToStateWithChangeScheduldDate(Guid PengadaanId, Guid UserId, EStatusPengadaan state, DateTime from, DateTime? to);
        List<PersonilPengadaan> getPersonilPengadaan(Guid PengadaanId);
        List<VWRiwayatPengadaan> GetRiwayatDokumenForVendor(Guid UserId);
        //workflow
       // Pengadaan PersetujuanWorkflow(Guid Id, Guid UserId);
        //int AjukanWorkflow(Guid Id, Guid UserId, Guid WorkflowtemplateId);
    }
    public class PengadaanRepo : IPengadaanRepo
    {
        JimbisContext ctx;
        public PengadaanRepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        public int arsipkan(Guid Id, Guid UserId)
        {
            int ispic = (from xx in ctx.PersonilPengadaans
                         where xx.PengadaanId == Id && xx.tipe == "pic" && xx.PersonilId == UserId
                         select xx).Count() > 0 ? 1 : 0;
            if (ispic == 0) return 0;
            Pengadaan oPengadaan = ctx.Pengadaans.Find(Id);
            if (oPengadaan == null) return 0;
            oPengadaan.GroupPengadaan = EGroupPengadaan.ARSIP;
            ctx.SaveChanges();
            return 1;
        }

        public List<vwProduk> GetAllProduk(string term)
        {
            var oProduk = ctx.Produks.Where(d => d.Nama.Contains(term)).Take(15).ToList();

            List<vwProduk> LstnVwProduk = new List<vwProduk>();
            foreach (var item in oProduk)
            {
                vwProduk nVwProduk = new vwProduk();
                var Keterangan = "";
                nVwProduk.Id = item.Id;
                nVwProduk.Nama = item.Nama;
                nVwProduk.Satuan = item.Satuan;
                nVwProduk.Deskripsi = item.Deskripsi;
                nVwProduk.RiwayatHarga = item.RiwayatHarga.ToList();
                if (item.KategoriSpesifikasi != null)
                {
                    var oSpesifikasi = item.KategoriSpesifikasi.AtributSpesifikasi.ToList();
                    if (oSpesifikasi.Count() > 0)
                    {
                        var firstSpek = oSpesifikasi.FirstOrDefault();
                        var lasttSpek = oSpesifikasi.LastOrDefault();
                        foreach (var spek in oSpesifikasi)
                        {
                            if (spek == firstSpek)
                                Keterangan = spek.Nama + ": " + spek.Nilai + ", ";
                            else if (spek == lasttSpek) Keterangan = Keterangan + spek.Nama + ": " + spek.Nilai;
                            else Keterangan = Keterangan + ", " + spek.Nama + ": " + spek.Nilai + ", ";

                        }
                    }
                }
                nVwProduk.Spesifikasi = Keterangan;
                LstnVwProduk.Add(nVwProduk);
            }
            return LstnVwProduk;
        }

        public List<vwProduk> GetItemByRegion(string term,string region)
        {
            var oProduk = ctx.Produks.Where(d => d.Nama.Contains(term)).Take(15).ToList();

            List<vwProduk> LstnVwProduk = new List<vwProduk>();
            foreach (var item in oProduk)
            {
                vwProduk nVwProduk = new vwProduk();
                var Keterangan = "";
                nVwProduk.Id = item.Id;
                nVwProduk.Nama = item.Nama;
                nVwProduk.Satuan = item.Satuan;
                nVwProduk.Deskripsi = item.Deskripsi;
                nVwProduk.RiwayatHarga = item.RiwayatHarga.Where(xx=>xx.Region==region).ToList();
                if (item.KategoriSpesifikasi != null)
                {
                    var oSpesifikasi = item.KategoriSpesifikasi.AtributSpesifikasi.ToList();
                    if (oSpesifikasi.Count() > 0)
                    {
                        var firstSpek = oSpesifikasi.FirstOrDefault();
                        var lasttSpek = oSpesifikasi.LastOrDefault();
                        foreach (var spek in oSpesifikasi)
                        {
                            if (spek == firstSpek)
                                Keterangan = spek.Nama + ": " + spek.Nilai + ", ";
                            else if (spek == lasttSpek) Keterangan = Keterangan + spek.Nama + ": " + spek.Nilai;
                            else Keterangan = Keterangan + ", " + spek.Nama + ": " + spek.Nilai + ", ";

                        }
                    }
                }
                nVwProduk.Spesifikasi = Keterangan;
                LstnVwProduk.Add(nVwProduk);
            }
            return LstnVwProduk;
        }
        
        public List<vwProduk> GetAllSatuan(string term)
        {
            return ctx.Produks.Where(d => d.Satuan.Contains(term)).Select(d => new vwProduk { Satuan = d.Satuan }).Distinct().Take(15).ToList();
        }

        public dataVendors GetVendors(ETipeVendor tipe, int start, string filter, EStatusVendor status, int limit)
        {
            if (limit > 0)
            {
                dataVendors odataVendors = new dataVendors();
                var oData =
                ctx.Vendors.Where(x => (tipe == ETipeVendor.NONE || x.TipeVendor == tipe)
                    && (status == EStatusVendor.NONE || x.StatusAkhir == status) && x.StatusAkhir != EStatusVendor.BLACKLIST);
                odataVendors.totalRecord = oData.Count();
                if (!string.IsNullOrEmpty(filter)) oData = oData.Where(d => d.Nama.Contains(filter));
                List<ViewVendors> lv = oData.Select(d => new ViewVendors
                {
                    Nama = d.Nama,
                    id = d.Id,
                    Telepon = d.Telepon,
                    Alamat = d.Alamat
                }).OrderByDescending(x => x.id).Skip(start).Take(limit)
                    .ToList();
                odataVendors.Vendors = lv;
                return odataVendors;
            }
            return new dataVendors();
        }

        public List<VWVendor> GetVendorsByPengadaanId(Guid PengadaanId)
        {
            var xx = (from b in ctx.Vendors
                      join c in ctx.KandidatPengadaans on b.Id equals c.VendorId
                      where c.PengadaanId == PengadaanId
                      select new VWVendor
                      {
                          email = (from bb in ctx.Vendors
                                   where bb.Id == c.VendorId
                                   select bb).FirstOrDefault() != null ? (from bb in ctx.Vendors
                                                                          where bb.Id == c.VendorId
                                                                          select bb).FirstOrDefault().Email : "",
                          Nama = (from bb in ctx.Vendors
                                  where bb.Id == c.VendorId
                                  select bb).FirstOrDefault() != null ? (from bb in ctx.Vendors
                                                                         where bb.Id == c.VendorId
                                                                         select bb).FirstOrDefault().Nama : ""
                      }).Distinct().ToList();
            return xx;

        }

        public List<VWVendor> GetVendorsKlarifikasiByPengadaanId(Guid PengadaanId)
        {
            var xx = (from b in ctx.Vendors
                      join c in ctx.PelaksanaanPemilihanKandidats on b.Id equals c.VendorId
                      where c.PengadaanId == PengadaanId
                      select new VWVendor
                      {
                          email = (from bb in ctx.Vendors
                                   where bb.Id == c.VendorId
                                   select bb).FirstOrDefault() != null ? (from bb in ctx.Vendors
                                                                          where bb.Id == c.VendorId
                                                                          select bb).FirstOrDefault().Email : "",
                          Nama = (from bb in ctx.Vendors
                                  where bb.Id == c.VendorId
                                  select bb).FirstOrDefault() != null ? (from bb in ctx.Vendors
                                                                         where bb.Id == c.VendorId
                                                                         select bb).FirstOrDefault().Nama : ""
                      }).Distinct().ToList();
            return xx;

        }

        public ViewPengadaan GetPengadaan(Guid id, Guid UserID, int approver)
        {
            // Guid manajer = new Guid(ConfigurationManager.AppSettings["manajer"].ToString());
            // int approver = UserID == manajer ? 1 : 0;
            int ispic = (from aa in ctx.PersonilPengadaans
                         where aa.PengadaanId == id && aa.tipe == "pic" && aa.PersonilId == UserID
                         select aa).Count() > 0 ? 1 : 0;
            int isteam = (from bb in ctx.PersonilPengadaans
                          where bb.PengadaanId == id && (bb.tipe == "tim" || bb.tipe == "pic") && bb.PersonilId == UserID
                          select bb).Count() > 0 ? 1 : 0;
            int isPersonil = (from cc in ctx.PersonilPengadaans
                              where cc.PengadaanId == id && cc.PersonilId == UserID
                              select cc).Count() > 0 ? 1 : 0;
            int isController = (from aa in ctx.PersonilPengadaans
                                where aa.PengadaanId == id && aa.tipe == "controller" && aa.PersonilId == UserID
                                select aa).Count() > 0 ? 1 : 0;
            int isCompliance = (from aa in ctx.PersonilPengadaans
                                where aa.PengadaanId == id && aa.tipe == "compliance" && aa.PersonilId == UserID
                                select aa).Count() > 0 ? 1 : 0;
            int isUser = (from aa in ctx.PersonilPengadaans
                          where aa.PengadaanId == id && aa.tipe == "staff" && aa.PersonilId == UserID
                          select aa).Count() > 0 ? 1 : 0;
            ViewPengadaan oVWPengadaan = (from b in ctx.Pengadaans
                                          where b.Id == id
                                          select new ViewPengadaan
                                          {
                                              Approver = b.Status == EStatusPengadaan.AJUKAN ? approver : 0,
                                              Id = b.Id,
                                              Judul = b.Judul,
                                              AturanBerkas = b.AturanBerkas,
                                              AturanPenawaran = b.AturanPenawaran,
                                              AturanPengadaan = b.AturanPengadaan,
                                              Keterangan = b.Keterangan,
                                              Status = b.Status,
                                              GroupPengadaan = b.GroupPengadaan,
                                              JenisPekerjaan = b.JenisPekerjaan,
                                              JenisPembelanjaan = b.JenisPembelanjaan,
                                              MataUang = b.MataUang,
                                              PeriodeAnggaran = b.PeriodeAnggaran,
                                              Pagu=b.Pagu,
                                              Region = b.Region,
                                              Provinsi = b.Provinsi,
                                              KualifikasiRekan = b.KualifikasiRekan,
                                              UnitKerjaPemohon = b.UnitKerjaPemohon,
                                              TitleDokumenNotaInternal = b.TitleDokumenNotaInternal,
                                              TitleDokumenLain = b.TitleDokumenLain,
                                              TitleBerkasRujukanLain = b.TitleBerkasRujukanLain,
                                              isCreated = UserID == b.CreatedBy ? 1 : 0,
                                              isPIC = ispic,
                                              isTEAM = isteam,
                                              isPersonil = isPersonil,
                                              isCompliance = isCompliance,
                                              isController = isController,
                                              isUser = isUser,
                                              NoPengadaan = b.NoPengadaan,
                                              PersonilPengadaans = (from bb in ctx.PersonilPengadaans
                                                                    where bb.PengadaanId == b.Id
                                                                    select new VWPersonilPengadaan
                                                                    {
                                                                        Id = bb.Id,
                                                                        Jabatan = bb.Jabatan,
                                                                        Nama = bb.Nama,
                                                                        PersonilId = bb.PersonilId,
                                                                        tipe = bb.tipe
                                                                    }).ToList(),
                                              KandidatPengadaans = (from bb in ctx.KandidatPengadaans
                                                                    join cc in ctx.Vendors on bb.VendorId equals cc.Id
                                                                    where bb.PengadaanId == b.Id
                                                                    select new VWKandidatPengadaan
                                                                    {
                                                                        Id = bb.Id,
                                                                        PengadaanId = bb.PengadaanId,
                                                                        VendorId = bb.VendorId,
                                                                        Nama = cc.Nama
                                                                    }).ToList(),
                                              JadwalPengadaans = (from bb in ctx.JadwalPengadaans
                                                                  where bb.PengadaanId == b.Id
                                                                  select new VWJadwalPengadaan
                                                                  {
                                                                      Id = bb.Id,
                                                                      PengadaanId = bb.PengadaanId,
                                                                      Mulai = bb.Mulai,
                                                                      Sampai = bb.Sampai,
                                                                      tipe = bb.tipe
                                                                  }).ToList(),
                                              DokumenPengadaans = (from bb in ctx.DokumenPengadaans
                                                                   where bb.PengadaanId == b.Id
                                                                   select new VWDokumenPengadaan
                                                                   {
                                                                       Id = bb.Id,
                                                                       PengadaanId = bb.PengadaanId,
                                                                       ContentType = bb.ContentType,
                                                                       File = bb.File,
                                                                       Tipe = bb.Tipe,
                                                                       Title = bb.Title
                                                                   }).ToList(),

                                              KualifikasiKandidats = (from bb in ctx.KualifikasiKandidats
                                                                      where bb.PengadaanId == b.Id
                                                                      select new VWKualifikasiKandidat
                                                                      {
                                                                          Id = bb.Id,
                                                                          PengadaanId = bb.PengadaanId,
                                                                          kualifikasi = bb.kualifikasi
                                                                      }).ToList()


                                          }).FirstOrDefault();
            if (oVWPengadaan.Status == EStatusPengadaan.DISETUJUI)
            {
                if (cekStateDiSetujui(oVWPengadaan.Id) == 1)
                {
                    oVWPengadaan.Status = EStatusPengadaan.AANWIJZING;
                }

            }
            //if (oVWPengadaan.Status == EStatusPengadaan.AANWIJZING)
            //{
            //    if (cekStateAanwijzing(oVWPengadaan.Id) == 1)
            //    {
            //        oVWPengadaan.Status = EStatusPengadaan.SUBMITPENAWARAN;
            //    }
            //}
            //if (oVWPengadaan.Status == EStatusPengadaan.SUBMITPENAWARAN)
            //{
            //    if (cekStateSubmitPenawaran(oVWPengadaan.Id) == 1)
            //    {
            //        oVWPengadaan.Status = EStatusPengadaan.BUKAAMPLOP;
            //    }
            //}
            //if (oVWPengadaan.Status == EStatusPengadaan.BUKAAMPLOP)
            //{
            //    if (cekStateBukaAmplop(oVWPengadaan.Id) == 1)
            //    {
            //        oVWPengadaan.Status = EStatusPengadaan.PENILAIAN;
            //    }
            //}

            //if (oVWPengadaan.Status == EStatusPengadaan.PENILAIAN)
            //{
            //    if (cekStatePenilaian(oVWPengadaan.Id) == 1)
            //    {
            //        oVWPengadaan.Status = EStatusPengadaan.KLARIFIKASI;
            //    }
            //}

            //if (oVWPengadaan.Status == EStatusPengadaan.KLARIFIKASI)
            //{
            //    if (cekStateKlarifikasi(oVWPengadaan.Id) == 1)
            //    {
            //        oVWPengadaan.Status = EStatusPengadaan.PEMENANG;
            //    }
            //}

            return oVWPengadaan;
        }

        public ViewPengadaan GetPengadaanForRekanan(Guid id, Guid UserId)
        {
            int isMasukKlarifikasi = 0;
            Vendor oVendor = ctx.Vendors.Where(d => d.Owner == UserId).FirstOrDefault();
            ViewPengadaan VWPengadaan = (from b in ctx.Pengadaans
                                         where b.Id == id
                                         select new ViewPengadaan
                                         {
                                             Id = b.Id,
                                             Judul = b.Judul,
                                             AturanBerkas = b.AturanBerkas,
                                             AturanPenawaran = b.AturanPenawaran,
                                             AturanPengadaan = b.AturanPengadaan,
                                             Keterangan = b.Keterangan,
                                             Status = b.Status,
                                             GroupPengadaan = b.GroupPengadaan,
                                             JenisPekerjaan = b.JenisPekerjaan,
                                             JenisPembelanjaan = b.JenisPembelanjaan,
                                             MataUang = b.MataUang,
                                             PeriodeAnggaran = b.PeriodeAnggaran,
                                             Region = b.Region,
                                             Provinsi = b.Provinsi,
                                             KualifikasiRekan = b.KualifikasiRekan,
                                             UnitKerjaPemohon = b.UnitKerjaPemohon,
                                             JadwalPengadaans = (from bb in ctx.JadwalPengadaans
                                                                 where bb.PengadaanId == b.Id
                                                                 select new VWJadwalPengadaan
                                                                 {
                                                                     Id = bb.Id,
                                                                     PengadaanId = bb.PengadaanId,
                                                                     Mulai = bb.Mulai,
                                                                     Sampai = bb.Sampai,
                                                                     tipe = bb.tipe
                                                                 }).ToList(),
                                             KualifikasiKandidats = (from bb in ctx.KualifikasiKandidats
                                                                     where bb.PengadaanId == b.Id
                                                                     select new VWKualifikasiKandidat
                                                                     {
                                                                         Id = bb.Id,
                                                                         PengadaanId = bb.PengadaanId,
                                                                         kualifikasi = bb.kualifikasi
                                                                     }).ToList(),
                                             isMasukKlarifikasi = (from bb in ctx.PelaksanaanPemilihanKandidats
                                                                   where bb.PengadaanId == b.Id
                                                                   & bb.VendorId == oVendor.Id
                                                                   select bb).FirstOrDefault() == null ? 0 : 1

                                         }).FirstOrDefault();
            if (VWPengadaan.Status == EStatusPengadaan.DISETUJUI)
            {
                if (cekStateDiSetujui(VWPengadaan.Id) == 1)
                {
                    VWPengadaan.Status = EStatusPengadaan.AANWIJZING;
                }

            }
            //if (VWPengadaan.Status == EStatusPengadaan.SUBMITPENAWARAN)
            //{
            //    if (cekStateSubmitPenawaran(VWPengadaan.Id) == 1)
            //    {
            //        VWPengadaan.Status = EStatusPengadaan.BUKAAMPLOP;
            //    }
            //}

            return VWPengadaan;
        }

        public DataPagePengadaan GetPengadaans(string search, int start, int limit, Guid? UserId, List<string> Roles, EGroupPengadaan groupstatus, List<Guid> lstMenejer, List<Guid> lstHead)
        {
            // Guid manajer =new Guid( ConfigurationManager.AppSettings["manajer"].ToString());
            search = search == null ? "" : search;
            DataPagePengadaan dataPagePengadaan = new DataPagePengadaan();
            if (limit > 0)
            {
                var VWPengadaans = (from b in ctx.Pengadaans
                                    join c in ctx.PersonilPengadaans on b.Id equals c.PengadaanId into ps
                                    from c in ps.DefaultIfEmpty()
                                    where b.GroupPengadaan == groupstatus &&
                                        //(c.PersonilId == UserId
                                        //|| c.tipe == "tim" || c.tipe == "pic" || (c.tipe=="tim"||c.tipe=="pic")
                                    (lstMenejer.Contains(UserId.Value) || (c.PersonilId == UserId) || b.CreatedBy == UserId || lstHead.Contains(UserId.Value)) //UserId == manajer)
                                    && (b.Judul.Contains(search) || b.NoPengadaan.Contains(search))
                                    group b by new
                                    {
                                        b.Id,
                                        b.Judul,
                                        b.AturanBerkas,
                                        b.AturanPenawaran,
                                        b.AturanPengadaan,
                                        b.Keterangan,
                                        b.Status,
                                        b.GroupPengadaan,
                                        b.TitleDokumenNotaInternal,
                                        b.TitleDokumenLain,
                                        b.TitleBerkasRujukanLain,
                                        b.CreatedBy,
                                        b.CreatedOn,
                                        b.NoPengadaan

                                    } into h
                                    select new ViewPengadaan
                                    {
                                        Id = h.Key.Id,
                                        Judul = h.Key.Judul,
                                        TitleDokumenNotaInternal = h.Key.TitleDokumenNotaInternal,
                                        TitleDokumenLain = h.Key.TitleDokumenLain,
                                        TitleBerkasRujukanLain = h.Key.TitleBerkasRujukanLain,
                                        isCreated = UserId == h.Key.CreatedBy ? 1 : 0,
                                        isPIC = (from xx in ctx.PersonilPengadaans
                                                 where xx.PengadaanId == h.Key.Id && xx.PersonilId == UserId && xx.tipe == "pic"
                                                 select new { xx.Id }).FirstOrDefault() != null ? 1 : 0,
                                        isTEAM = (from bb in ctx.PersonilPengadaans
                                                  where bb.PengadaanId == h.Key.Id && (bb.tipe == "tim" || bb.tipe == "pic") && bb.PersonilId == UserId
                                                  select bb).Count() > 0 ? 1 : 0,
                                        PersonilPengadaans = (from b in ctx.PersonilPengadaans
                                                              where b.PengadaanId == h.Key.Id
                                                              select new VWPersonilPengadaan
                                                              {
                                                                  Id = b.Id,
                                                                  Jabatan = b.Jabatan,
                                                                  Nama = b.Nama,
                                                                  PersonilId = b.PersonilId,
                                                                  tipe = b.tipe
                                                              }).ToList(),
                                        KandidatPengadaans = (from b in ctx.KandidatPengadaans
                                                              join c in ctx.Vendors on b.VendorId equals c.Id
                                                              where b.PengadaanId == h.Key.Id
                                                              select new VWKandidatPengadaan
                                                              {
                                                                  Id = b.Id,
                                                                  PengadaanId = b.PengadaanId,
                                                                  VendorId = b.VendorId,
                                                                  Nama = c.Nama
                                                              }).ToList(),
                                        JadwalPengadaans = (from b in ctx.JadwalPengadaans
                                                            where b.PengadaanId == h.Key.Id
                                                            select new VWJadwalPengadaan
                                                            {
                                                                Id = b.Id,
                                                                PengadaanId = b.PengadaanId,
                                                                Mulai = b.Mulai,
                                                                Sampai = b.Sampai,
                                                                tipe = b.tipe
                                                            }).ToList(),
                                        DokumenPengadaans = (from b in ctx.DokumenPengadaans
                                                             where b.PengadaanId == h.Key.Id
                                                             select new VWDokumenPengadaan
                                                             {
                                                                 Id = b.Id,
                                                                 PengadaanId = b.PengadaanId,
                                                                 ContentType = b.ContentType,
                                                                 File = b.File,
                                                                 Title = b.Title,
                                                                 Tipe = b.Tipe
                                                             }).ToList(),
                                        Keterangan = h.Key.Keterangan,
                                        Status = h.Key.Status,
                                        //StatusBintang = ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault() == null ? 0 : ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault().StatusBintang,
                                        AturanPengadaan = h.Key.AturanPengadaan,
                                        AturanBerkas = h.Key.AturanBerkas,
                                        AturanPenawaran = h.Key.AturanPenawaran,
                                        GroupPengadaan = h.Key.GroupPengadaan,
                                        CreatedOn = h.Key.CreatedOn,
                                        NoPengadaan = h.Key.NoPengadaan
                                    }).OrderByDescending(x => x.CreatedOn);//.Skip(start).Take(limit).ToList();

                dataPagePengadaan.TotalRecord = VWPengadaans.Count();
                dataPagePengadaan.data = VWPengadaans.Skip(start).Take(limit).ToList();
                return dataPagePengadaan;
            }
            return new DataPagePengadaan();
        }

        public List<ViewPengadaan> GetPerhatian(string search, int start, int limit, Guid? UserId, List<string> Roles, int approver, List<Guid> lstManager)
        {
            search = search == null ? "" : search;
            if (limit > 0)
            {
                List<ViewPengadaan> VWPengadaans = (from b in ctx.Pengadaans
                                    join c in ctx.PersonilPengadaans on b.Id equals c.PengadaanId into ps
                                    from c in ps.DefaultIfEmpty()
                                    where ((b.Status == EStatusPengadaan.AJUKAN) && c.PersonilId==UserId) ||
                                            (b.Status == EStatusPengadaan.DITOLAK && c.PersonilId == UserId) ||
                                                (lstManager.Contains(UserId.Value) && b.Status == EStatusPengadaan.AJUKAN)
                                                && (b.Judul.Contains(search) || b.NoPengadaan.Contains(search))
                                    group b by new
                                    {
                                        b.Id,
                                        b.Judul,
                                        b.AturanBerkas,
                                        b.AturanPenawaran,
                                        b.AturanPengadaan,
                                        b.Keterangan,
                                        b.Status,
                                        b.GroupPengadaan,
                                        b.TitleDokumenNotaInternal,
                                        b.TitleDokumenLain,
                                        b.TitleBerkasRujukanLain,
                                        b.CreatedBy,
                                        b.CreatedOn,
                                        b.NoPengadaan

                                    } into h
                                    select new ViewPengadaan
                                    {
                                        Id = h.Key.Id,
                                        Judul = h.Key.Judul,
                                        TitleDokumenNotaInternal = h.Key.TitleDokumenNotaInternal,
                                        TitleDokumenLain = h.Key.TitleDokumenLain,
                                        TitleBerkasRujukanLain = h.Key.TitleBerkasRujukanLain,
                                        isCreated = UserId == h.Key.CreatedBy ? 1 : 0,
                                        Approver = approver,
                                       // Approver=(from bb in ctx.Workflows where bb.DocumentId==h.Key.Id select bb).Count()>0?0:
                                       //         (from bb in ctx.Workflows where bb.DocumentId==h.Key.Id select bb).FirstOrDefault().NextUserId==UserId?1:0,
                                        isPIC = (from xx in ctx.PersonilPengadaans
                                                 where xx.PengadaanId == h.Key.Id && xx.PersonilId == UserId && xx.tipe == "pic"
                                                 select new { xx.Id }).FirstOrDefault() != null ? 1 : 0,
                                        isTEAM = (from bb in ctx.PersonilPengadaans
                                                  where bb.PengadaanId == h.Key.Id && (bb.tipe == "tim" || bb.tipe == "pic") && bb.PersonilId == UserId
                                                  select bb).Count() > 0 ? 1 : 0,
                                        PersonilPengadaans = (from b in ctx.PersonilPengadaans
                                                              where b.PengadaanId == h.Key.Id
                                                              select new VWPersonilPengadaan
                                                              {
                                                                  Id = b.Id,
                                                                  Jabatan = b.Jabatan,
                                                                  Nama = b.Nama,
                                                                  PersonilId = b.PersonilId,
                                                                  tipe = b.tipe
                                                              }).ToList(),
                                        KandidatPengadaans = (from b in ctx.KandidatPengadaans
                                                              join c in ctx.Vendors on b.VendorId equals c.Id
                                                              where b.PengadaanId == h.Key.Id
                                                              select new VWKandidatPengadaan
                                                              {
                                                                  Id = b.Id,
                                                                  PengadaanId = b.PengadaanId,
                                                                  VendorId = b.VendorId,
                                                                  Nama = c.Nama
                                                              }).ToList(),
                                        JadwalPengadaans = (from b in ctx.JadwalPengadaans
                                                            where b.PengadaanId == h.Key.Id
                                                            select new VWJadwalPengadaan
                                                            {
                                                                Id = b.Id,
                                                                PengadaanId = b.PengadaanId,
                                                                Mulai = b.Mulai,
                                                                Sampai = b.Sampai,
                                                                tipe = b.tipe
                                                            }).ToList(),
                                        DokumenPengadaans = (from b in ctx.DokumenPengadaans
                                                             where b.PengadaanId == h.Key.Id
                                                             select new VWDokumenPengadaan
                                                             {
                                                                 Id = b.Id,
                                                                 PengadaanId = b.PengadaanId,
                                                                 ContentType = b.ContentType,
                                                                 File = b.File,
                                                                 Title = b.Title,
                                                                 Tipe = b.Tipe
                                                             }).ToList(),
                                        Keterangan = h.Key.Keterangan,
                                        Status = h.Key.Status,
                                        //StatusBintang = ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault() == null ? 0 : ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault().StatusBintang,
                                        AturanPengadaan = h.Key.AturanPengadaan,
                                        AturanBerkas = h.Key.AturanBerkas,
                                        AturanPenawaran = h.Key.AturanPenawaran,
                                        GroupPengadaan = h.Key.GroupPengadaan,
                                        CreatedOn = h.Key.CreatedOn,
                                        NoPengadaan = h.Key.NoPengadaan
                                    }).OrderByDescending(x => x.CreatedOn).ToList();                                   
                return VWPengadaans;
            }

            return new List<ViewPengadaan>();
        }

        public List<ViewPengadaan> GetPerhatianWorkflow(string search, int start, int limit, Guid? UserId, List<Reston.Helper.Model.ViewWorkflowModel> lstDocument)
        {
            search = search == null ? "" : search;
            if (limit > 0)
            {
                List<ViewPengadaan> VWPengadaans = (from b in ctx.Pengadaans
                                                    join c in ctx.PersonilPengadaans on b.Id equals c.PengadaanId into ps
                                                    from c in ps.DefaultIfEmpty()
                                                    where ((b.Status == EStatusPengadaan.AJUKAN) && c.PersonilId == UserId) ||
                                                            (b.Status == EStatusPengadaan.DITOLAK && c.PersonilId == UserId) ||
                                                                (b.Status == EStatusPengadaan.AJUKAN)//lstDocumentId.Contains(b.Id) &&
                                                                && (b.Judul.Contains(search) || b.NoPengadaan.Contains(search))
                                                    group b by new
                                                    {
                                                        b.Id,
                                                        b.Judul,
                                                        b.AturanBerkas,
                                                        b.AturanPenawaran,
                                                        b.AturanPengadaan,
                                                        b.Keterangan,
                                                        b.Status,
                                                        b.GroupPengadaan,
                                                        b.TitleDokumenNotaInternal,
                                                        b.TitleDokumenLain,
                                                        b.TitleBerkasRujukanLain,
                                                        b.CreatedBy,
                                                        b.CreatedOn,
                                                        b.NoPengadaan

                                                    } into h
                                                    select new ViewPengadaan
                                                    {
                                                        Id = h.Key.Id,
                                                        Judul = h.Key.Judul,
                                                        TitleDokumenNotaInternal = h.Key.TitleDokumenNotaInternal,
                                                        TitleDokumenLain = h.Key.TitleDokumenLain,
                                                        TitleBerkasRujukanLain = h.Key.TitleBerkasRujukanLain,
                                                       // Approver=(from bb in lstDocument where bb.CurrentUserId==UserId && bb.DocumentId==h.Key.Id select bb).Count()>0?1:0,
                                                        isCreated = UserId == h.Key.CreatedBy ? 1 : 0,
                                                        isPIC = (from xx in ctx.PersonilPengadaans
                                                                 where xx.PengadaanId == h.Key.Id && xx.PersonilId == UserId && xx.tipe == "pic"
                                                                 select new { xx.Id }).FirstOrDefault() != null ? 1 : 0,
                                                        isTEAM = (from bb in ctx.PersonilPengadaans
                                                                  where bb.PengadaanId == h.Key.Id && (bb.tipe == "tim" || bb.tipe == "pic") && bb.PersonilId == UserId
                                                                  select bb).Count() > 0 ? 1 : 0,
                                                        PersonilPengadaans = (from b in ctx.PersonilPengadaans
                                                                              where b.PengadaanId == h.Key.Id
                                                                              select new VWPersonilPengadaan
                                                                              {
                                                                                  Id = b.Id,
                                                                                  Jabatan = b.Jabatan,
                                                                                  Nama = b.Nama,
                                                                                  PersonilId = b.PersonilId,
                                                                                  tipe = b.tipe
                                                                              }).ToList(),
                                                        KandidatPengadaans = (from b in ctx.KandidatPengadaans
                                                                              join c in ctx.Vendors on b.VendorId equals c.Id
                                                                              where b.PengadaanId == h.Key.Id
                                                                              select new VWKandidatPengadaan
                                                                              {
                                                                                  Id = b.Id,
                                                                                  PengadaanId = b.PengadaanId,
                                                                                  VendorId = b.VendorId,
                                                                                  Nama = c.Nama
                                                                              }).ToList(),
                                                        JadwalPengadaans = (from b in ctx.JadwalPengadaans
                                                                            where b.PengadaanId == h.Key.Id
                                                                            select new VWJadwalPengadaan
                                                                            {
                                                                                Id = b.Id,
                                                                                PengadaanId = b.PengadaanId,
                                                                                Mulai = b.Mulai,
                                                                                Sampai = b.Sampai,
                                                                                tipe = b.tipe
                                                                            }).ToList(),
                                                        DokumenPengadaans = (from b in ctx.DokumenPengadaans
                                                                             where b.PengadaanId == h.Key.Id
                                                                             select new VWDokumenPengadaan
                                                                             {
                                                                                 Id = b.Id,
                                                                                 PengadaanId = b.PengadaanId,
                                                                                 ContentType = b.ContentType,
                                                                                 File = b.File,
                                                                                 Title = b.Title,
                                                                                 Tipe = b.Tipe
                                                                             }).ToList(),
                                                        Keterangan = h.Key.Keterangan,
                                                        Status = h.Key.Status,
                                                        //StatusBintang = ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault() == null ? 0 : ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault().StatusBintang,
                                                        AturanPengadaan = h.Key.AturanPengadaan,
                                                        AturanBerkas = h.Key.AturanBerkas,
                                                        AturanPenawaran = h.Key.AturanPenawaran,
                                                        GroupPengadaan = h.Key.GroupPengadaan,
                                                        CreatedOn = h.Key.CreatedOn,
                                                        NoPengadaan = h.Key.NoPengadaan
                                                    }).OrderByDescending(x => x.CreatedOn).ToList();
                foreach (var item in VWPengadaans)
                {
                    var cekApprover = lstDocument.Where(d => d.DocumentId == item.Id && d.CurrentUserId == UserId).Count();
                    if (cekApprover > 0) item.Approver = 1;
                    else item.Approver = 0;
                }
                
                return VWPengadaans;
            }

            return new List<ViewPengadaan>();
        }

        public List<ViewPengadaan> GetPengadaansForRekanan(int start, int limit, Guid? UserId, List<string> Roles, EGroupPengadaan groupstatus)
        {
            //Guid manajer = new Guid(ConfigurationManager.AppSettings["manajer"].ToString());
            if (limit > 0)
            {
                Vendor oVendor = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault();
                if (oVendor == null) return new List<ViewPengadaan>();
                List<ViewPengadaan> VWPengadaans = (from b in ctx.Pengadaans
                                                    join c in ctx.KandidatPengadaans on b.Id equals c.PengadaanId into ps
                                                    from c in ps.DefaultIfEmpty()
                                                    where (groupstatus == EGroupPengadaan.ALL || b.GroupPengadaan == groupstatus) && c.VendorId == oVendor.Id
                                                    //contoh vendor
                                                    group b by new
                                                    {
                                                        b.Id,
                                                        b.Judul,
                                                        b.AturanBerkas,
                                                        b.AturanPenawaran,
                                                        b.AturanPengadaan,
                                                        b.Keterangan,
                                                        b.Status,
                                                        b.GroupPengadaan,
                                                        b.TitleDokumenNotaInternal,
                                                        b.TitleDokumenLain,
                                                        b.TitleBerkasRujukanLain,
                                                        b.Region,
                                                        b.Provinsi,
                                                        b.CreatedOn,
                                                        b.NoPengadaan
                                                    } into h
                                                    select new ViewPengadaan
                                                    {
                                                        Id = h.Key.Id,
                                                        Judul = h.Key.Judul,
                                                        Region = h.Key.Region,
                                                        Provinsi = h.Key.Provinsi,
                                                        NoPengadaan = h.Key.NoPengadaan,
                                                        PersonilPengadaans = (from b in ctx.PersonilPengadaans
                                                                              where b.PengadaanId == h.Key.Id
                                                                              select new VWPersonilPengadaan
                                                                              {
                                                                                  Id = b.Id,
                                                                                  Jabatan = b.Jabatan,
                                                                                  Nama = b.Nama,
                                                                                  PersonilId = b.PersonilId,
                                                                                  tipe = b.tipe
                                                                              }).ToList(),
                                                        KandidatPengadaans = (from b in ctx.KandidatPengadaans
                                                                              join c in ctx.Vendors on b.VendorId equals c.Id
                                                                              where b.PengadaanId == h.Key.Id
                                                                              select new VWKandidatPengadaan
                                                                              {
                                                                                  Id = b.Id,
                                                                                  PengadaanId = b.PengadaanId,
                                                                                  VendorId = b.VendorId,
                                                                                  Nama = c.Nama
                                                                              }).ToList(),
                                                        JadwalPengadaans = (from b in ctx.JadwalPengadaans
                                                                            where b.PengadaanId == h.Key.Id
                                                                            select new VWJadwalPengadaan
                                                                            {
                                                                                Id = b.Id,
                                                                                PengadaanId = b.PengadaanId,
                                                                                Mulai = b.Mulai,
                                                                                Sampai = b.Sampai,
                                                                                tipe = b.tipe
                                                                            }).ToList(),
                                                        DokumenPengadaans = (from b in ctx.DokumenPengadaans
                                                                             where b.PengadaanId == h.Key.Id
                                                                             select new VWDokumenPengadaan
                                                                             {
                                                                                 Id = b.Id,
                                                                                 PengadaanId = b.PengadaanId,
                                                                                 ContentType = b.ContentType,
                                                                                 File = b.File,
                                                                                 Title = b.Title,
                                                                                 Tipe = b.Tipe
                                                                             }).ToList(),
                                                        Keterangan = h.Key.Keterangan,
                                                        Status = h.Key.Status,
                                                        //StatusBintang = ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault() == null ? 0 : ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault().StatusBintang,
                                                        AturanPengadaan = h.Key.AturanPengadaan,
                                                        AturanBerkas = h.Key.AturanBerkas,
                                                        AturanPenawaran = h.Key.AturanPenawaran,
                                                        GroupPengadaan = h.Key.GroupPengadaan,
                                                        CreatedOn = h.Key.CreatedOn
                                                    }).OrderByDescending(x => x.CreatedOn).Skip(start).Take(limit).ToList();
                return VWPengadaans;
            }
            return new List<ViewPengadaan>();
        }

        public List<Pengadaan> GetAllPengadaan()
        {
            return ctx.Pengadaans.ToList();
        }

        public int DeletePengadaan(Guid Id, Guid UserId)
        {

            Pengadaan Mpengadaan = ctx.Pengadaans.Find(Id);

            if (Mpengadaan != null)
            {
                int ispic = (from xx in ctx.PersonilPengadaans
                             where xx.PengadaanId == Id && xx.tipe == "pic" && xx.PersonilId == UserId
                             select xx).Count() > 0 ? 1 : 0;
                if ((Mpengadaan.Status == EStatusPengadaan.DRAFT || Mpengadaan.Status == EStatusPengadaan.DITOLAK) && (Mpengadaan.CreatedBy == UserId || ispic == 1))
                {
                    if (Mpengadaan.CreatedBy != UserId && ctx.PersonilPengadaans.Where(d => d.tipe == "pic" && d.PersonilId == UserId && d.PengadaanId == Mpengadaan.Id) == null) return 0;
                    ctx.DokumenPengadaans.RemoveRange(Mpengadaan.DokumenPengadaans);
                    ctx.JadwalPengadaans.RemoveRange(Mpengadaan.JadwalPengadaans);
                    ctx.KualifikasiKandidats.RemoveRange(ctx.KualifikasiKandidats.Where(d => d.PengadaanId == Mpengadaan.Id));
                    ctx.KandidatPengadaans.RemoveRange(Mpengadaan.KandidatPengadaans);
                    ctx.PersonilPengadaans.RemoveRange(Mpengadaan.PersonilPengadaans);
                    var oPembobotanPengadaan = ctx.PembobotanPengadaans.Where(d => d.PengadaanId == Id);
                    ctx.PembobotanPengadaans.RemoveRange(oPembobotanPengadaan);
                    //ctx.SaveChanges();
                    List<RKSHeader> mrksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == Mpengadaan.Id).ToList();
                    List<RKSHeader> lstRksheader = new List<RKSHeader>();
                    List<RKSDetail> lstRKSDetail = new List<RKSDetail>();
                    if (mrksHeader.Count() > 0)
                    {
                        foreach (var item in mrksHeader)
                        {
                            lstRksheader.Add(item);
                            //var oRksDetails = ctx.RKSDetails.Where(d => d.RKSHeaderId == item.Id);
                            lstRKSDetail.AddRange(item.RKSDetails);
                            //ctx.RKSDetails.RemoveRange(oRksDetails);                               
                            //ctx.RKSHeaders.Remove(item);
                        }
                    }
                    ctx.RKSDetails.RemoveRange(lstRKSDetail);
                    ctx.RKSHeaders.RemoveRange(lstRksheader);
                    var oMessage = ctx.MessagePengadaans.Where(d => d.PengadaanId == Mpengadaan.Id);
                    ctx.MessagePengadaans.RemoveRange(oMessage);
                    ctx.Pengadaans.Remove(Mpengadaan);
                    ctx.SaveChanges();
                    return 1;
                }
            }
            return 0;

        }

        public Pengadaan Persetujuan(Guid? UserId, Guid id, int approver)
        {
            //Guid manajer = new Guid(ConfigurationManager.AppSettings["manajer"].ToString());
            // if (UserId == manajer)
            if (approver == 1)
            {
                Pengadaan pengadaan = ctx.Pengadaans.Find(id);
                if (pengadaan != null)
                {
                    string noPengadaan = GenerateNoPengadaan(UserId.Value);

                    pengadaan.Status = EStatusPengadaan.DISETUJUI;
                    pengadaan.GroupPengadaan = EGroupPengadaan.DALAMPELAKSANAAN;
                    if (string.IsNullOrEmpty(pengadaan.NoPengadaan))
                        pengadaan.NoPengadaan = noPengadaan;
                    pengadaan.TanggalMenyetujui = DateTime.Now;
                    ctx.MessagePengadaans.RemoveRange(ctx.MessagePengadaans.Where(d => d.PengadaanId == id));
                    ctx.SaveChanges();
                    MessagePengadaan mMessagePengadaan = new MessagePengadaan();
                    mMessagePengadaan.PengadaanId = pengadaan.Id;
                    mMessagePengadaan.Message = "Pengadaa Disetujui";
                    mMessagePengadaan.Status = EStatusPengadaan.DISETUJUI;
                    mMessagePengadaan.UserTo = pengadaan.CreatedBy;
                    mMessagePengadaan.FromTo = UserId;
                    mMessagePengadaan.Waktu = DateTime.Now;

                    //JadwalPelaksanaan mJadwalPelaksanaan = new JadwalPelaksanaan();
                    //mJadwalPelaksanaan.PengadaanId = pengadaan.Id;
                    //mJadwalPelaksanaan.statusPengadaan = EStatusPengadaan.AANWIJZING;
                    //mJadwalPelaksanaan.Mulai = ctx.JadwalPengadaans.Where(d => d.PengadaanId == pengadaan.Id && d.tipe == PengadaanConstants.Jadwal.Aanwijzing)
                    //                        .FirstOrDefault().Mulai;
                    //ctx.JadwalPelaksanaans.Add(mJadwalPelaksanaan);
                    ctx.SaveChanges();
                }
                return pengadaan;
            }
            return new Pengadaan();
        }

        public Pengadaan Penolakan(Guid? UserId, VWPenolakan vwPenolakan, int approver)
        {
            //Guid manajer = new Guid(ConfigurationManager.AppSettings["manajer"].ToString());
            //if (UserId == manajer)
            if (approver == 1)
            {
                Pengadaan pengadaan = ctx.Pengadaans.Find(vwPenolakan.PenolakanId);
                if (pengadaan != null)
                {
                    pengadaan.Status = EStatusPengadaan.DITOLAK;
                    pengadaan.GroupPengadaan = EGroupPengadaan.BELUMTERJADWAL;
                    MessagePengadaan oMessagePengadaan = ctx.MessagePengadaans.Where(d => d.PengadaanId == vwPenolakan.PenolakanId).FirstOrDefault();

                    ctx.MessagePengadaans.RemoveRange(ctx.MessagePengadaans.Where(d => d.PengadaanId == vwPenolakan.PenolakanId));
                    ctx.SaveChanges();
                    MessagePengadaan mMessagePengadaan = new MessagePengadaan();
                    mMessagePengadaan.PengadaanId = pengadaan.Id;
                    mMessagePengadaan.Message = "Pengadaan ini DiTolak";
                    mMessagePengadaan.Status = EStatusPengadaan.DITOLAK;
                    mMessagePengadaan.UserTo = oMessagePengadaan.FromTo;
                    mMessagePengadaan.FromTo = UserId;
                    mMessagePengadaan.Waktu = DateTime.Now;
                    ctx.MessagePengadaans.Add(mMessagePengadaan);
                    ctx.SaveChanges();

                    PenolakanPengadaan mPenolakanPengadaan = ctx.PenolakanPengadaans.Where(d => d.PengadaanId == vwPenolakan.PenolakanId && d.status == 1).FirstOrDefault();
                    if (mPenolakanPengadaan != null)
                        mPenolakanPengadaan.status = 0;
                    else
                    {
                        mPenolakanPengadaan = new PenolakanPengadaan();
                        mPenolakanPengadaan.PengadaanId = vwPenolakan.PenolakanId;
                        mPenolakanPengadaan.Keterangan = vwPenolakan.AlasanPenolakan;
                        mPenolakanPengadaan.CreateBy = UserId.Value;
                        mPenolakanPengadaan.CreateOn = DateTime.Now;
                        mPenolakanPengadaan.status = 1;
                        ctx.PenolakanPengadaans.Add(mPenolakanPengadaan);
                    }
                    ctx.SaveChanges();
                }
                return pengadaan;
            }
            return new Pengadaan();
        }

        public ResultMessage TolakPengadaan(Guid Id)
        {
            ResultMessage result = new ResultMessage();
            Pengadaan pengadaan = ctx.Pengadaans.Find(Id);
            pengadaan.Status = EStatusPengadaan.DITOLAK;
            pengadaan.GroupPengadaan = EGroupPengadaan.BELUMTERJADWAL;
            try
            {
                ctx.SaveChanges();
                result.message = Common.UpdateSukses();
                result.status = System.Net.HttpStatusCode.OK;
                result.Id = pengadaan.Id.ToString();
                return result;
            }
            catch(Exception ex){
                result.message = ex.ToString();
                result.status = System.Net.HttpStatusCode.InternalServerError;
                return result;
            }
            
        }
        
        public void Save()
        {
            ctx.SaveChanges();
        }

        public Pengadaan AddPengadaan(Pengadaan pengadaan, Guid UserId, List<Guid> manager)
        {
            // Guid manajer =new Guid( ConfigurationManager.AppSettings["manajer"].ToString());
            if (pengadaan.Status == EStatusPengadaan.DRAFT || pengadaan.Status == EStatusPengadaan.AJUKAN)
            {
                Pengadaan Mpengadaan = ctx.Pengadaans.Find(pengadaan.Id);
                if (Mpengadaan != null)
                {
                    int isteam = (from bb in ctx.PersonilPengadaans
                                  where bb.PengadaanId == Mpengadaan.Id && (bb.tipe == "tim" || bb.tipe == "pic") && bb.PersonilId == UserId
                                  select bb).Count() > 0 ? 1 : 0;
                    int isCreated = Mpengadaan.CreatedBy == UserId ? 1 : 0;
                    if (isteam == 0 && isCreated == 0) return pengadaan;
                    if (Mpengadaan.Status == EStatusPengadaan.AJUKAN)
                    {
                        return pengadaan;
                    }
                    Mpengadaan.Judul = pengadaan.Judul;
                    Mpengadaan.Keterangan = pengadaan.Keterangan;
                    Mpengadaan.AturanBerkas = pengadaan.AturanBerkas;
                    Mpengadaan.AturanPenawaran = pengadaan.AturanPenawaran;
                    Mpengadaan.AturanPengadaan = pengadaan.AturanPengadaan;
                    Mpengadaan.GroupPengadaan = EGroupPengadaan.BELUMTERJADWAL;
                    Mpengadaan.JenisPekerjaan = pengadaan.JenisPekerjaan;
                    Mpengadaan.JenisPembelanjaan = pengadaan.JenisPembelanjaan;
                    Mpengadaan.KualifikasiRekan = pengadaan.KualifikasiRekan;
                    Mpengadaan.MataUang = pengadaan.MataUang;
                    Mpengadaan.PeriodeAnggaran = pengadaan.PeriodeAnggaran;
                    Mpengadaan.Provinsi = pengadaan.Provinsi;
                    Mpengadaan.Region = pengadaan.Region;
                    Mpengadaan.UnitKerjaPemohon = pengadaan.UnitKerjaPemohon;
                    Mpengadaan.TitleDokumenNotaInternal = pengadaan.TitleDokumenNotaInternal;
                    Mpengadaan.TitleDokumenLain = pengadaan.TitleDokumenLain;
                    Mpengadaan.TitleBerkasRujukanLain = pengadaan.TitleBerkasRujukanLain;
                    Mpengadaan.ModifiedBy = UserId;
                    Mpengadaan.Pagu = pengadaan.Pagu;
                    Mpengadaan.Status = pengadaan.Status;
                    Mpengadaan.ModifiedOn = DateTime.Now;
                    if (Mpengadaan.JadwalPengadaans != null)
                    {
                        ctx.JadwalPengadaans.RemoveRange(Mpengadaan.JadwalPengadaans);
                    }
                    foreach (var item in pengadaan.JadwalPengadaans)
                    {
                        JadwalPengadaan mJadwalPengadaan = new JadwalPengadaan();
                        mJadwalPengadaan.Mulai = item.Mulai;
                        mJadwalPengadaan.Sampai = item.Sampai;
                        mJadwalPengadaan.tipe = item.tipe;
                        mJadwalPengadaan.PengadaanId = Mpengadaan.Id;
                        Mpengadaan.JadwalPengadaans.Add(mJadwalPengadaan);
                    }
                }
                else
                {
                    pengadaan.GroupPengadaan = EGroupPengadaan.BELUMTERJADWAL;
                    pengadaan.CreatedBy = UserId;
                    pengadaan.CreatedOn = DateTime.Now;
                    ctx.Pengadaans.Add(pengadaan);
                    ctx.SaveChanges();
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Pembuatan Dokumen Pengadaan";
                    nRiwayatDokumen.PengadaanId = pengadaan.Id;
                    nRiwayatDokumen.UserId = UserId;
                    AddRiwayatDokumen(nRiwayatDokumen);
                }
                if (pengadaan.Status == EStatusPengadaan.AJUKAN)
                {
                    RiwayatDokumen nRiwayatDokumen = new RiwayatDokumen();
                    nRiwayatDokumen.Status = "Pengajuan Dokumen Pengadaan";
                    nRiwayatDokumen.PengadaanId = pengadaan.Id;
                    nRiwayatDokumen.UserId = UserId;
                    AddRiwayatDokumen(nRiwayatDokumen);
                }
                try
                {
                    ctx.SaveChanges();
                    //if (pengadaan.Status == EStatusPengadaan.AJUKAN)
                    //{
                    //    AjukanWorkflow(pengadaan.Id, UserId, new Guid("B6EA58A3-345D-E611-A7D2-38EAA7E56C6E"));                       
                    //}
                }
                catch (Exception dbEx)
                {
                }
            }

            return pengadaan;
        }

        public RiwayatDokumen AddRiwayatDokumen(RiwayatDokumen nRiwayatDokumen)
        {
            nRiwayatDokumen.ActionDate = DateTime.Now;
            ctx.RiwayatDokumens.Add(nRiwayatDokumen);
            ctx.SaveChanges();
            return nRiwayatDokumen;
        }

        public List<RiwayatDokumen> lstRiwayatDokumen(Guid Id)
        {            
            return ctx.RiwayatDokumens.Where(d=>d.PengadaanId==Id).ToList();
        }

        public RKSHeader AddTotalHps(Guid PengadaanId, decimal Total, Guid UserId)
        {
            Pengadaan Mpengadaan = ctx.Pengadaans.Find(PengadaanId);
            if (Mpengadaan == null) return new RKSHeader();
            RKSHeader oRksHeader = new RKSHeader();
            oRksHeader.CreateBy = UserId;
            oRksHeader.CreateOn = DateTime.Now;
            oRksHeader.PengadaanId = PengadaanId;
            oRksHeader.Total = Total;
            ctx.RKSHeaders.Add(oRksHeader);
            ctx.SaveChanges();
            return new RKSHeader
            {
                Id = oRksHeader.Id,
                PengadaanId = oRksHeader.PengadaanId,
                Total = oRksHeader.Total

            };
        }

        public RKSHeader GetTotalHps(Guid PengadaanId, Guid UserId)
        {
            RKSHeader oRksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
            if (oRksHeader == null) return new RKSHeader();
            return new RKSHeader
            {
                Id = oRksHeader.Id,
                PengadaanId = oRksHeader.PengadaanId,
                Total = oRksHeader.Total

            };
        }

        public List<VWRKSDetail> getRKS(Guid id)
        {
            RKSHeader rksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == id).FirstOrDefault();
            if (rksHeader != null)
            {
                var rksDetails = rksHeader.RKSDetails.OrderBy(d=>d.grup).ThenBy(d=>d.level).ToList();
                List<VWRKSDetail> lstVWRksDetail = new List<VWRKSDetail>();
                foreach (var item in rksDetails)
                {
                    VWRKSDetail dVWRKSDetail = new VWRKSDetail();
                    dVWRKSDetail.judul = item.judul;
                    dVWRKSDetail.level = item.level;
                    dVWRKSDetail.grup = item.grup;
                    dVWRKSDetail.item = item.item;
                    dVWRKSDetail.satuan = item.satuan;
                    dVWRKSDetail.jumlah = item.jumlah;
                    dVWRKSDetail.hps = item.hps;
                    dVWRKSDetail.keterangan = item.keterangan;
                    if(item.hps!=null&& item.jumlah != null)
                    {
                        dVWRKSDetail.total = item.hps.Value * item.jumlah.Value;
                    }
                    lstVWRksDetail.Add(dVWRKSDetail);
                }
                return lstVWRksDetail;
            }
            return new List<VWRKSDetail>();
        }

        public List<VWRKSDetailRekanan> getRKSForRekanan(Guid id, Guid UserId)
        {
            RKSHeader rksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == id).FirstOrDefault();
            if (rksHeader != null)
            {
                List<VWRKSDetailRekanan> VWRksDEtail = (from b in rksHeader.RKSDetails
                                                        select new VWRKSDetailRekanan
                                                        {
                                                            Id = b.Id,
                                                            item = b.item,
                                                            keteranganItem = b.keterangan,
                                                            ItemId = b.ItemId,
                                                            jumlah = b.jumlah,
                                                            RKSHeaderId = b.RKSHeaderId,
                                                            satuan = b.satuan,
                                                            hargaEncript = ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? "" : ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().hargaEncrypt,
                                                            HargaRekananId = ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? Guid.Empty : ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().Id,
                                                            harga = ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? 0 : ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().harga,
                                                            keterangan = ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? "" : ctx.HargaRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().keterangan
                                                        }).ToList();
                foreach (var item in VWRksDEtail)
                {
                    JimbisEncrypt encod = new JimbisEncrypt();
                    if (item.hargaEncript != "")
                    {
                        decimal? harga = Convert.ToDecimal(encod.Decrypt(item.hargaEncript));
                        item.harga = harga;
                    }
                }
                return VWRksDEtail;
            }
            return new List<VWRKSDetailRekanan>();
        }

        public List<VWRKSDetailRekanan> getRKSForKlarifikasiRekanan(Guid id, Guid UserId)
        {
            RKSHeader rksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == id).FirstOrDefault();
            JimbisEncrypt encod = new JimbisEncrypt();

            if (rksHeader != null)
            {
                List<VWRKSDetailRekanan> VWRksDEtail = (from b in rksHeader.RKSDetails
                                                        select new VWRKSDetailRekanan
                                                        {
                                                            Id = b.Id,
                                                            item = b.item,
                                                            keteranganItem = b.keterangan,
                                                            ItemId = b.ItemId,
                                                            jumlah = b.jumlah,
                                                            RKSHeaderId = b.RKSHeaderId,
                                                            satuan = b.satuan,
                                                            HargaRekananId = ctx.HargaKlarifikasiRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? Guid.Empty : ctx.HargaKlarifikasiRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().Id,
                                                            harga = ctx.HargaKlarifikasiRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? 0 : ctx.HargaKlarifikasiRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().harga,
                                                            keterangan = ctx.HargaKlarifikasiRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault() == null ? "" : ctx.HargaKlarifikasiRekanans.Where(d => d.RKSDetailId == b.Id && d.VendorId == ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id).FirstOrDefault().keterangan
                                                        }).ToList();

                return VWRksDEtail;
            }
            return new List<VWRKSDetailRekanan>();
        }

        public RKSHeader saveRks(RKSHeader rks, Guid UserId)
        {
            int RKSEditAfterApprove =Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RKS_CHANGE_AFTER_APROVE"]);
            Pengadaan Mpengadaan = ctx.Pengadaans.Find(rks.PengadaanId);
            if (Mpengadaan != null)
            {
                if (Mpengadaan.Status == EStatusPengadaan.DRAFT || RKSEditAfterApprove==1)
                {
                    if (rks.Id != Guid.Empty)
                    {
                        RKSHeader MRKSHeader = ctx.RKSHeaders.Find(rks.Id);
                        if (MRKSHeader != null)
                        {
                            MRKSHeader.ModifiedBy = UserId;
                            MRKSHeader.ModifiedOn = DateTime.Now;
                            foreach (var item in rks.RKSDetails)
                            {
                                if (item.Id != Guid.Empty)
                                {
                                    RKSDetail rksdetail = ctx.RKSDetails.Find(item.Id);
                                    rksdetail.judul = item.judul;
                                    rksdetail.level = item.level;
                                    rksdetail.grup = item.grup;
                                    rksdetail.hps = item.hps;
                                    rksdetail.item = item.item;
                                    rksdetail.ItemId = item.ItemId;
                                    rksdetail.jumlah = item.jumlah;
                                    rksdetail.satuan = item.satuan;
                                    rksdetail.keterangan = item.keterangan;
                                    rksdetail.ModifiedBy = UserId;
                                    rksdetail.ModifiedOn = DateTime.Now;
                                }
                                else
                                {
                                    RKSDetail newrksdetail = new RKSDetail();
                                    newrksdetail.hps = item.hps;
                                    newrksdetail.item = item.item;
                                    newrksdetail.judul = item.judul;
                                    newrksdetail.level = item.level;
                                    newrksdetail.grup = item.grup;
                                    newrksdetail.ItemId = item.ItemId;
                                    newrksdetail.jumlah = item.jumlah;
                                    newrksdetail.satuan = item.satuan;
                                    newrksdetail.keterangan = item.keterangan;
                                    newrksdetail.RKSHeaderId = MRKSHeader.Id;
                                    newrksdetail.CreateOn = DateTime.Now;
                                    newrksdetail.CreateBy = UserId;
                                    MRKSHeader.RKSDetails.Add(newrksdetail);
                                }
                            }
                            var rksrequesIds = rks.RKSDetails.Select(d => d.Id);
                            var removeRksDetail = MRKSHeader.RKSDetails.Where(d => !rksrequesIds.Contains(d.Id));
                            ctx.RKSDetails.RemoveRange(removeRksDetail);
                        }
                        else
                        {
                            rks.CreateBy = UserId;
                            rks.CreateOn = DateTime.Now;
                            ctx.RKSHeaders.Add(rks);
                        }

                    }
                    else
                    {
                        rks.CreateBy = UserId;
                        rks.CreateOn = DateTime.Now;
                        ctx.RKSHeaders.Add(rks);
                    }
                    ctx.SaveChanges();
                }
            }
            else rks.Id = Guid.Empty;

            return rks;
        }

        public VWRKSHeaderPengadaan GetRKSHeaderPengadaan(Guid id)
        {
            //var oData = ctx.RKSHeaderTemplate.Find(Id);
            Pengadaan Mpengadaan = ctx.Pengadaans.Find(id);
            if (Mpengadaan == null) return new VWRKSHeaderPengadaan();
            VWRKSHeaderPengadaan MrksHeder = (from b in ctx.RKSHeaders
                                              join c in ctx.Pengadaans on b.PengadaanId equals c.Id
                                              where c.Id == id
                                              select new VWRKSHeaderPengadaan
                                              {
                                                  Judul = c.Judul,
                                                  Keterangan = c.Keterangan,
                                                  RKSHeaderId = b.Id,
                                                  Status = c.Status
                                              }
                                   ).FirstOrDefault();

            if (MrksHeder == null) return new VWRKSHeaderPengadaan();
            return MrksHeder;
        }

        public DokumenPengadaan GetDokumenPengadaan(Guid Id)
        {
            //return ctx.DokumenPengadaans.Where(d=>d.File.Contains(file)).FirstOrDefault();
            return ctx.DokumenPengadaans.Find(Id);
        }

        public List<VWDokumenPengadaan> GetListDokumenPengadaan(TipeBerkas tipe, Guid Id, Guid UserId)
        {
            List<VWDokumenPengadaan> lstVWDok = new List<VWDokumenPengadaan>();
            var oData = (from b in ctx.DokumenPengadaans
                         where b.PengadaanId == Id && b.Tipe == tipe
                         select b);
            Vendor oVendor = ctx.Vendors.Where(d => d.Owner == UserId).FirstOrDefault();
            if (oVendor != null)
            {
                lstVWDok = oData.Where(d => d.VendorId == oVendor.Id).Select(d => new VWDokumenPengadaan
                {
                    ContentType = d.ContentType,
                    Id = d.Id,
                    PengadaanId = d.PengadaanId,
                    Tipe = d.Tipe,
                    Title = d.Title,
                    File = d.File,
                    SizeFile = d.SizeFile,
                }).ToList();
                if (tipe == TipeBerkas.BerkasRujukanLain)
                {
                    var oDokumen = oData.Select(d => new VWDokumenPengadaan
                    {
                        ContentType = d.ContentType,
                        Id = d.Id,
                        PengadaanId = d.PengadaanId,
                        Tipe = d.Tipe,
                        Title = d.Title,
                        File = d.File,
                        SizeFile = d.SizeFile,
                    }).ToList();
                    foreach (var item in oDokumen)
                    {
                        lstVWDok.Add(item);
                    }
                }
            }
            else
            {
                lstVWDok = oData.Select(d => new VWDokumenPengadaan
                {
                    ContentType = d.ContentType,
                    Id = d.Id,
                    PengadaanId = d.PengadaanId,
                    Tipe = d.Tipe,
                    Title = d.Title,
                    File = d.File,
                    SizeFile = d.SizeFile,
                }).ToList();
            }

            return lstVWDok;

        }

        public List<VWDokumenPengadaan> GetListDokumenVendor(TipeBerkas tipe, Guid Id, Guid UserId, int VendorId)
        {
            List<VWDokumenPengadaan> lstVWDok = new List<VWDokumenPengadaan>();
            var oData = (from b in ctx.DokumenPengadaans
                         where b.PengadaanId == Id && b.Tipe == tipe && b.VendorId == VendorId
                         select b);
            Vendor oVendor = ctx.Vendors.Find(VendorId);
            if (oVendor != null)
            {
                lstVWDok = oData.Where(d => d.VendorId == oVendor.Id).Select(d => new VWDokumenPengadaan
                {
                    ContentType = d.ContentType,
                    Id = d.Id,
                    PengadaanId = d.PengadaanId,
                    Tipe = d.Tipe,
                    Title = d.Title,
                    File = d.File,
                    SizeFile = d.SizeFile,
                }).ToList();
            }
            else
            {
                lstVWDok = oData.Select(d => new VWDokumenPengadaan
                {
                    ContentType = d.ContentType,
                    Id = d.Id,
                    PengadaanId = d.PengadaanId,
                    Tipe = d.Tipe,
                    Title = d.Title,
                    File = d.File,
                    SizeFile = d.SizeFile,
                }).ToList();
            }

            return lstVWDok;

        }

        public DokumenPengadaan saveDokumenPengadaan(DokumenPengadaan dokumenPengadaan, Guid UserId)
        {

            DokumenPengadaan mdokPengadaan = ctx.DokumenPengadaans.Find(dokumenPengadaan.Id);
            if (mdokPengadaan != null)
            {
                mdokPengadaan.File = dokumenPengadaan.File;
                mdokPengadaan.ModifiedOn = DateTime.Now;
                mdokPengadaan.ModifiedBy = UserId;
                Vendor oVendor = ctx.Vendors.Where(d => d.Owner == UserId).FirstOrDefault();
                if (oVendor != null) mdokPengadaan.VendorId = oVendor.Id;
            }
            else
            {
                Vendor oVendor = ctx.Vendors.Where(d => d.Owner == UserId).FirstOrDefault();
                if (oVendor != null) dokumenPengadaan.VendorId = oVendor.Id;
                try
                {
                    dokumenPengadaan.CreateOn = DateTime.Now;
                    dokumenPengadaan.CreateBy = UserId;
                }
                catch (Exception ex) { }
                ctx.DokumenPengadaans.Add(dokumenPengadaan);
            }
            ctx.SaveChanges();
            return mdokPengadaan;
        }

        public int deleteDokumen(Guid Id)
        {
            try
            {

                DokumenPengadaan MdokPengadaan = ctx.DokumenPengadaans.Find(Id);
                Pengadaan mpengadaan = ctx.Pengadaans.Find(MdokPengadaan.PengadaanId);
                if (mpengadaan == null) return 0;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return 0;
                ctx.DokumenPengadaans.Remove(MdokPengadaan);
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public KandidatPengadaan saveKandidatPengadaan(KandidatPengadaan kandidat, Guid UserId)
        {
            try
            {
                Pengadaan mpengadaan = ctx.Pengadaans.Find(kandidat.PengadaanId);
                if (mpengadaan == null) return kandidat;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return kandidat;
                KandidatPengadaan mKandidatPengadaan = ctx.KandidatPengadaans.Find(kandidat.Id);
                if (mKandidatPengadaan != null)
                {
                    mKandidatPengadaan.VendorId = kandidat.VendorId;
                    mpengadaan.ModifiedBy = UserId;
                    mpengadaan.ModifiedOn = DateTime.Now;
                    ctx.SaveChanges();
                    return mKandidatPengadaan;
                }
                else
                {
                    mpengadaan.ModifiedBy = UserId;
                    mpengadaan.ModifiedOn = DateTime.Now;
                    ctx.KandidatPengadaans.Add(kandidat);
                    ctx.SaveChanges();
                    return kandidat;
                }
            }
            catch
            {
                return kandidat;
            }
        }

        public int deleteKandidatPengadaan(Guid Id, Guid UserId)
        {
            try
            {
                KandidatPengadaan MKandidatPengadaan = ctx.KandidatPengadaans.Find(Id);
                Pengadaan mpengadaan = ctx.Pengadaans.Find(MKandidatPengadaan.PengadaanId);
                if (mpengadaan == null) return 0;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return 0;
                ctx.KandidatPengadaans.Remove(MKandidatPengadaan);
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public int deleteJadwalPengadaan(Guid Id, Guid UserId)
        {
            try
            {
                JadwalPengadaan MJadwalPengadaan = ctx.JadwalPengadaans.Find(Id);
                Pengadaan mpengadaan = ctx.Pengadaans.Find(MJadwalPengadaan.PengadaanId);
                if (mpengadaan == null) return 0;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return 0;
                ctx.JadwalPengadaans.Remove(MJadwalPengadaan);
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public JadwalPengadaan saveJadwalPengadaan(JadwalPengadaan Jadwal, Guid UserId)
        {
            try
            {
                Pengadaan mpengadaan = ctx.Pengadaans.Find(Jadwal.PengadaanId);
                if (mpengadaan == null) return Jadwal;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return Jadwal;
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Find(Jadwal.Id);
                if (mJadwalPengadaan != null)
                {
                    mJadwalPengadaan.Mulai = Jadwal.Mulai;
                    mJadwalPengadaan.Sampai = Jadwal.Sampai;
                    mJadwalPengadaan.tipe = Jadwal.tipe;
                    mpengadaan.ModifiedBy = UserId;
                    mpengadaan.ModifiedOn = DateTime.Now;
                    ctx.SaveChanges();
                    return mJadwalPengadaan;
                }
                else
                {
                    mpengadaan.ModifiedBy = UserId;
                    mpengadaan.ModifiedOn = DateTime.Now;
                    ctx.JadwalPengadaans.Add(Jadwal);
                    ctx.SaveChanges();
                    return Jadwal;
                }
            }
            catch
            {
                return Jadwal;
            }
        }

        public PersonilPengadaan savePersonilPengadaan(PersonilPengadaan Personil, Guid UserId)
        {
            try
            {
                Pengadaan mpengadaan = ctx.Pengadaans.Find(Personil.PengadaanId);
                if (mpengadaan == null) return Personil;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return Personil;
                PersonilPengadaan mPersonilPengadaan = ctx.PersonilPengadaans.Find(Personil.Id);
                if (mPersonilPengadaan != null)
                {
                    mPersonilPengadaan.Nama = Personil.Nama;
                    mPersonilPengadaan.PersonilId = Personil.PersonilId;
                    mPersonilPengadaan.Jabatan = Personil.Jabatan;
                    mPersonilPengadaan.tipe = Personil.tipe;
                    mpengadaan.ModifiedBy = UserId;
                    mpengadaan.ModifiedOn = DateTime.Now;
                    ctx.SaveChanges();
                    return mPersonilPengadaan;
                }
                else
                {
                    mpengadaan.ModifiedBy = UserId;
                    mpengadaan.ModifiedOn = DateTime.Now;
                    ctx.PersonilPengadaans.Add(Personil);
                    ctx.SaveChanges();
                    return Personil;
                }
            }
            catch
            {
                return Personil;
            }
        }

        public int deletePersonilPengadaan(Guid Id, Guid UserId)
        {
            try
            {
                PersonilPengadaan MPersonilPengadaan = ctx.PersonilPengadaans.Find(Id);
                Pengadaan mpengadaan = ctx.Pengadaans.Find(MPersonilPengadaan.PengadaanId);
                if (mpengadaan == null) return 0;
                if (mpengadaan.Status != EStatusPengadaan.DRAFT) return 0;
                ctx.PersonilPengadaans.Remove(MPersonilPengadaan);
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public List<VWKandidatPengadaan> getListKandidatPengadaan(Guid PengadaanId)
        {
            var kandidats = (from b in ctx.KandidatPengadaans
                             join c in ctx.Vendors on b.VendorId equals c.Id
                             where b.PengadaanId == PengadaanId
                             select new VWKandidatPengadaan
                             {
                                 Id = b.Id,
                                 Nama = c.Nama,
                                 PengadaanId = b.PengadaanId,
                                 VendorId = b.VendorId,
                                 Telepon = c.Telepon,
                                 isReady=b.isReady
                             }).ToList();

            return kandidats;
        }

        public List<JadwalPengadaan> getListJadwalPengadaan(Guid PengadaanId)
        {
            return ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId).ToList();
        }

        public List<PersonilPengadaan> getListPersonilPengadaan(Guid PengadaanId)
        {
            return ctx.PersonilPengadaans.Where(d => d.PengadaanId == PengadaanId).ToList();
        }

        public JadwalPelaksanaan getPelaksanaanPendaftaran(Guid PengadaanId)
        {
            //PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId && d.statusPengadaan==EStatusPengadaan.DISETUJUI).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                MOJadwalPelaksanaan.Sampai = MjadwalPelaksanaan.Sampai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.Pendaftaran).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.Sampai = Mjadawal.Sampai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
        }

        public JadwalPelaksanaan getPelaksanaanAanwijing(Guid PengadaanId)
        {
            //PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId
                            && d.statusPengadaan == EStatusPengadaan.AANWIJZING).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.Aanwijzing).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
        }

        public JadwalPelaksanaan addPelaksanaanAanwijing(JadwalPelaksanaan pelaksanaanAanwijzing, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(pelaksanaanAanwijzing.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new JadwalPelaksanaan();
            if (ctx.Pengadaans.Find(pelaksanaanAanwijzing.PengadaanId) != null)
            {
                // PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
                JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId
                        && d.statusPengadaan == EStatusPengadaan.AANWIJZING).FirstOrDefault();
                if (MjadwalPelaksanaan != null)
                {
                    //MpelaksanaanAanwijzing.IsiUndangan = pelaksanaanAanwijzing.IsiUndangan;
                    MjadwalPelaksanaan.Mulai = pelaksanaanAanwijzing.Mulai;
                }
                else
                {
                    MjadwalPelaksanaan = new JadwalPelaksanaan();
                    MjadwalPelaksanaan.PengadaanId = pelaksanaanAanwijzing.PengadaanId;
                    MjadwalPelaksanaan.statusPengadaan = EStatusPengadaan.AANWIJZING;
                    //MpelaksanaanAanwijzing.IsiUndangan = pelaksanaanAanwijzing.IsiUndangan;
                    MjadwalPelaksanaan.Mulai = pelaksanaanAanwijzing.Mulai;
                    ctx.JadwalPelaksanaans.Add(MjadwalPelaksanaan);
                }
                ctx.SaveChanges();

                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MOJadwalPelaksanaan.Mulai;
                return MOJadwalPelaksanaan;
            }
            return new JadwalPelaksanaan();
        }

        public List<VWKehadiranKandidatAanwijzing> getKehadiranAanwijzings(Guid PengadaanId)
        {
            var lstKehadiranKandidat = (from b in ctx.KandidatPengadaans
                                        where b.PengadaanId == PengadaanId
                                        select new VWKehadiranKandidatAanwijzing
                                        {
                                            Id = b.Id,
                                            VendorId = b.VendorId,
                                            PengadaanId = b.PengadaanId,
                                            NamaVendor = (from bb in ctx.Vendors
                                                          where bb.Id == b.VendorId
                                                          select bb).FirstOrDefault().Nama,
                                            Telp = (from bb in ctx.Vendors
                                                    where bb.Id == b.VendorId
                                                    select bb).FirstOrDefault().Telepon,
                                            hadir = (from bb in ctx.KehadiranKandidatAanwijzings
                                                     where bb.VendorId == b.VendorId && bb.PengadaanId == b.PengadaanId
                                                     select bb).Count() > 0 ? 1 : 0
                                        }).ToList();
            return lstKehadiranKandidat;
        }

        public KehadiranKandidatAanwijzing addKehadiranAanwijzing(Guid Id, Guid UserId)
        {
            KandidatPengadaan MKandidatPengadaan = ctx.KandidatPengadaans.Find(Id);
            if (MKandidatPengadaan == null) return new KehadiranKandidatAanwijzing();
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == MKandidatPengadaan.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new KehadiranKandidatAanwijzing();
            KehadiranKandidatAanwijzing mKehadiranAazwjing = ctx.KehadiranKandidatAanwijzings
                                .Where(d => d.PengadaanId == MKandidatPengadaan.PengadaanId && d.VendorId == MKandidatPengadaan.VendorId).FirstOrDefault();
            if (mKehadiranAazwjing != null) return mKehadiranAazwjing;
            mKehadiranAazwjing = new KehadiranKandidatAanwijzing();
            mKehadiranAazwjing.PengadaanId = MKandidatPengadaan.PengadaanId;
            mKehadiranAazwjing.VendorId = MKandidatPengadaan.VendorId;
            ctx.KehadiranKandidatAanwijzings.Add(mKehadiranAazwjing);
            ctx.SaveChanges();
            return mKehadiranAazwjing;
        }

        public int DeleteKehadiranAanwijzing(Guid Id, Guid UserId)
        {
            try
            {
                KandidatPengadaan MKandidatPengadaan = ctx.KandidatPengadaans.Find(Id);
                if (MKandidatPengadaan == null) return 0;
                PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == MKandidatPengadaan.PengadaanId).FirstOrDefault();
                if (picPersonil == null) return 0;
                KehadiranKandidatAanwijzing mKejadiranAazwjing = ctx.KehadiranKandidatAanwijzings
                                    .Where(d => d.PengadaanId == MKandidatPengadaan.PengadaanId && d.VendorId == MKandidatPengadaan.VendorId).FirstOrDefault();
                if (mKejadiranAazwjing == null) return 0;
                ctx.KehadiranKandidatAanwijzings.Remove(mKejadiranAazwjing);
                ctx.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public JadwalPelaksanaan AddPelaksanaanSubmitPenawaran(JadwalPelaksanaan pelaksanaanSubmitPenawaran, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(pelaksanaanSubmitPenawaran.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == pelaksanaanSubmitPenawaran.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new JadwalPelaksanaan();
            if (ctx.Pengadaans.Find(pelaksanaanSubmitPenawaran.PengadaanId) != null)
            {
                // PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
                JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == pelaksanaanSubmitPenawaran.PengadaanId
                        && d.statusPengadaan == EStatusPengadaan.SUBMITPENAWARAN).FirstOrDefault();

                if (MjadwalPelaksanaan != null)
                {
                    //MpelaksanaanAanwijzing.IsiUndangan = pelaksanaanAanwijzing.IsiUndangan;
                    if (pelaksanaanSubmitPenawaran.Mulai >= DateTime.Now)
                        MjadwalPelaksanaan.Mulai = pelaksanaanSubmitPenawaran.Mulai;
                    if (pelaksanaanSubmitPenawaran.Sampai > DateTime.Now)
                        MjadwalPelaksanaan.Sampai = pelaksanaanSubmitPenawaran.Sampai;
                }
                else
                {
                    MjadwalPelaksanaan = new JadwalPelaksanaan();
                    MjadwalPelaksanaan.PengadaanId = pelaksanaanSubmitPenawaran.PengadaanId;
                    //MpelaksanaanAanwijzing.IsiUndangan = pelaksanaanAanwijzing.IsiUndangan;
                    MjadwalPelaksanaan.statusPengadaan = EStatusPengadaan.SUBMITPENAWARAN;
                    if (pelaksanaanSubmitPenawaran.Mulai >= DateTime.Now)
                        MjadwalPelaksanaan.Mulai = pelaksanaanSubmitPenawaran.Mulai;
                    if (pelaksanaanSubmitPenawaran.Sampai > DateTime.Now)
                        MjadwalPelaksanaan.Sampai = pelaksanaanSubmitPenawaran.Sampai;
                    ctx.JadwalPelaksanaans.Add(MjadwalPelaksanaan);
                }
                ctx.SaveChanges();
                return MjadwalPelaksanaan;
            }
            return new JadwalPelaksanaan();
        }

        public JadwalPelaksanaan getPelaksanaanSubmitPenawaran(Guid PengadaanId)
        {
            //PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId
                            && d.statusPengadaan == EStatusPengadaan.SUBMITPENAWARAN).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                MOJadwalPelaksanaan.Sampai = MjadwalPelaksanaan.Sampai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.PengisianHarga).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.Sampai = Mjadawal.Sampai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
            // return ctx.PelaksanaanSubmitPenawarans.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
        }

        public JadwalPelaksanaan AddPelaksanaanBukaAmplop(JadwalPelaksanaan pelaksanaanBukaAmplop, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(pelaksanaanBukaAmplop.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == pelaksanaanBukaAmplop.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new JadwalPelaksanaan();
            if (ctx.Pengadaans.Find(pelaksanaanBukaAmplop.PengadaanId) != null)
            {
                // PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
                JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == pelaksanaanBukaAmplop.PengadaanId
                        && d.statusPengadaan == EStatusPengadaan.BUKAAMPLOP).FirstOrDefault();
                if (MjadwalPelaksanaan != null)
                {
                    MjadwalPelaksanaan.Mulai = pelaksanaanBukaAmplop.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanBukaAmplop.Sampai;
                }
                else
                {
                    MjadwalPelaksanaan = new JadwalPelaksanaan();
                    MjadwalPelaksanaan.PengadaanId = pelaksanaanBukaAmplop.PengadaanId;
                    MjadwalPelaksanaan.statusPengadaan = EStatusPengadaan.BUKAAMPLOP;
                    MjadwalPelaksanaan.Mulai = pelaksanaanBukaAmplop.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanBukaAmplop.Sampai;
                    ctx.JadwalPelaksanaans.Add(MjadwalPelaksanaan);
                }
                ctx.SaveChanges();
                return MjadwalPelaksanaan;
            }
            return new JadwalPelaksanaan();
        }

        public JadwalPelaksanaan getPelaksanaanBukaAmplop(Guid PengadaanId)
        {
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId
                           && d.statusPengadaan == EStatusPengadaan.BUKAAMPLOP).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                MOJadwalPelaksanaan.Sampai = MjadwalPelaksanaan.Sampai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.BukaAmplop).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.Sampai = Mjadawal.Sampai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
            //return ctx.PelaksanaanBukaAmplops.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
        }

        public PersetujuanBukaAmplop AddPersetujuanBukaAmplop(Guid PengadaanId, Guid UserId)
        {
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(
                d => d.PengadaanId == PengadaanId && d.PersonilId == UserId
                    && (d.tipe == PengadaanConstants.StaffPeranan.PIC ||
                    d.tipe == PengadaanConstants.StaffPeranan.Compliance ||
                    d.tipe == PengadaanConstants.StaffPeranan.Controller ||
                    d.tipe == PengadaanConstants.StaffPeranan.Staff)).FirstOrDefault();
            if (picPersonil == null) return new PersetujuanBukaAmplop();

            PersetujuanBukaAmplop mmPersetujuan = ctx.PersetujuanBukaAmplops.Where(d => d.PengadaanId == PengadaanId
                    && d.UserId == UserId).FirstOrDefault();
            if (mmPersetujuan != null) return mmPersetujuan;
            PersetujuanBukaAmplop mPersetujuanBukaAmplop = new PersetujuanBukaAmplop();
            mPersetujuanBukaAmplop.PengadaanId = PengadaanId;
            mPersetujuanBukaAmplop.UserId = UserId;
            ctx.PersetujuanBukaAmplops.Add(mPersetujuanBukaAmplop);
            ctx.SaveChanges();
            return mPersetujuanBukaAmplop;
        }

        public List<VWPErsetujuanBukaAmplop> getPersetujuanBukaAmplop(Guid PengadaanId, Guid UserId)
        {
            //Guid manajer =new Guid( ConfigurationManager.AppSettings["manajer"].ToString());
            List<VWPErsetujuanBukaAmplop> lstPErsetujuan =
                (from b in ctx.PersetujuanBukaAmplops
                 join c in ctx.PersonilPengadaans on b.PengadaanId equals c.PengadaanId //into ps
                 where b.PengadaanId == PengadaanId && c.PersonilId == b.UserId
                 select new VWPErsetujuanBukaAmplop
                 {
                     Id = b.Id,
                     tipe = c.tipe,
                     UserId = c.PersonilId,
                     PengadaanId = c.PengadaanId,
                 }).ToList();
            return lstPErsetujuan;
        }

        public JadwalPelaksanaan AddPelaksanaanPenilaian(JadwalPelaksanaan pelaksanaanPenilaian, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(pelaksanaanPenilaian.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == pelaksanaanPenilaian.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new JadwalPelaksanaan();
            if (ctx.Pengadaans.Find(pelaksanaanPenilaian.PengadaanId) != null)
            {
                // PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
                JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == pelaksanaanPenilaian.PengadaanId
                        && d.statusPengadaan == EStatusPengadaan.PENILAIAN).FirstOrDefault();
                if (MjadwalPelaksanaan != null)
                {
                    MjadwalPelaksanaan.Mulai = pelaksanaanPenilaian.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanPenilaian.Sampai;
                }
                else
                {
                    MjadwalPelaksanaan = new JadwalPelaksanaan();
                    MjadwalPelaksanaan.PengadaanId = pelaksanaanPenilaian.PengadaanId;
                    MjadwalPelaksanaan.statusPengadaan = EStatusPengadaan.PENILAIAN;
                    MjadwalPelaksanaan.Mulai = pelaksanaanPenilaian.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanPenilaian.Sampai;
                    ctx.JadwalPelaksanaans.Add(MjadwalPelaksanaan);
                }
                ctx.SaveChanges();
                return MjadwalPelaksanaan;
            }
            return new JadwalPelaksanaan();
        }

        public JadwalPelaksanaan getPelaksanaanPenilaian(Guid PengadaanId, Guid UserId)
        {
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId
                           && d.statusPengadaan == EStatusPengadaan.PENILAIAN).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                MOJadwalPelaksanaan.Sampai = MjadwalPelaksanaan.Sampai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.Penilaian).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.Sampai = Mjadawal.Sampai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
            //return ctx.PelaksanaanBukaAmplops.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
        }

        public JadwalPelaksanaan AddPelaksanaanKlarifikasi(JadwalPelaksanaan pelaksanaanKlarifikasi, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(pelaksanaanKlarifikasi.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == pelaksanaanKlarifikasi.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new JadwalPelaksanaan();
            if (ctx.Pengadaans.Find(pelaksanaanKlarifikasi.PengadaanId) != null)
            {
                // PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
                JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == pelaksanaanKlarifikasi.PengadaanId
                        && d.statusPengadaan == EStatusPengadaan.KLARIFIKASI).FirstOrDefault();
                if (MjadwalPelaksanaan != null)
                {
                    MjadwalPelaksanaan.Mulai = pelaksanaanKlarifikasi.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanKlarifikasi.Sampai;
                }
                else
                {
                    MjadwalPelaksanaan = new JadwalPelaksanaan();
                    MjadwalPelaksanaan.PengadaanId = pelaksanaanKlarifikasi.PengadaanId;
                    MjadwalPelaksanaan.statusPengadaan = EStatusPengadaan.KLARIFIKASI;
                    MjadwalPelaksanaan.Mulai = pelaksanaanKlarifikasi.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanKlarifikasi.Sampai;
                    ctx.JadwalPelaksanaans.Add(MjadwalPelaksanaan);
                }
                ctx.SaveChanges();
                return MjadwalPelaksanaan;
            }
            return new JadwalPelaksanaan();
        }

        public JadwalPelaksanaan getPelaksanaanKlarifikasi(Guid PengadaanId, Guid UserId)
        {
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId
                           && d.statusPengadaan == EStatusPengadaan.KLARIFIKASI).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                MOJadwalPelaksanaan.Sampai = MjadwalPelaksanaan.Sampai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.Klarifikasi).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.Sampai = Mjadawal.Sampai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
            //return ctx.PelaksanaanBukaAmplops.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
        }

        public JadwalPelaksanaan AddPelaksanaanPemenang(JadwalPelaksanaan pelaksanaanPemenang, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(pelaksanaanPemenang.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == pelaksanaanPemenang.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new JadwalPelaksanaan();
            if (ctx.Pengadaans.Find(pelaksanaanPemenang.PengadaanId) != null)
            {
                // PelaksanaanAanwijzing MpelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings.Where(d => d.PengadaanId == pelaksanaanAanwijzing.PengadaanId).FirstOrDefault();
                JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == pelaksanaanPemenang.PengadaanId
                        && d.statusPengadaan == EStatusPengadaan.PEMENANG).FirstOrDefault();
                if (MjadwalPelaksanaan != null)
                {
                    MjadwalPelaksanaan.Mulai = pelaksanaanPemenang.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanPemenang.Sampai;
                }
                else
                {
                    MjadwalPelaksanaan = new JadwalPelaksanaan();
                    MjadwalPelaksanaan.PengadaanId = pelaksanaanPemenang.PengadaanId;
                    MjadwalPelaksanaan.statusPengadaan = EStatusPengadaan.PEMENANG;
                    MjadwalPelaksanaan.Mulai = pelaksanaanPemenang.Mulai;
                    MjadwalPelaksanaan.Sampai = pelaksanaanPemenang.Sampai;
                    ctx.JadwalPelaksanaans.Add(MjadwalPelaksanaan);
                }
                ctx.SaveChanges();
                return MjadwalPelaksanaan;
            }
            return new JadwalPelaksanaan();
        }

        public JadwalPelaksanaan getPelaksanaanPemenang(Guid PengadaanId, Guid UserId)
        {
            JadwalPelaksanaan MjadwalPelaksanaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId
                           && d.statusPengadaan == EStatusPengadaan.PEMENANG).FirstOrDefault();
            if (MjadwalPelaksanaan != null)
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                MOJadwalPelaksanaan.Id = MjadwalPelaksanaan.Id;
                MOJadwalPelaksanaan.PengadaanId = MjadwalPelaksanaan.PengadaanId;
                MOJadwalPelaksanaan.Mulai = MjadwalPelaksanaan.Mulai;
                MOJadwalPelaksanaan.Sampai = MjadwalPelaksanaan.Sampai;
                return MOJadwalPelaksanaan;
            }
            else
            {
                JadwalPelaksanaan MOJadwalPelaksanaan = new JadwalPelaksanaan();
                JadwalPengadaan Mjadawal = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.PenentuanPemenang).FirstOrDefault();
                if (Mjadawal != null)
                {
                    MOJadwalPelaksanaan.Mulai = Mjadawal.Mulai;
                    MOJadwalPelaksanaan.Sampai = Mjadawal.Sampai;
                    MOJadwalPelaksanaan.PengadaanId = PengadaanId;
                    MOJadwalPelaksanaan.Pengadaan = null;
                }
                return MOJadwalPelaksanaan;
            }
            //return ctx.PelaksanaanBukaAmplops.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
        }

        public int deleteDokumenPelaksanaan(Guid Id, Guid UserId, int isApprovel)
        {
            try
            {
                DokumenPengadaan MdokPengadaan = ctx.DokumenPengadaans.Find(Id);
                int isPic = ctx.PersonilPengadaans.Where(d => d.PengadaanId == MdokPengadaan.PengadaanId && d.PersonilId == UserId && d.tipe == "pic").ToList().Count() > 0 ? 1 : 0;
                if (isPic == 0) return 0;
                if (MdokPengadaan.Tipe == TipeBerkas.NOTA || MdokPengadaan.Tipe == TipeBerkas.DOKUMENLAIN || MdokPengadaan.Tipe == TipeBerkas.NOTA) return 0;
                ctx.DokumenPengadaans.Remove(MdokPengadaan);
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        public int deleteDokumenRekanan(Guid Id, Guid UserId)
        {
            try
            {
                DokumenPengadaan MdokPengadaan = ctx.DokumenPengadaans.Find(Id);
                var vendor =ctx.Vendors.Where(d=>d.Owner==UserId).FirstOrDefault();
                if (vendor==null) return 0;
                if (MdokPengadaan == null) return 0;
                Pengadaan oPengadaan = ctx.Pengadaans.Find(MdokPengadaan.PengadaanId);
                if (MdokPengadaan.Tipe != TipeBerkas.BerkasRekanan && MdokPengadaan.Tipe != TipeBerkas.BerkasRekananKlarifikasi) return 0;
                if (MdokPengadaan.Tipe == TipeBerkas.BerkasRekanan)
                {
                    if (oPengadaan.Status != EStatusPengadaan.SUBMITPENAWARAN) return 0;
                    cekStateSubmitPenawaran(oPengadaan.Id);
                }
                if (MdokPengadaan.Tipe == TipeBerkas.BerkasRekananKlarifikasi)
                {
                    if (oPengadaan.Status != EStatusPengadaan.KLARIFIKASI) return 0;
                    cekStateKlarifikasi(oPengadaan.Id);
                }

                ctx.DokumenPengadaans.Remove(MdokPengadaan);
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }      

        public int AjukanPengadaan(Guid Id, Guid UserId, List<Guid> manager)
        {
            // Guid manajer = new Guid(ConfigurationManager.AppSettings["manajer"].ToString());

            Pengadaan Mpengadaan = ctx.Pengadaans.Find(Id);
            if (Mpengadaan.Status == EStatusPengadaan.DRAFT)
            {
                ctx.MessagePengadaans.RemoveRange(ctx.MessagePengadaans.Where(d => d.PengadaanId == Id));
                ctx.SaveChanges();
                Mpengadaan.Status = EStatusPengadaan.AJUKAN;
                MessagePengadaan mMessagePengadaan = new MessagePengadaan();
                mMessagePengadaan.Message = "Mengajukan Pengadaan";
                mMessagePengadaan.PengadaanId = Mpengadaan.Id;
                mMessagePengadaan.UserTo = manager.FirstOrDefault();
                mMessagePengadaan.FromTo = UserId;
                mMessagePengadaan.Waktu = DateTime.Now;
                mMessagePengadaan.Status = EStatusPengadaan.AJUKAN;
                ctx.MessagePengadaans.Add(mMessagePengadaan);
                ctx.SaveChanges();
            }
            try
            {
                ctx.SaveChanges();
                return 1;
            }
            catch (Exception dbEx)
            {
                return 0;
            }


            return 0;
        }

        public int UpdateStatus(Guid Id, EStatusPengadaan status)
        {
            try
            {
                Pengadaan Mpengadaan = ctx.Pengadaans.Find(Id);
                Mpengadaan.Status = status;
                ctx.SaveChanges();
                return 1;
            }
            catch { return 0; }
        }

        //public StateJadwalPengadaan stateJadwal(EStatusPengadaan status, Guid PengadaanId)
        //{
        //    StateJadwalPengadaan MStateJadwalPengadaan = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId &&
        //                        d.statusPengadaan == status).Select(d=>new StateJadwalPengadaan{
        //                        PengadaanId=d.PengadaanId,
        //                        Mulai=d.Mulai,
        //                        Sampai=d.Sampai,
        //                        status=d.statusPengadaan
        //                        }).FirstOrDefault();
        //    if (MStateJadwalPengadaan == null)
        //    {
        //        var lstItem = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId);
        //        if (status == EStatusPengadaan.DISETUJUI)
        //            lstItem = lstItem.Where(d => d.tipe == PengadaanConstants.Jadwal.Aanwijzing);
        //        if (status == EStatusPengadaan.AANWIJZING)
        //            lstItem = lstItem.Where(d => d.tipe == PengadaanConstants.Jadwal.PengisianHarga);
        //        if (status == EStatusPengadaan.SUBMITPENAWARAN)
        //            lstItem = lstItem.Where(d => d.tipe == PengadaanConstants.Jadwal.BukaAmplop);
        //        if (status == EStatusPengadaan.BUKAAMPLOP)
        //            lstItem = lstItem.Where(d => d.tipe == PengadaanConstants.Jadwal.Penilaian);
        //    }
        //    return MStateJadwalPengadaan;
        //}

        public JadwalPelaksanaan saveJadwalPelaksanaan(JadwalPelaksanaan jadwalPelaksanaan)
        {
            Pengadaan Mpengadaan = ctx.Pengadaans.Where(d => d.Id == jadwalPelaksanaan.PengadaanId).FirstOrDefault();
            if (Mpengadaan == null) return new JadwalPelaksanaan();
            JadwalPelaksanaan MJadwalPelaksanaan = new JadwalPelaksanaan
            {
                PengadaanId = jadwalPelaksanaan.PengadaanId,
                Mulai = jadwalPelaksanaan.Mulai,
                Sampai = jadwalPelaksanaan.Sampai,
                statusPengadaan = jadwalPelaksanaan.statusPengadaan
            };
            ctx.JadwalPelaksanaans.Add(MJadwalPelaksanaan);

            return MJadwalPelaksanaan;
        }

        //public StateJadwalPengadaan stateJadwal( Guid PengadaanId)
        //{
        //    Pengadaan Mpengadaan = ctx.Pengadaans.Find(PengadaanId);
        //    StateJadwalPengadaan MnextJadwal = new StateJadwalPengadaan();
        //    if (Mpengadaan.Status == EStatusPengadaan.DISETUJUI)
        //    {
        //        JadwalPelaksanaan MPJadwalPelaksanaan = ctx.JadwalPelaksanaans
        //                    .Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
        //        if (MPJadwalPelaksanaan != null)
        //        {
        //            MnextJadwal.Mulai = MPJadwalPelaksanaan.Mulai;
        //            MnextJadwal.status = EStatusPengadaan.AANWIJZING;
        //            MnextJadwal.PengadaanId = PengadaanId;
        //        }
        //        else
        //        {
        //            JadwalPengadaan MJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId
        //                             && d.tipe == PengadaanConstants.StatusPengadaan.Aanwijzing).FirstOrDefault();
        //            if (MJadwalPengadaan == null) return new StateJadwalPengadaan();
        //            else
        //            {
        //                MnextJadwal.Mulai = MJadwalPengadaan.Mulai;
        //                MnextJadwal.PengadaanId = PengadaanId;
        //                MnextJadwal.status = EStatusPengadaan.AANWIJZING;
        //            }
        //        }
        //    }

        //    if (Mpengadaan.Status == EStatusPengadaan.AANWIJZING)
        //    {
        //        JadwalPelaksanaan MPJadwalPelaksanaan = ctx.JadwalPelaksanaans
        //                    .Where(d => d.PengadaanId == PengadaanId && d.statusPengadaan==Mpengadaan.Status).FirstOrDefault();
        //        if (MPelaksanaanAanwijzing != null)
        //        {
        //            MnextJadwal.Mulai = MPelaksanaanAanwijzing.Mulai;
        //            MnextJadwal.status = EStatusPengadaan.AANWIJZING;
        //            MnextJadwal.PengadaanId = PengadaanId;
        //        }
        //        else
        //        {
        //            JadwalPengadaan MJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId
        //                             && d.tipe ==PengadaanConstants.StatusPengadaan.Aanwijzing).FirstOrDefault();
        //            if (MJadwalPengadaan == null) return new StateJadwalPengadaan();
        //            else
        //            {
        //                MnextJadwal.Mulai = MJadwalPengadaan.Mulai;
        //                MnextJadwal.PengadaanId = PengadaanId;
        //                MnextJadwal.status = EStatusPengadaan.AANWIJZING;
        //            }
        //        }
        //    }

        //    return MnextJadwal;
        //}

        public int cekStateDiSetujui(Guid PengadaanId)
        {
            DateTime? anwijzingDate = new DateTime();
            JadwalPelaksanaan MJadwalPelaksanaan = ctx.JadwalPelaksanaans
                    .Where(d => d.statusPengadaan == EStatusPengadaan.AANWIJZING && d.PengadaanId == PengadaanId).FirstOrDefault();

            if (MJadwalPelaksanaan == null)
            {
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                                    d.tipe == PengadaanConstants.Jadwal.Aanwijzing).FirstOrDefault();
                anwijzingDate = mJadwalPengadaan.Mulai;
                // if (mJadwalPengadaan == null) return 0;
                //anwijzingDate = mJadwalPengadaan.Mulai;
                //if (mJadwalPengadaan.Mulai < DateTime.Now)
                //{
                //    JadwalPelaksanaan mmJadwalPelaksanaan = new JadwalPelaksanaan();
                //    mmJadwalPelaksanaan.PengadaanId = PengadaanId;
                //    mmJadwalPelaksanaan.Mulai = DateTime.Now.AddDays(1);
                //    mmJadwalPelaksanaan.statusPengadaan = EStatusPengadaan.AANWIJZING;
                //    ctx.JadwalPelaksanaans.Add(mmJadwalPelaksanaan);
                //    ctx.SaveChanges();
                //    UpdateStatus(PengadaanId, EStatusPengadaan.AANWIJZING);
                //}
                //return 1;

            }
            else
            {
                anwijzingDate = MJadwalPelaksanaan.Mulai;
            }
            if (anwijzingDate == null) return 0;
            if (DateTime.Now < anwijzingDate)
            {
                return 0;
            }
            else
            {
                //MJadwalPelaksanaan.Mulai = DateTime.Now.AddDays(1);
                UpdateStatus(PengadaanId, EStatusPengadaan.AANWIJZING);
                return 1;
            }

        }

        public int cekStateAanwijzing(Guid PengadaanId)
        {
            DateTime? anwijzingDate = new DateTime();
            JadwalPelaksanaan MJadwalPelaksanaan = ctx.JadwalPelaksanaans
                    .Where(d => d.statusPengadaan == EStatusPengadaan.SUBMITPENAWARAN && d.PengadaanId == PengadaanId).FirstOrDefault();

            if (MJadwalPelaksanaan == null)
            {
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                                    d.tipe == PengadaanConstants.Jadwal.PengisianHarga).FirstOrDefault();
                if (mJadwalPengadaan == null) return 0;
                anwijzingDate = mJadwalPengadaan.Mulai;
            }
            else anwijzingDate = MJadwalPelaksanaan.Mulai;

            if (anwijzingDate == null) return 0;
            if (DateTime.Now < anwijzingDate)
            {
                return 0;
            }
            else
            {
                UpdateStatus(PengadaanId, EStatusPengadaan.SUBMITPENAWARAN);
                return 1;
            }
        }

        public int cekStateSubmitPenawaran(Guid PengadaanId)
        {
            DateTime? SubmitPenawaranDate = new DateTime();


            JadwalPelaksanaan MJadwalPelaksanaan = ctx.JadwalPelaksanaans
                    .Where(d => d.statusPengadaan == EStatusPengadaan.SUBMITPENAWARAN && d.PengadaanId == PengadaanId).FirstOrDefault();

            if (MJadwalPelaksanaan == null)
            {
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                                    d.tipe == PengadaanConstants.Jadwal.PengisianHarga).FirstOrDefault();

                if (mJadwalPengadaan == null) return 0;
                SubmitPenawaranDate = mJadwalPengadaan.Sampai;
            }
            else
            {
                SubmitPenawaranDate = MJadwalPelaksanaan.Sampai;
            }
            if (SubmitPenawaranDate == null)
            {
                return 0;
            }
            if (DateTime.Now < SubmitPenawaranDate) return 0;
            else
            {
                //JadwalPengadaan MMJadwalPengadaan = ctx.JadwalPengadaans
                //    .Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.PengisianHarga)
                //    .FirstOrDefault();
                //if (SubmitPenawaranDate < DateTime.Now)
                //{
                //    JadwalPelaksanaan mmJadwalPelaksanaan = new JadwalPelaksanaan();
                //    mmJadwalPelaksanaan.Sampai = DateTime.Now.AddDays(1);
                //    mmJadwalPelaksanaan.PengadaanId = PengadaanId;
                //    mmJadwalPelaksanaan.Mulai=MMJadwalPengadaan.Mulai;
                //    mmJadwalPelaksanaan.statusPengadaan=EStatusPengadaan.BUKAAMPLOP;
                //    ctx.JadwalPelaksanaans.Add(mmJadwalPelaksanaan);
                //    ctx.SaveChanges();
                //}

                UpdateStatus(PengadaanId, EStatusPengadaan.BUKAAMPLOP);
                return 1;
            }
        }

        public int cekStateBukaAmplop(Guid PengadaanId)
        {
            DateTime? BukaAmplopDate = new DateTime();
            JadwalPelaksanaan MJadwalPelaksanaan = ctx.JadwalPelaksanaans
                    .Where(d => d.statusPengadaan == EStatusPengadaan.BUKAAMPLOP && d.PengadaanId == PengadaanId).FirstOrDefault();

            if (MJadwalPelaksanaan == null)
            {
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                                    d.tipe == PengadaanConstants.Jadwal.BukaAmplop).FirstOrDefault();
                if (mJadwalPengadaan == null) return 0;
                BukaAmplopDate = mJadwalPengadaan.Sampai;
            }
            else BukaAmplopDate = MJadwalPelaksanaan.Sampai;

            if (BukaAmplopDate == null)
            {
                return 0;
            }
            if (DateTime.Now < BukaAmplopDate) return 0;
            else
            {
                //JadwalPengadaan MMJadwalPengadaan = ctx.JadwalPengadaans
                //    .Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.BukaAmplop)
                //    .FirstOrDefault();
                //if (MMJadwalPengadaan.Sampai < DateTime.Now)
                //{
                //    JadwalPelaksanaan mmJadwalPelaksanaan = new JadwalPelaksanaan();
                //    mmJadwalPelaksanaan.Sampai = DateTime.Now.AddDays(1);
                //    mmJadwalPelaksanaan.PengadaanId = PengadaanId;
                //    mmJadwalPelaksanaan.Mulai = MMJadwalPengadaan.Mulai;
                //    mmJadwalPelaksanaan.statusPengadaan = EStatusPengadaan.PENILAIAN;
                //    ctx.JadwalPelaksanaans.Add(mmJadwalPelaksanaan);
                //    ctx.SaveChanges();
                //}
                UpdateStatus(PengadaanId, EStatusPengadaan.PENILAIAN);
                return 1;
            }
        }

        public int cekStatePenilaian(Guid PengadaanId)
        {
            DateTime? PenilaianDate = new DateTime();
            JadwalPelaksanaan MJadwalPelaksanaan = ctx.JadwalPelaksanaans
                    .Where(d => d.statusPengadaan == EStatusPengadaan.PENILAIAN && d.PengadaanId == PengadaanId).FirstOrDefault();

            if (MJadwalPelaksanaan == null)
            {
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                                    d.tipe == PengadaanConstants.Jadwal.Penilaian).FirstOrDefault();
                if (mJadwalPengadaan == null) return 0;
                PenilaianDate = mJadwalPengadaan.Sampai;
            }
            else PenilaianDate = MJadwalPelaksanaan.Sampai;

            if (PenilaianDate == null)
            {
                return 0;
            }
            if (DateTime.Now < PenilaianDate) return 0;
            else
            {
                //JadwalPengadaan MMJadwalPengadaan = ctx.JadwalPengadaans
                //    .Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.Penilaian)
                //    .FirstOrDefault();
                //if (MMJadwalPengadaan.Sampai < DateTime.Now)
                //{
                //    JadwalPelaksanaan mmJadwalPelaksanaan = new JadwalPelaksanaan();
                //    mmJadwalPelaksanaan.Sampai = DateTime.Now.AddDays(1);
                //    mmJadwalPelaksanaan.PengadaanId = PengadaanId;
                //    mmJadwalPelaksanaan.Mulai = MMJadwalPengadaan.Mulai;
                //    mmJadwalPelaksanaan.statusPengadaan = EStatusPengadaan.PENILAIAN;
                //    ctx.JadwalPelaksanaans.Add(mmJadwalPelaksanaan);
                //    ctx.SaveChanges();
                //}
                UpdateStatus(PengadaanId, EStatusPengadaan.KLARIFIKASI);
                return 1;
            }
        }

        public int cekStateKlarifikasi(Guid PengadaanId)
        {
            DateTime? KlarifikasiDate = new DateTime();
            JadwalPelaksanaan MJadwalPelaksanaan = ctx.JadwalPelaksanaans
                    .Where(d => d.statusPengadaan == EStatusPengadaan.KLARIFIKASI && d.PengadaanId == PengadaanId).FirstOrDefault();

            if (MJadwalPelaksanaan == null)
            {
                JadwalPengadaan mJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                                    d.tipe == PengadaanConstants.Jadwal.Klarifikasi).FirstOrDefault();
                if (mJadwalPengadaan == null) return 0;
                KlarifikasiDate = mJadwalPengadaan.Sampai;
            }
            else KlarifikasiDate = MJadwalPelaksanaan.Sampai;

            if (KlarifikasiDate == null)
            {
                return 0;
            }
            if (DateTime.Now < KlarifikasiDate) return 0;
            else
            {
                //JadwalPengadaan MMJadwalPengadaan = ctx.JadwalPengadaans
                //    .Where(d => d.PengadaanId == PengadaanId && d.tipe == PengadaanConstants.Jadwal.Klarifikasi)
                //    .FirstOrDefault();
                //if (MMJadwalPengadaan.Sampai < DateTime.Now)
                //{
                //    JadwalPelaksanaan mmJadwalPelaksanaan = new JadwalPelaksanaan();
                //    mmJadwalPelaksanaan.Sampai = DateTime.Now.AddDays(1);
                //    mmJadwalPelaksanaan.PengadaanId = PengadaanId;
                //    mmJadwalPelaksanaan.Mulai = MMJadwalPengadaan.Mulai;
                //    mmJadwalPelaksanaan.statusPengadaan = EStatusPengadaan.PENILAIAN;
                //    ctx.JadwalPelaksanaans.Add(mmJadwalPelaksanaan);
                //    ctx.SaveChanges();
                //}
                UpdateStatus(PengadaanId, EStatusPengadaan.PEMENANG);
                return 1;
            }
        }

        public KualifikasiKandidat addKualifikasiKandidat(KualifikasiKandidat dKualifikasiKandidat, Guid UserId)
        {
            if (ctx.Pengadaans.Find(dKualifikasiKandidat.PengadaanId) == null) return dKualifikasiKandidat;
            ctx.KualifikasiKandidats.Add(dKualifikasiKandidat);
            ctx.SaveChanges();
            return dKualifikasiKandidat;
        }

        //rekanana masukan harga tawaran pada submit penawaran
        public List<VWRKSDetailRekanan> addHargaRekanan(List<VWRKSDetailRekanan> dlstHargaRekanan, Guid PengadaanId, Guid UserId)
        {
            JimbisEncrypt code = new JimbisEncrypt();
            List<VWRKSDetailRekanan> newLstVWRKSDetailRekanan = new List<VWRKSDetailRekanan>();
            if (ctx.Pengadaans.Find(PengadaanId) == null) return new List<VWRKSDetailRekanan>();
            else
            {
                if (ctx.Pengadaans.Find(PengadaanId).Status != EStatusPengadaan.SUBMITPENAWARAN) return new List<VWRKSDetailRekanan>();
                if (cekStateSubmitPenawaran(PengadaanId) == 1) return new List<VWRKSDetailRekanan>();
            }
            foreach (var item in dlstHargaRekanan)
            {
                var vendorId = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id;
                var oldHargaRekanan = ctx.HargaRekanans.Where(d => d.RKSDetailId == item.Id && d.VendorId == vendorId).FirstOrDefault();

                //if (item.HargaRekananId != Guid.Empty && item.HargaRekananId != null)
                if (oldHargaRekanan != null)
                {
                    var hargaEncrypet = code.Encrypt(item.harga == null ? "0" : item.harga.ToString());
                    //var Dec = code.Decrypt(enco);
                    //HargaRekanan oldHargaRekanan = ctx.HargaRekanans.Find(item.HargaRekananId);
                    //HargaRekanan oldHargaRekanan = ctx.HargaRekanans.Find(item.HargaRekananId);
                    //oldHargaRekanan.harga = item.harga;
                    oldHargaRekanan.hargaEncrypt = hargaEncrypet;
                    oldHargaRekanan.keterangan = item.keterangan;
                    ctx.SaveChanges();

                    newLstVWRKSDetailRekanan.Add(new VWRKSDetailRekanan
                    {
                        Id = ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId).FirstOrDefault().Id,
                        item = ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId).FirstOrDefault().item,
                        ItemId = ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId).FirstOrDefault().ItemId,
                        jumlah = ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId) == null ? null : ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId).FirstOrDefault().jumlah,
                        satuan = ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == oldHargaRekanan.RKSDetailId).FirstOrDefault().satuan,
                        harga = oldHargaRekanan.harga,
                        HargaRekananId = oldHargaRekanan.Id,
                        keterangan = oldHargaRekanan.keterangan
                    });
                }
                else
                {
                    HargaRekanan newHargaRekanan = new HargaRekanan();
                    var hargaEncrypet = code.Encrypt(item.harga == null ? "0" : item.harga.ToString());
                    //newHargaRekanan.harga = item.harga;
                    newHargaRekanan.hargaEncrypt = hargaEncrypet;
                    newHargaRekanan.keterangan = item.keterangan;
                    newHargaRekanan.RKSDetailId = item.Id;
                    newHargaRekanan.VendorId = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id; //3;//contoh vendor
                    ctx.HargaRekanans.Add(newHargaRekanan);
                    ctx.SaveChanges();
                    newLstVWRKSDetailRekanan.Add(new VWRKSDetailRekanan
                    {
                        Id = ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId).FirstOrDefault().Id,
                        item = ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId).FirstOrDefault().item,
                        ItemId = ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId).FirstOrDefault().ItemId,
                        jumlah = ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId) == null ? null : ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId).FirstOrDefault().jumlah,
                        satuan = ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == newHargaRekanan.RKSDetailId).FirstOrDefault().satuan,
                        harga = newHargaRekanan.harga,
                        HargaRekananId = newHargaRekanan.Id,
                        keterangan = newHargaRekanan.keterangan
                    });
                }

            }

            return newLstVWRKSDetailRekanan;
        }

        //rekanana masukan harga tawaran pada klarifikasi 
        public List<VWRKSDetailRekanan> addHargaKlarifikasiRekanan(List<VWRKSDetailRekanan> dlstHargaKlarifikasiRekanan, Guid PengadaanId, Guid UserId)
        {
            List<VWRKSDetailRekanan> newLstVWRKSDetailRekanan = new List<VWRKSDetailRekanan>();
            if (ctx.Pengadaans.Find(PengadaanId) == null) return new List<VWRKSDetailRekanan>();
            else
            {
                if (ctx.Pengadaans.Find(PengadaanId).Status != EStatusPengadaan.KLARIFIKASI) return new List<VWRKSDetailRekanan>();
            }
            foreach (var item in dlstHargaKlarifikasiRekanan)
            {
                var vendorId = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id;
                HargaKlarifikasiRekanan oldHargaKlarifikasiRekanan = ctx.HargaKlarifikasiRekanans.Where(d => d.VendorId == vendorId && d.RKSDetailId == item.Id).FirstOrDefault();
                //if (item.HargaRekananId != Guid.Empty && item.HargaRekananId != null)
                if (oldHargaKlarifikasiRekanan != null)
                {

                    oldHargaKlarifikasiRekanan.harga = item.harga;
                    oldHargaKlarifikasiRekanan.keterangan = item.keterangan;
                    ctx.SaveChanges();
                    newLstVWRKSDetailRekanan.Add(new VWRKSDetailRekanan
                    {
                        Id = ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().Id,
                        item = ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().item,
                        ItemId = ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().ItemId,
                        jumlah = ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId) == null ? null : ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().jumlah,
                        satuan = ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == oldHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().satuan,
                        harga = oldHargaKlarifikasiRekanan.harga,
                        HargaRekananId = oldHargaKlarifikasiRekanan.Id,
                        keterangan = oldHargaKlarifikasiRekanan.keterangan
                    });
                }
                else
                {
                    HargaKlarifikasiRekanan newHargaKlarifikasiRekanan = new HargaKlarifikasiRekanan();
                    newHargaKlarifikasiRekanan.harga = item.harga;
                    newHargaKlarifikasiRekanan.keterangan = item.keterangan;
                    newHargaKlarifikasiRekanan.RKSDetailId = item.Id;
                    newHargaKlarifikasiRekanan.VendorId = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id; //3;//contoh vendor
                    ctx.HargaKlarifikasiRekanans.Add(newHargaKlarifikasiRekanan);
                    ctx.SaveChanges();
                    newLstVWRKSDetailRekanan.Add(new VWRKSDetailRekanan
                    {
                        Id = ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().Id,
                        item = ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().item,
                        ItemId = ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId) == null ? Guid.Empty : ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().ItemId,
                        jumlah = ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId) == null ? null : ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().jumlah,
                        satuan = ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId) == null ? "" : ctx.RKSDetails.Where(d => d.Id == newHargaKlarifikasiRekanan.RKSDetailId).FirstOrDefault().satuan,
                        harga = newHargaKlarifikasiRekanan.harga,
                        HargaRekananId = newHargaKlarifikasiRekanan.Id,
                        keterangan = newHargaKlarifikasiRekanan.keterangan
                    });
                }

            }

            return newLstVWRKSDetailRekanan;
        }

        public List<VWRekananSubmitHarga> getListRekananSubmit(Guid PengadaanId, Guid UserId)
        {
            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans where b.PengadaanId == PengadaanId select b).ToList();
            var xSubmitRekanan = (from b in ctx.HargaRekanans
                                  join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                  join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                  where d.PengadaanId == PengadaanId
                                  select b).Distinct().ToList();
            List<VWRekananSubmitHarga> lstVWRekananSubmitHarga = new List<VWRekananSubmitHarga>();
            foreach (var item in xKandidatPengadaans)
            {
                VWRekananSubmitHarga mVWRekananSubmitHarga = new VWRekananSubmitHarga();
                mVWRekananSubmitHarga.VendorId = item.VendorId;
                mVWRekananSubmitHarga.status = 0;
                mVWRekananSubmitHarga.NamaVendor = ctx.Vendors.Find(item.VendorId).Nama;

                foreach (var itemx in xSubmitRekanan)
                {
                    if (itemx.VendorId == item.VendorId) mVWRekananSubmitHarga.status = 1;
                }
                lstVWRekananSubmitHarga.Add(mVWRekananSubmitHarga);
            }
            return lstVWRekananSubmitHarga;
        }

        public List<VWRekananPenilaian> getListRekananPenilaian(Guid PengadaanId, Guid UserId)
        {
            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans
                                       join c in ctx.Vendors on b.VendorId equals c.Id
                                       where b.PengadaanId == PengadaanId
                                       select new VWRekananPenilaian
                                       {
                                           NamaVendor = c.Nama,
                                           VendorId = b.VendorId,
                                           NilaiKriteria = (from bb in ctx.PembobotanPengadaans
                                                            join cc in ctx.PembobotanPengadaanVendors on bb.KreteriaPembobotanId equals cc.KreteriaPembobotanId
                                                            where cc.PengadaanId == PengadaanId && cc.VendorId == b.VendorId && bb.PengadaanId == PengadaanId
                                                            select new
                                                            {
                                                                Bobot = bb.Bobot == null ? 0 : bb.Bobot.Value,
                                                                Nilai = cc.Nilai == null ? 0 : cc.Nilai.Value
                                                            }).Sum(dd => (dd.Nilai * dd.Bobot) / 100),
                                           total = (from bb in ctx.HargaRekanans
                                                    join cc in ctx.RKSDetails on bb.RKSDetailId equals cc.Id
                                                    join dd in ctx.RKSHeaders on cc.RKSHeaderId equals dd.Id
                                                    where dd.PengadaanId == PengadaanId && bb.VendorId == b.VendorId
                                                    select new item
                                                    {
                                                        harga = bb.harga,
                                                        jumlah = cc.jumlah
                                                    }).Sum(xx => xx.harga * xx.jumlah),
                                           terpilih = (from bb in ctx.PelaksanaanPemilihanKandidats
                                                       where bb.PengadaanId == PengadaanId &&
                                                       bb.VendorId == b.VendorId
                                                       select bb).FirstOrDefault() == null ? 0 : 1
                                       }).ToList();
            return xKandidatPengadaans;
        }

        public List<VWRekananPenilaian> getListPenilaianByVendor(Guid PengadaanId, Guid UserId, int VendorId)
        {
            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans
                                       join c in ctx.Vendors on b.VendorId equals c.Id
                                       where b.PengadaanId == PengadaanId & b.VendorId == VendorId
                                       select new VWRekananPenilaian
                                       {
                                           NamaVendor = c.Nama,
                                           VendorId = b.VendorId,
                                           NilaiKriteria = (from bb in ctx.PembobotanPengadaans
                                                            join cc in ctx.PembobotanPengadaanVendors on bb.KreteriaPembobotanId equals cc.KreteriaPembobotanId
                                                            where cc.PengadaanId == PengadaanId && cc.VendorId == b.VendorId && bb.PengadaanId == PengadaanId
                                                            select new
                                                            {
                                                                Bobot = bb.Bobot == null ? 0 : bb.Bobot.Value,
                                                                Nilai = cc.Nilai == null ? 0 : cc.Nilai.Value
                                                            }).Sum(dd => (dd.Nilai * dd.Bobot) / 100),
                                           total = (from bb in ctx.HargaRekanans
                                                    join cc in ctx.RKSDetails on bb.RKSDetailId equals cc.Id
                                                    join dd in ctx.RKSHeaders on cc.RKSHeaderId equals dd.Id
                                                    where dd.PengadaanId == PengadaanId && bb.VendorId == b.VendorId
                                                    select new item
                                                    {
                                                        harga = bb.harga,
                                                        jumlah = cc.jumlah
                                                    }).Sum(xx => xx.harga * xx.jumlah),
                                           terpilih = (from bb in ctx.PelaksanaanPemilihanKandidats
                                                       where bb.PengadaanId == PengadaanId &&
                                                       bb.VendorId == b.VendorId
                                                       select bb).FirstOrDefault() == null ? 0 : 1
                                       }).ToList();
            return xKandidatPengadaans;
        }

        public List<VWRekananSubmitHarga> getListRekananKlarifikasiSubmit(Guid PengadaanId, Guid UserId)
        {
            var xRekanan = (from b in ctx.PelaksanaanPemilihanKandidats
                            where b.PengadaanId == PengadaanId
                            select b).Distinct().ToList();

            var xSubmitRekanan = (from b in ctx.HargaKlarifikasiRekanans
                                  join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                  join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                  where d.PengadaanId == PengadaanId
                                  select b).Distinct().ToList();

            var LstVendor = xRekanan.Select(d => new { VendorId = d.VendorId }).Distinct().ToList();
            List<VWRekananSubmitHarga> lstVWRekananSubmitHarga = new List<VWRekananSubmitHarga>();
            foreach (var item in LstVendor)
            {
                VWRekananSubmitHarga mVWRekananSubmitHarga = new VWRekananSubmitHarga();
                mVWRekananSubmitHarga.VendorId = item.VendorId;
                mVWRekananSubmitHarga.status = 0;
                mVWRekananSubmitHarga.NamaVendor = ctx.Vendors.Find(item.VendorId).Nama;

                foreach (var itemx in xSubmitRekanan)
                {
                    if (itemx.VendorId == item.VendorId) mVWRekananSubmitHarga.status = 1;
                }
                lstVWRekananSubmitHarga.Add(mVWRekananSubmitHarga);
            }
            return lstVWRekananSubmitHarga;
        }

        public List<VWRekananPenilaian> getListRekananKlarifikasiPenilaian(Guid PengadaanId, Guid UserId)
        {
            var xKandidatPengadaans = (from b in ctx.PelaksanaanPemilihanKandidats
                                       join c in ctx.Vendors on b.VendorId equals c.Id
                                       where b.PengadaanId == PengadaanId
                                       select new VWRekananPenilaian
                                       {
                                           NamaVendor = c.Nama,
                                           VendorId = b.VendorId,
                                           total = (from bb in ctx.HargaKlarifikasiRekanans
                                                    join cc in ctx.RKSDetails on bb.RKSDetailId equals cc.Id
                                                    join dd in ctx.RKSHeaders on cc.RKSHeaderId equals dd.Id
                                                    where dd.PengadaanId == PengadaanId && bb.VendorId == b.VendorId
                                                    select new item
                                                    {
                                                        harga = bb.harga,
                                                        jumlah = cc.jumlah
                                                    }).Sum(xx => xx.harga * xx.jumlah),
                                           terpilih = (from bb in ctx.PemenangPengadaans
                                                       where bb.PengadaanId == PengadaanId &&
                                                       bb.VendorId == b.VendorId
                                                       select bb).FirstOrDefault() == null ? 0 : 1,
                                           NilaiKriteria = (from bb in ctx.PembobotanPengadaans
                                                            join cc in ctx.PembobotanPengadaanVendors on bb.KreteriaPembobotanId equals cc.KreteriaPembobotanId
                                                            where cc.PengadaanId == PengadaanId && cc.VendorId == b.VendorId && bb.PengadaanId == PengadaanId
                                                            select new
                                                            {
                                                                Bobot = bb.Bobot == null ? 0 : bb.Bobot.Value,
                                                                Nilai = cc.Nilai == null ? 0 : cc.Nilai.Value
                                                            }).Sum(dd => (dd.Nilai * dd.Bobot) / 100)
                                       }).ToList();
            return xKandidatPengadaans;
        }

        public int addPemenangPengadaan(PemenangPengadaan dtPemenangPengadaan, Guid UserId)
        {
            var oPIC = ctx.PersonilPengadaans.Where(d => d.PengadaanId == dtPemenangPengadaan.PengadaanId &&
                d.tipe == PengadaanConstants.StaffPeranan.PIC && d.PersonilId == UserId).FirstOrDefault();
            if (oPIC == null) return 0;
            var oPemenangPengadaan = ctx.PemenangPengadaans.Where(d => d.PengadaanId == dtPemenangPengadaan.PengadaanId).FirstOrDefault();//&& d.VendorId==dtPemenangPengadaan.VendorId

            if (oPemenangPengadaan == null)
            {
                dtPemenangPengadaan.CreatedBy = UserId;
                dtPemenangPengadaan.CreateOn = DateTime.Now;
                ctx.PemenangPengadaans.Add(dtPemenangPengadaan);
            }
            else
            {
                oPemenangPengadaan.ModifiedBy = UserId;
                oPemenangPengadaan.ModifiedOn = DateTime.Now;
                oPemenangPengadaan.VendorId = dtPemenangPengadaan.VendorId;
            }

            try
            {
                ctx.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }

        }

        public VWRKSVendors getRKSPenilaian(Guid PengadaanId, Guid UserId)
        {
            VWRKSVendors newVWRKSVendors = new VWRKSVendors();

            List<VWRKSPenilaian> hps = (from b in ctx.RKSDetails
                                        join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                        where c.PengadaanId == PengadaanId
                                        select new VWRKSPenilaian
                                        {
                                            Id = b.Id,
                                            harga = b.hps,
                                            item = b.item,
                                            jumlah = b.jumlah,
                                            satuan = b.satuan
                                        }).ToList();
            newVWRKSVendors.hps = hps;
            decimal? totalHps = hps.Sum(d => d.harga * d.jumlah);
            newVWRKSVendors.hps.Insert(0, new VWRKSPenilaian { item = "Total", harga = totalHps });

            List<VWVendorsHarga> vendors = new List<VWVendorsHarga>();

            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans where b.PengadaanId == PengadaanId select b).ToList();
            foreach (var item in xKandidatPengadaans)
            {
                //into ps
                //                from c in ps.DefaultIfEmpty()
                VWVendorsHarga mVWVendorsHarga = new VWVendorsHarga();
                mVWVendorsHarga.nama = ctx.Vendors.Find(item.VendorId).Nama;
                mVWVendorsHarga.VendorId = item.VendorId;
                var oPembobotanPengadaanVendor = ctx.PembobotanPengadaanVendors.Where(
                                    d => d.PengadaanId == PengadaanId && d.VendorId == item.VendorId).ToList();
                var totalNilaiKirteria = 0;
                foreach (var itemKriteriaVendor in oPembobotanPengadaanVendor)
                {
                    var oPembobotanPengadaans = ctx.PembobotanPengadaans.Where(d => d.KreteriaPembobotanId == itemKriteriaVendor.KreteriaPembobotanId && d.PengadaanId == PengadaanId).FirstOrDefault();
                    var bobot = oPembobotanPengadaans == null ? 0 : oPembobotanPengadaans.Bobot == null ? 0 : oPembobotanPengadaans.Bobot.Value;
                    var nilai = itemKriteriaVendor.Nilai == null ? 0 : itemKriteriaVendor.Nilai.Value;
                    totalNilaiKirteria = totalNilaiKirteria + ((bobot * nilai) / 100);
                }
                mVWVendorsHarga.NIlaiKriteria = totalNilaiKirteria;

                mVWVendorsHarga.items = (from b in ctx.HargaRekanans
                                         join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                         join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                         where d.PengadaanId == PengadaanId && b.VendorId == item.VendorId
                                         select new item
                                         {
                                             jumlah = c.jumlah,
                                             harga = b.harga
                                         }).ToList();



                if (mVWVendorsHarga.items.Count > 0)
                {
                    decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                    mVWVendorsHarga.items.Insert(0, new item { harga = total });
                    vendors.Add(mVWVendorsHarga);
                }
            }
            newVWRKSVendors.vendors = vendors;
            return newVWRKSVendors;
        }

        public VWRKSVendors getRKSPenilaian2Report(Guid PengadaanId, Guid UserId)
        {
            VWRKSVendors newVWRKSVendors = new VWRKSVendors();

            List<VWRKSPenilaian> hps = (from b in ctx.RKSDetails
                                        join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                        where c.PengadaanId == PengadaanId
                                        select new VWRKSPenilaian
                                        {
                                            Id = b.Id,
                                            harga = b.hps,
                                            item = b.item,
                                            keteranganItem = b.keterangan,
                                            jumlah = b.jumlah,
                                            satuan = b.satuan
                                        }).ToList();
            newVWRKSVendors.hps = hps;
            decimal? totalHps = hps.Sum(d => d.harga * d.jumlah);
            newVWRKSVendors.hps.Add(new VWRKSPenilaian { item = "Total", harga = totalHps });

            List<VWVendorsHarga> vendors = new List<VWVendorsHarga>();

            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans where b.PengadaanId == PengadaanId select b).ToList();

            foreach (var item in xKandidatPengadaans)
            {
                //into ps
                //                from c in ps.DefaultIfEmpty()
                VWVendorsHarga mVWVendorsHarga = new VWVendorsHarga();
                mVWVendorsHarga.nama = ctx.Vendors.Find(item.VendorId).Nama;
                mVWVendorsHarga.VendorId = item.VendorId;
                var oPembobotanPengadaanVendor = ctx.PembobotanPengadaanVendors.Where(
                                    d => d.PengadaanId == PengadaanId && d.VendorId == item.VendorId).ToList();
                var totalNilaiKirteria = 0;
                foreach (var itemKriteriaVendor in oPembobotanPengadaanVendor)
                {
                    var oPembobotanPengadaans = ctx.PembobotanPengadaans.Where(d => d.KreteriaPembobotanId == itemKriteriaVendor.KreteriaPembobotanId && d.PengadaanId == PengadaanId).FirstOrDefault();
                    var bobot = oPembobotanPengadaans == null ? 0 : oPembobotanPengadaans.Bobot == null ? 0 : oPembobotanPengadaans.Bobot.Value;
                    var nilai = itemKriteriaVendor.Nilai == null ? 0 : itemKriteriaVendor.Nilai.Value;
                    totalNilaiKirteria = totalNilaiKirteria + ((bobot * nilai) / 100);
                }
                mVWVendorsHarga.NIlaiKriteria = totalNilaiKirteria;

                mVWVendorsHarga.items = (from b in ctx.HargaRekanans
                                         join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                         join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                         where d.PengadaanId == PengadaanId && b.VendorId == item.VendorId
                                         select new item
                                         {
                                             Id = c.Id,
                                             jumlah = c.jumlah,
                                             harga = b.harga
                                         }).ToList();



                if (mVWVendorsHarga.items.Count > 0)
                {
                    decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                    mVWVendorsHarga.items.Add(new item { harga = total });
                    vendors.Add(mVWVendorsHarga);
                }
            }

            newVWRKSVendors.vendors = vendors;
            return newVWRKSVendors;
        }

        public VWRKSVendors getRKSKlarifikasi(Guid PengadaanId, Guid UserId)
        {
            VWRKSVendors newVWRKSVendors = new VWRKSVendors();

            List<VWRKSPenilaian> hps = (from b in ctx.RKSDetails
                                        join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                        where c.PengadaanId == PengadaanId
                                        select new VWRKSPenilaian
                                        {
                                            Id = b.Id,
                                            harga = b.hps,
                                            item = b.item,
                                            jumlah = b.jumlah,
                                            satuan = b.satuan
                                        }).ToList();
            newVWRKSVendors.hps = hps;
            decimal? totalHps = hps.Sum(d => d.harga * d.jumlah);
            newVWRKSVendors.hps.Insert(0, new VWRKSPenilaian { item = "Total", harga = totalHps });

            List<VWVendorsHarga> vendors = new List<VWVendorsHarga>();

            var xKandidatPengadaans = (from b in ctx.PelaksanaanPemilihanKandidats
                                       where b.PengadaanId == PengadaanId
                                       select b).Distinct().ToList();
            foreach (var item in xKandidatPengadaans)
            {
                //into ps
                //                from c in ps.DefaultIfEmpty()
                VWVendorsHarga mVWVendorsHarga = new VWVendorsHarga();
                mVWVendorsHarga.nama = ctx.Vendors.Find(item.VendorId).Nama;

                mVWVendorsHarga.items = (from b in ctx.HargaKlarifikasiRekanans
                                         join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                         join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                         where d.PengadaanId == PengadaanId && b.VendorId == item.VendorId
                                         select new item
                                         {
                                             harga = b.harga,
                                             jumlah = c.jumlah
                                         }).ToList();

                var oPembobotanPengadaanVendor = ctx.PembobotanPengadaanVendors.Where(
                                   d => d.PengadaanId == PengadaanId && d.VendorId == item.VendorId).ToList();
                var totalNilaiKirteria = 0;
                foreach (var itemKriteriaVendor in oPembobotanPengadaanVendor)
                {
                    var oPembobotanPengadaans = ctx.PembobotanPengadaans.Where(d => d.KreteriaPembobotanId == itemKriteriaVendor.KreteriaPembobotanId && d.PengadaanId == PengadaanId).FirstOrDefault();
                    var bobot = oPembobotanPengadaans == null ? 0 : oPembobotanPengadaans.Bobot == null ? 0 : oPembobotanPengadaans.Bobot.Value;
                    var nilai = itemKriteriaVendor.Nilai == null ? 0 : itemKriteriaVendor.Nilai.Value;
                    totalNilaiKirteria = totalNilaiKirteria + ((bobot * nilai) / 100);
                }
                mVWVendorsHarga.NIlaiKriteria = totalNilaiKirteria;

                if (mVWVendorsHarga.items.Count > 0)
                {
                    decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                    mVWVendorsHarga.items.Insert(0, new item { harga = total });
                    vendors.Add(mVWVendorsHarga);
                }
            }
            newVWRKSVendors.vendors = vendors;
            return newVWRKSVendors;
        }

        public VWRKSVendors getRKSKlarifikasiPenilaianVendor(Guid PengadaanId, Guid UserId, int VendorId)
        {
            VWRKSVendors newVWRKSVendors = new VWRKSVendors();

            List<VWRKSPenilaian> hps = (from b in ctx.RKSDetails
                                        join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                        where c.PengadaanId == PengadaanId
                                        select new VWRKSPenilaian
                                        {
                                            Id = b.Id,
                                            harga = b.hps,
                                            item = b.item,
                                            jumlah = b.jumlah,
                                            satuan = b.satuan
                                        }).ToList();
            newVWRKSVendors.hps = hps;
            decimal? totalHps = hps.Sum(d => d.harga * d.jumlah);
            newVWRKSVendors.hps.Insert(0, new VWRKSPenilaian { item = "Total", harga = totalHps });

            List<VWVendorsHarga> vendors = new List<VWVendorsHarga>();

            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans
                                       where b.PengadaanId == PengadaanId && b.VendorId == VendorId
                                       select b).Distinct().FirstOrDefault();

            VWVendorsHarga mVWVendorsHarga = new VWVendorsHarga();
            mVWVendorsHarga.nama = ctx.Vendors.Find(xKandidatPengadaans.VendorId).Nama + " (Submit Penawaran)";
            mVWVendorsHarga.Keterangan = "Keteragan Awal";

            var oPembobotanPengadaanVendor = ctx.PembobotanPengadaanVendors.Where(
                                    d => d.PengadaanId == PengadaanId && d.VendorId == xKandidatPengadaans.VendorId).ToList();
            var totalNilaiKirteria = 0;
            foreach (var itemKriteriaVendor in oPembobotanPengadaanVendor)
            {
                var oPembobotanPengadaans = ctx.PembobotanPengadaans.Where(d => d.KreteriaPembobotanId == itemKriteriaVendor.KreteriaPembobotanId && d.PengadaanId == PengadaanId).FirstOrDefault();
                var bobot = oPembobotanPengadaans == null ? 0 : oPembobotanPengadaans.Bobot == null ? 0 : oPembobotanPengadaans.Bobot.Value;
                var nilai = itemKriteriaVendor.Nilai == null ? 0 : itemKriteriaVendor.Nilai.Value;
                totalNilaiKirteria = totalNilaiKirteria + ((bobot * nilai) / 100);
            }
            mVWVendorsHarga.NIlaiKriteria = totalNilaiKirteria;
            mVWVendorsHarga.VendorId = mVWVendorsHarga.VendorId;
            mVWVendorsHarga.items = (from b in ctx.HargaRekanans
                                     join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                     join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                     where d.PengadaanId == PengadaanId && b.VendorId == xKandidatPengadaans.VendorId
                                     select new item
                                     {
                                         Keterangan = b.keterangan.ToString(),
                                         harga = b.harga,
                                         jumlah = c.jumlah

                                     }).ToList();
            if (mVWVendorsHarga.items.Count > 0)
            {
                decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                mVWVendorsHarga.items.Insert(0, new item { harga = total });
                vendors.Add(mVWVendorsHarga);
            }

            mVWVendorsHarga = new VWVendorsHarga();
            mVWVendorsHarga.nama = ctx.Vendors.Find(xKandidatPengadaans.VendorId).Nama + " (Klarifikasi Harga)";
            mVWVendorsHarga.Keterangan = "Keterangan Klarifikasi";
            var oPembobotanPengadaanVendor2 = ctx.PembobotanPengadaanVendors.Where(
                                    d => d.PengadaanId == PengadaanId && d.VendorId == xKandidatPengadaans.VendorId).ToList();
            var totalNilaiKirteria2 = 0;
            foreach (var itemKriteriaVendor in oPembobotanPengadaanVendor2)
            {
                var oPembobotanPengadaans = ctx.PembobotanPengadaans.Where(d => d.KreteriaPembobotanId == itemKriteriaVendor.KreteriaPembobotanId && d.PengadaanId == PengadaanId).FirstOrDefault();
                var bobot = oPembobotanPengadaans == null ? 0 : oPembobotanPengadaans.Bobot == null ? 0 : oPembobotanPengadaans.Bobot.Value;
                var nilai = itemKriteriaVendor.Nilai == null ? 0 : itemKriteriaVendor.Nilai.Value;
                totalNilaiKirteria2 = totalNilaiKirteria2 + ((bobot * nilai) / 100);
            }
            mVWVendorsHarga.NIlaiKriteria = totalNilaiKirteria2;
            mVWVendorsHarga.VendorId = mVWVendorsHarga.VendorId;
            mVWVendorsHarga.items = (from b in ctx.HargaKlarifikasiRekanans
                                     join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                     join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                     where d.PengadaanId == PengadaanId && b.VendorId == xKandidatPengadaans.VendorId
                                     select new item
                                     {
                                         Keterangan = b.keterangan.ToString(),
                                         harga = b.harga,
                                         jumlah = c.jumlah
                                     }).ToList();
            if (mVWVendorsHarga.items.Count > 0)
            {
                decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                mVWVendorsHarga.items.Insert(0, new item { harga = total });
                vendors.Add(mVWVendorsHarga);
            }



            newVWRKSVendors.vendors = vendors;
            return newVWRKSVendors;
        }

        public VWRKSVendors getRKSKlarifikasiPenilaian(Guid PengadaanId, Guid UserId)
        {
            VWRKSVendors newVWRKSVendors = new VWRKSVendors();

            List<VWRKSPenilaian> hps = (from b in ctx.RKSDetails
                                        join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                        where c.PengadaanId == PengadaanId
                                        select new VWRKSPenilaian
                                        {
                                            Id = b.Id,
                                            harga = b.hps,
                                            item = b.item,
                                            jumlah = b.jumlah,
                                            satuan = b.satuan
                                        }).ToList();
            newVWRKSVendors.hps = hps;
            decimal? totalHps = hps.Sum(d => d.harga * d.jumlah);
            newVWRKSVendors.hps.Add(new VWRKSPenilaian { item = "Total", harga = totalHps });

            List<VWVendorsHarga> vendors = new List<VWVendorsHarga>();

            var kandidatTerpilih = (from b in ctx.PelaksanaanPemilihanKandidats
                                    where b.PengadaanId == PengadaanId
                                    select b).ToList();
            var lastItem = kandidatTerpilih.Last();
            foreach (var item in kandidatTerpilih)
            {
                var xKandidatPengadaans = (from b in ctx.PelaksanaanPemilihanKandidats
                                           where b.PengadaanId == PengadaanId && b.VendorId == item.VendorId
                                           select b).Distinct().FirstOrDefault();

                VWVendorsHarga mVWVendorsHarga = new VWVendorsHarga();
                mVWVendorsHarga.nama = ctx.Vendors.Find(xKandidatPengadaans.VendorId).Nama + " (Submit Penawaran)";

                mVWVendorsHarga.items = (from b in ctx.HargaRekanans
                                         join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                         join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                         where d.PengadaanId == PengadaanId && b.VendorId == xKandidatPengadaans.VendorId
                                         select new item
                                         {
                                             Id = c.Id,
                                             harga = b.harga,
                                             jumlah = c.jumlah
                                         }).ToList();
                if (mVWVendorsHarga.items.Count > 0)
                {
                    decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                    mVWVendorsHarga.items.Add(new item { harga = total });
                    vendors.Add(mVWVendorsHarga);
                }

                mVWVendorsHarga = new VWVendorsHarga();
                mVWVendorsHarga.nama = ctx.Vendors.Find(xKandidatPengadaans.VendorId).Nama + " (Klarifikasi Harga)";

                mVWVendorsHarga.items = (from b in ctx.HargaKlarifikasiRekanans
                                         join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                         join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                         where d.PengadaanId == PengadaanId && b.VendorId == xKandidatPengadaans.VendorId
                                         select new item
                                         {
                                             Id = c.Id,
                                             harga = b.harga,
                                             jumlah = c.jumlah
                                         }).ToList();
                if (mVWVendorsHarga.items.Count > 0)
                {
                    decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                    mVWVendorsHarga.items.Add(new item { harga = total });
                    vendors.Add(mVWVendorsHarga);
                }

            }

            newVWRKSVendors.vendors = vendors;
            return newVWRKSVendors;
        }

        public VWRKSVendors getRKSKlarifikasiPenilaianVendor2(Guid PengadaanId, Guid UserId, int VendorId)
        {
            VWRKSVendors newVWRKSVendors = new VWRKSVendors();

            List<VWRKSPenilaian> hps = (from b in ctx.RKSDetails
                                        join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                        where c.PengadaanId == PengadaanId
                                        select new VWRKSPenilaian
                                        {
                                            Id = b.Id,
                                            harga = b.hps,
                                            item = b.item,
                                            jumlah = b.jumlah,
                                            satuan = b.satuan
                                        }).ToList();
            newVWRKSVendors.hps = hps;
            decimal? totalHps = hps.Sum(d => d.harga * d.jumlah);
            newVWRKSVendors.hps.Add(new VWRKSPenilaian { item = "Total", harga = totalHps });

            List<VWVendorsHarga> vendors = new List<VWVendorsHarga>();

            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans
                                       where b.PengadaanId == PengadaanId && b.VendorId == VendorId
                                       select b).Distinct().FirstOrDefault();

            VWVendorsHarga mVWVendorsHarga = new VWVendorsHarga();
            mVWVendorsHarga.nama = ctx.Vendors.Find(xKandidatPengadaans.VendorId).Nama + " (Submit Penawaran)";

            mVWVendorsHarga.items = (from b in ctx.HargaRekanans
                                     join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                     join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                     where d.PengadaanId == PengadaanId && b.VendorId == xKandidatPengadaans.VendorId
                                     select new item
                                     {
                                         Id = c.Id,
                                         harga = b.harga,
                                         jumlah = c.jumlah
                                     }).ToList();
            if (mVWVendorsHarga.items.Count > 0)
            {
                decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                mVWVendorsHarga.items.Add(new item { harga = total });
                vendors.Add(mVWVendorsHarga);
            }

            mVWVendorsHarga = new VWVendorsHarga();
            mVWVendorsHarga.nama = ctx.Vendors.Find(xKandidatPengadaans.VendorId).Nama + " (Klarifikasi Harga)";

            mVWVendorsHarga.items = (from b in ctx.HargaKlarifikasiRekanans
                                     join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                     join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                     where d.PengadaanId == PengadaanId && b.VendorId == xKandidatPengadaans.VendorId
                                     select new item
                                     {
                                         Id = c.Id,
                                         harga = b.harga,
                                         jumlah = c.jumlah
                                     }).ToList();
            if (mVWVendorsHarga.items.Count > 0)
            {
                decimal? total = mVWVendorsHarga.items.Sum(d => d.harga * d.jumlah);
                mVWVendorsHarga.items.Add(new item { harga = total });
                vendors.Add(mVWVendorsHarga);
            }



            newVWRKSVendors.vendors = vendors;
            return newVWRKSVendors;
        }

        public List<VWRekananPenilaian> getPemenangPengadaan(Guid PengadaanId, Guid UserId)
        {
            var xKandidatPengadaans = (from b in ctx.PemenangPengadaans
                                       join c in ctx.Vendors on b.VendorId equals c.Id
                                       where b.PengadaanId == PengadaanId
                                       select new VWRekananPenilaian
                                       {
                                           NamaVendor = c.Nama,
                                           VendorId = b.VendorId,
                                           total = (from bb in ctx.HargaKlarifikasiRekanans
                                                    join cc in ctx.RKSDetails on bb.RKSDetailId equals cc.Id
                                                    join dd in ctx.RKSHeaders on cc.RKSHeaderId equals dd.Id
                                                    where dd.PengadaanId == PengadaanId && bb.VendorId == b.VendorId
                                                    select new item
                                                    {
                                                        harga = bb.harga,
                                                        jumlah = cc.jumlah
                                                    }).Sum(xx => xx.harga * xx.jumlah),
                                           terpilih = (from bb in ctx.PemenangPengadaans
                                                       where bb.PengadaanId == PengadaanId &&
                                                       bb.VendorId == b.VendorId
                                                       select bb).FirstOrDefault() == null ? 0 : 1,
                                           NilaiKriteria = (from bb in ctx.PembobotanPengadaans
                                                            join cc in ctx.PembobotanPengadaanVendors on bb.KreteriaPembobotanId equals cc.KreteriaPembobotanId
                                                            where cc.PengadaanId == PengadaanId && cc.VendorId == b.VendorId && bb.PengadaanId == PengadaanId
                                                            select new
                                                            {
                                                                Bobot = bb.Bobot == null ? 0 : bb.Bobot.Value,
                                                                Nilai = cc.Nilai == null ? 0 : cc.Nilai.Value
                                                            }).Sum(dd => (dd.Nilai * dd.Bobot) / 100)
                                       }).ToList();
            return xKandidatPengadaans;
        }

        public List<VWRekananPenilaian> getKandidatPengadaan(Guid PengadaanId, Guid UserId)
        {
            var xKandidatPengadaans = (from b in ctx.KandidatPengadaans
                                       join c in ctx.Vendors on b.VendorId equals c.Id
                                       where b.PengadaanId == PengadaanId
                                       select new VWRekananPenilaian
                                       {
                                           NamaVendor = c.Nama,
                                           VendorId = b.VendorId,
                                           total = (from bb in ctx.HargaKlarifikasiRekanans
                                                    join cc in ctx.RKSDetails on bb.RKSDetailId equals cc.Id
                                                    join dd in ctx.RKSHeaders on cc.RKSHeaderId equals dd.Id
                                                    where dd.PengadaanId == PengadaanId && bb.VendorId == b.VendorId
                                                    select new item
                                                    {
                                                        harga = bb.harga,
                                                        jumlah = cc.jumlah
                                                    }).Sum(xx => xx.harga * xx.jumlah),
                                           terpilih = (from bb in ctx.PemenangPengadaans
                                                       where bb.PengadaanId == PengadaanId &&
                                                       bb.VendorId == b.VendorId
                                                       select bb).FirstOrDefault() == null ? 0 : 1,
                                           NilaiKriteria = (from bb in ctx.PembobotanPengadaans
                                                            join cc in ctx.PembobotanPengadaanVendors on bb.KreteriaPembobotanId equals cc.KreteriaPembobotanId
                                                            where cc.PengadaanId == PengadaanId && cc.VendorId == b.VendorId && bb.PengadaanId == PengadaanId
                                                            select new
                                                            {
                                                                Bobot = bb.Bobot == null ? 0 : bb.Bobot.Value,
                                                                Nilai = cc.Nilai == null ? 0 : cc.Nilai.Value
                                                            }).Sum(dd => (dd.Nilai * dd.Bobot) / 100)
                                       }).ToList();
            return xKandidatPengadaans;
        }

        public List<VWRKSDetail> getRKSDetails(Guid PengadaanId, Guid UserId)
        {
            List<VWRKSDetail> hps = (from b in ctx.RKSDetails
                                     join c in ctx.RKSHeaders on b.RKSHeaderId equals c.Id
                                     where c.PengadaanId == PengadaanId
                                     select new VWRKSDetail
                                     {
                                         Id = b.Id,
                                         hps = b.hps,
                                         item = b.item,
                                         judul = b.judul,
                                         level = b.level,
                                         grup = b.grup,
                                         jumlah = b.jumlah,
                                         satuan = b.satuan,
                                         total = b.jumlah == null ? 0 : b.jumlah * b.hps,
                                         keterangan = b.keterangan
                                     }).OrderBy(d => d.grup).ThenBy(d => d.level).ToList();
            //hps.Add(new VWRKSDetail { item = "Total", total = hps.Sum(d => d.total) });
            return hps;
        }

        public List<VWPembobotanPengadaan> getKriteriaPembobotan(Guid PengadaanId)
        {
            var oKriteriaPembobotan = (from b in ctx.KreteriaPembobotans
                                       select new VWPembobotanPengadaan
                                       {
                                           Id = b.Id,
                                           NamaKreteria = b.NamaKreteria,
                                           Bobot = ctx.PembobotanPengadaans.Where(v => v.KreteriaPembobotanId == b.Id
                                            && v.PengadaanId == PengadaanId).FirstOrDefault() == null ? b.Bobot :
                                            ctx.PembobotanPengadaans.Where(v => v.KreteriaPembobotanId == b.Id
                                            && v.PengadaanId == PengadaanId).FirstOrDefault().Bobot
                                       }).OrderBy(d => d.NamaKreteria).ToList();

            return oKriteriaPembobotan;
        }

        public int addPembobtanPengadaan(PembobotanPengadaan dataPembobotanPengadaan, Guid UserId)
        {
            var oPembobotanPengadaan = ctx.PembobotanPengadaans.Where(
                             d => d.PengadaanId == dataPembobotanPengadaan.PengadaanId && d.KreteriaPembobotanId == dataPembobotanPengadaan.KreteriaPembobotanId).FirstOrDefault();

            int totalPembobotan = ctx.PembobotanPengadaans.Where(d => d.PengadaanId == dataPembobotanPengadaan.PengadaanId).Sum(d => d.Bobot) == null ? 0 : ctx.PembobotanPengadaans.Where(d => d.PengadaanId == dataPembobotanPengadaan.PengadaanId).Sum(d => d.Bobot).Value;


            try
            {
                if (oPembobotanPengadaan != null)
                {
                    totalPembobotan = oPembobotanPengadaan.Bobot == null ? 0 : totalPembobotan - oPembobotanPengadaan.Bobot.Value;
                    totalPembobotan = dataPembobotanPengadaan.Bobot == null ? 0 : totalPembobotan + dataPembobotanPengadaan.Bobot.Value;
                    if (totalPembobotan > 100) return 0;
                    oPembobotanPengadaan.Bobot = dataPembobotanPengadaan.Bobot;
                    //oPembobotanPengadaan.Nilai = dataPembobotanPengadaan.Nilai;
                    ctx.SaveChanges();
                    return 1;
                }
                else
                {
                    totalPembobotan = dataPembobotanPengadaan.Bobot == null ? 0 : totalPembobotan + dataPembobotanPengadaan.Bobot.Value;
                    if (totalPembobotan > 100) return 0;
                    ctx.PembobotanPengadaans.Add(dataPembobotanPengadaan);
                    ctx.SaveChanges();
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        public int addLstPembobtanPengadaan(List<PembobotanPengadaan> dataLstPembobotanPengadaan, Guid UserId)
        {
            foreach (var item in dataLstPembobotanPengadaan)
            {
                var oPembobotanPengadaan = ctx.PembobotanPengadaans.Where(
                                 d => d.PengadaanId == item.PengadaanId && d.KreteriaPembobotanId == item.KreteriaPembobotanId).FirstOrDefault();

                int totalPembobotan = dataLstPembobotanPengadaan.Sum(d => d.Bobot) == null ? 0 : dataLstPembobotanPengadaan.Sum(d => d.Bobot).Value;

                //ctx.PembobotanPengadaans.Where(d => d.PengadaanId == item.PengadaanId).Sum(d => d.Bobot) == null ? 0 : ctx.PembobotanPengadaans.Where(d => d.PengadaanId == item.PengadaanId).Sum(d => d.Bobot).Value;
                try
                {
                    if (oPembobotanPengadaan != null)
                    {
                        // totalPembobotan = oPembobotanPengadaan.Bobot == null ? 0 : totalPembobotan - oPembobotanPengadaan.Bobot.Value;
                        //totalPembobotan = item.Bobot == null ? 0 : totalPembobotan + item.Bobot.Value;
                        if (totalPembobotan > 100) break;
                        oPembobotanPengadaan.Bobot = item.Bobot;
                        //oPembobotanPengadaan.Nilai = dataPembobotanPengadaan.Nilai;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        //totalPembobotan = item.Bobot == null ? 0 : totalPembobotan + item.Bobot.Value;
                        if (totalPembobotan > 100) break;
                        ctx.PembobotanPengadaans.Add(item);
                        ctx.SaveChanges();
                    }
                }
                catch
                {
                    return 0;
                }
            }
            return 1;
        }

        public int addLstPenilaianKriteriaVendor(List<PembobotanPengadaanVendor> dataLstPenilaianKriteriaVendor, Guid UserId)
        {
            foreach (var item in dataLstPenilaianKriteriaVendor)
            {
                var oPembobotanPengadaanVendor = ctx.PembobotanPengadaanVendors.Where(
                                 d => d.PengadaanId == item.PengadaanId
                                     && d.KreteriaPembobotanId == item.KreteriaPembobotanId
                                    && d.VendorId == item.VendorId).FirstOrDefault();
                try
                {
                    if (oPembobotanPengadaanVendor != null)
                    {
                        oPembobotanPengadaanVendor.Nilai = item.Nilai;
                        ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.PembobotanPengadaanVendors.Add(item);
                        ctx.SaveChanges();
                    }
                }
                catch
                {
                    return 0;
                }
            }
            return 1;
        }

        public List<PembobotanPengadaan> getPembobtanPengadaan(Guid PengadaanId, Guid UserId)
        {
            return ctx.PembobotanPengadaans.Where(d => d.PengadaanId == PengadaanId).ToList();
        }

        public List<VWPembobotanPengadaanVendor> getPembobtanPengadaanVendor(Guid PengadaanId, int VendorId, Guid UserId)
        {
            var oPembobotanVendor = (from b in ctx.KreteriaPembobotans
                                     join c in ctx.PembobotanPengadaans on b.Id equals c.KreteriaPembobotanId into ps
                                     from c in ps.DefaultIfEmpty()
                                     //where c.PengadaanId == PengadaanId
                                     select new VWPembobotanPengadaanVendor
                                     {
                                         Id = c.KreteriaPembobotanId,
                                         NamaKreteria = b.NamaKreteria,
                                         Bobot = ctx.PembobotanPengadaans.Where(v => v.KreteriaPembobotanId == b.Id
                                                  && v.PengadaanId == PengadaanId).FirstOrDefault() == null ? b.Bobot :
                                                  ctx.PembobotanPengadaans.Where(v => v.KreteriaPembobotanId == b.Id
                                                  && v.PengadaanId == PengadaanId).FirstOrDefault().Bobot,
                                         Nilai = ctx.PembobotanPengadaanVendors.Where(
                                                 d => d.KreteriaPembobotanId == c.KreteriaPembobotanId && d.PengadaanId == PengadaanId
                                              && d.VendorId == VendorId).FirstOrDefault() == null ? 0 :
                                              ctx.PembobotanPengadaanVendors.Where(
                                                d => d.KreteriaPembobotanId == c.KreteriaPembobotanId && d.PengadaanId == PengadaanId
                                              && d.VendorId == VendorId).FirstOrDefault().Nilai,
                                         VendorId = VendorId
                                     }).OrderBy(d => d.NamaKreteria).Distinct().ToList();
            return oPembobotanVendor;
        }

        public int deleteKualifikasiKandidat(Guid Id, Guid UserId)
        {
            KualifikasiKandidat mKualifikasiKandidat = ctx.KualifikasiKandidats.Find(Id);
            if (ctx.Pengadaans.Find(mKualifikasiKandidat.PengadaanId) == null) return 0;
            ctx.KualifikasiKandidats.Remove(mKualifikasiKandidat);
            ctx.SaveChanges();
            return 1;
        }

        public PelaksanaanPemilihanKandidat addKandidatPilihan(PelaksanaanPemilihanKandidat oPelaksanaanPemilihanKandidat, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(oPelaksanaanPemilihanKandidat.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == oPelaksanaanPemilihanKandidat.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return new PelaksanaanPemilihanKandidat();
            PelaksanaanPemilihanKandidat oldoPelaksanaanPemilihanKandidat =
                    ctx.PelaksanaanPemilihanKandidats.Where(d => d.PengadaanId == oPelaksanaanPemilihanKandidat.PengadaanId
                                    && d.VendorId == oPelaksanaanPemilihanKandidat.VendorId).FirstOrDefault();
            if (oldoPelaksanaanPemilihanKandidat == null)
            {
                oPelaksanaanPemilihanKandidat.CreatedBy = UserId;
                oPelaksanaanPemilihanKandidat.CreatedDate = DateTime.Now;
                ctx.PelaksanaanPemilihanKandidats.Add(oPelaksanaanPemilihanKandidat);
            }
            else ctx.PelaksanaanPemilihanKandidats.Remove(oldoPelaksanaanPemilihanKandidat);
            ctx.SaveChanges();
            return oPelaksanaanPemilihanKandidat;
        }

        public string statusVendor(Guid PengadaanId, Guid UserId)
        {
            var VendorId = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault().Id;
            Pengadaan oPengadaan = ctx.Pengadaans.Find(PengadaanId);
            if (oPengadaan.Status == EStatusPengadaan.AANWIJZING)
            {
                return "Dalam Proses Aanwijzing";
            }
            else if (oPengadaan.Status == EStatusPengadaan.SUBMITPENAWARAN)
            {
                return "Dalam Proses Submit Penawaran";
            }
            else if (oPengadaan.Status == EStatusPengadaan.BUKAAMPLOP)
            {
                return "Dalam Proses Buka Amplop";
            }
            else if (oPengadaan.Status == EStatusPengadaan.PENILAIAN)
            {
                return "Dalam Proses Penilaian";
            }
            else if (oPengadaan.Status == EStatusPengadaan.KLARIFIKASI)
            {
                var oPelaksanaanPemilihanKandidat = ctx.PelaksanaanPemilihanKandidats.Where(d => d.PengadaanId == PengadaanId &&
                                d.VendorId == VendorId).FirstOrDefault();
                if (oPelaksanaanPemilihanKandidat == null)
                    return "Pengajuan Anda DiTolak";
                return "Dalam Proses Klarifikasi";
            }
            else if (oPengadaan.Status == EStatusPengadaan.PEMENANG)
            {
                var oPelaksanaanPemenang = ctx.PemenangPengadaans.Where(d => d.PengadaanId == PengadaanId &&
                               d.VendorId == VendorId).FirstOrDefault();

                if (oPelaksanaanPemenang == null)
                    return "Pengajuan Anda DiTolak";
                var beritaAcara = ctx.BeritaAcaras.Where(d => d.PengadaanId == PengadaanId && d.Tipe == TipeBerkas.BeritaAcaraPenentuanPemenang).FirstOrDefault();

                if (beritaAcara == null)
                    return "Dalam Proses Penentuan Pemenang";
                else
                    return "Anda Sebagai Pemenang Pengadaan Ini";
            }
            else
            {
                return "Dalam Proses";
            }
        }

        public int deleteKandidatPilihan(PelaksanaanPemilihanKandidat oPelaksanaanPemilihanKandidat, Guid UserId)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(oPelaksanaanPemilihanKandidat.PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == oPelaksanaanPemilihanKandidat.PengadaanId).FirstOrDefault();
            if (picPersonil == null) return 0;
            var oldoPelaksanaanPemilihanKandidat =
                    ctx.PelaksanaanPemilihanKandidats.Where(d => d.PengadaanId == oPelaksanaanPemilihanKandidat.PengadaanId
                                    && d.VendorId == oPelaksanaanPemilihanKandidat.VendorId);
            ctx.PelaksanaanPemilihanKandidats.RemoveRange(oldoPelaksanaanPemilihanKandidat);
            ctx.SaveChanges();
            return 1;
        }

        public int nextToState(Guid PengadaanId, Guid UserId, EStatusPengadaan state)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == PengadaanId).FirstOrDefault();
            if (picPersonil == null) return 0;
            Mpengadaaan.Status = state;
            ctx.SaveChanges();
            return 1;
        }


        public int isNotaUploaded(Guid PengadaanId, Guid UserId)
        {
            var beritaAcaraPe = ctx.DokumenPengadaans.Where(d => d.PengadaanId == PengadaanId && d.Tipe == TipeBerkas.BeritaAcaraPenentuanPemenang).FirstOrDefault();

            if (beritaAcaraPe == null) return 0;
            else return 1;
        }
        public int isSpkUploaded(Guid PengadaanId, Guid UserId)
        {
            var beritaAcaraPe = ctx.DokumenPengadaans.Where(d => d.PengadaanId == PengadaanId && d.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault();

            if (beritaAcaraPe == null) return 0;
            else return 1;
        }


        public string GenerateNoPengadaan(Guid UserId)
        {
            var oNoDokumen = ctx.NoDokumenGenerators.Where(d => d.tipe == TipeNoDokumen.PENGADAAN).OrderByDescending(d => d.Id).FirstOrDefault();
            var KodePengadaan = System.Configuration.ConfigurationManager.AppSettings["KODE_PENGADAAN"].ToString();
            if (oNoDokumen == null)
            {
                string newNODok = "1" + KodePengadaan + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;
                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNODok;
                newoNoDokumen.tipe = TipeNoDokumen.PENGADAAN;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
            else
            {
                var arrNo = oNoDokumen.No.Split('/');
                int NextNo = Convert.ToInt32(arrNo[0]);
                NextNo = NextNo + 1;
                int oldYear = Convert.ToInt32(arrNo[4]);
                string newNoDokmen = "";
                if (oldYear == DateTime.Now.Year)
                    newNoDokmen = NextNo.ToString() + KodePengadaan + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + +DateTime.Now.Year;
                else newNoDokmen = "1" + KodePengadaan + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;

                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNoDokmen;
                newoNoDokumen.tipe = TipeNoDokumen.PENGADAAN;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
        }

        public string GenerateBeritaAcara(Guid UserId)
        {
            var oNoDokumen = ctx.NoDokumenGenerators.Where(d => d.tipe == TipeNoDokumen.BERITAACARA).OrderByDescending(d => d.Id).FirstOrDefault();
            var KodeBeritaAcara = System.Configuration.ConfigurationManager.AppSettings["KODE_BERITAACARA"].ToString();
            if (oNoDokumen == null)
            {
                string newNODok = "1" + KodeBeritaAcara + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;
                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNODok;
                newoNoDokumen.tipe = TipeNoDokumen.BERITAACARA;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
            else
            {
                var arrNo = oNoDokumen.No.Split('/');
                int NextNo = Convert.ToInt32(arrNo[0]);
                NextNo = NextNo + 1;
                int oldYear = Convert.ToInt32(arrNo[3]);
                string newNoDokmen = "";
                if (oldYear == DateTime.Now.Year)
                    newNoDokmen = NextNo.ToString() + KodeBeritaAcara + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + +DateTime.Now.Year;
                else newNoDokmen = "1" + KodeBeritaAcara + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;

                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNoDokmen;
                newoNoDokumen.tipe = TipeNoDokumen.BERITAACARA;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
        }

        public string GenerateBeritaAcaraNota(Guid UserId)
        {
            var oNoDokumen = ctx.NoDokumenGenerators.Where(d => d.tipe == TipeNoDokumen.NOTA).OrderByDescending(d => d.Id).FirstOrDefault();
            var KodeNota = System.Configuration.ConfigurationManager.AppSettings["KODE_NOTA"].ToString();
            if (oNoDokumen == null)
            {
                string newNODok = "1" + KodeNota + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;
                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNODok;
                newoNoDokumen.tipe = TipeNoDokumen.NOTA;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
            else
            {
                var arrNo = oNoDokumen.No.Split('/');
                int NextNo = Convert.ToInt32(arrNo[0]);
                NextNo = NextNo + 1;
                int oldYear = Convert.ToInt32(arrNo[4]);
                string newNoDokmen = "";
                if (oldYear == DateTime.Now.Year)
                    newNoDokmen = NextNo.ToString() + KodeNota + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + +DateTime.Now.Year;
                else newNoDokmen = "1" + KodeNota + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;

                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNoDokmen;
                newoNoDokumen.tipe = TipeNoDokumen.NOTA;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
        }

        public string GenerateBeritaAcaraSPK(Guid UserId)
        {
            var oNoDokumen = ctx.NoDokumenGenerators.Where(d => d.tipe == TipeNoDokumen.SPK).OrderByDescending(d => d.Id).FirstOrDefault();
            var KODESPK = System.Configuration.ConfigurationManager.AppSettings["KODE_SPK"].ToString();
            if (oNoDokumen == null)
            {
                string newNODok = "1" + KODESPK + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;
                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNODok;
                newoNoDokumen.tipe = TipeNoDokumen.SPK;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
            else
            {
                var arrNo = oNoDokumen.No.Split('/');
                int NextNo = Convert.ToInt32(arrNo[0]);
                NextNo = NextNo + 1;
                int oldYear = Convert.ToInt32(arrNo[4]);
                string newNoDokmen = "";
                if (oldYear == DateTime.Now.Year)
                    newNoDokmen = NextNo.ToString() + KODESPK + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + +DateTime.Now.Year;
                else newNoDokmen = "1" + KODESPK + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;

                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNoDokmen;
                newoNoDokumen.tipe = TipeNoDokumen.SPK;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges();
                return newoNoDokumen.No;
            }
        }

        public List<BeritaAcara> getBeritaAcara(Guid PengadaanId, Guid UserId)
        {
            return ctx.BeritaAcaras.Where(d => d.PengadaanId == PengadaanId).ToList();
        }

        public BeritaAcara getBeritaAcaraByTipe(Guid PengadaanId, TipeBerkas tipe, Guid UserId)
        {
            return ctx.BeritaAcaras.Where(d => d.PengadaanId == PengadaanId && d.Tipe == tipe).FirstOrDefault();
        }

        public BeritaAcara addBeritaAcara(BeritaAcara newBeritaAcara, Guid UserId)
        {
            Pengadaan opengadaan = ctx.Pengadaans.Find(newBeritaAcara.PengadaanId);
            if (opengadaan == null) return new BeritaAcara();
            BeritaAcara oBeritaAcara = ctx.BeritaAcaras.Where(d => d.PengadaanId == newBeritaAcara.PengadaanId
                            && d.Tipe == newBeritaAcara.Tipe).FirstOrDefault();
            if (oBeritaAcara == null)
            {
                if (newBeritaAcara.Tipe == TipeBerkas.BeritaAcaraPenentuanPemenang)
                {
                    newBeritaAcara.NoBeritaAcara = GenerateBeritaAcaraNota(UserId);
                }
                else if (newBeritaAcara.Tipe == TipeBerkas.SuratPerintahKerja)
                {
                    newBeritaAcara.NoBeritaAcara = GenerateBeritaAcaraSPK(UserId);
                }
                else
                {
                    newBeritaAcara.NoBeritaAcara = GenerateBeritaAcara(UserId);
                }
                ctx.BeritaAcaras.Add(newBeritaAcara);
                ctx.SaveChanges();
                return newBeritaAcara;
            }
            else
            {
                oBeritaAcara.tanggal = newBeritaAcara.tanggal;
                ctx.SaveChanges();
                return oBeritaAcara;
            }
        }

        public int CekBukaAmplop(Guid PengadaanId)
        {
            List<VWPErsetujuanBukaAmplop> lstPErsetujuan =
                 (from b in ctx.PersetujuanBukaAmplops
                  join c in ctx.PersonilPengadaans on b.PengadaanId equals c.PengadaanId //into ps
                  where b.PengadaanId == PengadaanId && c.PersonilId == b.UserId
                  select new VWPErsetujuanBukaAmplop
                  {
                      Id = b.Id,
                      tipe = c.tipe,
                      UserId = c.PersonilId,
                      PengadaanId = c.PengadaanId,
                  }).ToList();

            var PIC = lstPErsetujuan.Where(d => d.tipe == PengadaanConstants.StaffPeranan.PIC).FirstOrDefault();
            if (PIC == null) return 0;
            var User = lstPErsetujuan.Where(d => d.tipe == PengadaanConstants.StaffPeranan.Staff).FirstOrDefault();
            if (User == null) return 0;
            var Compl = lstPErsetujuan.Where(d => d.tipe == PengadaanConstants.StaffPeranan.Compliance).FirstOrDefault();
            if (Compl == null) return 0;
            var Contr = lstPErsetujuan.Where(d => d.tipe == PengadaanConstants.StaffPeranan.Controller).FirstOrDefault();
            if (Compl == null) return 0;

            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(PengadaanId);
            //PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == PengadaanId).FirstOrDefault();
            ///if (picPersonil == null) return 0;           

            var oKandidatPengadaan = ctx.KandidatPengadaans.Where(d => d.PengadaanId == Mpengadaaan.Id).ToList();
            if (oKandidatPengadaan.Count() > 0)
            {

                foreach (var item in oKandidatPengadaan)
                {
                    var xSubmitRekanan = (from b in ctx.HargaRekanans
                                          join c in ctx.RKSDetails on b.RKSDetailId equals c.Id
                                          join d in ctx.RKSHeaders on c.RKSHeaderId equals d.Id
                                          where d.PengadaanId == PengadaanId && b.VendorId == item.VendorId
                                          select b).Distinct().ToList();
                    foreach (var itemx in xSubmitRekanan)
                    {
                        JimbisEncrypt encod = new JimbisEncrypt();
                        decimal? harga = encod.Decrypt(itemx.hargaEncrypt) == "" ? 0 : Convert.ToDecimal(encod.Decrypt(itemx.hargaEncrypt));
                        itemx.harga = harga;
                    }
                }
            }

            ctx.SaveChanges();

            return 1;
        }

        public ViewVendors GetVendorById(int VendorId)
        {
            var oVendor = ctx.Vendors.Where(d => d.Id == VendorId).Select(d => new ViewVendors
            {
                id = d.Id,
                Alamat = d.Alamat,
                Email = d.Email,
                Nama = d.Nama,
                Owner = d.Owner
            }).FirstOrDefault();
            return oVendor;
        }

        public decimal? efisiensi(Guid Id, Guid UserId)
        {
            var totalHps = (from bb in ctx.RKSHeaders
                            join cc in ctx.RKSDetails on bb.Id equals cc.RKSHeaderId
                            where bb.PengadaanId == Id
                            select cc).Sum(xx => xx.hps) == null ? 0 :
                                 (from bb in ctx.RKSHeaders
                                  join cc in ctx.RKSDetails on bb.Id equals cc.RKSHeaderId
                                  where bb.PengadaanId == Id
                                  select cc).Sum(xx => xx.hps).Value;
            var totalReliat = getPemenangPengadaan(Id, UserId).FirstOrDefault() == null ? 0 :
                getPemenangPengadaan(Id, UserId).FirstOrDefault().total;

            if (totalReliat == null) return null;
            if (totalHps == null) return null;
            var scoring = ((totalHps - totalReliat) / totalHps) * 100;
            return scoring.Value;

        }

        public List<VWReportPengadaan> GetRepotPengadan(DateTime? dari, DateTime? sampai, Guid UserId)
        {
            var oReport = (from b in ctx.Pengadaans
                           //join c in ctx.BeritaAcaras on b.Id equals c.PengadaanId
                           where b.TanggalMenyetujui >= dari && b.TanggalMenyetujui <= sampai //c.tanggal >= dari && c.tanggal <= sampai// && c.Tipe == TipeBerkas.BeritaAcaraPenentuanPemenang
                           select new VWReportPengadaan
                           {
                               PengadaanId = b.Id,
                               Judul = b.Judul,
                               User = b.UnitKerjaPemohon,
                               hps = (from bb in ctx.RKSHeaders
                                      join cc in ctx.RKSDetails on bb.Id equals cc.RKSHeaderId
                                      where bb.PengadaanId == b.Id
                                      select cc).Sum(xx => xx.hps) == null ? 0 :
                                    (from bb in ctx.RKSHeaders
                                     join cc in ctx.RKSDetails on bb.Id equals cc.RKSHeaderId
                                     where bb.PengadaanId == b.Id
                                     select cc).Sum(xx => xx.hps * xx.jumlah).Value
                           }).Distinct().ToList();
            foreach (var item in oReport)
            {
                item.realitas = getPemenangPengadaan(item.PengadaanId.Value, UserId).FirstOrDefault() == null ? 0 :
                    getPemenangPengadaan(item.PengadaanId.Value, UserId).FirstOrDefault().total;
                item.efisiensi = efisiensi(item.PengadaanId.Value, UserId);
                item.Pemenang = getPemenangPengadaan(item.PengadaanId.Value, UserId).FirstOrDefault() == null ? "" :
                            getPemenangPengadaan(item.PengadaanId.Value, UserId).FirstOrDefault().NamaVendor;
                item.Aanwjzing = getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraAanwijzing, UserId) == null ? null :
                        getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraAanwijzing, UserId).tanggal;
                item.PembukaanAmplop = getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraBukaAmplop, UserId) == null ? null :
                        getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraBukaAmplop, UserId).tanggal;
                item.Klasrifikasi = getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraKlarifikasi, UserId) == null ? null :
                    getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraKlarifikasi, UserId).tanggal;
                item.Scoring = getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraPenilaian, UserId) == null ? null :
                    getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraPenilaian, UserId).tanggal;
                item.NotaPemenang = getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraPenentuanPemenang, UserId) == null ? null :
                    getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.BeritaAcaraPenentuanPemenang, UserId).tanggal;
                item.SPK = getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.SuratPerintahKerja, UserId) == null ? null :
                getBeritaAcaraByTipe(item.PengadaanId.Value, TipeBerkas.SuratPerintahKerja, UserId).tanggal;
            }
            return oReport;
        }

        public List<VWStaffCharges> GetSummaryTotal(DateTime dari, DateTime sampai, int limit = Int32.MaxValue, int skip = 0)
        {
            var fPengadaan = ctx.Pengadaans
                .Where(b => b.TanggalMenyetujui >= dari && b.TanggalMenyetujui <= sampai)
                .OrderBy(x => x.TanggalMenyetujui)
                .Skip(skip).Take(limit);
            VWStaffCharges v0 = new VWStaffCharges { Nama = "BERJALAN", Jumlah = fPengadaan.Where(x => x.GroupPengadaan == EGroupPengadaan.DALAMPELAKSANAAN).Count() };
            VWStaffCharges v1 = new VWStaffCharges { Nama = "SELESAI", Jumlah = fPengadaan.Where(x => x.GroupPengadaan == EGroupPengadaan.ARSIP && x.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null).Count() };
            VWStaffCharges v2 = new VWStaffCharges { Nama = "BATAL", Jumlah = fPengadaan.Where(x => x.Status == EStatusPengadaan.DIBATALKAN).Count() };
            return new List<VWStaffCharges> { v0, v1, v2 };
        }

        public List<VWProgressReport> GetProgressReport(DateTime dari, DateTime sampai, int limit = Int32.MaxValue, int skip = 0)
        {
            return ctx.Pengadaans
                .Where(b => b.TanggalMenyetujui >= dari && b.TanggalMenyetujui <= sampai)
                .OrderBy(x => new { x.Status, x.TanggalMenyetujui })//.OrderBy(x => x.Status)
                .Skip(skip).Take(limit)
                .Select(x => new VWProgressReport { Judul = x.Judul, Progress = (int)x.Status }).ToList();
        }

        public List<VWStaffCharges> GetStaffCharges(string charge, DateTime dari, DateTime sampai, int limit = Int32.MaxValue, int skip = 0)
        {
            List<Guid> lg = ctx.Pengadaans
                .Where(b => b.TanggalMenyetujui >= dari && b.TanggalMenyetujui <= sampai)
                .OrderBy(x => x.TanggalMenyetujui)
                .Skip(skip).Take(limit)
                .Select(x => x.Id).ToList();
            if (lg != null)
            {
                return ctx.PersonilPengadaans
                    .Where(x => lg.Contains((Guid)x.PengadaanId) && x.tipe == charge)
                    .GroupBy(x => new { Nama = x.Nama })
                    .Select(x => new VWStaffCharges
                    {
                        Nama = x.Key.Nama,
                        Jumlah = x.Count()
                    }).ToList();
            }
            return new List<VWStaffCharges>();
        }
        public int PembatalanPengadaan(VWPembatalanPengadaan vwPembatalan, Guid UserId)
        {


            var oPIC = ctx.PersonilPengadaans.Where(d => d.PengadaanId == vwPembatalan.PengadaanId && d.tipe == PengadaanConstants.StaffPeranan.PIC).FirstOrDefault();
            if (oPIC == null) return 0;

            var oPengadaan = ctx.Pengadaans.Find(vwPembatalan.PengadaanId);
            oPengadaan.Status = EStatusPengadaan.DIBATALKAN;
            oPengadaan.GroupPengadaan = EGroupPengadaan.ARSIP;

            PembatalanPengadaan oPembatalanPengadaan = ctx.PembatalanPengadaans.Where(d => d.PengadaanId == vwPembatalan.PengadaanId).FirstOrDefault();
            if (oPembatalanPengadaan == null)
            {
                oPembatalanPengadaan = new PembatalanPengadaan();

                oPembatalanPengadaan.PengadaanId = vwPembatalan.PengadaanId;
                oPembatalanPengadaan.Keterangan = vwPembatalan.Keterangan;
                oPembatalanPengadaan.CreateOn = DateTime.Now;
                oPembatalanPengadaan.CreateBy = UserId;
                ctx.PembatalanPengadaans.Add(oPembatalanPengadaan);
            }
            ctx.SaveChanges();
            return 1;
        }

        public PenolakanPengadaan GetPenolakanMessage(Guid Id, Guid UserId)
        {
            return ctx.PenolakanPengadaans.Where(d => d.PengadaanId == Id && d.status == 1).FirstOrDefault();
        }

        public PembatalanPengadaan GetPembatalanPengadaan(Guid Id, Guid UserId)
        {
            return ctx.PembatalanPengadaans.Where(d => d.PengadaanId == Id).FirstOrDefault();
        }
        
        public Vendor GetPemenang(Guid Id, Guid userId)
        {
            if (isSpkUploaded(Id, userId) == 0)
                return new Vendor();
            else
            {
                int VEndorId = ctx.PemenangPengadaans.Where(d => d.PengadaanId == Id).FirstOrDefault() == null ? 0 : ctx.PemenangPengadaans.Where(d => d.PengadaanId == Id).FirstOrDefault().VendorId.Value;
                if (VEndorId == 0) return new Vendor();
                else
                {
                    Vendor oVendor = ctx.Vendors.Where(d => d.Id == VEndorId).FirstOrDefault();
                    if (oVendor == null) return new Vendor();
                    else return new Vendor
                    {
                        Id = oVendor.Id,
                        Nama = oVendor.Nama,
                        Email = oVendor.Email,
                        Telepon = oVendor.Telepon
                    };
                }
            }

        }
        
        public Pengadaan ChangeStatusPengadaan(Guid Id,EStatusPengadaan status,Guid UserId)
        {
            Pengadaan oPengadaan = ctx.Pengadaans.Find(Id);
            try
            {
                if(EStatusPengadaan.DISETUJUI==status)
                    oPengadaan.NoPengadaan= GenerateNoPengadaan(UserId);
                oPengadaan.Status = status;
                oPengadaan.GroupPengadaan = EGroupPengadaan.DALAMPELAKSANAAN;
                ctx.SaveChanges();
                return oPengadaan;
            }
            catch
            {
                return new Pengadaan();
            }
            
        }

        public int nextToStateWithChangeScheduldDate(Guid PengadaanId, Guid UserId, EStatusPengadaan state,DateTime from,DateTime? to)
        {
            Pengadaan Mpengadaaan = ctx.Pengadaans.Find(PengadaanId);
            PersonilPengadaan picPersonil = ctx.PersonilPengadaans.Where(d => d.PersonilId == UserId && d.tipe == "pic" && d.PengadaanId == PengadaanId).FirstOrDefault();
            if (picPersonil == null) return 0;

            Mpengadaaan.Status = state;

            JadwalPelaksanaan dtJadwl = ctx.JadwalPelaksanaans.Where(d => d.PengadaanId == PengadaanId && d.statusPengadaan == state).FirstOrDefault();
            if (dtJadwl == null)
            {
               JadwalPelaksanaan newJadwal = new JadwalPelaksanaan();
               newJadwal.Mulai = from;
               if (state != EStatusPengadaan.PEMENANG)
                    newJadwal.Sampai = to;
                newJadwal.statusPengadaan = state;
                newJadwal.PengadaanId = PengadaanId;
                ctx.JadwalPelaksanaans.Add(newJadwal);
            }
            else
            {
                dtJadwl.Mulai = from;
                if (state != EStatusPengadaan.PEMENANG)
                    dtJadwl.Sampai = to;
            }
            try
            {
                RiwayatDokumen newRiwayatPengadaan = new RiwayatDokumen();
                newRiwayatPengadaan.PengadaanId = PengadaanId;
                newRiwayatPengadaan.Status = state.ToString();
                newRiwayatPengadaan.ActionDate = DateTime.Now;
                ctx.RiwayatDokumens.Add(newRiwayatPengadaan);
                RiwayatPengadaan newRiwayatPengadaan2 = new RiwayatPengadaan();
                newRiwayatPengadaan2.PengadaanId = PengadaanId;
                newRiwayatPengadaan2.Status = state;
                newRiwayatPengadaan2.Waktu = DateTime.Now;
                ctx.RiwayatPengadaans.Add(newRiwayatPengadaan2);
                ctx.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
           
        }

        public List<PersonilPengadaan> getPersonilPengadaan(Guid PengadaanId)
        {
            try
            {
                return ctx.PersonilPengadaans.Where(d=>d.PengadaanId==PengadaanId).ToList();
            }
            catch
            {
                return new List<PersonilPengadaan>();
            }

        }

        public List<VWRiwayatPengadaan> GetRiwayatDokumenForVendor(Guid UserId)
        {
            var UserVendor=ctx.Vendors.Where(d=>d.Owner==UserId).FirstOrDefault();
            if (UserVendor == null) return new List<VWRiwayatPengadaan>();
            var lstRiwyatDokumen = (from b in ctx.RiwayatPengadaans
                                    join c in ctx.Pengadaans on b.PengadaanId equals c.Id
                                    join d in ctx.KandidatPengadaans on b.PengadaanId equals d.PengadaanId
                                    where d.VendorId == UserVendor.Id && b.Status > EStatusPengadaan.DISETUJUI
                                    select new VWRiwayatPengadaan
                                    {
                                        Waktu = b.Waktu,
                                        Komentar = b.Komentar,
                                        Id = b.Id,
                                        PengadaanId = b.PengadaanId,
                                        Status = b.Status.ToString(),
                                        JudulPengadaan = c.Judul
                                    }).ToList();
            return lstRiwyatDokumen;
        }

        //workflow
        //public Pengadaan PersetujuanWorkflow(Guid Id, Guid UserId)
        //{
        //    Pengadaan oPengadaan = ctx.Pengadaans.Find(Id);
        //    if (oPengadaan == null) return new Pengadaan();
        //    var oPersonil = ctx.PersonilPengadaans.Where(d => d.PengadaanId == oPengadaan.Id );
        //    if (oPersonil.Where(d=>d.tipe == PengadaanConstants.StaffPeranan.PIC).FirstOrDefault() == null) return new Pengadaan();


        //    Workflow nWorkflow = ctx.Workflows.Where(d => d.DocumentId == Id).FirstOrDefault();
        //    if (nWorkflow == null)
        //    {
        //        return new Pengadaan();
        //    }
        //    else
        //    {
        //        WorkflowApproval oWorkflowApproval = ctx.WorkflowApprovals.Where(d => d.WorkflowId == nWorkflow.Id).OrderBy(d => d.SegOrder).LastOrDefault();
               
        //        var oWorkflowMasterTemplateDetail = ctx.WorkflowMasterTemplateDetails.Where(d => d.WorkflowMasterTemplateId == nWorkflow.WorkflowMasterTemplateId).OrderBy(d=>d.SegOrder);
        //        var maxSegOrder = oWorkflowMasterTemplateDetail.LastOrDefault().SegOrder;
        //        var curSegOrder = oWorkflowApproval.SegOrder;
        //        var nextSegOrder = curSegOrder + 1;
        //        var prevSegOrder = curSegOrder - 1;
        //        var UserApporer = oPersonil.Where(d => d.tipe == PengadaanConstants.StaffPeranan.Controller).LastOrDefault().PersonilId;
        //        if (oWorkflowApproval != null)
        //        {
        //            UserApporer = oWorkflowMasterTemplateDetail.Where(d => d.SegOrder == curSegOrder).FirstOrDefault().UserId;
        //        }
        //        if (UserId != UserApporer) return new Pengadaan();
        //        oWorkflowApproval = new WorkflowApproval();
        //        oWorkflowApproval.SegOrder = oWorkflowMasterTemplateDetail.FirstOrDefault().SegOrder;
        //        oWorkflowApproval.UserId = UserId;
        //        oWorkflowApproval.WorkflowId = nWorkflow.Id;
        //        oWorkflowApproval.WorkflowStatusCode = 1;
        //        ctx.WorkflowApprovals.Add(oWorkflowApproval);

        //        if (oWorkflowMasterTemplateDetail.Where(d => d.SegOrder == curSegOrder).LastOrDefault().UserId != UserId) return new Pengadaan();
        //        if (nWorkflow.LastStatusCode == 2) return new Pengadaan();
        //        if (curSegOrder == maxSegOrder)
        //        {
        //            nWorkflow.NextUserId = null;
        //            nWorkflow.PrevUserId = oWorkflowMasterTemplateDetail.Where(d => d.SegOrder == curSegOrder).LastOrDefault().UserId;
        //            nWorkflow.NextStatusCode = null;
        //            nWorkflow.LastStatusCode = 2;
        //            oPengadaan.Status = EStatusPengadaan.DISETUJUI;
        //        }
        //        nWorkflow.NextUserId = oWorkflowMasterTemplateDetail.Where(d => d.SegOrder == nextSegOrder).LastOrDefault().UserId;
        //        nWorkflow.NextStatusCode = 1;
        //        nWorkflow.LastStatusCode = 1;
        //        if (nextSegOrder == maxSegOrder)
        //        {
        //            nWorkflow.NextStatusCode = 2;
        //            nWorkflow.LastStatusCode = 1;
        //        }
        //    }
        //    ctx.SaveChanges();
        //    return new Pengadaan();
        //}

        //public int AjukanWorkflow(Guid Id, Guid UserId, Guid WorkflowtemplateId)
        //{
        //    Pengadaan oPengadaan = ctx.Pengadaans.Find(Id);
        //    if (oPengadaan == null) return 0;
        //    var oPersonil = ctx.PersonilPengadaans.Where(d => d.PengadaanId == oPengadaan.Id);
        //    if (oPersonil.Where(d => d.tipe == PengadaanConstants.StaffPeranan.PIC).FirstOrDefault() == null) return 0;

        //    Workflow nWorkflow = ctx.Workflows.Where(d => d.DocumentId == Id).FirstOrDefault();
        //    if (nWorkflow == null)
        //    {
        //        nWorkflow = new Workflow();
        //        nWorkflow.DocumentId = Id;
        //        nWorkflow.WorkflowMasterTemplateId = WorkflowtemplateId;
        //        nWorkflow.NextUserId = oPersonil.Where(d => d.tipe == PengadaanConstants.StaffPeranan.Controller).FirstOrDefault().PersonilId;
        //        nWorkflow.NextStatusCode = 1;
        //        nWorkflow.LastStatusCode = 1;
        //        ctx.Workflows.Add(nWorkflow);
        //    }
        //    else
        //    {
        //        return 0;                
        //    }
        //    oPengadaan.Status = EStatusPengadaan.AJUKAN;
        //    ctx.SaveChanges();
        //    return 1;
        //}

    }
}


//(from b in ctx.Pengadaans
//      from c in ctx.PersonilPengadaans
//        .Where(x => x. == users.BE_ID).DefaultIfEmpty()

//ctx.Pengadaans
//.SelectMany(d => d.PersonilPengadaans
//    .Where(x => x.PersonilId == UserId || Roles.Contains(PengadaanConstants.Role.DepHead)));

//(from b in ctx.Pengadaans
//                 where b.PersonilPengadaans.Where(x=>x.PersonilId==UserId) 
//                 select b).OrderByDescending(d=>d.Id).Skip(start).Take(limit).ToList();



//(from b in ctx.Pengadaans
//.Where(x => x.GroupPengadaan == groupstatus)
//from c in ctx.PersonilPengadaans
//.Where(x => x.PersonilId == UserId || Roles.Contains(PengadaanConstants.Role.DepHead)).DefaultIfEmpty()
//select b).Distinct().OrderBy(d=>d.Id).Skip(start).Take(limit).ToList();


//public List<ViewPengadaan> GetPengadaans(int start, int limit, Guid? UserId, List<string> Roles, EGroupPengadaan groupstatus)
//       {
//           //Guid manajer =new Guid( ConfigurationManager.AppSettings["manajer"].ToString());
//           if (limit > 0)
//           {
//               List<ViewPengadaan> VWPengadaans = (from b in ctx.Pengadaans
//                   .Where(x => x.GroupPengadaan == groupstatus)
//                   from c in ctx.PersonilPengadaans
//                   .Where(x => x.PersonilId == UserId).DefaultIfEmpty()
//                   grup b by new
//                   {
//                       b.Id,
//                       b.Judul,
//                       b.AturanBerkas,
//                       b.AturanPenawaran,
//                       b.AturanPengadaan,
//                       b.Keterangan,
//                       b.Status,
//                       b.GroupPengadaan,
//                       b.TitleDokumenNotaInternal,
//                       b.TitleDokumenLain,
//                       b.TitleBerkasRujukanLain,
//                       //b.PersonilPengadaans,
//                       //b.KandidatPengadaans,
//                       //b.JadwalPengadaans,
//                       //b.DokumenPengadaans
//                   } into h
//                   select new ViewPengadaan
//                   {
//                       Id = h.Key.Id,
//                       Judul = h.Key.Judul,
//                       TitleDokumenNotaInternal = h.Key.TitleDokumenNotaInternal,
//                       TitleDokumenLain = h.Key.TitleDokumenLain,
//                       TitleBerkasRujukanLain = h.Key.TitleBerkasRujukanLain,
//                       PersonilPengadaans = (from b in ctx.PersonilPengadaans
//                                             where b.PengadaanId==h.Key.Id
//                                             select new VWPersonilPengadaan
//                                             {
//                                                 Id = b.Id,
//                                                 Jabatan = b.Jabatan,
//                                                 Nama = b.Nama,
//                                                 PersonilId = b.PersonilId,
//                                                 tipe = b.tipe
//                                             }).ToList(),
//                       KandidatPengadaans = (from b in ctx.KandidatPengadaans
//                                             join c in ctx.Vendors on b.VendorId equals c.Id
//                                             where b.PengadaanId == h.Key.Id
//                                             select new VWKandidatPengadaan
//                                             {
//                                                 Id = b.Id,
//                                                 PengadaanId = b.PengadaanId,
//                                                 VendorId = b.VendorId,
//                                                 Nama = c.Nama
//                                             }).ToList(),
//                       JadwalPengadaans = (from b in ctx.JadwalPengadaans
//                                           where b.PengadaanId == h.Key.Id
//                                           select new VWJadwalPengadaan
//                                           {
//                                               Id = b.Id,
//                                               PengadaanId = b.PengadaanId,
//                                               Mulai = b.Mulai,
//                                               Sampai = b.Sampai,
//                                               tipe = b.tipe
//                                           }).ToList(),
//                       DokumenPengadaans = (from b in ctx.DokumenPengadaans
//                                            where b.PengadaanId == h.Key.Id
//                                            select new VWDokumenPengadaan
//                                            {
//                                                Id = b.Id,
//                                                PengadaanId = b.PengadaanId,
//                                                ContentType = b.ContentType,
//                                                File = b.File,
//                                                Title = b.Title,
//                                                Tipe=b.Tipe
//                                            }).ToList(),
//                       Keterangan = h.Key.Keterangan,
//                       Status = h.Key.Status,
//                       //StatusBintang = ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault() == null ? 0 : ctx.BintangPengadaans.Where(x => x.PengadaanId == h.Key.Id && x.UserId == UserId).FirstOrDefault().StatusBintang,
//                       AturanPengadaan = h.Key.AturanPengadaan,
//                       AturanBerkas = h.Key.AturanBerkas,
//                       AturanPenawaran = h.Key.AturanPenawaran,
//                       GroupPengadaan = h.Key.GroupPengadaan
//                   }).OrderBy(x => x.Id).Skip(start).Take(limit).ToList();
//               return VWPengadaans;
//           }
//           return new List<ViewPengadaan>();
//       }





//public StateJadwalPengadaan stateJadwal(EStatusPengadaan status, Guid PengadaanId)
//        {
//            StateJadwalPengadaan MnextJadwal = new StateJadwalPengadaan();
//            if (status == EStatusPengadaan.DISETUJUI)
//            {
//                PelaksanaanAanwijzing MPelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings
//                            .Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
//                if (MPelaksanaanAanwijzing != null)
//                {
//                    MnextJadwal.Mulai = MPelaksanaanAanwijzing.Mulai;
//                    MnextJadwal.status = EStatusPengadaan.AANWIJZING;
//                    MnextJadwal.PengadaanId = PengadaanId;
//                }
//                else
//                {
//                    JadwalPengadaan MJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId
//                                     && d.tipe == "Aanwijzing").FirstOrDefault();
//                    if (MJadwalPengadaan == null) return new StateJadwalPengadaan();
//                    else
//                    {
//                        MnextJadwal.Mulai = MJadwalPengadaan.Mulai;
//                        MnextJadwal.PengadaanId = PengadaanId;
//                        MnextJadwal.status = EStatusPengadaan.AANWIJZING;
//                    }
//                }
//            }

//            if (status == EStatusPengadaan.AANWIJZING)
//            {
//                PelaksanaanAanwijzing MPelaksanaanAanwijzing = ctx.PelaksanaanAanwijzings
//                            .Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
//                if (MPelaksanaanAanwijzing != null)
//                {
//                    MnextJadwal.Mulai = MPelaksanaanAanwijzing.Mulai;
//                    MnextJadwal.status = EStatusPengadaan.AANWIJZING;
//                    MnextJadwal.PengadaanId = PengadaanId;
//                }
//                else
//                {
//                    JadwalPengadaan MJadwalPengadaan = ctx.JadwalPengadaans.Where(d => d.PengadaanId == PengadaanId
//                                     && d.tipe ==PengadaanConstants.StatusPengadaan.Aanwijzing).FirstOrDefault();
//                    if (MJadwalPengadaan == null) return new StateJadwalPengadaan();
//                    else
//                    {
//                        MnextJadwal.Mulai = MJadwalPengadaan.Mulai;
//                        MnextJadwal.PengadaanId = PengadaanId;
//                        MnextJadwal.status = EStatusPengadaan.AANWIJZING;
//                    }
//                }
//            }

//            return MnextJadwal;
//        }