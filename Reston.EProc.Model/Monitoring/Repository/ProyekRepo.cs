using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Eproc.Model.Monitoring.Repository;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Repository
{
    public interface IProyekRepo
    {
        ResultMessage SimpanPenilaian(List<PenilaianVendor> Nilai, Guid UserId);
        ResultMessage deleteTahap(Guid Id, Guid UserId);
        ResultMessage deleteDokTahap(Guid Id, Guid UserId);
        ResultMessage deletePICProyek(Guid Id, Guid UserId);
        ViewProyekPerencanaan GetDataProyek(Guid PengadaanId);
        ResultMessage savePICProyek(ViewUntukProyekAddPersonil Personil, Guid UserId);
        ResultMessage SimpanRencanaProyekRepo(Guid xPengadaanId,string xStatus, Guid UserId, DateTime? xStartDate, DateTime? xEndDate);
        ResultMessage SimpanTahapanPekerjaanDokumenRepo(Guid xId_Tahapan, string xNamaDokumen, string xJenisDokumen, Guid UserId);
        ResultMessage SimpanTahapanPembayaranDokumenRepo(Guid xId_Tahapan, string xNamaDokumen, string xJenisDokumen, Guid UserId);
        ResultMessage SimpanProyekRepo(Guid Id, string NoKontrak, string Status, Guid UserId);
        ResultMessage SimpanTahapanPekerjaanRepo(Guid xPengadaanId, string xNamaTahapanPekerjaan, string xJenisPekerjaan, decimal xBobotPekerjaan, Guid UserId, DateTime? xTanggalMulai, DateTime? xTanggalSelesai);
        ResultMessage SimpanTahapanPekerjaanRekananRepo(Guid xProyekId, string xNamaTahapanPekerjaan, string xJenisPekerjaan, decimal xBobotPekerjaan, Guid UserId, DateTime? xTanggalMulai, DateTime? xTanggalSelesai);
        DataTableViewTahapanPekerjaan GetDataPekerjaan(Guid PengadaanId);
        DataTableViewTahapanPembayaran GetDataPembayaran(Guid PengadaanId);
        DataTableViewDokumenTahapanPekerjaan GetDataDokumenPekerjaan(Guid TahapanId);
        DataTableViewDokumenTahapanPekerjaan GetDataDokumenPembayaran(Guid TahapanId);
        DataTableViewPenilaian GetDataPenilaian(Guid IdProyek);
        DataTableViewPenilaian GetDataPenilaianRekanan(Guid IdProyek);
    }

    public class ProyekRepo : IProyekRepo
    {
        JimbisContext ctx;
        ResultMessage msg = new ResultMessage();

        public ProyekRepo(JimbisContext j)
        {
            ctx = j;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        // Simpan Dokumen Tahap Pekerjaan
        public ResultMessage SimpanTahapanPekerjaanDokumenRepo(Guid xId_Tahapan, string xNamaDokumen, string xJenisDokumen, Guid UserId)
        {
            ResultMessage rm2 = new ResultMessage();
            try
            {
                DokumenProyek dkm = new DokumenProyek
                {
                    TahapanId = xId_Tahapan,
                    NamaDokumen = xNamaDokumen,
                    JenisDokumen = xJenisDokumen,
                    CreatedOn = DateTime.Now,
                    CreatedBy = UserId,
                };
                ctx.DokumenProyeks.Add(dkm);
                ctx.SaveChanges(UserId.ToString());
                rm2.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                rm2.status = HttpStatusCode.ExpectationFailed;
                rm2.message = ex.ToString();
            }
            return rm2;
        }

        // Simpan Dokumen Tahap Pembayaran
        public ResultMessage SimpanTahapanPembayaranDokumenRepo(Guid xId_Tahapan, string xNamaDokumen, string xJenisDokumen, Guid UserId)
        {
            ResultMessage rm2 = new ResultMessage();
            try
            {
                DokumenProyek dkm = new DokumenProyek
                {
                    TahapanId = xId_Tahapan,
                    NamaDokumen = xNamaDokumen,
                    JenisDokumen = xJenisDokumen,
                    CreatedOn = DateTime.Now,
                    CreatedBy = UserId,
                };
                ctx.DokumenProyeks.Add(dkm);
                ctx.SaveChanges(UserId.ToString());
                rm2.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                rm2.status = HttpStatusCode.ExpectationFailed;
                rm2.message = ex.ToString();
            }
            return rm2;
        }

        // Ambil Data Proyek
        public ViewProyekPerencanaan GetDataProyek(Guid PengadaanId)
        {
            var oProyekPerencanaan = ctx.Pengadaans.Where(d =>d.Id == PengadaanId).FirstOrDefault();
            var vendorId= ctx.PemenangPengadaans.Where(d =>d.PengadaanId == PengadaanId).FirstOrDefault() != null ? ctx.PemenangPengadaans.Where(d =>d.PengadaanId == PengadaanId).FirstOrDefault().VendorId : null;
            var proyek = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
            var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault();
            var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah * d.hps == null ? 0 : d.jumlah * d.hps) : 0;

            return new ViewProyekPerencanaan
            {
                Id = oProyekPerencanaan.Id,
                Judul = oProyekPerencanaan.Judul,
                NoPengadaan = oProyekPerencanaan.NoPengadaan,
                NoKontrak = proyek == null ? null : proyek.NoKontrak != null ? proyek.NoKontrak : null,
                NilaiKontrak = TotalHps.Value != 0 ? TotalHps.Value : 0,
                Pelaksana = ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault() != null ? ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault().Nama : null,
                TanggalMulai = proyek != null ? proyek.StartDate : null,
                TanggalSelesai = proyek != null ? proyek.EndDate : null,
                PIC = proyek != null ? proyek.PICProyeks.Select(d => new ViewPIC { Id = d.Id, NamaPIC = d.Nama }).ToList() : null
            };
        }
        
        // Simpan Tahapan Pekerjaan PIC
        public  ResultMessage SimpanTahapanPekerjaanRepo(Guid xPengadaanId,string xNamaTahapanPekerjaan, string xJenisPekerjaan, decimal xBobotPekerjaan, Guid UserId, DateTime? xTanggalMulai, DateTime? xTanggalSelesai)
        {
            ResultMessage rkk = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.PengadaanId == xPengadaanId).FirstOrDefault();
                var IdProyek = odata.Id;
                var BlmAdaTahapan = ctx.TahapanProyeks.Where(d => d.ProyekId == IdProyek).Count();
                if (BlmAdaTahapan != 0)
                {
                    var TotalBobotPekerjaanDb = ctx.TahapanProyeks.Where(d => d.ProyekId == IdProyek).Sum(d => d.BobotPekerjaan != 0 ? d.BobotPekerjaan : 0);
                    var TotalBobotSeluruh = TotalBobotPekerjaanDb + xBobotPekerjaan;

                    if (TotalBobotPekerjaanDb <= 100)
                    {
                        TahapanProyek th = new TahapanProyek
                        {
                            ProyekId = IdProyek,
                            NamaTahapan = xNamaTahapanPekerjaan,
                            TanggalMulai = xTanggalMulai,
                            TanggalSelesai = xTanggalSelesai,
                            CreatedOn = DateTime.Now,
                            CreatedBy = UserId,
                            JenisTahapan = xJenisPekerjaan,
                            BobotPekerjaan = xBobotPekerjaan
                        };
                        ctx.TahapanProyeks.Add(th);
                        ctx.SaveChanges(UserId.ToString());
                        rkk.status = HttpStatusCode.OK;
                    }
                    else
                    {
                        rkk.message = "Error (Total Bobot Pekerjaan Tidak Bisa lebih Dari 100 %)";
                    }
                }
                else
                {
                    var TotalBobotPekerjaanDb = ctx.TahapanProyeks.Where(d => d.ProyekId == IdProyek).Sum(d => d.BobotPekerjaan);

                    var TotalBobotSeluruh = TotalBobotPekerjaanDb + xBobotPekerjaan;
                    if (TotalBobotSeluruh <= 100)
                    {
                        TahapanProyek th = new TahapanProyek
                        {
                            ProyekId = IdProyek,
                            NamaTahapan = xNamaTahapanPekerjaan,
                            TanggalMulai = xTanggalMulai,
                            TanggalSelesai = xTanggalSelesai,
                            CreatedOn = DateTime.Now,
                            CreatedBy = UserId,
                            JenisTahapan = xJenisPekerjaan,
                            BobotPekerjaan = xBobotPekerjaan
                        };
                        ctx.TahapanProyeks.Add(th);
                        ctx.SaveChanges(UserId.ToString());
                        rkk.status = HttpStatusCode.OK;
                        rkk.message = "Data Berhasil Di Simpan";
                    }
                    else
                    {
                        rkk.message = "Error (Total Bobot Pekerjaan Tidak Bisa lebih Dari 100 %)";
                    }
                }
            }
            catch (Exception ex)
            {
                rkk.status = HttpStatusCode.ExpectationFailed;
                rkk.message = ex.ToString();
            }
            return rkk;
        }

        // Simpan Tahapan Pekerjaan Vendor
        public ResultMessage SimpanTahapanPekerjaanRekananRepo(Guid xProyekId, string xNamaTahapanPekerjaan, string xJenisPekerjaan, decimal xBobotPekerjaan, Guid UserId, DateTime? xTanggalMulai, DateTime? xTanggalSelesai)
        {
            ResultMessage rkk = new ResultMessage();
            try
            {
                var idproyek = xProyekId;
                var BlmAdaTahapan = ctx.TahapanProyeks.Where(d => d.ProyekId == idproyek).Count();
                if (BlmAdaTahapan != 0)
                {
                    var TotalBobotPekerjaanDb = ctx.TahapanProyeks.Where(d => d.ProyekId == idproyek).Sum(d => d.BobotPekerjaan != 0 ? d.BobotPekerjaan : 0);
                    var TotalBobotSeluruh = TotalBobotPekerjaanDb + xBobotPekerjaan;

                    if (TotalBobotPekerjaanDb <= 100)
                    {
                        TahapanProyek th = new TahapanProyek
                        {
                            ProyekId = idproyek,
                            NamaTahapan = xNamaTahapanPekerjaan,
                            TanggalMulai = xTanggalMulai,
                            TanggalSelesai = xTanggalSelesai,
                            CreatedOn = DateTime.Now,
                            CreatedBy = UserId,
                            JenisTahapan = xJenisPekerjaan,
                            BobotPekerjaan = xBobotPekerjaan
                        };
                        ctx.TahapanProyeks.Add(th);
                        ctx.SaveChanges(UserId.ToString());
                        rkk.status = HttpStatusCode.OK;
                    }
                    else
                    {
                        rkk.message = "Error (Total Bobot Pekerjaan Tidak Bisa lebih Dari 100 %)";
                    }
                }
                else
                {
                    var TotalBobotSeluruh = xBobotPekerjaan;

                    if (TotalBobotSeluruh <= 100)
                    {
                        TahapanProyek th = new TahapanProyek
                        {
                            ProyekId = idproyek,
                            NamaTahapan = xNamaTahapanPekerjaan,
                            TanggalMulai = xTanggalMulai,
                            TanggalSelesai = xTanggalSelesai,
                            CreatedOn = DateTime.Now,
                            CreatedBy = UserId,
                            JenisTahapan = xJenisPekerjaan,
                            BobotPekerjaan = xBobotPekerjaan
                        };
                        ctx.TahapanProyeks.Add(th);
                        ctx.SaveChanges(UserId.ToString());
                        rkk.status = HttpStatusCode.OK;
                    }
                    else
                    {
                        rkk.message = "Error (Total Bobot Pekerjaan Tidak Bisa lebih Dari 100 %)";
                    }
                }
            }
            catch (Exception ex)
            {
                rkk.status = HttpStatusCode.ExpectationFailed;
                rkk.message = ex.ToString();
            }
            return rkk;
        }

        // Simpan Rencana Proyek
        public ResultMessage SimpanRencanaProyekRepo(Guid xPengadaanId, string xStatus, Guid UserId, DateTime? xStartDate, DateTime? xEndDate)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.PengadaanId == xPengadaanId).FirstOrDefault();

                if (odata != null)
                { }
                else
                {
                    RencanaProyek m2 = new RencanaProyek
                    {
                        PengadaanId = xPengadaanId,
                        StartDate = xStartDate,
                        EndDate = xEndDate,
                        Status = xStatus,
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    };

                    ctx.RencanaProyeks.Add(m2);
                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }
            return rm;
        }

        // Simpan Proyek
        public ResultMessage SimpanProyekRepo(Guid Id, string NoKontrak, string Status, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.PengadaanId == Id).FirstOrDefault();

                if (odata != null)
                {
                    odata.NoKontrak = NoKontrak;
                    odata.Status = Status;
                }
                ctx.SaveChanges(UserId.ToString());
                rm.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }
            return rm;
        }

        // Simpan PIC Proyek
        public ResultMessage savePICProyek(ViewUntukProyekAddPersonil Personil, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.PengadaanId == Personil.PengadaanId).FirstOrDefault();
                var idproyek = odata.Id;
                var idata = ctx.PICProyeks.Where(d => d.ProyekId == idproyek).FirstOrDefault();
                if (idata != null)
                { }
                else
                {
                    PICProyek p1 = new PICProyek
                    {
                        ProyekId = idproyek,
                        UserId = Personil.UserId,
                        Nama = Personil.Nama,
                        Jabatan = Personil.Jabatan,
                        tipe = Personil.tipe,
                        CreatedOn = DateTime.Now,
                        CreatedBy = UserId
                    };

                    ctx.PICProyeks.Add(p1);
                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.Id = p1.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }
            return rm;
        }

        // Get Data Pekerjaan
        public DataTableViewTahapanPekerjaan GetDataPekerjaan(Guid PengadaanId)
        {
            DataTableViewTahapanPekerjaan tp = new DataTableViewTahapanPekerjaan();
            var CekData = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).Count();
            if (CekData != 0)
            {
                var ProyekId = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault().Id;

                // record total yang tampil 
                tp.recordsTotal = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == ProyekId).Count();

                // filter berdasarkan Id
                tp.recordsFiltered = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == ProyekId).Count();

                var caritahapanpekerjaan = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == ProyekId).ToList();

                List<ViewListTahapan> vListTahapanPekerjaan = new List<ViewListTahapan>();
                foreach (var item in caritahapanpekerjaan)
                {
                    ViewListTahapan nViewListTahapanPekerjaan = new ViewListTahapan();

                    nViewListTahapanPekerjaan.Id = item.Id;
                    nViewListTahapanPekerjaan.NamaTahapan = item.NamaTahapan;
                    nViewListTahapanPekerjaan.TanggalMulai = item.TanggalMulai.Value;
                    nViewListTahapanPekerjaan.TanggalSelesai = item.TanggalSelesai.Value;
                    nViewListTahapanPekerjaan.JenisTahapan = item.JenisTahapan;
                    vListTahapanPekerjaan.Add(nViewListTahapanPekerjaan);
                }
                tp.data = vListTahapanPekerjaan;
            }
            else
            {
                var CekData2 = ctx.RencanaProyeks.Where(d => d.Id == PengadaanId).Count();

                // record total yang tampil 
                tp.recordsTotal = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == PengadaanId).Count();

                // filter berdasarkan Id
                tp.recordsFiltered = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == PengadaanId).Count();

                var caritahapanpekerjaan = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pekerjaan" && d.ProyekId == PengadaanId).ToList();

                List<ViewListTahapan> vListTahapanPekerjaan = new List<ViewListTahapan>();
                foreach (var item in caritahapanpekerjaan)
                {
                    ViewListTahapan nViewListTahapanPekerjaan = new ViewListTahapan();

                    nViewListTahapanPekerjaan.Id = item.Id;
                    nViewListTahapanPekerjaan.NamaTahapan = item.NamaTahapan;
                    nViewListTahapanPekerjaan.TanggalMulai = item.TanggalMulai.Value;
                    nViewListTahapanPekerjaan.TanggalSelesai = item.TanggalSelesai.Value;
                    nViewListTahapanPekerjaan.JenisTahapan = item.JenisTahapan;
                    vListTahapanPekerjaan.Add(nViewListTahapanPekerjaan);
                }
                tp.data = vListTahapanPekerjaan;
            }
            return tp;
        }

        // Get Data Pembayaran
        public DataTableViewTahapanPembayaran GetDataPembayaran(Guid PengadaanId)
        {
            DataTableViewTahapanPembayaran tp = new DataTableViewTahapanPembayaran();
            var CekData = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).Count();
            if (CekData != 0)
            {
                var ProyekId = ctx.RencanaProyeks.Where(d => d.PengadaanId == PengadaanId).FirstOrDefault().Id;

                // record total yang tampil 
                tp.recordsTotal = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == ProyekId).Count();

                // filter berdasarkan Id
                tp.recordsFiltered = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == ProyekId).Count();

                var caritahapanpembayaran = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == ProyekId).ToList();

                List<ViewListTahapan> vListTahapanPembayaran = new List<ViewListTahapan>();
                foreach (var item in caritahapanpembayaran)
                {
                    ViewListTahapan nViewListTahapanPembayaran = new ViewListTahapan();

                    nViewListTahapanPembayaran.Id = item.Id;
                    nViewListTahapanPembayaran.NamaTahapan = item.NamaTahapan;
                    nViewListTahapanPembayaran.TanggalMulai = item.TanggalMulai.Value;
                    nViewListTahapanPembayaran.TanggalSelesai = item.TanggalSelesai.Value;
                    nViewListTahapanPembayaran.JenisTahapan = item.JenisTahapan;
                    vListTahapanPembayaran.Add(nViewListTahapanPembayaran);
                }
                tp.data = vListTahapanPembayaran;
            }
            else {

                // record total yang tampil 
                tp.recordsTotal = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == PengadaanId).Count();

                // filter berdasarkan Id
                tp.recordsFiltered = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == PengadaanId).Count();

                var caritahapanpembayaran = ctx.TahapanProyeks.Where(d => d.JenisTahapan == "Pembayaran" && d.ProyekId == PengadaanId).ToList();

                List<ViewListTahapan> vListTahapanPembayaran = new List<ViewListTahapan>();
                foreach (var item in caritahapanpembayaran)
                {
                    ViewListTahapan nViewListTahapanPembayaran = new ViewListTahapan();

                    nViewListTahapanPembayaran.Id = item.Id;
                    nViewListTahapanPembayaran.NamaTahapan = item.NamaTahapan;
                    nViewListTahapanPembayaran.TanggalMulai = item.TanggalMulai.Value;
                    nViewListTahapanPembayaran.TanggalSelesai = item.TanggalSelesai.Value;
                    nViewListTahapanPembayaran.JenisTahapan = item.JenisTahapan;
                    vListTahapanPembayaran.Add(nViewListTahapanPembayaran);
                }
                tp.data = vListTahapanPembayaran;
            }
            return tp;
        }

        // Get Dokumen Tahap Pekerjaan
        public DataTableViewDokumenTahapanPekerjaan GetDataDokumenPekerjaan(Guid TahapanId)
        {
            DataTableViewDokumenTahapanPekerjaan dtd = new DataTableViewDokumenTahapanPekerjaan();
            // record total yang tampil 
            dtd.recordsTotal = ctx.DokumenProyeks.Where(d => d.TahapanId == TahapanId).Count();
            // filter berdasarkan Id
            dtd.recordsFiltered = ctx.DokumenProyeks.Where(d => d.TahapanId == TahapanId).Count();
            var caritahapandokumenpekerjaan = ctx.DokumenProyeks.Where(d => d.TahapanId == TahapanId).ToList();

            List<ViewListTahapanDokumenPekerjaan> vlistViewListTahapanDokumenPekerjaan = new List<ViewListTahapanDokumenPekerjaan>();

            foreach (var item in caritahapandokumenpekerjaan)
            {
                ViewListTahapanDokumenPekerjaan nViewListTahapanDokumenPekerjaan = new ViewListTahapanDokumenPekerjaan();

                nViewListTahapanDokumenPekerjaan.Id = item.Id;
                nViewListTahapanDokumenPekerjaan.NamaDokumen = item.NamaDokumen;
                nViewListTahapanDokumenPekerjaan.URL = item.URL;

                vlistViewListTahapanDokumenPekerjaan.Add(nViewListTahapanDokumenPekerjaan);
            }
            dtd.data = vlistViewListTahapanDokumenPekerjaan;
            return dtd;
        }

        // Get Dokumen Tahap Pekerjaan
        public DataTableViewDokumenTahapanPekerjaan GetDataDokumenPembayaran(Guid TahapanId)
        {
            DataTableViewDokumenTahapanPekerjaan dtd = new DataTableViewDokumenTahapanPekerjaan();

            var caritahapandokumenpekerjaan = ctx.DokumenProyeks.Where(d => d.TahapanId == TahapanId).ToList();

            List<ViewListTahapanDokumenPekerjaan> vlistViewListTahapanDokumenPekerjaan = new List<ViewListTahapanDokumenPekerjaan>();

            foreach (var item in caritahapandokumenpekerjaan)
            {
                ViewListTahapanDokumenPekerjaan nViewListTahapanDokumenPekerjaan = new ViewListTahapanDokumenPekerjaan();

                nViewListTahapanDokumenPekerjaan.Id = item.Id;
                nViewListTahapanDokumenPekerjaan.NamaDokumen = item.NamaDokumen;
                vlistViewListTahapanDokumenPekerjaan.Add(nViewListTahapanDokumenPekerjaan);
            }
            dtd.data = vlistViewListTahapanDokumenPekerjaan;
            return dtd;
        }

        // Get Dokumen Penilaian PIC
        public DataTableViewPenilaian GetDataPenilaian(Guid IdProyek)
        {
            DataTableViewPenilaian dtd = new DataTableViewPenilaian();

            var caripenilaian = ctx.ReferenceDatas.Where(d=>d.Qualifier=="Penilaian").ToList();

            var pengadaanid = ctx.RencanaProyeks.Where(d => d.Id == IdProyek).FirstOrDefault().PengadaanId;
            var vendorid = ctx.PemenangPengadaans.Where(p => p.PengadaanId == pengadaanid).FirstOrDefault().VendorId;

            List<ViewListPenilaian> vlistViewListPenilaian = new List<ViewListPenilaian>();

            foreach (var item in caripenilaian)
            {
                ViewListPenilaian nViewListPenilaian = new ViewListPenilaian();

                nViewListPenilaian.Id = item.Id;
                nViewListPenilaian.NamaPenilaian = item.LocalizedName;
                nViewListPenilaian.VendorId = vendorid.ToString();
                vlistViewListPenilaian.Add(nViewListPenilaian);
            }
            dtd.data = vlistViewListPenilaian;
            return dtd;
        }

        // Get Dokumen Penilaian Vendor
        public DataTableViewPenilaian GetDataPenilaianRekanan(Guid IdProyek)
        {
            DataTableViewPenilaian dtd = new DataTableViewPenilaian();

            var caripenilaian = ctx.ReferenceDatas.Where(d => d.Qualifier == "Penilaian").ToList();

            var pengadaanid = ctx.RencanaProyeks.Where(d => d.Id == IdProyek).FirstOrDefault().PengadaanId;
            var vendorid = ctx.PemenangPengadaans.Where(p => p.PengadaanId == pengadaanid).FirstOrDefault().VendorId;
            List<ViewListPenilaian> vlistViewListPenilaian = new List<ViewListPenilaian>();

            foreach (var item in caripenilaian)
            {
                ViewListPenilaian nViewListPenilaian = new ViewListPenilaian();

                nViewListPenilaian.Id = item.Id;
                nViewListPenilaian.NamaPenilaian = item.LocalizedName;
                nViewListPenilaian.Nilai = item.PenilaianVendors.FirstOrDefault().Nilai.ToString();
                nViewListPenilaian.Catatan = item.PenilaianVendors.FirstOrDefault().Catatan.ToString();
                nViewListPenilaian.VendorId = vendorid.ToString();
                vlistViewListPenilaian.Add(nViewListPenilaian);
            }
            dtd.data = vlistViewListPenilaian;
            return dtd;
        }

        // Hapus Tahapan Baik Pekerjaan Maupun Pembayaran
        public ResultMessage deleteTahap(Guid Id, Guid UserId)
        {
            TahapanProyek oData = ctx.TahapanProyeks.Find(Id);
            ctx.TahapanProyeks.Remove(oData);
            try
            {
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
            }
            catch (Exception ex)
            {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }

        // Hapus Tahapan Baik Pekerjaan Maupun Pembayaran
        public ResultMessage deleteDokTahap(Guid Id, Guid UserId)
        {
            DokumenProyek oData = ctx.DokumenProyeks.Find(Id);
            ctx.DokumenProyeks.Remove(oData);
            try
            {
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
            }
            catch (Exception ex)
            {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }

        // Hapus PIC Proyek
        public ResultMessage deletePICProyek(Guid Id, Guid UserId)
        {
            PICProyek oData = ctx.PICProyeks.Find(Id);
            ctx.PICProyeks.Remove(oData);
            try
            {
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
            }
            catch (Exception ex)
            {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }

        public ResultMessage SimpanPenilaian(List<PenilaianVendor> Nilai, Guid UserId)
        {
            ResultMessage msg = new ResultMessage();
            try
            {
                foreach (var item in Nilai)
                {
                    PenilaianVendor pv = new PenilaianVendor();
                    pv.ProyekId = item.ProyekId;
                    pv.VendorId = item.VendorId;
                    pv.ReferenceDataId = item.ReferenceDataId;
                    pv.Nilai = item.Nilai;
                    pv.Catatan = item.Catatan;
                    pv.CreatedOn = DateTime.Now;
                    pv.CreatedBy = UserId;

                    ctx.PenilaianVendors.Add(pv);
                    ctx.SaveChanges(UserId.ToString());
                    msg.status = HttpStatusCode.OK;
                    msg.Id = pv.Id.ToString();
                }
            }
            catch (Exception ex) {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }
    }
}
