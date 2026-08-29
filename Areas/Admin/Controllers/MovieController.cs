using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Models;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieRepo _repo;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await _repo.GetMoviesAsync();
            var dtoList = _mapper.Map<List<MovieDto>>(movies);
            return View(dtoList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new MovieCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Insert(MovieCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var movie = _mapper.Map<Movie>(model);
            var res = await _repo.Insert(movie);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to save the movie. Please try again.");
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _repo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<MovieEditDto>(movie);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(MovieEditDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var movie = _mapper.Map<Movie>(model);
            var res = await _repo.Update(movie);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update the movie. Please try again.");
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _repo.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<MovieDto>(movie);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
