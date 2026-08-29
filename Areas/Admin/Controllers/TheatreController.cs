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
    public class TheatreController : Controller
    {
        private readonly ITheatreRepo _repo;
        private readonly IMapper _mapper;

        public TheatreController(ITheatreRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var theatres = await _repo.GetTheatresAsync();
            var dtoList = _mapper.Map<List<TheatreDto>>(theatres);
            return View(dtoList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new TheatreCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Insert(TheatreCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var theatre = _mapper.Map<Theatre>(model);
            var res = await _repo.Insert(theatre);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to save the theatre. Please try again.");
            return View("Create", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var theatre = await _repo.GetTheatreById(id);
            if (theatre == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<TheatreEditDto>(theatre);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TheatreEditDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var theatre = _mapper.Map<Theatre>(model);
            var res = await _repo.Update(theatre);

            if (res > 0)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update the theatre. Please try again.");
            return View("Edit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var theatre = await _repo.GetTheatreById(id);
            if (theatre == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<TheatreDto>(theatre);
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
