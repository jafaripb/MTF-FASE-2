using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model
{
    public class Menu
    {
        public Nullable<int> id { get; set; }
        public string menu { get; set; }
        public string url { get; set; }
        public string css { get; set; }
    }

    public class UserInfo
    {
        public Nullable<int> Id { get; set; }
        public List<role> Roles { get; set; }
        public string username { get; set; }
        public string jabatan { get; set; }
    }

    public class role
    {
        public string roleName { get; set; }
    }
}
