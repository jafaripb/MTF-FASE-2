using Reston.Eproc.Model.Monitoring.Entities;
using Reston.Eproc.Model.Monitoring.Model;
using Reston.Pinata.Model;
using Reston.Pinata.Model.Helper;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

// ---------      query ada di sini

namespace Reston.Eproc.Model.Monitoring.Repository
{
    public interface IMoritoringRepo
    {
        ResultMessage DeleteDokumenProyek(Guid Id, Guid UserId);
        DataTableViewMonitoring GetDataMonitoringSelection(string search, int start, int length, StatusSeleksi? status);
        DataTableViewProyekSistemMonitoring GetDataListProyekMonitoring(string search, int start, int length, Klasifikasi? dklasifikasi);
        DataTableViewProyekSistemMonitoring GetDataListProyekMonitoringRekanan(string search, int start, int length, Klasifikasi? dklasifikasi, Guid? UserId);
		DataTableViewProyekSistemMonitoringPembayaran GetDataListProyekDetailMonitoringPembayaran(string search, int start, int length, Guid Id);      
        DataTableViewProyekDetailMonitoring GetDataListProyekDetailMonitoring(string search, int start, int length,Guid Id, Klasifikasi? dklasifikasi);
        ResultMessage Save(Guid PengadaanId, StatusMonitored nStatusMonitoring, StatusSeleksi nStatusSeleksi,Guid UserId);
        ViewResumeProyek GetResumeProyek();
        ViewDetailMonitoring GetDetailProyek(Guid ProyekId);
        ResultMessage SimpanProgresPekerjaan(List<TahapanProyek> Tahapan, Guid UserId);
        ResultMessage SimpanProgresPembayaran(List<TahapanProyek> Tahapan, Guid UserId);
        ResultMessage saveDokumenProyeks(Guid DokumenId,string NamaFileSave,string extension, Guid UserId);
        ResultMessage toTidakDimonitor(Guid Id, string status, Guid UserId);
        DokumenProyek GetDokumenProyek(Guid Id);
        ResultMessage toFinishRepo(Guid xProyekId,string xStatus, Guid UserId);
        ResultMessage toDisableRepo(Guid xProyekId, string xStatus, Guid UserId);
        ResultMessage toEnableRepo(Guid xProyekId, string xStatus, Guid UserId);
        DataTableViewMonitoring GetDataMonitoringSelectionSelesai(string search, int start, int length, StatusSeleksi? status);
        DataTableViewMonitoring GetDataMonitoringSelectionDraf(string search, int start, int length, StatusSeleksi? status);
        DataTableViewMonitoring GetDataMonitoringSelectionSedangBerjalan(string search, int start, int length, StatusSeleksi? status);
    }

    public class MonitoringRepo : IMoritoringRepo
    {
        JimbisContext ctx;

        // koneksi ke database
        public MonitoringRepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        //--

        private string FILE_DOKUMEN_PATH = System.Configuration.ConfigurationManager.AppSettings["FILE_UPLOAD_DOCPRO"];

        public DataTableViewProyekSistemMonitoringPembayaran GetDataListProyekDetailMonitoringPembayaran(string search, int start, int length, Guid Id)
        {
            DataTableViewProyekSistemMonitoringPembayaran dt = new DataTableViewProyekSistemMonitoringPembayaran();

            // Total
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();

            // Filter
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();

            var tampildetailtahapan = ctx.TahapanProyeks.Where(d => d.ProyekId == Id && d.JenisTahapan == "Pembayaran").ToList();
            var statusproyek = ctx.RencanaProyeks.Where(d => d.Id == Id).FirstOrDefault().Status;

            List<ViewProyekSistemMonitoringPembayaran> LstnViewTableDetailPembayaran = new List<ViewProyekSistemMonitoringPembayaran>();

            foreach (var item in tampildetailtahapan)
            {
                ViewProyekSistemMonitoringPembayaran vt = new ViewProyekSistemMonitoringPembayaran();

                vt.ID = item.Id;
                vt.NamaPembayaran = item.NamaTahapan;
                vt.PersenPembayaran = item.PersenPembayaran;
                vt.Status = item.StatusPembayaran;
                vt.TanggalPembayaran = item.TanggalPembayaran;
                vt.StatusProyek = statusproyek;

                LstnViewTableDetailPembayaran.Add(vt);
            }
            dt.data = LstnViewTableDetailPembayaran;
            return dt;
        }

