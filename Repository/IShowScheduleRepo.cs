using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public interface IShowScheduleRepo
    {
        Task<List<ShowSchedule>> GetShowsAsync();
        Task<ShowSchedule?> GetShowById(int id);
        Task<List<ShowSchedule>> GetShowsByMovie(int movieId);
        Task<int> Insert(ShowSchedule obj);
        Task<int> Update(ShowSchedule obj);
        Task<int> Delete(int id);
    }
}
