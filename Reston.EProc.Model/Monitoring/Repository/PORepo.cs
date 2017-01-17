using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using Model.Helper;
using Reston.Pinata.Model.PengadaanRepository.View;
using System.Configuration;
using Reston.Pinata.Model.JimbisModel;
using Newtonsoft.Json;
using System.Net;
using Reston.Helper.Util;
using Reston.Eproc.Model.Monitoring.Entities;

namespace Reston.Pinata.Model.PengadaanRepository
{
    public interface IPORepo
    {
        ResultMessage Delete(Guid Id, Guid UserId);
        DataTablePO List(string search, int start, int limit, string NoPO);
        DataTablePODetail ListItem(string search, int start, int limit, Guid POId);
        VWPO detail(Guid Id, Guid UserId);
        ResultMessage save(PO po, Guid UserId);
        ResultMessage saveItem(PODetail PODetail, Guid UserId);
        ResultMessage saveDokumen(DokumenPO data, Guid UserId);
        List<VWDokumenPO> GetListDokumenPO(Guid Id);
        int deleteDokumenPO(Guid Id, Guid UserId);
        ResultMessage DeleteItem(Guid Id, Guid UserId);
        string GenerateNoPO( Guid UserId);
        PO get(Guid Id);
        DokumenPO GetDokumenPO(Guid Id);
    }
    public class PORepo : IPORepo
    {
        JimbisContext ctx;
        private IPengadaanRepo _repoPengadaan;

        public PORepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
            _repoPengadaan = new PengadaanRepo(new JimbisContext());
        }

        ResultMessage msg = new ResultMessage();

        public void Save()
        {
            ctx.SaveChanges();
        }

        public PO get(Guid Id)
        {
            return ctx.POs.Find(Id);
        }

        public ResultMessage Delete(Guid Id, Guid UserId)
        {
            try
            {
                var oData = ctx.POs.Find(Id);
                if (oData != null)
                {
                    ctx.POs.Remove(oData);
                }
                ctx.SaveChanges(UserId.ToString());
                return new ResultMessage()
                {
                    Id=oData.Id.ToString(),
                    message=Common.DeleteSukses(),
                    status=HttpStatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return new ResultMessage()
                {
                    Id = "0",
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

        public DataTablePO List(string search, int start, int limit,string NoPO)
        {
            search = search == null ? "" : search;
            NoPO = NoPO == null ? "" : NoPO;
            DataTablePO dtTable = new DataTablePO();
            if (limit > 0)
            {
                var data = ctx.POs.AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(NoPO))
                {
                    data = data.Where(d => d.NoPO.Contains(NoPO));
                }
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.Prihal.Contains(search));
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreatedOn).Skip(start).Take(limit);
                dtTable.data = data.Select(d => new VWPO
                {
                    Id = d.Id,
                    NilaiPO = d.PODetail.Sum(dd=>dd.Banyak*dd.Harga),
                    NoPO = d.NoPO,
                    Prihal = d.Prihal,
                    TanggalPO = d.TanggalPO,
                    Vendor = d.Vendor,
                    UP = d.UP,
                    CreatedId=d.CreatedBy
                }).ToList();
            }
            return dtTable;
        }

        public DataTablePODetail ListItem(string search, int start, int limit, Guid POId)
        {
            search = search == null ? "" : search;
            DataTablePODetail dtTable = new DataTablePODetail();
            if (limit > 0)
            {

                var data = ctx.PODetails.Where(d => d.POId == POId).AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(search))
                {
                    data = data.Where(d => d.PO.Prihal.Contains(search));
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.CreatedOn).Skip(start).Take(limit);
                dtTable.data = data.Select(d => new VWPODetail
                {
                    Id = d.Id,
                    POId=d.POId,
                    NamaBarang=d.NamaBarang,
                    Kode=d.Kode,
                    Harga=d.Harga,
                    Deskripsi=d.Deskripsi,
                    Banyak=d.Banyak,
                    Satuan=d.Satuan,
                    Keterangan = d.Keterangan 
                }).ToList();

            }
            return dtTable;
        }

