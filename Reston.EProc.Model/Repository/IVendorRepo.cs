using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public interface IVendorRepo
    {
        Vendor GetVendor(int id);
        Vendor GetVendorByUser(Guid id);
        List<Vendor> GetVendors(ETipeVendor tipe, EStatusVendor status, int limit);
        List<Vendor> GetAllVendor();
        int AddVendor(Vendor v);
        int CheckNomor(string no);
        void Save();
    }
}
