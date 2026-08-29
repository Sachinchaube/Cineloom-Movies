using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBookingPro.Models
{
    public class Screen
    {
        [Key]
        public int ScreenId { get; set; }

        [Required(ErrorMessage = "Theatre is required")]
        public int TheatreId { get; set; }

        [ForeignKey("TheatreId")]
        public Theatre? Theatre { get; set; }

        [Required(ErrorMessage = "Screen name is required")]
        [StringLength(30, ErrorMessage = "Length of {0} should not exceed {1}")]
        public string ScreenName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seat capacity is required")]
        [Range(1, 1000, ErrorMessage = "Seat capacity must be between {1} and {2}")]
        public int SeatCapacity { get; set; }

        public List<ShowSchedule>? ShowSchedules { get; set; }
    }
}
