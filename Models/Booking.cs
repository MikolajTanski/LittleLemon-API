using System.ComponentModel.DataAnnotations;

namespace LittleLemon_API.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Meal name is required")]
        [StringLength(150)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        [StringLength(150)]
        public DateTime Date { get; set; }
    }
}
