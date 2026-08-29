using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBookingPro.Models
{
    public class ShowSchedule
    {
        [Key]
        public int ShowId { get; set; }

        [Required(ErrorMessage = "Movie is required")]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        [Required(ErrorMessage = "Screen is required")]
        public int ScreenId { get; set; }

        [ForeignKey("ScreenId")]
        public Screen? Screen { get; set; }

        [Required(ErrorMessage = "Show date is required")]
        [DataType(DataType.Date)]
        public DateTime ShowDate { get; set; }

        [Required(ErrorMessage = "Show time is required")]
        [DataType(DataType.Time)]
        public TimeSpan ShowTime { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000, ErrorMessage = "Price must be between {1} and {2}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public List<Booking>? Bookings { get; set; }
    }
}
