using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdLdap.Models
{
    public class DataPageUsers
    {
        public int? totalRecord { get; set; }
        public List<Userx> Users { get; set; }
    }

    public class Userx
    {
        public string PersonilId { get; set; }
        public string Nama { get; set; }
        public string jabatan { get; set; }
        public string Email { get; set; }
        public string tlp { get; set; }
        public string FullName { get; set; }
    }
}