        public VWPO detail(Guid Id, Guid UserId)
        {
            return ctx.POs.Where(d => d.Id == Id).Select(d => new VWPO()
            {
                Id = d.Id,
                Prihal = d.Prihal,
                Vendor = d.Vendor,
                NoPO = d.NoPO,
                TanggalPO = d.TanggalPO,
                NilaiPO = d.PODetail.Sum(dd => dd.Banyak * dd.Harga),
                UP = d.UP,
                PeriodeDari = d.PeriodeDari,
                PeriodeSampai = d.PeriodeSampai,
                NamaBank = d.NamaBank,
                AtasNama = d.AtasNama,
                NoRekening = d.NoRekening,
                AlamatPengirimanBarang = d.AlamatPengirimanBarang,
                UPPengirimanBarang = d.UPPengirimanBarang,
                TelpPengirimanBarang = d.TelpPengirimanBarang,
                AlamatKwitansi = d.AlamatKwitansi,
                NPWP = d.NPWP,
                AlamatPengirimanKwitansi = d.AlamatPengirimanKwitansi,
                UPPengirimanKwitansi = d.UPPengirimanKwitansi,
                Ttd1 = d.Ttd1,
                Ttd2 = d.Ttd2,
                Ttd3 = d.Ttd3,
                Discount = d.Discount.Value,
                PPN = d.PPN.Value,
                PPH = d.PPH.Value,

            }).FirstOrDefault();
        }

