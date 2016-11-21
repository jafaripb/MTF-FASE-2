using Microsoft.Owin.FileSystems;
using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.Repository;
using Reston.Pinata.WebService.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Net;
using Model.Helper;
using Reston.Pinata.WebService.Helper;
//using System.Web.Helpers;

namespace Reston.Pinata.WebService
{
    public class ReferenceDataController : ApiController
    {
        private IReferenceDataRepo _repository;
        public ReferenceDataController()
        {
            _repository = new ReferenceDataRepo(new JimbisContext());
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllProvinsi() {
            return _repository.GetData(RefDataQualifier.PROVINCE, null).Select(x => new ReferenceDataViewModel() { 
                id = x.Id, Code = x.Code, Name = x.LocalizedName, Desc = x.LocalizedDesc, Str1 = x.StringAttr1, Int1 = x.IntAttr1, Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllCurrency()
        {
            return _repository.GetData(RefDataQualifier.CURRENCY, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin})]
        [ApiAuthorize]
        public IHttpActionResult GetTblCurrency() {
            return Json(new { aaData = GetAllCurrency() });
        }

        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.Roles.pRole_procurement_end_user, IdLdapConstants.Roles.pRole_procurement_head, IdLdapConstants.Roles.pRole_procurement_manager})]
        [HttpGet]
        [ApiAuthorize]
        public List<ReferenceDataViewModel> GetAllRegion()
        {
            return _repository.GetData(RefDataQualifier.REGION, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblRegion()
        {
            return Json(new { aaData = GetAllRegion() });
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllSatuan()
        {
            return _repository.GetData(RefDataQualifier.SATUAN, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblSatuan()
        {
            return Json(new { aaData = GetAllSatuan() });
        }
        
        [HttpGet]
        public List<ReferenceDataViewModel> GetAllBank()
        {
            return _repository.GetData(RefDataQualifier.BANK, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id, Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblBank()
        {
            return Json(new { aaData = GetAllBank() });
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllPeriodeAnggaran()
        {
            return _repository.GetData(RefDataQualifier.PeriodeAnggaran, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblPeriodeAnggaran()
        {
            return Json(new { aaData = GetAllPeriodeAnggaran() });
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllUnitKerja()
        {
            return _repository.GetData(RefDataQualifier.UnitKerja, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblUnitKerja()
        {
            return Json(new { aaData = GetAllUnitKerja() });
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllJenisPekerjaan()
        {
            return _repository.GetData(RefDataQualifier.JenisPekerjaan, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblJenisPekerjaan()
        {
            return Json(new { aaData = GetAllJenisPekerjaan() });
        }

        [HttpGet]
        public List<ReferenceDataViewModel> GetAllJenisPembelanjaan()
        {
            return _repository.GetData(RefDataQualifier.JenisPembelanjaan, null).Select(x => new ReferenceDataViewModel()
            {
                id = x.Id,
                Code = x.Code,
                Name = x.LocalizedName,
                Desc = x.LocalizedDesc,
                Str1 = x.StringAttr1,
                Int1 = x.IntAttr1,
                Flag1 = x.FlagAttr1
            }).ToList();
        }

        [HttpGet]
        //[ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_user, IdLdapConstants.Roles.pRole_compliance, IdLdapConstants.Roles.pRole_procurement_admin })]
        [ApiAuthorize]
        public IHttpActionResult GetTblJenisPembelanjaan()
        {
            return Json(new { aaData = GetAllJenisPembelanjaan() });
        }
       
        //master crud
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        private string AddReferenceData(ReferenceData r) {
            List<ReferenceData> lr = _repository.GetData(r.Qualifier, r.Code);
            if (lr.Count >0) //exist boss
            {
                return "Gagal! Duplikat data.";
            }
            _repository.SaveData(r);
            return "Sukses!";
        }

        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddPeriodeAnggaran(string code, string nama, string deskripsi) {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.PeriodeAnggaran, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi});
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddUnitKerja(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.UnitKerja, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddJenisBelanja(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.JenisPembelanjaan, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddJenisPekerjaan(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.JenisPekerjaan, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddCurrency(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.CURRENCY, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddRegion(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.REGION, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddSatuan(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.SATUAN, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string AddBank(string code, string nama, string deskripsi)
        {
            return AddReferenceData(new ReferenceData() { Qualifier = RefDataQualifier.BANK, Code = code, LocalizedName = nama, LocalizedDesc = deskripsi });
        }
        [HttpGet]
        [ApiAuthorize(new string[] { IdLdapConstants.Roles.pRole_procurement_admin, IdLdapConstants.App.Roles.IdLdapProcurementAdminRole })]
        //[ApiAuthorize]
        public string DeleteReferenceData(int id) {
            ReferenceData r = _repository.GetDataById(id);
            if (r != null) {
                _repository.Delete(r);
                return "Sukses!";
            }
            return "Gagal! Tidak ditemukan.";
        }
    }
}
