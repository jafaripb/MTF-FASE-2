using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.WebService.ViewModels
{
    public class ReferenceDataViewModel
    {
        public ReferenceDataViewModel()
        {
            //this.VendorPerson;
        }
        public int id { get;set;}
        public string Code { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Str1 { get; set; }
        public int? Int1 { get; set; }
        public bool? Flag1 { get; set; }
    }

}
