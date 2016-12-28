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
    public interface IBudgetRepo
    {
        DataTableBudget List(int start, int limit, string cari);
        ResultMessage add(List<COA> nlstcoa, Guid UserId);
    }
    public class BudgetRepo : IBudgetRepo
    {
        JimbisContext ctx;

        public BudgetRepo(JimbisContext j)
        {
            ctx = j;
            //ctx.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = true;
        }

        ResultMessage msg = new ResultMessage();



        public DataTableBudget List(int start, int limit, string cari)
        {
            cari = cari == null ? "" : cari;
            DataTableBudget dtTable = new DataTableBudget();
            if (limit > 0)
            {
                var data = ctx.COAs.AsQueryable();
                dtTable.recordsTotal = data.Count();
                if (!string.IsNullOrEmpty(cari))
                {
                    data = data.Where(d => d.Pengadaan.Judul.Contains(cari));
                }
                dtTable.recordsFiltered = data.Count();
                data = data.OrderByDescending(d => d.UploadedOn).Skip(start).Take(limit);
                var lol = data.ToList();
                dtTable.data = data.Select(d => new VVWCOA
                {
                    Id = d.Id,
                    Divisi = d.Divisi,
                    GroupAset = d.GroupAset,
                    JenisAset = d.JenisAset,
                    NilaiAset = d.NilaiAset,
                    Periode = d.Periode,
                    Region = d.Region,
                    NoCoa = d.NoCoa
                }).ToList();
            }
            return dtTable;
        }

        public ResultMessage add(List<COA> nlstcoa, Guid UserId)
        {
            try
            {
                foreach (var ncoa in nlstcoa)
                {
                    var oCoa = ctx.COAs.Where(d => d.NoCoa == ncoa.NoCoa).FirstOrDefault();
                    if (oCoa == null)
                    {
                        //var pengadaan = ctx.Pengadaans.Where(d => d.NoCOA == ncoa.NoCoa).FirstOrDefault();
                        ////if (pengadaan != null)
                       // {
                           // ncoa.PengadaanId = pengadaan.Id;
                            ncoa.UploadedBy = UserId;
                            ncoa.UploadedOn = DateTime.Now;
                            ctx.COAs.Add(ncoa);
                       // }
                    }
                    else
                    {
                        oCoa.Divisi = ncoa.Divisi;
                        oCoa.GroupAset = ncoa.GroupAset;
                        oCoa.JenisAset = ncoa.JenisAset;
                        oCoa.ModifiedUploadBy = UserId;
                        oCoa.NilaiAset = ncoa.NilaiAset;
                        oCoa.Periode = ncoa.Periode;
                        oCoa.Region = ncoa.Region;
                    }
                }
                ctx.SaveChanges();
                return new ResultMessage()
                {
                    message=Common.SaveSukses(),
                    status=HttpStatusCode.OK
                };
            }
            catch(Exception ex)
            {
                return  new ResultMessage()
                {
                    message = ex.ToString(),
                    status=HttpStatusCode.NotImplemented
                };;
            }
        }
    }
}


