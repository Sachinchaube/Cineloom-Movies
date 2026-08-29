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
    public class ShowScheduleController : Controller
    {
        private readonly IShowScheduleRepo _repo;
        private readonly IMovieRepo _movieRepo;
        private readonly IScreenRepo _screenRepo;
        private readonly IMapper _mapper;

        public ShowScheduleController(
            IShowScheduleRepo repo,
            IMovieRepo movieRepo,
            IScreenRepo screenRepo,
            IMapper mapper)
        {
            _repo = repo;
            _movieRepo = movieRepo;
            _screenRepo = screenRepo;
            _mapper = mapper;
        }

        private async Task LoadDropdowns()
        {
            var movies = await _movieRepo.GetMoviesAsync();
            var screens = await _screenRepo.GetScreensAsync();
            ViewBag.Movies = _mapper.Map<List<MovieDto>>(movies);
            ViewBag.Screens = _mapper.Map<List<ScreenDto>>(screens);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shows = await _repo.GetShowsAsync();
            var dtoList = _mapper.Map<List<ShowScheduleDto>>(shows);
            return View(dtoList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();

            var model = new ShowScheduleCreateDto
            {
                ShowDate = DateTime.Today,
                ShowTime = DateTime.Now.TimeOfDay
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ShowScheduleCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View("Create", model);
            }

            var show = _mapper.Map<ShowSchedule>(model);
            var res = await _repo.Insert(show);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to save the show schedule. Please try again.");
            await LoadDropdowns();
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var show = await _repo.GetShowById(id);
            if (show == null)
            {
                return NotFound();
            }

            await LoadDropdowns();
            var dto = _mapper.Map<ShowScheduleEditDto>(show);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ShowScheduleEditDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View("Edit", model);
            }

            var show = _mapper.Map<ShowSchedule>(model);
            var res = await _repo.Update(show);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update the show schedule. Please try again.");
            await LoadDropdowns();
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var show = await _repo.GetShowById(id);
            if (show == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ShowScheduleDto>(show);
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