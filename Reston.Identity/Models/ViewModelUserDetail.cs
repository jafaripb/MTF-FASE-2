using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Models
{
    public class ViewModelUserDetail
    {
        public UserDetail UserDetail { get; set; }
        public List<RoleUser> RoleUser { get; set; }
    }
}
