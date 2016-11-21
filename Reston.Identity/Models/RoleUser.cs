using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdLdap.Models
{
    public class RoleUser
    {
        // Summary:
        //     Role id
        public string Id { get; set; }
        //
        // Summary:
        //     Role name
        public string Name { get; set; }

        public bool Selected { get; set; }

        public string RoleDescription { get; set; }
    }
}
