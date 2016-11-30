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
}
