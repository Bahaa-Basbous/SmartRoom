using SmartRoom.Entities;

namespace SmartRoom.Repositories
{
    public interface IMeetingRepository
    {
        Task<IEnumerable<Meeting>> GetAllAsync();
        Task<Meeting?> GetByIdAsync(int id);
        Task CreateAsync(Meeting meeting);
        Task UpdateAsync(Meeting meeting);
        Task DeleteAsync(int id);
    }
}
