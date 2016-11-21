using Reston.Pinata.Model.JimbisModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.Repository
{
    public interface IReferenceDataRepo
    {
        List<ReferenceData> GetData(string qualifier, string code);
        ReferenceData GetDataById(int id);
        void Delete(ReferenceData d);
        void SaveData(ReferenceData d);
    }
}
