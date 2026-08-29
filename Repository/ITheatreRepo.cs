using MovieBookingPro.Models;

namespace MovieBookingPro.Repository
{
    public interface ITheatreRepo
    {
        Task<List<Theatre>> GetTheatresAsync();
        Task<Theatre?> GetTheatreById(int id);
        Task<int> Insert(Theatre obj);
        Task<int> Update(Theatre obj);
        Task<int> Delete(int id);
    }
}
