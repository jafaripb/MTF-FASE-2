using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.PengadaanRepository.View
{
    public class VWPengadaanList
    {
        public Guid Id { get; set; }
        public string Judul { get; set; }
        public string AturanPengadaan { get; set; }
        public string AturanBerkas { get; set; }
        public string AturanPenawaran { get; set; }
        public string StatusPengadaan { get; set; }
        public string tanggal { get; set; }
        public List<VWVendorPengadaanList> VWVendorPengadaanLists { get; set; }
        public List<VWDokumenPengadaanList> VWDokumenPengadaanLists { get; set; }
        public List<VWPersonilPengadaanList> VWPersonilPengadaanLists { get; set; }
    }

    public class VWVendorPengadaanList
    {
        public Guid VendorId { get; set; }
        public string NamaVendor { get; set; }
    }

    public class VWDokumenPengadaanList
    {
        public Guid DokumenId { get; set; }
        public string Title { get; set; }
    }
    public class VWPersonilPengadaanList
    {
        public Guid PersonilId { get; set; }
        public string NamaPersonil { get; set; }
        public string tipe { get; set; }
    }
}
