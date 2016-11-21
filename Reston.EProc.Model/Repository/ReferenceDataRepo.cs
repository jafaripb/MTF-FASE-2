using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public class ReferenceDataRepo : IReferenceDataRepo
    {
        JimbisContext ctx;
        public ReferenceDataRepo(JimbisContext j)
        {
            ctx = j;
        }

        public List<ReferenceData> GetData(string qualifier, string code){
            return ctx.ReferenceDatas.Where(x => x.Qualifier == qualifier && (code == null || x.Code == code)).ToList();
        }

        public void SaveData(ReferenceData d) {
            ctx.ReferenceDatas.Add(d);
            ctx.SaveChanges();
        }

        public void Delete(ReferenceData d)
        {
            ctx.ReferenceDatas.Remove(d);
            ctx.SaveChanges();
        }

        public ReferenceData GetDataById(int id){
            return ctx.ReferenceDatas.Find(id);
        }
    }
}
