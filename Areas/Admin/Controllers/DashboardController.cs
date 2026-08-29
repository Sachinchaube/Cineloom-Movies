using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingPro.Repository;

namespace MovieBookingPro.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IMovieRepo _movieRepo;
        private readonly ITheatreRepo _theatreRepo;
        private readonly IScreenRepo _screenRepo;
        private readonly IShowScheduleRepo _showRepo;
        private readonly IBookingRepo _bookingRepo;

        public DashboardController(
            IMovieRepo movieRepo,
            ITheatreRepo theatreRepo,
            IScreenRepo screenRepo,
            IShowScheduleRepo showRepo,
            IBookingRepo bookingRepo)
        {
            _movieRepo = movieRepo;
            _theatreRepo = theatreRepo;
            _screenRepo = screenRepo;
            _showRepo = showRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.MovieCount = (await _movieRepo.GetMoviesAsync()).Count;
            ViewBag.TheatreCount = (await _theatreRepo.GetTheatresAsync()).Count;
            ViewBag.ScreenCount = (await _screenRepo.GetScreensAsync()).Count;
            ViewBag.ShowCount = (await _showRepo.GetShowsAsync()).Count;

            var bookings = await _bookingRepo.GetAllBookingsAsync();
            ViewBag.BookingCount = bookings.Count;
            ViewBag.TotalRevenue = bookings
                .Where(b => b.Status == Models.BookingStatus.Confirmed)
                .Sum(b => b.TotalAmount);

            return View();
        }
    }
}
