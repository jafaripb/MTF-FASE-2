using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reston.Pinata.Model;
using Reston.Pinata.Model.JimbisModel;
using Reston.Pinata.Model.PengadaanRepository;

namespace Reston.Eproc.Model.Monitoring.Entities
{
    [Table("Menu", Schema = JimbisContext.MENU_SCHEMA_NAME)]
    public class Menu
    {
        //{ "id": 1 , "menu": "Master Data", "url": "master.html","css":"fa fa-cubes"   },
        [Key]
        public int Id { get; set; }
        public string menu { get; set; }
        public string Deskripsi { get; set; }
        public string url { get; set; }
        public string css { get; set; }
    }

    [Table("UserMenu", Schema = JimbisContext.MENU_SCHEMA_NAME)]
    public class Menu
    {
        [Key]
        public int Id { get; set; }
         [ForeignKey("Menu")]
        public int MenuId { get; set; }
        public string UserRole { get; set; }
        public virtual Menu PO { get; set; }
    }
}
