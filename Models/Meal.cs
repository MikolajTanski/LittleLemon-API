using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleLemon_API.Models
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Meal name is required")]
        [StringLength(150)]
        public string Name { get; set; }

        public string Type { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

    }
}
