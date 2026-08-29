using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieBookingPro.Models
{
    public enum BookingStatus
    {
        Confirmed,
        Cancelled
    }

    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        [Required(ErrorMessage = "Show is required")]
        public int ShowId { get; set; }

        [ForeignKey("ShowId")]
        public ShowSchedule? Show { get; set; }

        [Required(ErrorMessage = "Number of seats is required")]
        [Range(1, 10, ErrorMessage = "You can book between {1} and {2} seats at a time")]
        public int SeatCount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    }
}
