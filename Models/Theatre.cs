using System.ComponentModel.DataAnnotations;

namespace MovieBookingPro.Models
{
    public class Theatre
    {
        [Key]
        public int TheatreId { get; set; }

        [Required(ErrorMessage = "Theatre name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length of {0} should be between {2} and {1}")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150, ErrorMessage = "Length of {0} should not exceed {1}")]
        public string Location { get; set; } = string.Empty;

        public List<Screen>? Screens { get; set; }
    }
}
