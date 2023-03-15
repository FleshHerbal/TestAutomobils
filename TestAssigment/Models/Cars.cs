using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestAssigment.Models
{
    public class Cars
    {
        [Table("cars_brand")]
        public class Brand 
        {
            [Key] [Column("guid")]
            public Guid Guid { get; set; }

            [Column("brand_name")] [Required]
            public string Name { get; set; }

            [Column("is_active")]
            public bool IsActive { get; set; }

        }
        [Table("cars_model")]
        public class Model
        {
            [Key] [Column("guid")]
            public Guid Guid { get; set; }

            [Column("model_name")] [Required]
            public string Name { get; set; }

            [Column("brand_guid")] [Required]
            public Guid BrandGuid { get; set; }

            [Column("is_active")]
            public bool IsActive { get; set; }
        }
    }
}
