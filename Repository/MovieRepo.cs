using Microsoft.EntityFrameworkCore;
using MovieBookingPro.DAL;
using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public class MovieRepo : IMovieRepo
    {
        private readonly ApplicationDbContext _context;

        public MovieRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetMovieById(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<int> Insert(Movie obj)
        {
            await _context.Movies.AddAsync(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(Movie obj)
        {
            _context.Movies.Update(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return 0;
            _context.Movies.Remove(movie);
            return await _context.SaveChangesAsync();
        }
    }
}