        public ResultMessage save(PO po, Guid UserId)
        {
            try
            {
                var data = ctx.POs.Find(po.Id);

                if (data != null)
                {
                    data.Prihal = po.Prihal;
                    data.Vendor = po.Vendor;
                    data.NoPO = po.NoPO;
                    data.TanggalPO = po.TanggalPO;
                    data.NilaiPO = po.NilaiPO;
                    data.UP = po.UP;
                    data.PeriodeDari = po.PeriodeDari;
                    data.PeriodeSampai = po.PeriodeSampai;
                    data.NamaBank = po.NamaBank;
                    data.AtasNama = po.AtasNama;
                    data.NoRekening = po.NoRekening;
                    data.AlamatPengirimanBarang = po.AlamatPengirimanBarang;
                    data.UPPengirimanBarang = po.UPPengirimanBarang;
                    data.TelpPengirimanBarang = po.TelpPengirimanBarang;
                    data.AlamatKwitansi = po.AlamatKwitansi;
                    data.NPWP = po.NPWP;
                    data.AlamatPengirimanKwitansi = po.AlamatPengirimanKwitansi;
                    data.UPPengirimanKwitansi = po.UPPengirimanKwitansi;
                    data.Ttd1 = po.Ttd1;
                    data.Ttd2 = po.Ttd2;
                    data.Ttd3 = po.Ttd3;
                    data.Discount = po.Discount;
                    data.PPN = po.PPN;
                    data.PPH = po.PPH;

                    data.CreatedBy = UserId;
                    data.CreatedOn = DateTime.Now;
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = data.Id.ToString(),
                        message = Common.UpdateSukses(),
                        status = HttpStatusCode.OK
                    };
                }
                else
                {
                    po.CreatedBy = UserId;
                    po.CreatedOn = DateTime.Now;
                    po.NoPO = GenerateNoPO(UserId);
                    ctx.POs.Add(po);
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = po.Id.ToString(),
                        message = Common.SaveSukses(),
                        status = HttpStatusCode.OK
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };

            }
        }

        public ResultMessage saveItem(PODetail PODetail, Guid UserId)
        {
            try
            {
                var data = ctx.PODetails.Find(PODetail.Id);

                if (data != null)
                {

                    data.Kode = PODetail.Kode;
                    data.Harga = PODetail.Harga;
                    data.Deskripsi = PODetail.Deskripsi;
                    data.Keterangan = PODetail.Keterangan;
                    data.NamaBarang = PODetail.NamaBarang;
                    data.Satuan = PODetail.Satuan;
                    data.Banyak = PODetail.Banyak;
                    data.CreatedBy = UserId;
                    data.CreatedOn = DateTime.Now;
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = data.Id.ToString(),
                        message = Common.UpdateSukses(),
                        status = HttpStatusCode.OK
                    };
                }
                else
                {
                    PODetail.CreatedBy = UserId;
                    PODetail.CreatedOn = DateTime.Now;
                    ctx.PODetails.Add(PODetail);
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = PODetail.Id.ToString(),
                        message = Common.SaveSukses(),
                        status = HttpStatusCode.OK
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };

            }
        }

        public ResultMessage DeleteItem(Guid Id, Guid UserId)
        {
            try
            {
                var oData = ctx.PODetails.Find(Id);
                if (oData != null)
                {
                    ctx.PODetails.Remove(oData);
                }
                ctx.SaveChanges(UserId.ToString());
                return new ResultMessage()
                {
                    Id = oData.Id.ToString(),
                    message = Common.DeleteSukses(),
                    status = HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ResultMessage()
                {
                    Id = "0",
                    message = ex.ToString(),
                    status = HttpStatusCode.NotImplemented
                };
            }
        }

        public ResultMessage saveDokumen(DokumenPO data, Guid UserId)
        {
            try
            {
                data.CreateOn = DateTime.Now;
                data.CreateBy = UserId;
                ctx.DokumenPO.Add(data);
                ctx.SaveChanges(UserId.ToString());
                return new ResultMessage
                {
                    status = HttpStatusCode.OK,
                    message = Common.SaveSukses(),
                    Id = data.Id.ToString()
                };
            }
            catch(Exception ex){
                return new ResultMessage()
                {
                    status = HttpStatusCode.NotImplemented,
                    message = ex.ToString()
                };
            }
        }

        public List<VWDokumenPO> GetListDokumenPO(Guid Id)
        {
            return ctx.DokumenPO.Where(d => d.POId == Id).Select(d => new VWDokumenPO()
            {
                Id=d.Id,
                POId = d.POId,
                SizeFile=d.SizeFile,
                ContentType=d.ContentType,
                File=d.File
            }).ToList();
        }

        public int deleteDokumenPO(Guid Id, Guid UserId)
        {
            try
            {
                DokumenPO data = ctx.DokumenPO.Find(Id);
                if (data != null)
                {
                    ctx.DokumenPO.Remove(data);
                    ctx.SaveChanges(UserId.ToString());
                }
                return 1;
                
            }
            catch { return 0; }
        }

        public string GenerateNoPO(Guid UserId)
        {
            var oNoDokumen = ctx.NoDokumenGenerators.Where(d => d.tipe == TipeNoDokumen.PO).OrderByDescending(d => d.Id).FirstOrDefault();
            var KODE = System.Configuration.ConfigurationManager.AppSettings["KODE_PO"].ToString();
            if (oNoDokumen == null)
            {
                string newNODok = "1" + KODE + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;
                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNODok;
                newoNoDokumen.tipe = TipeNoDokumen.PO;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges(UserId.ToString());
                return newoNoDokumen.No;
            }
            else
            {
                var arrNo = oNoDokumen.No.Split('/');
                int NextNo = Convert.ToInt32(arrNo[0]);
                NextNo = NextNo + 1;
                int oldYear = Convert.ToInt32(arrNo[4]);
                string newNoDokmen = "";
                if (oldYear == DateTime.Now.Year)
                    newNoDokmen = NextNo.ToString() + KODE + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + +DateTime.Now.Year;
                else newNoDokmen = "1" + KODE + Common.ConvertBulanRomawi(DateTime.Now.Month) + "/" + DateTime.Now.Year;

                NoDokumenGenerator newoNoDokumen = new NoDokumenGenerator();
                newoNoDokumen.CreateOn = DateTime.Now;
                newoNoDokumen.CreateBy = UserId;
                newoNoDokumen.No = newNoDokmen;
                newoNoDokumen.tipe = TipeNoDokumen.PO;
                ctx.NoDokumenGenerators.Add(newoNoDokumen);
                ctx.SaveChanges(UserId.ToString());
                return newoNoDokumen.No;
            }
        }


        public DokumenPO GetDokumenPO(Guid Id)
        {
            return ctx.DokumenPO.Find(Id);
        }
    }
}