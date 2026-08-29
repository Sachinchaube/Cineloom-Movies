using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public interface IMovieRepo
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<Movie?> GetMovieById(int id);
        Task<int> Insert(Movie obj);
        Task<int> Update(Movie obj);
        Task<int> Delete(int id);
    }
}
