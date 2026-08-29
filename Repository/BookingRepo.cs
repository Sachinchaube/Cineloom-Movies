using Microsoft.EntityFrameworkCore;
using MovieBookingPro.DAL;
using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public class BookingRepo : IBookingRepo
    {
        private readonly ApplicationDbContext _context;

        public BookingRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(sc => sc!.Theatre)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByUser(string userId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(sc => sc!.Theatre)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(sc => sc!.Theatre)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<int> Insert(Booking obj)
        {
            await _context.Bookings.AddAsync(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return 0;
            booking.Status = BookingStatus.Cancelled;
            _context.Bookings.Update(booking);
            return await _context.SaveChangesAsync();
        }
    }
}
