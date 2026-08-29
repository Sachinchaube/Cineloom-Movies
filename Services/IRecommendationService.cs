using MovieBookingPro.Models;

namespace MovieBookingPro.Services
{
    public interface IRecommendationService
    {
        Task<List<Movie>> GetRecommendationsAsync(Movie currentMovie, List<Movie> allMovies);
    }
}
