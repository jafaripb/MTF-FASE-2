using SharpConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Helper
{
    public class IdConfiguration
    {
        public static Configuration eConfig { get; set; }


        public static Configuration GetIdLdapConfiguration()
        {
            String filePath = string.Format(@"{0}\Configuration\{1}", AppDomain.CurrentDomain.BaseDirectory, "Configuration.txt");

            eConfig = Configuration.LoadFromStream(new FileStream(filePath, FileMode.Open,
                            FileAccess.Read, FileShare.Read));

            return eConfig;
        }
    }
}
