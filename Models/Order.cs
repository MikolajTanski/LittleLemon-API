using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LittleLemon_API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual ICollection<Meal> Meals { get; set; }

        public float Total { get; set; }
    }
}
