using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.Models;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMovieRepo _movieRepo;

        public HomeController(IMovieRepo movieRepo)
        {
            _movieRepo = movieRepo;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieRepo.GetMoviesAsync();
            var upcoming = movies
                .OrderByDescending(m => m.ReleaseDate)
                .Take(8)
                .ToList();
            return View(upcoming);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
