using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Helper.Model
{
    [Table("Captcha", Schema = SchemaConstants.HELPER_SCHEMA_NAME)]
    public class Captcha
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(10)]
        public string Text { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
