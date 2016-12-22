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
    public interface ISpkRepo
    {

        Spk saveSpkPertam(Spk spk, Guid UserId);
        int Delete(Guid Id, Guid UserId);

    }
    public class SpkRepo : ISpkRepo
    {
        JimbisContext ctx;
        public SpkRepo(JimbisContext j)
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

        public Spk saveSpkPertam(Spk spk, Guid UserId)
        {
            var oSpk = ctx.Spk.Where(d => d.NoSPk == spk.NoSPk && d.PksId==null).FirstOrDefault();
            if (oSpk == null)
            {

                spk.CreateOn = DateTime.Now;
                spk.CreateBy = UserId;
                ctx.Spk.Add(spk);
            }
            else
            {
                //oSpk = new Spk();
                oSpk.NoSPk = spk.NoSPk;
                oSpk.Note = spk.Note;
                oSpk.PemenangPengadaanId = spk.PemenangPengadaanId;
                oSpk.StatusSpk = spk.StatusSpk;
                oSpk.Title = spk.Title;
                oSpk.WorkflowId = spk.WorkflowId;
                oSpk.ModifiedBy = UserId;
                oSpk.ModifiedOn = DateTime.Now;
            }
            ctx.SaveChanges();
            return oSpk;
        }

        public int Delete(Guid Id, Guid UserId)
        {
            try
            {
                var oSpk = ctx.Spk.Find(Id);
                if (oSpk != null)
                {
                    ctx.Spk.Remove(oSpk);
                }
                ctx.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }


        }
        
    }
}


