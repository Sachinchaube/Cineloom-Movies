using System.ComponentModel.DataAnnotations;

namespace MovieBookingPro.DTOs
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int ShowId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheatreName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public DateTime ShowDate { get; set; }
        public TimeSpan ShowTime { get; set; }
        public int SeatCount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class BookingCreateDto
    {
        [Required]
        public int ShowId { get; set; }

        [Required(ErrorMessage = "Number of seats is required")]
        [Range(1, 10, ErrorMessage = "You can book between {1} and {2} seats at a time")]
        public int SeatCount { get; set; }
    }
}
