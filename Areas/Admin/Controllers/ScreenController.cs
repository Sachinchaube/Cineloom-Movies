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
    public class ScreenController : Controller
    {
        private readonly IScreenRepo _repo;
        private readonly ITheatreRepo _theatreRepo;
        private readonly IMapper _mapper;

        public ScreenController(IScreenRepo repo, ITheatreRepo theatreRepo, IMapper mapper)
        {
            _repo = repo;
            _theatreRepo = theatreRepo;
            _mapper = mapper;
        }

        private async Task LoadTheatresDropdown()
        {
            var theatres = await _theatreRepo.GetTheatresAsync();
            ViewBag.Theatres = _mapper.Map<List<TheatreDto>>(theatres);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var screens = await _repo.GetScreensAsync();
            var dtoList = _mapper.Map<List<ScreenDto>>(screens);
            return View(dtoList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadTheatresDropdown();
            return View(new ScreenCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ScreenCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadTheatresDropdown();
                return View("Create", model);
            }

            var screen = _mapper.Map<Screen>(model);
            var res = await _repo.Insert(screen);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to save the screen. Please try again.");
            await LoadTheatresDropdown();
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var screen = await _repo.GetScreenById(id);
            if (screen == null)
            {
                return NotFound();
            }

            await LoadTheatresDropdown();
            var dto = _mapper.Map<ScreenEditDto>(screen);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ScreenEditDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadTheatresDropdown();
                return View("Edit", model);
            }

            var screen = _mapper.Map<Screen>(model);
            var res = await _repo.Update(screen);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update the screen. Please try again.");
            await LoadTheatresDropdown();
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var screen = await _repo.GetScreenById(id);
            if (screen == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ScreenDto>(screen);
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
