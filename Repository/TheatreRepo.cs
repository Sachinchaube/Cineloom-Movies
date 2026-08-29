using Microsoft.EntityFrameworkCore;
using MovieBookingPro.DAL;
using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public class TheatreRepo : ITheatreRepo
    {
        private readonly ApplicationDbContext _context;

        public TheatreRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Theatre>> GetTheatresAsync()
        {
            return await _context.Theatres.ToListAsync();
        }

        public async Task<Theatre?> GetTheatreById(int id)
        {
            return await _context.Theatres.FindAsync(id);
        }

        public async Task<int> Insert(Theatre obj)
        {
            await _context.Theatres.AddAsync(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Theatre obj)
        {
            _context.Theatres.Update(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var theatre = await _context.Theatres.FindAsync(id);
            if (theatre == null) return 0;
            _context.Theatres.Remove(theatre);
            return await _context.SaveChangesAsync();
        }
    }
}
