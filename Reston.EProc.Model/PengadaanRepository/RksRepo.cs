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
    public interface IRksRepo
    {
        ResultMessage saveRks(RKSHeaderTemplate rks, Guid UserId);
        ResultMessage deleteRks(Guid Id, Guid UserId);
        DataTableRksTemplate GetRks(string search, int start, int limit, string klasifikasi);
        DataTableRksDetailTemplate GetRksDetail(Guid Id);
        VWRKSTemplate getRksHeader(Guid Id);
        RKSHeader MapRksFromTemplate(Guid RksTempalteId, Guid PengadaanId);
    }
    public class RksRepo : IRksRepo
    {
        JimbisContext ctx;
        public RksRepo(JimbisContext j)
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

        public DataTableRksTemplate GetRks(string search, int start, int limit,string klasifikasi)
        {
            search = search == null ? "" : search;
            DataTableRksTemplate dtTable=new DataTableRksTemplate();
            if (limit > 0)
            {
              var data= ctx.RKSHeaderTemplate.Where(d => d.Title.Contains(search));

             if(!string.IsNullOrEmpty(klasifikasi)) {
                KlasifikasiPengadaan kls= (KlasifikasiPengadaan)Convert.ToInt32(klasifikasi);
                data = data.Where(d => d.Klasifikasi == kls);
             }
              dtTable.recordsTotal=data.Count();        
              data=data.OrderByDescending(d => d.CreateOn).Skip(start).Take(limit);
              dtTable.recordsFiltered=data.Count();
              dtTable.data=data.Select(d=>new VWRKSTemplate{
                         Id=d.Id,
                         Deskripsi=d.Description,
                         Klasifikasi=d.Klasifikasi,
                         Title=d.Title                        
                        }).ToList();
             }
            return dtTable;
        }

        public DataTableRksDetailTemplate GetRksDetail(Guid Id)
        {

            DataTableRksDetailTemplate dtTable = new DataTableRksDetailTemplate();
            
            var data = ctx.RKSDetailTemplate.Where(d => d.RKSHeaderTemplateId==Id);           
            dtTable.recordsTotal = data.Count();
            dtTable.recordsFiltered = data.Count();
            dtTable.data = data.Select(b => new VWRKSDetailTemplate
            {
                Id = b.Id,
                hps = b.hps,
                item = b.item,
                ItemId = b.ItemId,
                jumlah = b.jumlah,
                RKSHeaderTemplateId = b.RKSHeaderTemplateId,
                satuan = b.satuan,
                keterangan = b.keterangan
            }).ToList();
            
            return dtTable;
        }
        
        public ResultMessage saveRks(RKSHeaderTemplate rks, Guid UserId)
        {
            ResultMessage msg = new ResultMessage();
            if (rks.Id != Guid.Empty)
            {
                RKSHeaderTemplate MRKSHeader = ctx.RKSHeaderTemplate.Find(rks.Id);
                if (MRKSHeader != null)
                {
                    MRKSHeader.Klasifikasi = rks.Klasifikasi;
                    MRKSHeader.Title = rks.Title;
                    MRKSHeader.Klasifikasi = rks.Klasifikasi;
                    MRKSHeader.Description = rks.Description;
                    MRKSHeader.ModifiedBy = UserId;
                    MRKSHeader.ModifiedOn = DateTime.Now;
                    foreach (var item in rks.RKSDetailTemplate)
                    {
                        if (item.Id != Guid.Empty)
                        {
                            RKSDetailTemplate rksdetail = ctx.RKSDetailTemplate.Find(item.Id);
                            rksdetail.item = item.item;
                            rksdetail.hps = item.hps;
                            rksdetail.item = item.item;
                            rksdetail.ItemId = item.ItemId;
                            rksdetail.jumlah = item.jumlah;
                            rksdetail.satuan = item.satuan;
                            rksdetail.keterangan = item.keterangan;
                            rksdetail.ModifiedBy = UserId;
                            rksdetail.ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            RKSDetailTemplate newrksdetail = new RKSDetailTemplate();
                            newrksdetail.hps = item.hps;
                            newrksdetail.item = item.item;
                            newrksdetail.ItemId = item.ItemId;
                            newrksdetail.jumlah = item.jumlah;
                            newrksdetail.satuan = item.satuan;
                            newrksdetail.keterangan = item.keterangan;
                            newrksdetail.RKSHeaderTemplateId = MRKSHeader.Id;
                            newrksdetail.CreateOn = DateTime.Now;
                            newrksdetail.CreateBy = UserId;
                            MRKSHeader.RKSDetailTemplate.Add(newrksdetail);
                        }
                    }
                    var rksrequesIds = rks.RKSDetailTemplate.Select(d => d.Id);
                    var removeRksDetail = MRKSHeader.RKSDetailTemplate.Where(d => !rksrequesIds.Contains(d.Id));
                    ctx.RKSDetailTemplate.RemoveRange(removeRksDetail);

                }
                else
                {
                    rks.CreateBy = UserId;
                    rks.CreateOn = DateTime.Now;
                    ctx.RKSHeaderTemplate.Add(rks);
                }

            }
            else
            {
                rks.CreateBy = UserId;
                rks.CreateOn = DateTime.Now;
                ctx.RKSHeaderTemplate.Add(rks);
            } 
            try
            {
                ctx.SaveChanges(UserId.ToString());
                msg.status = HttpStatusCode.OK;
                msg.message = "Sukses";
                msg.Id = rks.Id.ToString();
               // msg.exData = JsonConvert.SerializeObject(rks);
            }
            catch (Exception ex)
            {
                msg.status = HttpStatusCode.ExpectationFailed;
                msg.message = ex.ToString();
                
            }
            return msg;
        }

        public ResultMessage deleteRks(Guid Id, Guid UserId)
        {            
            RKSHeaderTemplate oData = ctx.RKSHeaderTemplate.Find(Id);
            var oLstDataDetail = ctx.RKSDetailTemplate.Where(d => d.RKSHeaderTemplateId == oData.Id);
            ctx.RKSDetailTemplate.RemoveRange(oLstDataDetail);
            ctx.RKSHeaderTemplate.Remove(oData);
            try
            {
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


        public VWRKSTemplate getRksHeader(Guid Id)
        {
            var oData=ctx.RKSHeaderTemplate.Find(Id);
            return new VWRKSTemplate()
            {
                Id = oData.Id,
                Deskripsi = oData.Description,
                Klasifikasi = oData.Klasifikasi,
                Title = oData.Title
            };
        }

        public RKSHeader MapRksFromTemplate(Guid RksTempalteId, Guid PengadaanId)
        {
            var odataRksTempalte = ctx.RKSHeaderTemplate.Find(RksTempalteId);
            var oDataRksHeader = new RKSHeader();
            oDataRksHeader.PengadaanId = PengadaanId;
            List<RKSDetail> lstRksDetail = new List<RKSDetail>();
            foreach (var item in odataRksTempalte.RKSDetailTemplate)
            {
                RKSDetail oRksDetail = new RKSDetail();
                oRksDetail.item = item.item;
                oRksDetail.ItemId = item.ItemId;
                oRksDetail.jumlah = item.jumlah;
                oRksDetail.hps = item.hps;
                oRksDetail.satuan = item.satuan;
                oRksDetail.keterangan = item.keterangan;
                lstRksDetail.Add(oRksDetail);
            }
            oDataRksHeader.RKSDetails = lstRksDetail;

            return oDataRksHeader;
        }

    }
}


