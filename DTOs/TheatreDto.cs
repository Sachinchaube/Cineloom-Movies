using System.ComponentModel.DataAnnotations;

namespace MovieBookingPro.DTOs
{
    public class TheatreDto
    {
        public int TheatreId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class TheatreCreateDto
    {
        [Required(ErrorMessage = "Theatre name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Length of {0} should be between {2} and {1}")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;
    }

    public class TheatreEditDto : TheatreCreateDto
    {
        public int TheatreId { get; set; }
    }
}
