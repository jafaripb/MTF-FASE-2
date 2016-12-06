using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Eproc.Model.Monitoring.Model
{
    public class ViewProyekPerencanaan
    {
        public Guid Id { get; set; }
        public string Judul { get; set; }
        public string NoPengadaan { get; set; }
        public string NoKontrak { get; set; }
        public decimal NilaiKontrak { get; set; }
        public string Pelaksana { get; set; }
        public Nullable<DateTime> TanggalMulai { get; set; }
        public Nullable<DateTime> TanggalSelesai { get; set; }
        public List<ViewTahapan> Tahapan { get; set; }
        public List<ViewPIC> PIC { get; set; }
    }

    public class ViewTahapan
    {
        public Guid Id { get; set; }
        public string NamaTahapan { get; set; }
        public string TanggalTahapan { get; set; }
        public List<ViewDokumen> Dokumen { get; set; }
    }

    public class ViewDokumen
    {
        public Guid Id { get; set; }
        public string NamaDokumen { get; set; }
        public string JenisDokumen { get; set; }
    }

    public class ViewPIC
    {
        public Guid Id { get; set; }
        public string NamaPIC { get; set; }
    }

    public class DataTableViewProyek
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewProyekPerencanaan> data { get; set; }
    }

    public class DataTableViewTahapanPekerjaan
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewListTahapan> data { get; set; }
    }

    public class DataTableViewTahapanPembayaran
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewListTahapan> data { get; set; }
    }

    public class DataTableViewDokumenTahapanPekerjaan
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<ViewListTahapanDokumenPekerjaan> data { get; set; }
    }

    public class ViewListTahapan
    {
        public Guid Id { get; set; }
        public string NamaTahapan { get; set; }
        public DateTime? TanggalMulai { get; set; }
        public DateTime? TanggalSelesai { get; set; }
        public string JenisTahapan { get; set; }
    }

    public class ViewUntukProyekAddPersonil
    {
        public Guid PengadaanId { get; set; }
        public Guid UserId { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string tipe { get; set; }
    }

    public class ViewListTahapanDokumenPekerjaan
    {
        public Guid Id { get; set; }
        public string NamaDokumen { get; set; }
    }
}
