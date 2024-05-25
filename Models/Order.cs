using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleLemon_API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        // Collection of Meals within the Order
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}
