using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BookingController : Controller
    {
        private readonly IBookingRepo _repo;
        private readonly IMapper _mapper;

        public BookingController(IBookingRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var bookings = await _repo.GetAllBookingsAsync();
            var dtoList = _mapper.Map<List<BookingDto>>(bookings);
            return View(dtoList);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            await _repo.CancelBooking(id);
            return RedirectToAction("Index");
        }
    }
}
