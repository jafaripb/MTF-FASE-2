using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public interface IDokumenRepo
    {
        Dokumen GetDokumen(Guid guid);
        IQueryable<Dokumen> GetAllDokumenByVendor(int id);
        Guid AddDokumen(Dokumen d);
    }
}
