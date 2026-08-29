using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public interface IBookingRepo
    {
        Task<List<Booking>> GetAllBookingsAsync();
        Task<List<Booking>> GetBookingsByUser(string userId);
        Task<Booking?> GetBookingById(int id);
        Task<int> Insert(Booking obj);
        Task<int> CancelBooking(int id);
    }
}
