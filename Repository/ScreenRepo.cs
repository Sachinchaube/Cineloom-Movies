using Microsoft.EntityFrameworkCore;
using MovieBookingPro.DAL;
using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public class ScreenRepo : IScreenRepo
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Screen>> GetScreensAsync()
        {
            return await _context.Screens
                .Include(s => s.Theatre)
                .ToListAsync();
        }

        public async Task<Screen?> GetScreenById(int id)
        {
            return await _context.Screens
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(s => s.ScreenId == id);
        }

        public async Task<int> Insert(Screen obj)
        {
            await _context.Screens.AddAsync(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Screen obj)
        {
            _context.Screens.Update(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null) return 0;
            _context.Screens.Remove(screen);
            return await _context.SaveChangesAsync();
        }
    }
}
