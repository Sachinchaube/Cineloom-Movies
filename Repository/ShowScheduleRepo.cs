using Microsoft.EntityFrameworkCore;
using MovieBookingPro.DAL;
using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public class ShowScheduleRepo : IShowScheduleRepo
    {
        private readonly ApplicationDbContext _context;

        public ShowScheduleRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShowSchedule>> GetShowsAsync()
        {
            return await _context.ShowSchedules
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                .Include(s => s.Bookings)
                .OrderBy(s => s.ShowDate)
                    .ThenBy(s => s.ShowTime)
                .ToListAsync();
        }

        public async Task<ShowSchedule?> GetShowById(int id)
        {
            return await _context.ShowSchedules
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                .Include(s => s.Bookings)
                .FirstOrDefaultAsync(s => s.ShowId == id);
        }

        public async Task<List<ShowSchedule>> GetShowsByMovie(int movieId)
        {
            return await _context.ShowSchedules
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc!.Theatre)
                .Include(s => s.Bookings)
                .Where(s => s.MovieId == movieId && s.ShowDate >= DateTime.Today)
                .OrderBy(s => s.ShowDate)
                    .ThenBy(s => s.ShowTime)
                .ToListAsync();
        }

        public async Task<int> Insert(ShowSchedule obj)
        {
            await _context.ShowSchedules.AddAsync(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ShowSchedule obj)
        {
            _context.ShowSchedules.Update(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var show = await _context.ShowSchedules.FindAsync(id);
            if (show == null) return 0;
            _context.ShowSchedules.Remove(show);
            return await _context.SaveChangesAsync();
        }
    }
}
