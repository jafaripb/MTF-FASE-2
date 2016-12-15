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

namespace Reston.Pinata.Model.PengadaanRepository
{
    public interface IPksRepo
    {
        DataTablePksTemplate List(string search, int start, int limit, string klasifikasi);
        VWPks detail(Guid PengadaanId, int VendorId);
        ResultMessage save(Pks pks, Guid UserId);
        ResultMessage deletePks(Guid Id, Guid UserId);
        Pks get(Guid id);        
    }
    public class PksRepo : IPksRepo
    {
        JimbisContext ctx;
        public PksRepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        ResultMessage msg = new ResultMessage();

    
        public void Save()
        {
            ctx.SaveChanges();
        }

        public DataTablePksTemplate List(string search, int start, int limit, string klasifikasi)
        {
            search = search == null ? "" : search;
            DataTablePksTemplate dtTable = new DataTablePksTemplate();
            if (limit > 0)
            {
              var data= ctx.DokumenPengadaans.Where(d =>d.Tipe==TipeBerkas.SuratPerintahKerja);
              dtTable.recordsTotal = data.Count();   
             if(!string.IsNullOrEmpty(klasifikasi)) {
                 data = data.Where(d => d.Pengadaan.JenisPekerjaan==klasifikasi);
             }
             if (!string.IsNullOrEmpty(search))
             {
                 data = data.Where(d => d.Pengadaan.Judul.Contains(search)||d.Pengadaan.NoPengadaan.Contains(search));
             }
             dtTable.recordsFiltered = data.Count();
              data=data.OrderByDescending(d => d.CreateOn).Skip(start).Take(limit);              
              dtTable.data = data.Select(d => new VWPks
              {
                         Id=d.Id,
                         PengadaanId=d.PengadaanId,
                         NoSpk=d.NoDokumen,
                         VendorId=d.VendorId,
                         Judul=d.Pengadaan.Judul,
                         JenisPekerjaan=d.Pengadaan.JenisPekerjaan,
                         Vendor=d.Vendor==null?"":d.Vendor.Nama,
                         AturanPengadaan=d.Pengadaan.AturanPengadaan,
                         HPS=d.Pengadaan.RKSHeaders.FirstOrDefault()==null?null:d.Pengadaan.RKSHeaders.FirstOrDefault().RKSDetails.Where(dd => dd.RKSHeaderId == d.Pengadaan.RKSHeaders.FirstOrDefault().Id).Sum(dx => dx.jumlah * dx.hps == null ? 0 : dx.jumlah * dx.hps)
                        }).ToList();
             }
            return dtTable;
        }
        
        public ResultMessage save(Pks pks, Guid UserId)
        {
            try
            {
                if (pks.Id != Guid.Empty && pks.Id != null)
                {
                    var oldPks = ctx.Pks.Find(pks.Id);
                    if (oldPks == null) return new ResultMessage();
                    oldPks.DokumenPengadaanId = pks.DokumenPengadaanId;
                    oldPks.ModifiedBy = UserId;
                    oldPks.ModifiedOn = DateTime.Now;
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = pks.Id.ToString(),
                        message = Common.UpdateSukses(),
                        status = HttpStatusCode.OK
                    };
                }
                else
                {
                    pks.CreateBy = UserId;
                    pks.CreateOn = DateTime.Now;
                    ctx.Pks.Add(pks);
                    ctx.SaveChanges(UserId.ToString());
                    return new ResultMessage()
                    {
                        Id = pks.Id.ToString(),
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
                    status = HttpStatusCode.ExpectationFailed
                };

            }
        }

        public Pks get(Guid id)
        {
            return ctx.Pks.Find(id);
        }

        public ResultMessage deletePks(Guid Id, Guid UserId)
        {   
            try
            {
                var oldData = ctx.Pks.Find(Id);
                ctx.Pks.Remove(oldData);
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
            }
            catch (Exception ex) {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
            }
            return msg;
        }

        public VWPks detail(Guid PengadaanId, int VendorId)
        {
            return ctx.Pengadaans.Where(d => d.Id == PengadaanId).Select(d => new VWPks()
            {
                Id=PengadaanId,
                JenisPekerjaan=d.JenisPekerjaan,
                Judul=d.Judul,
                NoPengadaan=d.NoPengadaan,
                Keterangan=d.Keterangan,
                HPS=d.RKSHeaders.FirstOrDefault().RKSDetails.Sum(dd=>dd.hps*dd.jumlah==null?0:dd.hps*dd.jumlah).Value,
                NoSpk = d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault() == null ? "" : d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault().NoBeritaAcara,
                Vendor=d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault() == null ? "" : d.BeritaAcaras.Where(dd => dd.VendorId == VendorId).FirstOrDefault().Vendor.Nama             
            }).FirstOrDefault();
        }

        
        
    }
}


