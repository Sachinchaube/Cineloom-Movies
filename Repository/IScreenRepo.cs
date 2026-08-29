using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public interface IScreenRepo
    {
        Task<List<Screen>> GetScreensAsync();
        Task<Screen?> GetScreenById(int id);
        Task<int> Insert(Screen obj);
        Task<int> Update(Screen obj);
        Task<int> Delete(int id);
    }
}
