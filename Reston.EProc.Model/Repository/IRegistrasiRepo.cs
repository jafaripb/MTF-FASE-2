using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public interface IRegistrasiRepo
    {
        RegVendor GetVendor(int id);
        RegVendor GetVendor(string noPengajuan);
        List<RegVendor> GetVendors(ETipeVendor tipe, EStatusVendor status, int limit,string search);
        List<RegVendor> GetAllVendor();
        int AddVendor(RegVendor v);
        void Save();
        RegDokumen GetDokumen(Guid guid);
        IQueryable<RegDokumen> GetAllDokumenByVendor(int id);
        Guid AddDokumen(RegDokumen d);
        int CheckNomor(string no);
        int RegisterVerifiedVendor(Vendor v);
        CaptchaRegistration GetCaptchaRegistration(Guid id);
        void AddCaptchaRegistration(CaptchaRegistration c);
    }
}
