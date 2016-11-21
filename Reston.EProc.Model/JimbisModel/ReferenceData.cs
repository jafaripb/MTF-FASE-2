using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reston.Pinata.Model.JimbisModel
{
    [Table("ReferenceData", Schema=JimbisContext.MASTER_SCHEMA_NAME)]
    public class ReferenceData
    {
        public const int QUALIIFIER_MAX_LENGTH = 26;
        public const int CODE_MAX_LENGTH = 30;

        private const string QUALIFIER_CODE_INDEX_NAME = "IndexQualifierCode";

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(QUALIIFIER_MAX_LENGTH)]
        [Index(QUALIFIER_CODE_INDEX_NAME, Order = 1, IsUnique = true)]
        public string Qualifier { get; set; }

        [Required]
        [MaxLength(CODE_MAX_LENGTH)]
        [Index(QUALIFIER_CODE_INDEX_NAME, Order = 2, IsUnique = true)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string LocalizedName { get; set; }

        [MaxLength(256)]
        public string LocalizedDesc { get; set; }

        [MaxLength(256)]
        public string StringAttr1 { get; set; }
        [MaxLength(256)]
        public string StringAttr2 { get; set; }
        [MaxLength(256)]
        public string StringAttr3 { get; set; }

        public Nullable<int> IntAttr1 { get; set; }
        public Nullable<int> IntAttr2 { get; set; }
        public Nullable<int> IntAttr3 { get; set; }

        public Nullable<bool> FlagAttr1 { get; set; }
        public Nullable<bool> FlagAttr2 { get; set; }
        public Nullable<bool> FlagAttr3 { get; set; }
    }
}
