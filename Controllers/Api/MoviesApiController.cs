using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Models;
using MovieBookingPro.Repository;
using MovieBookingPro.Services;

namespace MovieBookingPro.Controllers.Api
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesApiController : ControllerBase
    {
        private readonly IMovieRepo _movieRepo;
        private readonly IShowScheduleRepo _showRepo;
        private readonly IRecommendationService _recommendationService;
        private readonly IMapper _mapper;

        public MoviesApiController(
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
        public async Task<ActionResult<List<MovieDto>>> GetMovies([FromQuery] string? search, [FromQuery] string? genre)
        {
            var movies = await _movieRepo.GetMoviesAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                movies = movies
                    .Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                             || m.Genre.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(genre) && !genre.Equals("All Genres", StringComparison.OrdinalIgnoreCase))
            {
                movies = movies
                    .Where(m => m.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var dtoList = _mapper.Map<List<MovieDto>>(movies);
            return Ok(dtoList);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<object>> GetMovieById(int id)
        {
            var movie = await _movieRepo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound(new { message = $"Movie with ID {id} not found." });
            }

            var shows = await _showRepo.GetShowsByMovie(id);
            var allMovies = await _movieRepo.GetMoviesAsync();
            var recommendations = await _recommendationService.GetRecommendationsAsync(movie, allMovies);

            return Ok(new
            {
                movie = _mapper.Map<MovieDto>(movie),
                shows = _mapper.Map<List<ShowScheduleDto>>(shows),
                recommendations = _mapper.Map<List<MovieDto>>(recommendations)
            });
        }

        [HttpGet("{id:int}/recommendations")]
        public async Task<ActionResult<List<MovieDto>>> GetRecommendations(int id)
        {
            var movie = await _movieRepo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound(new { message = $"Movie with ID {id} not found." });
            }

            var allMovies = await _movieRepo.GetMoviesAsync();
            var recommendations = await _recommendationService.GetRecommendationsAsync(movie, allMovies);
            return Ok(_mapper.Map<List<MovieDto>>(recommendations));
        }
    }
}
