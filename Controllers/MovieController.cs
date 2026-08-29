using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Repository;
using MovieBookingPro.Services;

namespace MovieBookingPro.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepo _movieRepo;
        private readonly IShowScheduleRepo _showRepo;
        private readonly IRecommendationService _recommendationService;
        private readonly IMapper _mapper;

        public MovieController(
            IMovieRepo movieRepo,
            IShowScheduleRepo showRepo,
            IRecommendationService recommendationService,
            IMapper mapper)
        {
            _movieRepo = movieRepo;
            _showRepo = showRepo;
            _recommendationService = recommendationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var movies = await _movieRepo.GetMoviesAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                movies = movies
                    .Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                             || m.Genre.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var dtoList = _mapper.Map<List<MovieDto>>(movies);
            ViewBag.Search = search;
            return View(dtoList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _movieRepo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var shows = await _showRepo.GetShowsByMovie(id);
            var allMovies = await _movieRepo.GetMoviesAsync();

            var recommendations = await _recommendationService.GetRecommendationsAsync(movie, allMovies);

            ViewBag.Movie = _mapper.Map<MovieDto>(movie);
            ViewBag.Shows = _mapper.Map<List<ShowScheduleDto>>(shows);
            ViewBag.Recommendations = _mapper.Map<List<MovieDto>>(recommendations);

            return View();
        }
    }
}
