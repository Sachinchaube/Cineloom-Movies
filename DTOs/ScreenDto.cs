using System.ComponentModel.DataAnnotations;

namespace MovieBookingPro.DTOs
{
    public class ScreenDto
    {
        public int ScreenId { get; set; }
        public int TheatreId { get; set; }
        public string TheatreName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public int SeatCapacity { get; set; }
    }

    public class ScreenCreateDto
    {
        [Required(ErrorMessage = "Theatre is required")]
        public int TheatreId { get; set; }

        [Required(ErrorMessage = "Screen name is required")]
        [StringLength(30)]
        public string ScreenName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seat capacity is required")]
        [Range(1, 1000, ErrorMessage = "Seat capacity must be between {1} and {2}")]
        public int SeatCapacity { get; set; }
    }

    public class ScreenEditDto : ScreenCreateDto
    {
        public int ScreenId { get; set; }
    }
}
