using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTF_x.Models
{
    public class AnnouncementPengadaan
    {
        public AnnouncementPengadaan()
        {

        }
        public AnnouncementPengadaan(Pengadaan pengadaan)
        {
            this.AturanBerkas = pengadaan.AturanBerkas;
            this.AturanPenawaran = pengadaan.AturanPenawaran;
            this.AturanPengadaan = pengadaan.AturanPengadaan;
            this.CreatedBy = pengadaan.CreatedBy;
            this.CreatedOn = pengadaan.CreatedOn;
            this.GroupPengadaan = pengadaan.GroupPengadaan;
            this.HpsId = pengadaan.HpsId;
            this.Id = pengadaan.Id;
            this.JadwalPengadaans = pengadaan.JadwalPengadaans;
            this.JenisPekerjaan = pengadaan.JenisPekerjaan;
            this.Judul = pengadaan.Judul;
            this.Keterangan = pengadaan.Keterangan;
            this.KualifikasiRekan = pengadaan.KualifikasiRekan;
            this.MataUang = pengadaan.MataUang;
            this.ModifiedBy = pengadaan.ModifiedBy;
            this.ModifiedOn = pengadaan.ModifiedOn;
            this.NoPengadaan = pengadaan.NoPengadaan;
            this.PeriodeAnggaran = pengadaan.PeriodeAnggaran;
            this.Provinsi = pengadaan.Provinsi;
            this.Region = pengadaan.Region;
            this.Status = pengadaan.Status;
            this.TanggalMenyetujui = pengadaan.TanggalMenyetujui;
            this.TitleBerkasRujukanLain = pengadaan.TitleBerkasRujukanLain;
            this.TitleDokumenLain = pengadaan.TitleDokumenLain;
            this.TitleDokumenNotaInternal = pengadaan.TitleDokumenNotaInternal;
            this.UnitKerjaPemohon = pengadaan.UnitKerjaPemohon;
            this.Pagu = pengadaan.Pagu;

            var dt = pengadaan.JadwalPengadaans.FirstOrDefault(p => p.Tipe == "pendaftaran");
            if (dt != null)
                this.AkhirPendaftaran = dt.Sampai;
        }
        public DateTime? AkhirPendaftaran { get; set; }


        public Guid Id { get; set; }

        public string Judul { get; set; }

        public string Keterangan { get; set; }

        public string AturanPengadaan { get; set; }

        public string AturanBerkas { get; set; }

        public string AturanPenawaran { get; set; }

        public string MataUang { get; set; }

        public string PeriodeAnggaran { get; set; }

        public string JenisPembelanjaan { get; set; }
        public Guid? HpsId { get; set; }
        public string TitleDokumenNotaInternal { get; set; }

        public string UnitKerjaPemohon { get; set; }
        public string TitleDokumenLain { get; set; }

        public string Region { get; set; }

        public string Provinsi { get; set; }

        public string KualifikasiRekan { get; set; }

        public string JenisPekerjaan { get; set; }
        public EStatusPengadaan? Status { get; set; }
        public EGroupPengadaan? GroupPengadaan { get; set; }
        public string TitleBerkasRujukanLain { get; set; }
        public string NoPengadaan { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? TanggalMenyetujui { get; set; }
        public Decimal? Pagu { get; set; }
        public ICollection<JadwalPengadaan> JadwalPengadaans { get; set; }

    }

    public class KriteriaKualifikasi
    {
        public KriteriaKualifikasi()
        {
        }

        public KriteriaKualifikasi(Pengadaan pengadaan)
        {
            Id = pengadaan.Id;
            NamaPengadaan = pengadaan.Judul;
            NoPengadaan = pengadaan.NoPengadaan;
            Kualifikasi = pengadaan.KualifikasiRekan;
            LokasiPengadaan = pengadaan.Provinsi;
            JenisPengadaan = "??";
            Pengalaman = "??";
            NilaiKontrakMinimum = "??";
            Tahun = "?? s/d ??";
            IjinUsaha = "??";
            Klasifikasi = "";
            foreach (var x in pengadaan.KualifikasiKandidats)
            {
                Klasifikasi += x.Kualifikasi + System.Environment.NewLine;
            }
        }
        public Guid Id { get; set; }
        public string NoPengadaan { get; set; }
        public string NamaPengadaan { get; set; }
        public string Kualifikasi { get; set; }
        public string LokasiPengadaan { get; set; }
        public string JenisPengadaan { get; set; }
        public string Pengalaman { get; set; }
        public string NilaiKontrakMinimum { get; set; }
        public string Tahun { get; set; }
        public string IjinUsaha { get; set; }
        public string Klasifikasi { get; set; }
    }
}