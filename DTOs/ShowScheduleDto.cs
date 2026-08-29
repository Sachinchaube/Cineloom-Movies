using System.ComponentModel.DataAnnotations;

namespace MovieBookingPro.DTOs
{
    public class ShowScheduleDto
    {
        public int ShowId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }
        public int ScreenId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public string TheatreName { get; set; } = string.Empty;
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public decimal Price { get; set; }
        public int SeatCapacity { get; set; }
        public int SeatsBooked { get; set; }
        public int SeatsAvailable => SeatCapacity - SeatsBooked;
    }

    public class ShowScheduleCreateDto
    {
        [Required(ErrorMessage = "Movie is required")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Screen is required")]
        public int ScreenId { get; set; }

        [Required(ErrorMessage = "Show date is required")]
        [DataType(DataType.Date)]
        public DateTime ShowDate { get; set; }

        [Required(ErrorMessage = "Show time is required")]
        [DataType(DataType.Time)]
        public TimeSpan ShowTime { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000, ErrorMessage = "Price must be between {1} and {2}")]
        public decimal Price { get; set; }
    }

    public class ShowScheduleEditDto : ShowScheduleCreateDto
    {
        public int ShowId { get; set; }
    }
}
