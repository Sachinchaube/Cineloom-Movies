using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.DTOs;
using MovieBookingPro.Models;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IShowScheduleRepo _showRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public BookingController(
            IBookingRepo bookingRepo,
            IShowScheduleRepo showRepo,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _bookingRepo = bookingRepo;
            _showRepo = showRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int showId)
        {
            var show = await _showRepo.GetShowById(showId);
            if (show == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ShowScheduleDto>(show);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(BookingCreateDto model)
        {
            var show = await _showRepo.GetShowById(model.ShowId);
            if (show == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ShowScheduleDto>(show);

            if (!ModelState.IsValid)
            {
                return View("Create", dto);
            }

            if (model.SeatCount > dto.SeatsAvailable)
            {
                ModelState.AddModelError(string.Empty, $"Only {dto.SeatsAvailable} seats are available for this show.");
                return View("Create", dto);
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            var booking = new Booking
            {
                UserId = userId,
                ShowId = model.ShowId,
                SeatCount = model.SeatCount,
                TotalAmount = model.SeatCount * dto.Price,
                BookingDate = DateTime.Now,
                Status = BookingStatus.Confirmed
            };

            var res = await _bookingRepo.Insert(booking);
            if (res > 0)
            {
                return RedirectToAction("Confirmation", new { id = booking.BookingId });
            }

            ModelState.AddModelError(string.Empty, "Something went wrong while confirming your booking. Please try again.");
            return View("Create", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
        {
            var booking = await _bookingRepo.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<BookingDto>(booking);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            var bookings = await _bookingRepo.GetBookingsByUser(userId);
            var dtoList = _mapper.Map<List<BookingDto>>(bookings);
            return View(dtoList);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _bookingRepo.GetBookingById(id);
            var userId = _userManager.GetUserId(User);

            if (booking == null || booking.UserId != userId)
            {
                return NotFound();
            }

            await _bookingRepo.CancelBooking(id);
            return RedirectToAction("MyBookings");
        }
    }
}
