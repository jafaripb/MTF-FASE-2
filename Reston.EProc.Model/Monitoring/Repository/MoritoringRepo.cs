﻿using Reston.Eproc.Model.Monitoring.Entities;
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
        DokumenProyek GetDokumenProyek(Guid Id);
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

        public DataTableViewProyekSistemMonitoringPembayaran GetDataListProyekDetailMonitoringPembayaran(string search, int start, int length, Guid Id)
        {
            DataTableViewProyekSistemMonitoringPembayaran dt = new DataTableViewProyekSistemMonitoringPembayaran();

            // Total
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();

            // Filter
            dt.recordsTotal = ctx.TahapanProyeks.Where(d => d.ProyekId == Id).Count();

            var tampildetailtahapan = ctx.TahapanProyeks.Where(d => d.ProyekId == Id && d.JenisTahapan == "Pembayaran").ToList();

            List<ViewProyekSistemMonitoringPembayaran> LstnViewTableDetailPembayaran = new List<ViewProyekSistemMonitoringPembayaran>();

            foreach (var item in tampildetailtahapan)
            {
                ViewProyekSistemMonitoringPembayaran vt = new ViewProyekSistemMonitoringPembayaran();

                vt.ID = item.Id;
                vt.NamaPembayaran = item.NamaTahapan;
                vt.PersenPembayaran = item.PersenPembayaran;
                vt.Status = item.StatusPembayaran;
                vt.TanggalPembayaran = item.TanggalPembayaran;

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

                LstnViewTableDetailPekerjaan.Add(vt);
            }
            dt.data = LstnViewTableDetailPekerjaan;
            return dt;
        }

        public DataTableViewProyekSistemMonitoring GetDataListProyekMonitoring(string search, int start, int length, Klasifikasi? dklasifikasi)
        {
            DataTableViewProyekSistemMonitoring dt = new DataTableViewProyekSistemMonitoring();
            // record total
            dt.recordsTotal = ctx.RencanaProyeks.Where(d => d.Status == "DIJALANKAN").Count();

            //filter berdasarkan pencarian
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Status == "DIJALANKAN").Count();

            var tampilproyek = ctx.RencanaProyeks.Where(d => d.Status == "DIJALANKAN").ToList();

            List<ViewProyekSistemMonitoring> LstnViewProyekSistemMonitoring = new List<ViewProyekSistemMonitoring>();

            foreach (var item in tampilproyek)
            {
                ViewProyekSistemMonitoring vp = new ViewProyekSistemMonitoring();

                var vendorId = ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() == null ? 0 :
                     ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().VendorId;
                var vendor = ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault() == null ? "" :
                     ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault().Nama;
                var aklasifikasi = ctx.Pengadaans.Where(d => d.Id == item.PengadaanId).FirstOrDefault().JenisPekerjaan;
                
                var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == item.Id).FirstOrDefault();
                var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah * d.hps == null ? 0 : d.jumlah * d.hps) : 0;

                var Proyek = ctx.RencanaProyeks.Where(d => d.PengadaanId == item.Id).FirstOrDefault();

                var PersenPekerjaan = ctx.TahapanProyeks.Where(d => d.ProyekId == item.Id).Count()==0 ? 0 : ctx.TahapanProyeks.Where(d => d.ProyekId == item.Id).Sum(d => (d.Progress*d.BobotPekerjaan)/100);
                var PersenPembayaran = ctx.TahapanProyeks.Where(d => d.ProyekId == item.Id && d.StatusPembayaran == "Sudah Dibayar").Count() == 0 ? 0 : ctx.TahapanProyeks.Where(d => d.ProyekId == item.Id && d.StatusPembayaran == "Sudah Dibayar").Sum(d => d.PersenPembayaran);

                vp.id = item.Id;
                vp.NOSPK = item.NoKontrak;
                vp.NamaProyek = item.Pengadaan.Judul;
                vp.NamaPelaksana = vendor;
                vp.Klasifikasi = aklasifikasi;
                vp.TanggalMulai = item.StartDate;
                vp.TanggalSelesai = item.EndDate;
                vp.PersenPekerjaan = PersenPekerjaan;
                vp.PersenPembayaran = PersenPembayaran;

                LstnViewProyekSistemMonitoring.Add(vp);
            }
            dt.data = LstnViewProyekSistemMonitoring;
            return dt;
        }

        public DataTableViewProyekSistemMonitoring GetDataListProyekMonitoringRekanan(string search, int start, int length, Klasifikasi? dklasifikasi, Guid? UserId)
        {
            DataTableViewProyekSistemMonitoring dt = new DataTableViewProyekSistemMonitoring();

            Vendor oVendor = ctx.Vendors.Where(xx => xx.Owner == UserId).FirstOrDefault();
            List<ViewProyekSistemMonitoring> LstnViewProyekSistemMonitoring = new List<ViewProyekSistemMonitoring>();
            var tampilproyek = ctx.RencanaProyeks.Where(d => d.Status == "DIJALANKAN").Where(d => d.Pengadaan.PemenangPengadaans.Where(dd => dd.VendorId == oVendor.Id).Count() > 0).Where(d => d.Pengadaan.BeritaAcaras.Where(dd => dd.VendorId == oVendor.Id && dd.Tipe == TipeBerkas.SuratPerintahKerja).Count() > 0).ToList();

            dt.recordsTotal = ctx.RencanaProyeks.Where(d => d.Status == "DIJALANKAN").Where(d => d.Pengadaan.PemenangPengadaans.Where(dd => dd.VendorId == oVendor.Id).Count() > 0).Count();
            dt.recordsFiltered = ctx.RencanaProyeks.Where(d => d.Status == "DIJALANKAN").Where(d => d.Pengadaan.PemenangPengadaans.Where(dd => dd.VendorId == oVendor.Id).Count() > 0).Count();

            foreach (var item in tampilproyek)
            {
                ViewProyekSistemMonitoring vp = new ViewProyekSistemMonitoring();
                var proyek = ctx.RencanaProyeks.Where(d => d.PengadaanId == item.PengadaanId).FirstOrDefault();
                var aNamaProyek = ctx.Pengadaans.Where(d => d.Id == item.PengadaanId).FirstOrDefault().Judul;
                var vendorId = ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.PengadaanId).FirstOrDefault() == null ? 0 :
                     ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.PengadaanId).FirstOrDefault().VendorId;
                var vendor = ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault() == null ? "" :
                     ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault().Nama;
                var aklasifikasi = ctx.Pengadaans.Where(d => d.Id == item.PengadaanId).FirstOrDefault().JenisPekerjaan;
                var spk = ctx.BeritaAcaras.Where(d=>d.PengadaanId == item.PengadaanId && d.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault().NoBeritaAcara;

                vp.id = proyek.Id;
                vp.NOSPK = spk;
                vp.NamaProyek = aNamaProyek;
                vp.NamaPelaksana = vendor;
                vp.Klasifikasi = aklasifikasi;

                LstnViewProyekSistemMonitoring.Add(vp);
                
            }
            dt.data = LstnViewProyekSistemMonitoring;
            return dt;
        }

        public DataTableViewMonitoring GetDataMonitoringSelection(string search, int start, int length,StatusSeleksi? status)
        {
            DataTableViewMonitoring dt = new DataTableViewMonitoring();

            // record total yang tampil 
            dt.recordsTotal = ctx.Pengadaans.Where(d => d.Status == EStatusPengadaan.PEMENANG && 
                d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja ).FirstOrDefault() != null).Count();
            
            // filter berdasarkan pencarian
            dt.recordsFiltered=ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.PEMENANG && d.Judul.Contains(search)  && 
                d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null
                 && d.MonitoringPekerjaans.Where(x=>x.StatusMonitoring==StatusMonitored.TIDAK).Count()==0 &&   (d.MonitoringPekerjaans.Where(x=>x.StatusSeleksi==status).Count()>0|| status==null)).Count();     
            
            var carimonitoring = ctx.Pengadaans.Where(d=>d.Status==EStatusPengadaan.PEMENANG && d.Judul.Contains(search)&&
                d.DokumenPengadaans.Where(y => y.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault() != null &&
                 d.MonitoringPekerjaans.Where(x=>x.StatusMonitoring!=StatusMonitored.TIDAK).Count()==0 && (d.MonitoringPekerjaans.Where(x => x.StatusSeleksi == status).Count() > 0 || status == null)).OrderByDescending(d => d.CreatedOn).Skip(start).Take(length).ToList();
           
            List<ViewMonitoringSelection> LstnViewMonitoringSelection = new List<ViewMonitoringSelection>();
            foreach (var item in carimonitoring)
            {
                ViewMonitoringSelection nViewMonitoringSelection = new ViewMonitoringSelection();

                var vendorId = ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() == null ? 0 : 
                     ctx.PemenangPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().VendorId;
                var vendor = ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault() == null ? "" :
                     ctx.Vendors.Where(d => d.Id == vendorId).FirstOrDefault().Nama;
                var tanggalpenentuanpemenang = ctx.DokumenPengadaans.Where(d => d.PengadaanId == item.Id && d.Tipe==TipeBerkas.SuratPerintahKerja).FirstOrDefault().CreateOn;
                var pic = ctx.PersonilPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() == null ? "" :
                     ctx.PersonilPengadaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().Nama;
                var spk = ctx.BeritaAcaras.Where(d => d.PengadaanId == item.Id && d.Tipe == TipeBerkas.SuratPerintahKerja).FirstOrDefault().NoBeritaAcara;

                nViewMonitoringSelection.Id = item.Id;
                nViewMonitoringSelection.NoPengadaan = item.NoPengadaan;
                nViewMonitoringSelection.NOSPK = spk;
                nViewMonitoringSelection.Judul = item.Judul;
                nViewMonitoringSelection.Pemenang = vendor;
                nViewMonitoringSelection.Klasifikasi = item.JenisPekerjaan;
                nViewMonitoringSelection.TanggalPenentuanPemenang = tanggalpenentuanpemenang;
                var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == item.Id).FirstOrDefault();
                var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah * d.hps == null ? 0 : d.jumlah * d.hps) : 0;
                nViewMonitoringSelection.NilaiKontrak = TotalHps.Value;
                nViewMonitoringSelection.PIC = pic;

                //var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == item.Id).FirstOrDefault();
                //var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah == null ? 0 : d.jumlah * d.hps == null ? 0 : d.hps) : 0;
                //nViewMonitoringSelection.NilaiKontrak = TotalHps.Value;
                if (ctx.MonitoringPekerjaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault() != null)
                {
                    nViewMonitoringSelection.Monitored = ctx.MonitoringPekerjaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().StatusMonitoring.Value;
                    nViewMonitoringSelection.Status = ctx.MonitoringPekerjaans.Where(d => d.PengadaanId == item.Id).FirstOrDefault().StatusSeleksi.Value;
                }
                LstnViewMonitoringSelection.Add(nViewMonitoringSelection);
            }
            dt.data = LstnViewMonitoringSelection;
            return dt;
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
            var NamaProyek = ctx.Pengadaans.Where(d => d.Id == odata.PengadaanId).FirstOrDefault().Judul;
            var RksHeader = ctx.RKSHeaders.Where(d => d.PengadaanId == odata.PengadaanId).FirstOrDefault();
            var TotalHps = RksHeader != null ? ctx.RKSDetails.Where(d => d.RKSHeaderId == RksHeader.Id).Sum(d => d.jumlah * d.hps == null ? 0 : d.jumlah * d.hps) : 0;

            return new ViewDetailMonitoring
            {
                Id = odata.Id,
                NamaProyek = NamaProyek,
                TanggalMulai = odata.StartDate,
                TanggalSelesai = odata.EndDate,
                NilaiKontrak = TotalHps.Value
            };
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

            foreach(var item in Tahapan)
            {
                var odata = ctx.TahapanProyeks.Where(d =>d.Id == item.Id).FirstOrDefault();

                if(odata != null)
                {
                    odata.Progress = item.Progress;
                    odata.TanggalMulai = item.TanggalMulai;
                    odata.TanggalSelesai = item.TanggalSelesai;

                    ctx.SaveChanges(UserId.ToString());
                    rkm.status = HttpStatusCode.OK;
                    rkm.message = "Sukses";
                }
                else
                {
                    rkm.message = "Gagal";
                }
            }

            return rkm;
        }

        public DokumenProyek GetDokumenProyek(Guid Id)
        {
            return ctx.DokumenProyeks.Find(Id);
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

                            ctx.SaveChanges(UserId.ToString());
                            rkm.status = HttpStatusCode.OK;
                            //rkm.message = "Sukses";
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
                        //rkm.message = "Sukses";
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
