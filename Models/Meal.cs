using System;
using System.ComponentModel.DataAnnotations;

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

        public float Price { get; set; }
        
    }
}