        public DataTableViewProyekDetailMonitoring GetDataListProyekDetailMonitoring(string search, int start, int length,Guid Id, Klasifikasi? dklasifikasi)
        {
            DataTableViewProyekDetailMonitoring dt = new DataTableViewProyekDetailMonitoring();
            // total
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();
            // record
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();
            var tampildetailtahapan = ctx.TahapanProyeks.Where(d => d.ProyekId == Id && d.JenisTahapan == "Pekerjaan") .ToList();

            var statusproyek = ctx.RencanaProyeks.Where(d =>d.Id == Id).FirstOrDefault().Status;

            List<ViewTableDetailPekerjaan> LstnViewTableDetailPekerjaan = new List<ViewTableDetailPekerjaan>();

            foreach (var item in tampildetailtahapan)
            {
                ViewTableDetailPekerjaan vt = new ViewTableDetailPekerjaan();

                vt.Id = item.Id;
                vt.NamaPekerjaan = item.NamaTahapan;
                vt.BobotPekerjaan = item.BobotPekerjaan;
                vt.Progress = item.Progress;
                vt.StartDate = item.TanggalMulai;
                vt.EndDate = item.TanggalSelesai;
                vt.Status = statusproyek;

                LstnViewTableDetailPekerjaan.Add(vt);
            }
            dt.data = LstnViewTableDetailPekerjaan;
            return dt;
        }

        public DataTableViewProyekSistemMonitoring GetDataListProyekMonitoring(string search, int start, int length, Klasifikasi? dklasifikasi)
        {
            search = search == null ? "" : search;
            DataTableViewProyekSistemMonitoring dt = new DataTableViewProyekSistemMonitoring();
            
            dt.recordsTotal = ctx.RencanaProyeks.Where(d =>d.Status== "dijalankan").Count();
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status == "dijalankan").Count();
            var data = ctx.RencanaProyeks.Where(d=>d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status == "dijalankan").OrderByDescending(d=>d.CreatedOn).Select(
                d=>new ViewProyekSistemMonitoring(){
                    NoPengadaan = d.Spk.PemenangPengadaan.Pengadaan.NoPengadaan,
                    id = d.Id,
                    NOSPK = d.Spk.NoSPk,
                    NamaProyek = d.Spk.PemenangPengadaan.Pengadaan.Judul,
                    NamaPelaksana = d.Spk.PemenangPengadaan.Vendor.Nama,
                    Klasifikasi = d.Spk.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    TanggalMulai = d.StartDate,
                    TanggalSelesai = d.EndDate,
                    PersenPekerjaan = d.TahapanProyeks.Where(dd => dd.ProyekId == d.Id).Count()==0 ? 0 :  d.TahapanProyeks.Where(dd => dd.ProyekId == d.Id).Sum(dd => (dd.Progress*dd.BobotPekerjaan)/100),
                    PersenPembayaran = d.TahapanProyeks.Where(dd => dd.ProyekId == d.Id && dd.StatusPembayaran == "Sudah Dibayar").Count() == 0 ? 0 : ctx.TahapanProyeks.Where(dd => dd.ProyekId == d.Id && dd.StatusPembayaran == "Sudah Dibayar").Sum(dd => dd.PersenPembayaran)
                }).Skip(start).Take(length).ToList();

            dt.data = data;
            return dt;
        }

        public DataTableViewProyekSistemMonitoring GetDataListProyekMonitoringRekanan(string search, int start, int length, Klasifikasi? dklasifikasi, Guid? UserId)
        {
            search = search == null ? "" : search;
            DataTableViewProyekSistemMonitoring dt = new DataTableViewProyekSistemMonitoring();

            Vendor oVendor = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault();
            List<ViewProyekSistemMonitoring> LstnViewProyekSistemMonitoring = new List<ViewProyekSistemMonitoring>();

            dt.recordsTotal = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.VendorId == oVendor.Id && d.Status == "DIJALANKAN").Count();

            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.VendorId == oVendor.Id && d.Status == "DIJALANKAN" && d.Spk.Pengadaan.Judul.Contains(search)).Count();

            var data = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Spk.PemenangPengadaan.VendorId==oVendor.Id).OrderByDescending(d => d.CreatedOn).Select(
                d => new ViewProyekSistemMonitoring()
                {
                    id = d.Id,
                    NOSPK = d.Spk.NoSPk,
                    NamaProyek = d.Spk.PemenangPengadaan.Pengadaan.Judul,
                    NamaPelaksana = d.Spk.PemenangPengadaan.Vendor.Nama,
                    Klasifikasi = d.Spk.PemenangPengadaan.Pengadaan.JenisPekerjaan,
              }).Skip(start).Take(length).ToList();

            dt.data = data;            
            return dt;
        }

        public DataTableViewMonitoring GetDataMonitoringSelectionSelesai(string search, int start, int length, StatusSeleksi? status)
        {
            search = search == null ? "" : search;
            DataTableViewMonitoring dt = new DataTableViewMonitoring();
            dt.recordsTotal = ctx.RencanaProyeks.Count();
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status=="SELESAI").Count();
            var data = ctx.RencanaProyeks.Where(d=>d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search)&& d.Status=="SELESAI").OrderByDescending(d=>d.CreatedOn).Select(
                d => new ViewMonitoringSelection()
                {
                    NoPengadaan = d.Spk.PemenangPengadaan.Pengadaan.NoPengadaan,
                    Id = d.Id,
                    NOSPK = d.Spk.NoSPk,
                    Judul = d.Spk.PemenangPengadaan.Pengadaan.Judul,
                    Pemenang = d.Spk.PemenangPengadaan.Vendor.Nama,
                    Klasifikasi = d.Spk.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    TanggalPenentuanPemenang=d.Spk.TanggalSPK,
                    NilaiKontrak=d.Spk.NilaiSPK,
                    PIC=d.Spk.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd=>dd.tipe=="pic").FirstOrDefault()==null?"":d.Spk.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd=>dd.tipe=="pic").FirstOrDefault().Nama
                }).Skip(start).Take(length).ToList();

            dt.data = data;
            return dt;
        }

        public DataTableViewMonitoring GetDataMonitoringSelectionDraf(string search, int start, int length, StatusSeleksi? status)
        {
            search = search == null ? "" : search;
            DataTableViewMonitoring dt = new DataTableViewMonitoring();
            dt.recordsTotal = ctx.RencanaProyeks.Count();
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status == "DRAF").Count();
            var data = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status == "DRAF").OrderByDescending(d => d.CreatedOn).Select(
                d => new ViewMonitoringSelection()
                {
                    NoPengadaan = d.Spk.PemenangPengadaan.Pengadaan.NoPengadaan,
                    Id = d.Id,
                    NOSPK = d.Spk.NoSPk,
                    Judul = d.Spk.PemenangPengadaan.Pengadaan.Judul,
                    Pemenang = d.Spk.PemenangPengadaan.Vendor.Nama,
                    Klasifikasi = d.Spk.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    TanggalPenentuanPemenang = d.Spk.TanggalSPK,
                    NilaiKontrak = d.Spk.NilaiSPK,
                    PIC = d.Spk.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd => dd.tipe == "pic").FirstOrDefault() == null ? "" : d.Spk.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd => dd.tipe == "pic").FirstOrDefault().Nama
                }).Skip(start).Take(length).ToList();

            dt.data = data;
            return dt;
        }

        public DataTableViewMonitoring GetDataMonitoringSelectionSedangBerjalan(string search, int start, int length, StatusSeleksi? status)
        {
            search = search == null ? "" : search;
            DataTableViewMonitoring dt = new DataTableViewMonitoring();
            dt.recordsTotal = ctx.RencanaProyeks.Count();
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status == "DIJALANKAN").Count();
            var data = ctx.RencanaProyeks.Where(d => d.Spk.PemenangPengadaan.Pengadaan.Judul.Contains(search) && d.Status == "DIJALANKAN").OrderByDescending(d => d.CreatedOn).Select(
                d => new ViewMonitoringSelection()
                {
                    NoPengadaan = d.Spk.PemenangPengadaan.Pengadaan.NoPengadaan,
                    Id = d.Id,
                    NOSPK = d.Spk.NoSPk,
                    Judul = d.Spk.PemenangPengadaan.Pengadaan.Judul,
                    Pemenang = d.Spk.PemenangPengadaan.Vendor.Nama,
                    Klasifikasi = d.Spk.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    TanggalPenentuanPemenang = d.Spk.TanggalSPK,
                    NilaiKontrak = d.Spk.NilaiSPK,
                    PIC = d.Spk.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd => dd.tipe == "pic").FirstOrDefault() == null ? "" : d.Spk.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd => dd.tipe == "pic").FirstOrDefault().Nama
                }).Skip(start).Take(length).ToList();

            dt.data = data;
            return dt;
        }

        public DataTableViewMonitoring GetDataMonitoringSelection(string search, int start, int length,StatusSeleksi? status)
        {
            search = search == null ? "" : search;
            DataTableViewMonitoring dtTable = new DataTableViewMonitoring();
            if (length > 0)
            {
                var data = ctx.Spk.AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.Title.Contains(d.Title));                   
                }
                data = data.Where(x => !ctx.RencanaProyeks.Select(xx => xx.SpkId).Contains(x.Id));
                //data = data.Where(u => ctx)
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreateOn).Skip(start).Take(length);

                dtTable.data = data.Select(d => new ViewMonitoringSelection
                {
                    Id = d.Id,
                    NoPengadaan = d.PemenangPengadaan.Pengadaan.NoPengadaan,
                    NOSPK = d.NoSPk,
                    Judul = d.PemenangPengadaan.Pengadaan.Judul,
                    Pemenang = d.PemenangPengadaan.Vendor.Nama,
                    Klasifikasi = d.PemenangPengadaan.Pengadaan.JenisPekerjaan,
                    TanggalPenentuanPemenang = d.TanggalSPK,
                    NilaiKontrak = d.NilaiSPK == null ? 0 : d.NilaiSPK.Value,
                    PIC = d.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd => dd.tipe == "pic").FirstOrDefault() == null ? "" : d.PemenangPengadaan.Pengadaan.PersonilPengadaans.Where(dd => dd.tipe == "pic").FirstOrDefault().Nama
                }).ToList();
            }
            return dtTable;
        }

        public ViewResumeProyek GetResumeProyek()
        {
            var today = DateTime.Today;

            var TotalProyekDalamPelaksanaan = ctx.RencanaProyeks.Where(d => d.Status == "dijalankan" && d.EndDate >= today).Count();
            var TotalProyekLewatWaktuPelaksanaan = ctx.RencanaProyeks.Where(d => d.EndDate < today ).Count();
            var TotalProyekMendekatiWaktuPelaksanaan = ctx.RencanaProyeks.Where(d => d.StartDate > today).Count();

            return new ViewResumeProyek
            {
                ProyekDalamPelaksanaan = TotalProyekDalamPelaksanaan,
                ProyekLewatWaktuPelaksanaan = TotalProyekLewatWaktuPelaksanaan,
                ProyekMendekatiWaktuPelaksanaan = TotalProyekMendekatiWaktuPelaksanaan,
            };
        }

        public ViewDetailMonitoring GetDetailProyek(Guid ProyekId)
        {
            var odata = ctx.RencanaProyeks.Where(d => d.Id == ProyekId).FirstOrDefault();
            //var NamaProyek = ctx.Pengadaans.Where(d => d.Id == odata.PengadaanId).FirstOrDefault().Judul;
            // var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == odata.PengadaanId).FirstOrDefault();
            //var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah * d.hps == null ? 0 : d.jumlah * d.hps) : 0;
            var vendor = ctx.Vendors.Where(d => d.Id == odata.Spk.PemenangPengadaan.VendorId);
            return new ViewDetailMonitoring
            {
                Id = odata.Id,
                NamaProyek = odata.Spk.PemenangPengadaan.Pengadaan.Judul,
                TanggalMulai = odata.StartDate,
                TanggalSelesai = odata.EndDate,
                NilaiKontrak = odata.Spk.NilaiSPK,
                StatusProyek = odata.Status,
                VendorId = odata.Spk.PemenangPengadaan.VendorId.Value,
                StatusLockTahapan = odata.StatusLockTahapan
            };
        }

        public ResultMessage toDisableRepo(Guid xProyekId, string xStatus, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.Id == xProyekId).FirstOrDefault();

                if (odata != null)
                {
                    odata.StatusLockTahapan = xStatus;

                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.message = "Sukses";
                }
                else
                {
                    rm.message = "Gagal";
                }
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }
        public ResultMessage toEnableRepo(Guid xProyekId, string xStatus, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.Id == xProyekId).FirstOrDefault();

                if (odata != null)
                {
                    odata.StatusLockTahapan = xStatus;

                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.message = "Sukses";
                }
                else
                {
                    rm.message = "Gagal";
                }
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }

        public ResultMessage toFinishRepo(Guid xProyekId, string xStatus, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d =>d.Id == xProyekId).FirstOrDefault();

                if(odata != null)
                {
                    odata.Status = xStatus;

                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.message = "Status Proyek Telah Di Ubah";
                }
                else
                {
                    rm.message = "Gagal";
                }
            }
            catch(Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }

        public ResultMessage toTidakDimonitor(Guid Id, string status, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.RencanaProyeks.Where(d => d.SpkId == Id).FirstOrDefault();

                if(odata != null)
                {
                    odata.SpkId = Id;
                    odata.Status = status;
                    odata.CreatedBy = UserId;
                    odata.CreatedOn = DateTime.Now;
                }
                else
                {
                    RencanaProyek m2 = new RencanaProyek
                    {
                        SpkId = Id,
                        Status = status,
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    };

                    ctx.RencanaProyeks.Add(m2);
                }

                ctx.SaveChanges(UserId.ToString());
                rm.status = HttpStatusCode.OK;
                rm.message = "Pekerjaan Tidak Dimonitoring";
            }
            catch (Exception ex)
            {
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }

        public ResultMessage Save(Guid nPengadaanId, StatusMonitored nStatusMonitoring, StatusSeleksi nStatusSeleksi,Guid UserId)
        {
            ResultMessage rm = new ResultMessage();
            try
            {
                var odata = ctx.MonitoringPekerjaans.Where(d=>d.PengadaanId==nPengadaanId).FirstOrDefault();

                if(odata!=null)
                {
                    odata.StatusMonitoring = nStatusMonitoring;
                    odata.StatusSeleksi = nStatusSeleksi;
                    odata.ModifiedBy = UserId;
                    odata.ModifiedOn = DateTime.Now;
                }
                else
                {

                    MonitoringPekerjaan m2 = new MonitoringPekerjaan
                    {
                        PengadaanId = nPengadaanId,
                        StatusMonitoring = nStatusMonitoring,
                        StatusSeleksi = nStatusSeleksi,
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    };

                    ctx.MonitoringPekerjaans.Add(m2);
                }

            
                    ctx.SaveChanges(UserId.ToString());
                    rm.status = HttpStatusCode.OK;
                    rm.message = "Sukses";
            }
            catch(Exception ex){
                rm.status = HttpStatusCode.ExpectationFailed;
                rm.message = ex.ToString();
            }

            return rm;
        }

        public ResultMessage SimpanProgresPekerjaan(List<TahapanProyek> Tahapan, Guid UserId)
        {
            ResultMessage rkm = new ResultMessage();

            // data masuk
            foreach (var item in Tahapan)
            {
                var odata = ctx.TahapanProyeks.Where(d => d.Id == item.Id).FirstOrDefault();

                if (odata != null)
                {
                    odata.Progress = item.Progress;
                    odata.TanggalMulai = item.TanggalMulai;
                    odata.TanggalSelesai = item.TanggalSelesai;
                    odata.BobotPekerjaan = item.BobotPekerjaan;

                    ctx.SaveChanges(UserId.ToString());
                    rkm.status = HttpStatusCode.OK;
                    rkm.message = "Sukses";
                }
                else
                {
                    rkm.message = "Gagal";
                }

            }

            // cek total
            var tahapanid = Tahapan.ElementAt(0).Id;
            var proyekid = ctx.TahapanProyeks.Where(d => d.Id == tahapanid).FirstOrDefault().ProyekId;
            var TotalBobotPekerjaanDb = Tahapan.Sum(x => x.BobotPekerjaan);
            if (TotalBobotPekerjaanDb > 100)
            {
                foreach (var iitem in Tahapan)
                {
                    var a = ctx.TahapanProyeks.Where(d => d.Id == iitem.Id).FirstOrDefault();

                    if (a != null)
                    {
                        a.Progress = iitem.Progress;
                        a.TanggalMulai = iitem.TanggalMulai;
                        a.TanggalSelesai = iitem.TanggalSelesai;
                        a.BobotPekerjaan = 0;

                        ctx.SaveChanges(UserId.ToString());
                        rkm.status = HttpStatusCode.OK;
                        rkm.message = "Bobot Pekerjaan lebih dari 100 % Silahkan Isi Kembali Bobot Pekerjaan";
                    }
                    else
                    {
                        rkm.message = "Gagal";
                    }
                }
            }
            return rkm;
        }

        public DokumenProyek GetDokumenProyek(Guid Id)
        {
            return ctx.DokumenProyeks.Find(Id);
        }

        public ResultMessage DeleteDokumenProyek(Guid Id, Guid UserId)
        {
            var oData = ctx.DokumenProyeks.Find(Id);
            ResultMessage msg = new ResultMessage();
            try
            {
                oData.URL = "";
                oData.ContentType = "";

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

        public ResultMessage saveDokumenProyeks(Guid DokumenId, string NamaFileSave, string extension, Guid UserId)
        {
            ResultMessage rm = new ResultMessage();

            var odata = ctx.DokumenProyeks.Where(d => d.Id == DokumenId).FirstOrDefault();

            if(odata != null)
            {
                odata.URL = NamaFileSave;
                odata.ContentType = extension;
                ctx.SaveChanges(UserId.ToString());
                rm.status = HttpStatusCode.OK;
                rm.message = "Sukses";
            }
            else
            {

            }

            return rm;
        }

        public ResultMessage SimpanProgresPembayaran(List<TahapanProyek> Tahapan, Guid UserId)
        {
            ResultMessage rkm = new ResultMessage();

            foreach (var item in Tahapan)
            {
                var odata = ctx.TahapanProyeks.Where(d => d.Id == item.Id).FirstOrDefault();
                
                if(item.StatusPembayaran == "Sudah Dibayar")
                {
                    if (item.TanggalPembayaran != null)
                    {
                        if (odata != null)
                        {
                            odata.PersenPembayaran = item.PersenPembayaran;
                            odata.StatusPembayaran = item.StatusPembayaran;
                            odata.TanggalPembayaran = item.TanggalPembayaran;

                            if(odata.StatusPembayaran == "Sudah Dibayar")
                            {
                                odata.KonfirmasiPengecekanDokumen = "Sudah Konfirmasi";
                            }

                            ctx.SaveChanges(UserId.ToString());
                            rkm.status = HttpStatusCode.OK;
                            rkm.message = "Pembayaran Dikonfirmasi";
                        }
                        else
                        {
                            rkm.message = "Gagal";
                        }
                    }
                    else
                    {
                        rkm.message = "Tanggal Pembayaran Tidak Boleh Kosong";
                    }
                }
                else
                {
                    if (odata != null)
                    {
                        odata.PersenPembayaran = item.PersenPembayaran;
                        odata.StatusPembayaran = item.StatusPembayaran;
                        odata.TanggalPembayaran = item.TanggalPembayaran;

                        ctx.SaveChanges(UserId.ToString());
                        rkm.status = HttpStatusCode.OK;
                        rkm.message = "Sukses";
                    }
                    else
                    {
                        rkm.message = "Gagal";
                    }
                }
                
            }

            return rkm;
        }
     
    }
}
